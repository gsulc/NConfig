using System;

namespace NConfig.Ini
{
    public class SectionAttribute : Attribute
    {
        private readonly Section _section;
        public SectionAttribute(Section section)
        {
            _section = section;
        }

        public Section Section => _section;
    }
}
