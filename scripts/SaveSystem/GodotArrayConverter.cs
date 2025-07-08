using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Godot;

public class GodotArrayConverter : JsonConverter<Godot.Collections.Array>
{
	public override void WriteJson(JsonWriter writer, Godot.Collections.Array value, JsonSerializer serializer)
	{
		writer.WriteStartArray();
		foreach (var item in value)
		{
			serializer.Serialize(writer, item);
		}
		writer.WriteEndArray();
	}

	public override Godot.Collections.Array ReadJson(JsonReader reader, Type objectType, Godot.Collections.Array existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		var jArray = JArray.Load(reader);
		var godotArray = new Godot.Collections.Array();

		foreach (var token in jArray)
		{
			// Deserialize each token to .NET object and add to Godot Array
			object value = token.ToObject<object>(serializer);
			godotArray.Add((Variant)value);
		}

		return godotArray;
	}
}
