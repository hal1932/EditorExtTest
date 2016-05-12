using System;
using System.IO;
using System.Net;

namespace HlLib.Unity.Http
{
    public class HttpResponse : IDisposable
    {
        public string ContentType { get { return _response.ContentType; } }
        public long ContentLength { get { return _response.ContentLength; } }

        internal HttpResponse(WebResponse response)
        {
            _response = response;
        }

        public Stream GetStream()
        {
            return _response.GetResponseStream();
        }

        #region IDisposable
        ~HttpResponse()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            lock (_disposingLock)
            {
                if (_disposed)
                {
                    return;
                }
                _disposed = true;

                if (_response != null)
                {
                    _response.Close();
                    _response = null;
                }
            }
        }

        private bool _disposed;
        private object _disposingLock = new object();
        #endregion

        private WebResponse _response;
    }
}
