using System;
using System.Net;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Scraper
{
    public class WebPage : IDisposable
    {
        public static WebPage FromHtml(string html, string url)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var doc = parser.ParseDocument(html);

            var page = new WebPage(doc)
            {
                Uri = new Uri(url),
            };

            return page;
        }
        public WebPage(IHtmlDocument document, HttpWebRequest fromRequest) : this(document)
        {
            if (fromRequest != null)
                Headers = fromRequest.Headers;
        }

        public WebPage(IHtmlDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));

            Cookies = new CookieContainer();
            var viewStateElement = document.QuerySelector(@"#__VIEWSTATE");
            if (viewStateElement != null)
                ViewState = viewStateElement.GetAttribute("value", string.Empty);
            var eventElement = document.QuerySelector(@"#__EVENTVALIDATION");
            if (eventElement != null)
                EventData = eventElement.GetAttribute("value", string.Empty);
        }

        public Uri Uri { get; set; }
        public IHtmlDocument Document { get; private set; }

        public CookieContainer Cookies { get; set; }
        public string ViewState { get; set; }
        public string EventData { get; set; }
        public WebHeaderCollection Headers { get; set; }

        #region IDisposable

        // ReSharper disable ArrangeTypeMemberModifiers

        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        private bool __alreadyDisposed;
#pragma warning restore IDE1006 // Naming Styles

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isInFinalizer)
        {
            if (__alreadyDisposed)
                return;

            // If this is being called from the finalizer, do not attempt to
            // dispose of managed objects. Only unmanaged resources should be
            // released.
            if (!isInFinalizer)
                DisposeManagedObjects();

            ReleaseUnmanagedResources();

            __alreadyDisposed = true;
        }

        private void DisposeManagedObjects()
        {
            // Dispose managed objects here
            Cookies = null;
            ViewState = null;
            EventData = null;
            Headers = null;
            Document = null;
        }

        private void ReleaseUnmanagedResources()
        {
            // Release unmanaged resources here
        }

        ~WebPage()
        {
            Dispose(true);
        }

        // ReSharper restore ArrangeTypeMemberModifiers

        #endregion

    }
}
