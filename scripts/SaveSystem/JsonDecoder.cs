using Godot;
using Newtonsoft.Json;
public class JsonDecoder : IDecoder
{
    public string Encode<T>(T content)
    {
        return JsonConvert.SerializeObject(content);
    }
    public T Decode<T>(string content)
    {
        return JsonConvert.DeserializeObject<T>(content);
    }
}