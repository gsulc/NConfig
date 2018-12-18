using NConfig.Abstractions;
using System.Collections.Generic;
using System.IO;

namespace NConfig.Json
{
    public class JsonListFileConfiguration<TConfig> 
        : JsonFileConfiguration<List<TConfig>>, IListConfiguration<TConfig>
        where TConfig : class, new()
    {
        public JsonListFileConfiguration(string filePath) : base(filePath)
        {
        }

        public new static JsonListFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}s.json");
            return new JsonListFileConfiguration<TConfig>(path);
        }
    }
}
