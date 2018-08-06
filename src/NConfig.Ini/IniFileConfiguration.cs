using NConfig.Abstractions;
using System;
using System.IO;

namespace NConfig.Ini
{
    public class IniFileConfiguration<TConfig> : FileConfiguration<TConfig>
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

        public override TConfig Load()
        {
            return Load(FilePath);
        }

        public static TConfig Load(string path)
        {
            return (TConfig)_serializer.Deserialize(path);
        }

        public override void Save(TConfig value)
        {
            Save(value, FilePath);
        }

        public static void Save(TConfig value, string path)
        {
            _serializer.Serialize(value, path);
        }
    }
}
