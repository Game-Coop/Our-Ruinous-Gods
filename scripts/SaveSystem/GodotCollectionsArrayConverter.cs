using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Godot;

public class GodotCollectionsArrayConverter : JsonConverter<Godot.Collections.Array>
{
	public override void WriteJson(JsonWriter writer, Godot.Collections.Array value, JsonSerializer serializer)
	{
		writer.WriteStartArray();
		foreach (var item in value)
			serializer.Serialize(writer, item);
		writer.WriteEndArray();
	}

	public override Godot.Collections.Array ReadJson(JsonReader reader, Type objectType, Godot.Collections.Array existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		var jArray = JArray.Load(reader);
		var godotArray = new Godot.Collections.Array();

		foreach (var token in jArray)
		{
			godotArray.Add(ConvertToGodotCompatible(token));
		}

		return godotArray;
	}

	private Variant ConvertToGodotCompatible(JToken token)
	{
		switch (token.Type)
		{
			case JTokenType.Boolean:
				return token.Value<bool>();

			case JTokenType.Integer:
				return token.Value<int>();

			case JTokenType.Float:
				return token.Value<float>();

			case JTokenType.String:
				return token.Value<string>();

			case JTokenType.Null:
				return default;

			case JTokenType.Array:
			{
				var innerArray = new Godot.Collections.Array();
				foreach (var item in token)
					innerArray.Add(ConvertToGodotCompatible(item));
				return innerArray;
			}

			case JTokenType.Object:
			{
				var dict = new Godot.Collections.Dictionary();
				foreach (var kvp in (JObject)token)
					dict[kvp.Key] = ConvertToGodotCompatible(kvp.Value);
				return dict;
			}

			default:
				GD.PushError($"Unsupported JSON token type: {token.Type}");
				return default;
		}
	}
}
