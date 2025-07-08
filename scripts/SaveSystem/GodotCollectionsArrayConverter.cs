using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Godot;
using Godot.Collections;

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
			// No cast is needed here anymore
			godotArray.Add(ConvertToGodotCompatible(token));
		}

		return godotArray;
	}

	// ✅ Changed return type from 'object' to 'Variant'
	private Variant ConvertToGodotCompatible(JToken token)
	{
		switch (token.Type)
		{
			case JTokenType.Boolean:
				return token.Value<bool>();

			case JTokenType.Integer:
				return token.Value<int>(); // Implicitly converted to Variant

			case JTokenType.Float:
				return token.Value<float>(); // Implicitly converted to Variant

			case JTokenType.String:
				return token.Value<string>(); // Implicitly converted to Variant

			case JTokenType.Null:
				return default; // A null Variant

			case JTokenType.Array:
			{
				var innerArray = new Godot.Collections.Array();
				foreach (var item in token)
					// ✅ Recursive call now returns Variant, so no cast is needed
					innerArray.Add(ConvertToGodotCompatible(item));
				return innerArray; // Godot.Collections.Array is already a Variant-compatible type
			}

			case JTokenType.Object:
			{
				var dict = new Godot.Collections.Dictionary();
				foreach (var kvp in (JObject)token)
					 // ✅ Recursive call now returns Variant, so no cast is needed
					dict[kvp.Key] = ConvertToGodotCompatible(kvp.Value);
				return dict; // Godot.Collections.Dictionary is already a Variant-compatible type
			}

			default:
				GD.PushError($"Unsupported JSON token type: {token.Type}");
				return default;
		}
	}
}
