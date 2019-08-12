using System.Collections.Generic;
using System.IO;

namespace NConfig.Xml
{
    public class XmlListFileConfiguration<TConfig>
        : XmlFileConfiguration<List<TConfig>>, IListConfiguration<TConfig>
        where TConfig : class, new()
    {
        public XmlListFileConfiguration(string path) : base(path)
        {
        }

        public new static XmlListFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}s.xml");
            return new XmlListFileConfiguration<TConfig>(path);
        }
    }
}
