using NConfig.Abstractions;
using System.IO;
using System.Xml.Serialization;

namespace NConfig.Xml
{
    public class XmlFileConfiguration<TConfig> : FileConfiguration<TConfig>, IConfiguration<TConfig> where TConfig : class, new()
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(TConfig));

        public XmlFileConfiguration(string path)
            : base(path)
        {
        }

        public static XmlFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}.xml");
            return new XmlFileConfiguration<TConfig>(path);
        }

        public override TConfig Load()
        {
            return Load(FilePath);
        }

        public static TConfig Load(string path)
        {
            using (var stream = new StreamReader(path))
            {
                return (TConfig)_serializer.Deserialize(stream);
            }
        }

        public override void Save(TConfig value)
        {
            Save(value, FilePath);
        }

        public static void Save(TConfig value, string path)
        {
            using (var stream = new StreamWriter(path))
            {
                _serializer.Serialize(stream, value);
            }
        }
    }
}
