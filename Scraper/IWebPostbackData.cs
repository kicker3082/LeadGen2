using System;
using System.Collections.Specialized;
using System.Net;

namespace Scraper
{
    public interface IWebPostbackData
    {
        Uri PostbackUrl { get; }
        CookieContainer Cookies { get; }
        string ViewState { get; }
        string EventData { get; }
        NameValueCollection Data { get; }
    }
}
