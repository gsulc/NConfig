﻿using NConfig.Abstractions;
using System.IO;
using System.Xml.Serialization;

namespace NConfig.Xml
{
    public class XmlFileConfiguration<TConfig> : 
        FileConfiguration<TConfig>, IConfiguration<TConfig> where TConfig : class, new()
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

        public TConfig Load()
        {
            return Load(FilePath);
        }

        public static TConfig Load(string filePath)
        {
            using (var stream = new StreamReader(filePath))
            {
                return (TConfig)_serializer.Deserialize(stream);
            }
        }

        public void Save(TConfig value)
        {
            Save(value, FilePath);
        }

        public static void Save(TConfig value, string filePath)
        {
            using (var stream = new StreamWriter(filePath))
            {
                _serializer.Serialize(stream, value);
            }
        }
    }
}
