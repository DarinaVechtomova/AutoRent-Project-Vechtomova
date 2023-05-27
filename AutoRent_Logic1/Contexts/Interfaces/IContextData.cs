using System.Collections.Generic;

namespace AutoRent_Logic.Contexts.Interfaces
{
    public interface IContextData<T>
    {
        void SaveData(List<T> t, string path);
        List<T> ReadFromJson(string path);
    }
}
