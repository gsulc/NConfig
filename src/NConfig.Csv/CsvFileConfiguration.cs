using NConfig.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NConfig.Csv
{
    public class CsvFileConfiguration<TConfig> : 
        FileConfiguration<TConfig>, IListConfiguration<TConfig>
        where TConfig : class, new()
    {
        private PropertyInfo[] _configProperties;

        public CsvFileConfiguration(string filePath)
            : base(filePath)
        {
        }

        private PropertyInfo[] ConfigProperties => 
            _configProperties ?? (_configProperties = typeof(TConfig).GetProperties());

        public bool UsingHeader { get; set; } = true;

        public static CsvFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}.csv");
            return new CsvFileConfiguration<TConfig>(path);
        }

        public List<TConfig> Load()
        {
            var list = new List<TConfig>();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                var elements = line.Split(',');
                if (UsingHeader && line == lines.First())
                {
                    ParseHeader(line);
                }
                else
                {
                    var obj = CreateObject(elements);
                    list.Add(obj);
                }
            }
            return list;
        }

        // preserves the order of the properties in the file
        private void ParseHeader(string line)
        {
            var elements = line.Split(',');
            var properties = typeof(TConfig).GetProperties();
            _configProperties = new PropertyInfo[properties.Length];
            for (int i = 0; i < elements.Length; ++i)
            {
                var name = elements[i];
                _configProperties[i] = properties.Where(p => p.Name == name).First();
            }
        }

        private TConfig CreateObject(string[] propertyStringValues)
        {
            var obj = (TConfig)Activator.CreateInstance(typeof(TConfig));
            for (int i = 0; i < ConfigProperties.Length; ++i)
            {
                var stringValue = propertyStringValues[i];
                var type = ConfigProperties[i].PropertyType;
                var value = Convert.ChangeType(stringValue, type);
                ConfigProperties[i].SetValue(obj, value);
            }
            return obj;
        }

        public void Save(List<TConfig> list)
        {
            using (var stream = new StreamWriter(FullPath))
            {
                if (UsingHeader)
                    stream.WriteLine(GetHeader());
                foreach (var item in list)
                    stream.WriteLine(CreateItemLine(item));
            }
        }

        private string GetHeader()
        {
            return BuildLine(ConfigProperties.Select(p => p.Name));
        }

        private string CreateItemLine(object item)
        {
            var values = ConfigProperties.Select(p => p.GetValue(item).ToString());
            return BuildLine(values);
        }

        private string BuildLine(IEnumerable<string> elements)
        {
            var builder = new StringBuilder();
            var lastElement = elements.Last();
            foreach (var element in elements)
            {
                builder.Append(element);
                bool atTheEnd = element == lastElement;
                if (!atTheEnd)
                    builder.Append(",");
            }
            return builder.ToString();
        }
    }
}
