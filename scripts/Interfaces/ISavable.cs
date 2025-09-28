public interface ISavable<T>
{
    void OnSave(T data);
    void OnLoad(T data);
}