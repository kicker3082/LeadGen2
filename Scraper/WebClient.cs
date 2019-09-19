using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Creative.System.Core;
using HtmlAgilityPack;
using Scraper.Core;

namespace Scraper
{
    public class WebClient : IWebClient, IDisposable
    {
        class CookieAwareWebClient : global::System.Net.WebClient
        {
            readonly CookieContainer _container;
            public Uri UriAfterRedirect { get; private set; }

            public CookieAwareWebClient() : this(new CookieContainer()) { }
            public CookieAwareWebClient(CookieContainer cookies)
            {
                _container = cookies;
            }

            /// <summary>
            /// Returns a <see cref="T:System.Net.WebRequest"/> object for the specified resource.
            /// </summary>
            /// <returns>
            /// A new <see cref="T:System.Net.WebRequest"/> object for the specified resource.
            /// </returns>
            /// <param name="address">A <see cref="T:System.Uri"/> that identifies the resource to request.</param>
            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);

                if (request == null)
                    return null;

                var httpRequest = request as HttpWebRequest;
                if (httpRequest == null)
                    return request;

                Request = httpRequest;
                Request.CookieContainer = _container;

                return request;
            }

            protected override WebResponse GetWebResponse(WebRequest request)
            {
                var response = base.GetWebResponse(request);

                Debug.Assert(response != null, "response != null");

                //// This will add whatever cookies are returned from the client to the 
                //// cookies to be submitted the next time this same uri is requested.

                //// They can be read out and carried forward to the request for a different submission
                //var returnedCookies = response.Headers[HttpResponseHeader.SetCookie];
                //_container.SetCookies(request.RequestUri, returnedCookies);

                UriAfterRedirect = response.ResponseUri;

                return response;
            }

            public HttpWebRequest Request { get; private set; }
        }

        readonly CookieAwareWebClient _cookieAwareWebClient;

        public WebPage GetPageWithAdditionalCookies(Uri uri, CookieCollection cookies)
        {
            var container = new CookieContainer();
            container.Add(cookies);

            var content = _cookieAwareWebClient.DownloadString(uri);
            var uriOfResponse = _cookieAwareWebClient.UriAfterRedirect;
            var page = MakeWebPageFromHtml(content, _cookieAwareWebClient.ResponseHeaders[HttpResponseHeader.SetCookie], uriOfResponse, _cookieAwareWebClient.Request);
            return page;
        }

        readonly IFileSystem _file;

        public WebClient(IFileSystem file)
        {
            _file = file;
            _cookieAwareWebClient = new CookieAwareWebClient();
        }

        public void DownloadFile(Uri uri, string filename)
        {
            var tempFile = Path.GetTempFileName();
            _cookieAwareWebClient.DownloadFile(uri, tempFile);
            _file.Copy(tempFile, filename);
        }

        public WebPage DownloadPageIntoFile(Uri uri, string filename)
        {
            var content = _cookieAwareWebClient.DownloadString(uri);
            var encoding = _cookieAwareWebClient.ResponseHeaders[HttpResponseHeader.ContentEncoding];
            _file.WriteAllText(filename, content, Encoding.GetEncoding(encoding));

            var page = MakeWebPageFromHtml(content, _cookieAwareWebClient.ResponseHeaders[HttpResponseHeader.SetCookie], uri, _cookieAwareWebClient.Request);
            return page;
        }

        public virtual async Task<WebPage> GetPageAsync(Uri uri)
        {
            var content = await _cookieAwareWebClient.DownloadStringTaskAsync(uri);
            var uriOfResponse = _cookieAwareWebClient.UriAfterRedirect ?? uri;
            var page = MakeWebPageFromHtml(content,
                // The WebClient being disposed should not affect the response headers.
                _cookieAwareWebClient.ResponseHeaders[HttpResponseHeader.SetCookie], uriOfResponse, _cookieAwareWebClient.Request);
            return page;
        }
        public virtual WebPage GetPage(Uri uri)

        {
            var content = _cookieAwareWebClient.DownloadString(uri);
            var uriOfResponse = _cookieAwareWebClient.UriAfterRedirect;
            var page = MakeWebPageFromHtml(content, _cookieAwareWebClient.ResponseHeaders[HttpResponseHeader.SetCookie], uriOfResponse, _cookieAwareWebClient.Request);
            return page;
        }

        public string PostRaw(Uri uri, IWebPostbackData postbackData)
        {
            return PostRaw(uri, postbackData, null, false);
        }

        public IWebClient WithAdditionalCookies(CookieCollection cookies)
        {
            throw new NotImplementedException();
            //_cookieAwareWebClient.Request.CookieContainer.SetCookies(uri, cookies.Get);
        }

        public string PostRaw(Uri uri, IWebPostbackData postbackData, IDictionary<string, string> additionalHeaders, bool keepAlive)
        {
            var reqparm = postbackData.Data;
            var headers = _cookieAwareWebClient.Headers;

            _cookieAwareWebClient.Request.KeepAlive = keepAlive;

            if (additionalHeaders != null)
                foreach (var item in additionalHeaders)
                    AddHeaderItemIfNotPresent(headers, item.Key, item.Value);

            // These are for debugging
            // ReSharper disable UnusedVariable
            var formPostValues = reqparm.AllKeys.SelectMany(name => reqparm.GetValues(name), (key, value) => new { key, value }).ToArray();
            var headerValues = headers.AllKeys.SelectMany(name => headers.GetValues(name), (key, value) => new { key, value }).ToArray();
            // ReSharper restore UnusedVariable


            var responseBytes = _cookieAwareWebClient.UploadValues(uri, reqparm);
            var content = Encoding.UTF8.GetString(responseBytes);
            return content;
        }

        void AddHeaderItemIfNotPresent(NameValueCollection headers, string keyToAdd, string valueToAdd)
        {
            var headerValues = headers.AllKeys.SelectMany(headers.GetValues, (key, value) => new { Key = key, Value = value }).ToArray();

            var referer = headerValues.FirstOrDefault(h => h.Key == keyToAdd)?.Value;
            if (referer == null)
                _cookieAwareWebClient.Headers.Add(keyToAdd, valueToAdd);
        }


        public WebPage Post(Uri uri, IWebPostbackData postbackData)
        {
            var content = PostRaw(uri, postbackData, null, true);
            var uriOfResponse = _cookieAwareWebClient.UriAfterRedirect;
            var page = MakeWebPageFromHtml(content, _cookieAwareWebClient.ResponseHeaders[HttpResponseHeader.SetCookie], uriOfResponse, _cookieAwareWebClient.Request);
            return page;
        }

        static WebPage MakeWebPageFromHtml(string html, string cookies, Uri uri, HttpWebRequest request)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var page = new WebPage(doc, request);
            if (cookies != null)
                page.Cookies.SetCookies(uri, cookies);

            page.Uri = uri;

            return page;
        }

        #region IDisposable

        private bool _alreadyDisposed;

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isInFinalizer)
        {
            if (_alreadyDisposed)
                return;

            // If this is being called from the finalizer, do not attempt to
            // dispose of managed objects. Only unmanaged resources should be
            // released.
            if (!isInFinalizer)
                DisposeManagedObjects();

            ReleaseUnmanagedResources();

            _alreadyDisposed = true;
        }

        private void DisposeManagedObjects()
        {
            // Dispose managed objects here
            _cookieAwareWebClient?.Dispose();
        }

        private void ReleaseUnmanagedResources()
        {
            // Release unmanaged resources here
        }

        string IWebClient.DownloadString(string address)
        {
            throw new NotImplementedException();
        }

        string IWebClient.DownloadString(Uri address)
        {
            throw new NotImplementedException();
        }

        Task<string> IWebClient.DownloadStringTaskAsync(string address)
        {
            throw new NotImplementedException();
        }

        Task<string> IWebClient.DownloadStringTaskAsync(Uri address)
        {
            throw new NotImplementedException();
        }

        ~WebClient()
        {
            Dispose(true);
        }

        #endregion

    }
}
