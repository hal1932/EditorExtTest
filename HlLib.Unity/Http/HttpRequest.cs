using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace HlLib.Unity.Http
{
    public enum HttpMethod
    {
        Get,
        Post,
    }

    public class HttpRequest : IDisposable
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public int Timeout
        {
            get { return _request.Timeout; }
            set { _request.Timeout = value; }
        }

        public HttpResponse LastResponse { get; private set; }

        public HttpRequest(string uri, HttpMethod method)
        {
            _request = (HttpWebRequest)WebRequest.Create(uri);
            _request.Method = (method == HttpMethod.Get) ? "GET" : "POST";
        }

        public Stream GetRequestStream()
        {
            return _request.GetRequestStream();
        }

        public HttpResponse Send(HttpContent content = null)
        {
            ApplyHeaders();
            ApplyContent(content);

            LastResponse = new HttpResponse(_request.GetResponse());
            return LastResponse;
        }

        public ManualResetEvent SendAsync(HttpContent content = null, Action<HttpResponse> onResponse = null)
        {
            ApplyHeaders();
            ApplyContent(content);

            _sendEvent = new ManualResetEvent(false);

            var result = _request.BeginGetResponse(ar =>
            {
                LastResponse = new HttpResponse(_request.EndGetResponse(ar));
                if (onResponse != null)
                {
                    onResponse(LastResponse);
                }
                _sendEvent.Set();
            }, null);

            if (Timeout > 0)
            {
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, (state, timeout) =>
                {
                    if (timeout)
                    {
                        var request = (WebRequest)state;
                        if (request != null)
                        {
                            request.Abort();
                        }
                        if (onResponse != null)
                        {
                            onResponse(null);
                        }
                        _sendEvent.Set();
                    }
                }, _request, TimeSpan.FromMilliseconds(Timeout), true);
            }

            return _sendEvent;
        }

        private void ApplyHeaders()
        {
            _request.Headers.Clear();

            foreach (var header in Headers)
            {
                switch (header.Key)
                {
                    case "Accept": _request.Accept = header.Value; break;
                    case "Connection": _request.Connection = header.Value; break;
                    case "Content-Length": _request.ContentLength = long.Parse(header.Value); break;
                    case "Content-Type": _request.ContentType = header.Value; break;
                    case "Expect": _request.Expect = header.Value; break;
                    case "If-Modified-Since": _request.IfModifiedSince = DateTime.Parse(header.Value); break;
                    case "Range": throw new ArgumentException("Range header is not supported");
                    case "Referer": _request.Referer = header.Value; break;
                    case "Transfer-Encoding": _request.TransferEncoding = header.Value; break;
                    case "User-Agent": _request.UserAgent = header.Value; break;
                    default:
                        _request.Headers.Add(header.Key, header.Value);
                        break;
                }
            }
        }

        private void ApplyContent(HttpContent content)
        {
            if (content != null)
            {
                _request.ContentType = content.ContentType;
                _request.ContentLength = content.Data.Length;

                var data = content.Data;
                using (var stream = _request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        #region IDisposable
        ~HttpRequest()
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

                if (LastResponse != null)
                {
                    LastResponse.Dispose();
                    LastResponse = null;
                }
                if (_sendEvent != null)
                {
                    _sendEvent.Close();
                    _sendEvent = null;
                }
            }
        }

        private bool _disposed;
        private object _disposingLock = new object();
        #endregion

        private HttpWebRequest _request;
        private ManualResetEvent _sendEvent;
    }
}
