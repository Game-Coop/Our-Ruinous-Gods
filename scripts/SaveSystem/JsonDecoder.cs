using Godot;
using Newtonsoft.Json;
public partial class JsonDecoder : IDecoder
{
    private static JsonSerializerSettings settings;

    static JsonDecoder()
    {
        settings = new JsonSerializerSettings();
        settings.Converters.Add(new GodotCollectionsArrayConverter());
    }
    public string Encode<T>(T content)
    {
        return JsonConvert.SerializeObject(content, settings);
    }
    public T Decode<T>(string content)
    {
        return JsonConvert.DeserializeObject<T>(content, settings);
    }
}