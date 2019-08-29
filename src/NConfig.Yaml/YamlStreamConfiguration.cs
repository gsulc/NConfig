using System.IO;
using YamlDotNet.Serialization;

namespace NConfig.Yaml
{
    public class YamlStreamConfiguration<TConfig> :
        StreamConfiguration<TConfig>, IConfiguration<TConfig> where TConfig : class, new()
    {
        public YamlStreamConfiguration(Stream stream)
            : base (stream)
        {

        }

        public TConfig Load()
        {
            var d = new Deserializer();
            using (var reader = new StreamReader(Stream))
            {
                return (TConfig)d.Deserialize(reader);
            }
        }

        public void Save(TConfig value)
        {
            using (var writer = new StreamWriter(Stream))
            {
                var serializer = new Serializer();
                serializer.Serialize(writer, value);
            }
        }
    }
}
