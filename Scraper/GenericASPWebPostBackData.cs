using System;
using System.Collections.Specialized;
using System.Net;

namespace Scraper
{
    public class GenericASPWebPostBackData : IWebPostbackData
    {
        public Uri PostbackUrl { get; set; }
        public CookieContainer Cookies { get; set; }
        public string ViewState { get; set; }
        public string EventData { get; set; }
        public NameValueCollection Data { get; set; }

        public static IWebPostbackData FromPage(WebPage page, string url, NameValueCollection values = null)
        {
            var postData = new GenericASPWebPostBackData
            {
                Cookies = page.Cookies,
                EventData = page.EventData,
                ViewState = page.ViewState,
                PostbackUrl = new Uri(url),
                Data = new NameValueCollection(),
            };

            if (page.ViewState != null)
                postData.Data.Add("__VIEWSTATE", page.ViewState);
            if (page.EventData != null)
                postData.Data.Add("__EVENTVALIDATION", page.EventData);
            if (values != null)
                postData.Data.Add(values);

            return postData;
        }
    }
}
