using System.IO;

namespace NConfig.Abstractions
{
    public abstract class FileConfiguration<TConfig> : IConfiguration<TConfig>
    {
        private readonly string _filePath;

        public FileConfiguration(string path)
        {
            _filePath = path;
        }

        public string FilePath => _filePath;
        public string FullPath => Path.GetFullPath(_filePath); // Requires netstandard1.3+
        public string FileName => Path.GetFileName(_filePath);
        public string FileExtension => Path.GetExtension(_filePath);

        public abstract TConfig Load();
        public abstract void Save(TConfig value);
    }
}
