using System;
using System.IO;

namespace NConfig.Abstractions
{
    public abstract class StreamConfiguration<TConfig> : IDisposable
    {
        public StreamConfiguration(Stream stream)
        {
            Stream = stream;
        }

        protected Stream Stream { get; private set; }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stream.Dispose();
                }

                Stream = null;

                _disposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
