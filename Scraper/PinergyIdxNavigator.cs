using System;
using Scraper.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper
{
    public class PinergyIdxNavigator : ICrawler<TempListingDataItem>
    {
        IEnumerable<CrawlerPage<TempListingDataItem>> ICrawler<TempListingDataItem>.CrawlWeb(string startingUrl)
        {
            if (startingUrl == null)
                throw new ArgumentNullException(nameof(startingUrl));

            if (string.IsNullOrWhiteSpace(startingUrl))
                throw new ArgumentOutOfRangeException(nameof(startingUrl));


                throw new System.NotImplementedException();
        }

        Task<IEnumerable<CrawlerPage<TempListingDataItem>>> ICrawler<TempListingDataItem>.CrawlWebAsync(string startingUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}
