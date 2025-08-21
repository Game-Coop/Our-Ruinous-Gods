#if TOOLS
using Godot;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System;

[Tool]
public partial class WwiseCsharpIdGenerator : EditorPlugin
{
	private const string OutputPath = "res://scripts/Wwise/WwiseIds.cs";

	public override void _EnterTree()
	{
		AddToolMenuItem("Generate WwiseIds.cs", Callable.From(OnGenerateClicked));
	}

	public override void _ExitTree()
	{
		RemoveToolMenuItem("Generate WwiseIds.cs");
	}

	private void OnGenerateClicked()
	{
		var xmlRelative = FindFirstSoundbanksInfoXml();
		if (xmlRelative == null)
		{
			GD.PrintErr("No SoundbanksInfo.xml found.");
			return;
		}

		var xmlPath = ProjectSettings.GlobalizePath(xmlRelative);
		var outputPath = ProjectSettings.GlobalizePath(OutputPath);

		try
		{
			Generate(xmlPath, outputPath);
			GD.Print($"✅ WwiseIds.cs generated at: {OutputPath}");
		}
		catch (System.Exception ex)
		{
			GD.PrintErr("Error generating WwiseIds.cs:", ex.Message);
		}
	}
	public void Generate(string xmlPath, string outputPath)
	{
		var xml = new XmlDocument();
		xml.Load(xmlPath);

		using var writer = new StreamWriter(outputPath, false);

		writer.WriteLine("// Auto-generated from Wwise SoundBanksInfo.xml");
		writer.WriteLine("namespace AK");
		writer.WriteLine("{");

		WriteSoundBanks(writer, xml);

		WriteIdConstants(writer, "EVENTS", xml.SelectNodes("//Event"));
		WriteIdConstants(writer, "BUSSES", xml.SelectNodes("//Bus"));
		WriteIdConstants(writer, "RTPCS", xml.SelectNodes("//GameParameter"));

		WriteAudioDevices(writer, xml);
		WriteSwitchGroups(writer, xml);
		WriteStateGroups(writer, xml);

		writer.WriteLine("} // namespace AK");
	}

	private string FindFirstSoundbanksInfoXml()
	{
		const string settingKey = "wwise/common_user_settings/root_output_path";

		if (!ProjectSettings.HasSetting(settingKey))
		{
			GD.PrintErr($"Missing ProjectSetting: {settingKey}");
			return null;
		}

		var basePath = ProjectSettings.GetSetting(settingKey).AsString();
		var globalBase = ProjectSettings.GlobalizePath(basePath);

		if (!Directory.Exists(globalBase))
		{
			GD.PrintErr($"Wwise output directory not found: {globalBase}");
			return null;
		}

		foreach (var dir in Directory.GetDirectories(globalBase))
		{
			var candidate = Path.Combine(dir, "SoundbanksInfo.xml");
			if (File.Exists(candidate))
				return ProjectSettings.LocalizePath(candidate);
		}

		return null;
	}

	private void WriteIdConstants(StreamWriter writer, string category, XmlNodeList nodes)
	{
		if (nodes == null || nodes.Count == 0)
			return;

		var uniqueNames = new HashSet<string>();

		WriteClass(writer, category, () =>
		{
			foreach (XmlNode node in nodes)
			{
				var name = node.Attributes?["Name"]?.Value;
				var id = node.Attributes?["Id"]?.Value;

				if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(id))
					continue;

				var safeName = MakeSafe(name).ToUpperInvariant();
				if (!uniqueNames.Add(safeName))
					continue;

				writer.WriteLine($"        public static readonly (string Name, uint Id) {safeName} = (\"{name}\", {id}U);");
			}
		});
	}

	private void WriteSoundBanks(StreamWriter writer, XmlDocument xml)
	{
		var nodes = xml.SelectNodes("//SoundBank");
		if (nodes == null || nodes.Count == 0)
			return;

		var uniqueNames = new HashSet<string>();

		WriteClass(writer, "BANKS", () =>
		{
			foreach (XmlNode node in nodes)
			{
				var id = node.Attributes?["Id"]?.Value;
				var shortNameNode = node.SelectSingleNode("ShortName");
				if (string.IsNullOrEmpty(id) || shortNameNode == null)
					continue;

				var name = shortNameNode.InnerText;
				var safeName = MakeSafe(name).ToUpperInvariant();
				if (!uniqueNames.Add(safeName))
					continue;

				writer.WriteLine($"        public static readonly (string Name, uint Id) {safeName} = (\"{name}\", {id}U);");
			}
		});
	}

	private static void WriteClass(StreamWriter writer, string className, Action body, int tabCount = 1)
	{
		var tabString = string.Concat(System.Linq.Enumerable.Repeat("\t", tabCount));
		writer.WriteLine($"{tabString}public static class {className}");
		writer.WriteLine($"{tabString}{{");
		body.Invoke();
		writer.WriteLine($"{tabString}}}");
	}
	private void WriteAudioDevices(StreamWriter writer, XmlDocument xml)
	{
		var audioDeviceNodes = new List<XmlNode>();

		// Find all SoundBank nodes
		var soundBanks = xml.SelectNodes("//SoundBank");
		if (soundBanks == null) return;

		foreach (XmlNode bank in soundBanks)
		{
			var devices = bank.SelectNodes("Plugins/AudioDevices/Plugin");
			if (devices != null)
				foreach (XmlNode device in devices)
					audioDeviceNodes.Add(device);
		}

		if (audioDeviceNodes.Count == 0)
			return;

		var uniqueNames = new HashSet<string>();
		WriteClass(writer, "AUDIO_DEVICES", () =>
		{
			foreach (var node in audioDeviceNodes)
			{
				var id = node.Attributes?["Id"]?.Value;
				var name = node.Attributes?["Name"]?.Value;

				if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
					continue;

				var safeName = MakeSafe(name).ToUpperInvariant();
				if (!uniqueNames.Add(safeName))
					continue;

				writer.WriteLine($"        public static readonly (string Name, uint Id) {safeName} = (\"{name}\", {id}U);");
			}
		});
	}
	private void WriteSwitchGroups(StreamWriter writer, XmlDocument xml)
	{
		var groups = xml.SelectNodes("//SwitchGroup");
		if (groups == null || groups.Count == 0)
			return;

		var writtenGroups = new HashSet<string>(); // Track written group names

		WriteClass(writer, "SWITCHES", () =>
		{
			foreach (XmlNode group in groups)
			{
				var groupName = MakeSafe(group.Attributes?["Name"]?.Value);
				var groupId = group.Attributes?["Id"]?.Value;

				if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(groupId))
					continue;

				if (writtenGroups.Contains(groupName))
					continue; // Skip if already written

				writtenGroups.Add(groupName);

				WriteClass(writer, groupName.ToUpperInvariant(), () =>
				{
					writer.WriteLine($"            public const uint GROUP = {groupId}U;");
					WriteClass(writer, "SWITCH", () =>
					{
						var switches = group.SelectNodes("Switches/Switch");
						foreach (XmlNode sw in switches)
						{
							var switchName = MakeSafe(sw.Attributes?["Name"]?.Value);
							var switchId = sw.Attributes?["Id"]?.Value;

							if (string.IsNullOrEmpty(switchName) || string.IsNullOrEmpty(switchId))
								continue;

							writer.WriteLine($"                public const uint {switchName.ToUpperInvariant()} = {switchId}U;");
						}
					}, 3);
				}, 2);
			}
		});
	}
	private void WriteStateGroups(StreamWriter writer, XmlDocument xml)
	{
		var groups = xml.SelectNodes("//StateGroup");
		if (groups == null || groups.Count == 0)
			return;

		var writtenGroups = new HashSet<string>();

		WriteClass(writer, "STATES", () =>
		{
			foreach (XmlNode group in groups)
			{
				var groupName = MakeSafe(group.Attributes?["Name"]?.Value);
				var groupId = group.Attributes?["Id"]?.Value;
				if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(groupId))
					continue;

				if (writtenGroups.Contains(groupName))
					continue;

				writtenGroups.Add(groupName);

				WriteClass(writer, groupName.ToUpperInvariant(), () =>
				{
					writer.WriteLine($"            public const uint GROUP = {groupId}U;");
					WriteClass(writer, "STATE", () =>
					{
						var states = group.SelectNodes("States/State");
						foreach (XmlNode state in states)
						{
							var stateName = MakeSafe(state.Attributes?["Name"]?.Value);
							var stateId = state.Attributes?["Id"]?.Value;
							if (string.IsNullOrEmpty(stateName) || string.IsNullOrEmpty(stateId))
								continue;

							writer.WriteLine($"                public const uint {stateName.ToUpperInvariant()} = {stateId}U;");
						}
					}, 3);
				}, 2);
			}
		});
	}
	private string MakeSafe(string input)
	{
		var clean = input
			.Replace(" ", "_")
			.Replace("-", "_")
			.Replace(".", "_")
			.Replace("/", "_")
			.Replace("\\", "_");

		// C# enums cannot start with digits
		if (char.IsDigit(clean[0]))
			clean = "_" + clean;

		return clean;
	}
}
#endif
