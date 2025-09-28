
public interface IDecoder
{
    string Encode<T>(T content);
    T Decode<T>(string content);
}