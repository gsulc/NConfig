using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NConfig.Ini
{
    public class IniSerializer
    {
        private static readonly FileIniDataParser parser = new FileIniDataParser();
        private readonly Type _type;
        private readonly PropertyInfo[] _propertyInfos;
        
        public IniSerializer(Type type)
        {
            _type = type;
            _propertyInfos = _type.GetProperties();
        }

        public void Serialize(object obj, string path)
        {
            var iniData = ConstructIniData(obj);
            Serialize(iniData, path);
        }

        public IniData ConstructIniData(object obj)
        {
            IniData data = new IniData();
            foreach (var section in _propertyInfos.Where(i => i.GetValue(obj) is Section))
            {
                string sectionName = section.GetType().ToString();
                data.Sections.AddSection(sectionName);
                //data[sectionName].
                foreach (var pi in GetPropertyInfosInSection(_propertyInfos, sectionName))
                {
                    object value = pi.GetValue(obj);
                    data[sectionName].AddKey(pi.Name, value.ToString());
                }
            }
            return data;
        }

        private IEnumerable<PropertyInfo> GetPropertyInfosInSection(
            IEnumerable<PropertyInfo> propertyInfos, 
            string sectionName)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                var sa = propertyInfo.GetCustomAttribute<SectionAttribute>(true);
                string name = sa.GetType().ToString();
                if (sa != null && string.Equals(sectionName, name))
                    yield return propertyInfo;
            }
        }

        public void Serialize(string path, IniData data)
        {
            parser.WriteFile(path, data);
        }

        public object Deserialize(string path)
        {
            var data = parser.ReadFile(path);
            return Deserialize(data);
        }

        public object Deserialize(IniData data)
        {
            object obj = Activator.CreateInstance(_type);
            foreach (var section in data.Sections)
            {
                //section.
                foreach (var key in section.Keys)
                {
                    var pi = _propertyInfos.Where(i => i.Name == key.KeyName).FirstOrDefault();
                    object value = Convert.ChangeType(key.Value, _type);
                    pi.SetValue(obj, value);
                }
            }
            return obj;
        }
    }
}