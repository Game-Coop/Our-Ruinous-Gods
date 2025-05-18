
using System;
using System.Collections.Generic;

public interface ISaveSystem
{
    void Save<T>(string saveFileName, T data);
    void Load<T>(string saveFileName, out T data) where T : new();
    void Clear(string saveFileName);
    List<string> GetValidSaves(SaveFormat format);
    bool HasSave(string saveFileName);
}