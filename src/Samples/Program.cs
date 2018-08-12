using NConfig.Abstractions;
using NConfig.Ini;
using System;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration<ExampleConfig> configuration = 
                new IniFileConfiguration<ExampleConfig>("Example.ini");
            ExampleConfig config = configuration.Load();
            Console.WriteLine(
                "The answer to the ultimate question of life, the universe and everything is {0}.",
                config.Ultimate.TheAnswer);
            Console.WriteLine("Example double is set to {0}", config.ExampleDouble);
            Console.Write("The rythm section thinks {0} feels {1}.", 
                config.RythmSection.TimeSignature, config.RythmSection.FeelsGood ? "good" : "bad");
            Console.ReadKey();
            var random = new Random();
            int newNumTimesOneHundred = (int)(random.NextDouble() * 10000);
            config.ExampleDouble = (newNumTimesOneHundred / 100.0);
            configuration.Save(config);
        }
    }
}
