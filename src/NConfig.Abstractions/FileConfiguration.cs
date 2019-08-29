using System;
using System.IO;

namespace NConfig
{
    public abstract class FileConfiguration<TConfig>
    {
        public FileConfiguration(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"The file path for type '{nameof(TConfig)}' was not specified.");
            FilePath = path;
        }

        public string FilePath { get; protected set; }
        public string FullPath => Path.GetFullPath(FilePath);
        public string FileName => Path.GetFileName(FilePath);
        public string FileExtension => Path.GetExtension(FilePath);
    }
}
