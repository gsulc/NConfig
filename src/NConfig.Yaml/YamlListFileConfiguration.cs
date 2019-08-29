using System.Collections.Generic;
using System.IO;

namespace NConfig.Yaml
{
    public class YamlListFileConfiguration<TConfig> 
        : YamlFileConfiguration<List<TConfig>>, IListConfiguration<TConfig>
        where TConfig : class, new()
    {
        public YamlListFileConfiguration(string filePath)
            : base (filePath)
        {

        }

        new public static YamlListFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}s.yaml");
            return new YamlListFileConfiguration<TConfig>(path);
        }
    }
}
