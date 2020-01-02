using System;
using System.Net;

namespace Creative.System.Core.Web
{
    public interface IWebPage
    {

            Uri Uri { get; set; }
            CookieContainer Cookies { get; set; }
            string ViewState { get; set; }
            string EventData { get; set; }
            WebHeaderCollection Headers { get; set; }
    }
}
