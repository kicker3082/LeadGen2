using System.Collections.Generic;

namespace Scraper.Core
{
    public class CrawlerPage<T>
    {
        public string PageUrl { get; set; }
        public string HTMLContent { get; set; }
        public IEnumerable<string> LinksInPage { get; set; }
        public IEnumerable<T> DataItemsInPage { get; set; }
        public int? HttpErrorCode { get; set; }
    }
}
