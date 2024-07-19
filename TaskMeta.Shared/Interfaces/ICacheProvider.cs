namespace TaskMeta.Shared.Interfaces
{
    public interface ICacheProvider
    {
        void Clear();
        T? Get<T>(string key);
        void Remove(string key);
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, int minutes);
        
        
    }
}