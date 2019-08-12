using System;
using System.IO;

namespace NConfig.Ini
{
    public class IniFileConfiguration<TConfig> : 
        FileConfiguration<TConfig>, IConfiguration<TConfig> where TConfig : class, new()
    {
        private static readonly IniSerializer _serializer = new IniSerializer(typeof(TConfig));

        public IniFileConfiguration(string path)
            : base(path)
        {
        }

        public static IniFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}.ini");
            return new IniFileConfiguration<TConfig>(path);
        }

        public TConfig Load()
        {
            return Load(FilePath);
        }

        public static TConfig Load(string path)
        {
            try
            {
                return (TConfig)_serializer.Deserialize(path);
            }
            catch (Exception e)
            {
                throw new ConfigFileLoadException(path, e);
            }
        }

        public void Save(TConfig value)
        {
            Save(value, FilePath);
        }

        public static void Save(TConfig value, string path)
        {
            _serializer.Serialize(value, path);
        }
    }
}
