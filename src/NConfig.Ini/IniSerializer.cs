using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using MadMilkman.Ini;

namespace NConfig.Ini
{
    public class IniSerializer
    {
        private static readonly FileIniDataParser parser = new FileIniDataParser();
        private readonly Type _type;
        private readonly PropertyInfo[] _propertyInfos;
        private object _object;

        public IniSerializer(Type type)
        {
            _type = type;
            _propertyInfos = _type.GetProperties();
        }

        public void Serialize(object obj, string path)
        {
            _object = obj;
            var iniData = ConstructIniData(obj);
            Serialize(iniData, path);
        }

        public IniData ConstructIniData(object obj)
        {
            throw new NotImplementedException();
            //IniData data = new IniData();
            //foreach (var section in _propertyInfos.Where(i => i.GetValue(obj) is IniSection))
            //{
            //    string sectionName = section.GetType().ToString();
            //    data.Sections.AddSection(sectionName);
            //    //data[sectionName].
            //    foreach (var pi in GetPropertyInfosInSection(_propertyInfos, sectionName))
            //    {
            //        object value = pi.GetValue(obj);
            //        data[sectionName].AddKey(pi.Name, value.ToString());
            //    }
            //}
            //return data;
        }

        private IEnumerable<PropertyInfo> GetPropertyInfosInSection(
            IEnumerable<PropertyInfo> propertyInfos,
            string sectionName)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                var sa = propertyInfo.GetCustomAttribute<IniSectionAttribute>(true);
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
            _object = Activator.CreateInstance(_type);
            PopulateGlobalData(data);
            PopulateSectionData(data);
            return _object;
        }

        private void PopulateGlobalData(IniData data)
        {
            foreach (var key in data.Global)
            {
                var keyInfo = _propertyInfos.Where(i => string.Equals(i.Name, key.KeyName)).First();
                var keyType = keyInfo.PropertyType;
                object keyObject = Activator.CreateInstance(keyType);
                object value = Convert.ChangeType(key.Value, keyType);
                keyInfo.SetValue(_object, value);
            }
        }

        private void PopulateSectionData(IniData data)
        {
            foreach (var section in data.Sections)
            {
                var sectionInfo = GetSectionPropertInfo(section.SectionName);
                var sectionType = sectionInfo.PropertyType;
                object sectionObject = section.Deserialize(sectionType);
                sectionInfo.SetValue(_object, sectionObject);
            }
        }

        public PropertyInfo GetSectionPropertInfo(string sectionName)
        {
            foreach (var propertyInfo in _propertyInfos)
            {
                var sa = propertyInfo.GetCustomAttribute<IniSectionAttribute>();
                if (sa == null)
                    sa = propertyInfo.PropertyType.GetCustomAttribute<IniSectionAttribute>();
                string name = sa.Name;
                if (sa != null && string.Equals(sectionName, name))
                    return propertyInfo;
            }
            return null;
        }
    }
}