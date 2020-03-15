using System;
using System.IO;
using YamlDotNet.Serialization;

namespace NConfig.Yaml
{
    public class YamlFileConfiguration<TConfig> : FileConfiguration<TConfig>, IConfiguration<TConfig> 
        where TConfig : class, new()
    {
        public YamlFileConfiguration(string filePath)
            : base (filePath)
        {

        }

        public static YamlFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}.yaml");
            return new YamlFileConfiguration<TConfig>(path);
        }

        public TConfig Load()
        {
            return Load(FullPath);
        }

        public static TConfig Load(string filePath)
        {
            try
            {
                using (var s = new FileStream(filePath, FileMode.Open))
                using (var reader = new StreamReader(s))
                {
                    var serializer = new Deserializer();
                    return (TConfig)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new ConfigFileLoadException(filePath, e);
            }
        }

        public void Save(TConfig value)
        {
            Save(value, FullPath);
        }

        public static void Save(TConfig value, string filePath)
        {
            try
            {
                using (var s = new FileStream(filePath, FileMode.Create))
                using (var writer = new StreamWriter(s))
                {
                    var serializer = new Serializer();
                    serializer.Serialize(writer, value);
                }
            }
            catch (Exception e)
            {
                throw new ConfigFileSaveException(typeof(TConfig), filePath, e);
            }
        }
    }
}
