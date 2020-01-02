using System.Collections.Generic;

namespace Scraper.Core
{
    public class CrawlerPageNode
    {
        public string PageUrl { get; set; }
        public string HTMLContent { get; set; }
        public IEnumerable<string> LinksInPage { get; set; }
        public int? HttpErrorCode { get; set; }
        public CrawlerPageNode Parent { get; set; }
    }
}
