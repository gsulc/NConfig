using NConfig.Abstractions;
using System.IO;
using System.Xml.Serialization;

namespace NConfig.Xml
{
    public class XmlStreamConfiguration<TConfig> :
        StreamConfiguration<TConfig>, IConfiguration<TConfig> where TConfig : class, new()
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(TConfig));

        public XmlStreamConfiguration(Stream stream) : base(stream)
        { }

        public TConfig Load()
        {
            return (TConfig)_serializer.Deserialize(Stream);
        }

        public void Save(TConfig value)
        {
            _serializer.Serialize(Stream, value);
        }
    }
}
