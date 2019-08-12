using System.Collections.Generic;

namespace NConfig
{
    public interface IListConfiguration<TConfig>
        : IConfiguration<List<TConfig>>
        where TConfig : class, new()
    {
    }
}
