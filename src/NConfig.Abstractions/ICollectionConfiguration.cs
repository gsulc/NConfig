using System.Collections.Generic;

namespace NConfig.Abstractions
{
    public interface ICollectionConfiguration<TConfig> 
        : IListConfiguration<TConfig>, IEnumerableConfiguration<TConfig> where TConfig : class, new()
    {
    }

    public interface IListConfiguration<TConfig> : IConfiguration<List<TConfig>>
        where TConfig : class, new()
    {
    }

    public interface IEnumerableConfiguration<TConfig> where TConfig : class, new()
    {
        IEnumerable<TConfig> Load();
        void Save(IEnumerable<TConfig> values);
    }
}
