using NConfig.Abstractions;
using System.IO;
using System.Xml.Serialization;

namespace NConfig.Xml
{
    public class XmlStreamConfiguration<TConfig> : StreamConfiguration<TConfig> where TConfig : class
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(TConfig));

        public XmlStreamConfiguration(Stream stream) : base(stream)
        { }

        public override TConfig Load()
        {
            return (TConfig)_serializer.Deserialize(Stream);
        }

        public override void Save(TConfig value)
        {
            _serializer.Serialize(Stream, value);
        }
    }
}
