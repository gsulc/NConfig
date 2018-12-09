using System.Collections.Generic;

namespace NConfig.Abstractions
{
    public interface IListConfiguration<TConfig>
        : IConfiguration<List<TConfig>>
        where TConfig : class, new()
    {
    }
}
