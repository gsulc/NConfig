#if !NETSTANDARD1_3

using System;
using System.IO;

namespace NConfig
{
    public class FileWatcher : IDisposable
    {
        public event EventHandler<FileChangedEventArgs> Changed;
        public event EventHandler<FileReloadErrorEventArgs> FileReloadError;

        private readonly FileSystemWatcher _watcher;
        private readonly FileConfiguration<object> _fileConfiguration;
        private bool _reloadWhenChanged = false;

        public FileWatcher(FileConfiguration<object> fileConfiguration)
        {
            _fileConfiguration = fileConfiguration ?? throw new ArgumentException(nameof(fileConfiguration));
            var path = fileConfiguration.FullPath;
            var directory = Path.GetDirectoryName(path);
            _watcher = new FileSystemWatcher()
            {
                Path = directory,
                Filter = Path.GetFileName(path),
                NotifyFilter = NotifyFilters.LastWrite
            };

            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Watces the config file and tries to reload the file when it changes. 
        /// Raises the FileReloadError event if there was an error reloading.
        /// </summary>
        public bool ReloadWhenChanged
        {
            get
            {
                return _reloadWhenChanged;
            }
            set
            {
                if (_reloadWhenChanged == value)
                    return;

                _reloadWhenChanged = value;
                if (_reloadWhenChanged)
                    _watcher.Changed += Reload;
                else
                    _watcher.Changed -= Reload;
            }
        }

        private void Reload(object sender, FileSystemEventArgs args)
        {
            try
            {
                var config = _fileConfiguration as IConfiguration<object>;
                config.Load();
            }
            catch (Exception e)
            {
                FileReloadError?.Invoke(this, new FileReloadErrorEventArgs(_fileConfiguration, e));
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Changed?.Invoke(this, new FileChangedEventArgs(_fileConfiguration));
        }

        #region IDisposable
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _watcher?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public class FileChangedEventArgs : EventArgs
    {
        public FileChangedEventArgs(FileConfiguration<object> fileConfiguration)
            : base()
        {
            FileConfiguration = fileConfiguration;
        }

        public FileConfiguration<object> FileConfiguration { get; private set; }
    }

    public class FileReloadErrorEventArgs : EventArgs
    {
        public FileReloadErrorEventArgs(FileConfiguration<object> fileConfiguration, Exception exception) 
            : base()
        {
            FileConfiguration = fileConfiguration;
            Exception = exception;
        }

        public FileConfiguration<object> FileConfiguration { get; private set; }
        public Exception Exception { get; private set; }
    }
}

#endif
