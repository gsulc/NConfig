using NConfig.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    public class ExampleConfig
    {
        [IniSection("Ultimate")]
        public Ultimate Ultimate { get; set; }

        
        public RythmSection RythmSection { get; set; }

        public double ExampleDouble { get; set; }
    }
    
    public class Ultimate
    {
        public int TheAnswer { get; set; }
    }

    [IniSection("The Rythm Section")]
    public class RythmSection
    {
        public string TimeSignature { get; set; }
        public bool FeelsGood { get; set; }
    }

    public class Question
    {
        public bool WorkedOut { get; set; }
        public string Scientist1Name { get; set; }
        public string Scientist2Name { get; set; }
        //public List<Mouse> Scientists { get; set; }
    }

    public class Mouse
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Age { get; set; }
    }
}
