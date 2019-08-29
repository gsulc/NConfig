using Newtonsoft.Json;
using System;
using System.IO;

namespace NConfig.Json
{
    public class JsonFileConfiguration<TConfig> : 
        FileConfiguration<TConfig>, IConfiguration<TConfig> where TConfig : class, new()
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer();

        public JsonFileConfiguration(string path)
            : base(path)
        {
        }

        public static JsonFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}.json");
            return new JsonFileConfiguration<TConfig>(path);
        }

        public TConfig Load()
        {
            return Load(FilePath);
        }

        public static TConfig Load(string path)
        {
            try
            {
                using (var stream = new StreamReader(path))
                using (var jsonReader = new JsonTextReader(stream))
                {
                    return (TConfig)_serializer.Deserialize(jsonReader);
                }
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
            try
            {
                using (var stream = new StreamWriter(path))
                {
                    _serializer.Serialize(stream, value);
                }
            }
            catch (Exception e)
            {
                throw new ConfigFileSaveException(typeof(TConfig), path, e);
            }
        }
    }
}
