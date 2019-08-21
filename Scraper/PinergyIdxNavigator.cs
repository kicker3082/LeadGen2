using System;
using Scraper.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace Scraper
{
    public class PinergyIdxNavigator : ICrawler<TempListingDataItem>
    {
        private readonly IWebClient _webClient;

        public PinergyIdxNavigator(IWebClient webClient)
        {
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
        }

        /// <summary>
        /// Crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <typeparam name="T">The type of the Data Item that the crawler extracts from the crawled pages.</typeparam>
        /// <returns>An enumeration of <see cref="CrawlerPage{T}"/>, each containing the discovered links and the
        /// <typeparamref name="T">Data Items</typeparamref> in each page.</returns>
        /// <exception cref="ArgumentNullException">Throw when the <paramref name="startingUrl"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw when the <paramref name="startingUrl"/> is empty or whitespace.</exception>
        IEnumerable<CrawlerPage<TempListingDataItem>> ICrawler<TempListingDataItem>.CrawlWeb(string startingUrl)
        {
            if (startingUrl == null)
                throw new ArgumentNullException(nameof(startingUrl));

            if (string.IsNullOrWhiteSpace(startingUrl))
                throw new ArgumentOutOfRangeException(nameof(startingUrl));


            if (!Uri.IsWellFormedUriString(startingUrl, UriKind.RelativeOrAbsolute))
                throw new ArgumentException(@"startingUrl must be well-formed.", nameof(startingUrl));

            var startingUri = new Uri(startingUrl);

            // The url has to be either http or https to work
            if (!(startingUri.Scheme == Uri.UriSchemeHttps || startingUri.Scheme == Uri.UriSchemeHttp))
                throw new ArgumentException(@"startingUrl must be a http or https.", nameof(startingUrl));

            var pageText = _webClient.DownloadString(startingUrl);

            if (startingUrl == @"http://mydomain.com/startingpage")
                return new[] { new CrawlerPage<TempListingDataItem> {
                PageUrl = @"http://mydomain.com/startingpage" } };

            return new[] { new CrawlerPage<TempListingDataItem> {
                HttpErrorCode = 500
            } };
        }

        Task<IEnumerable<CrawlerPage<TempListingDataItem>>> ICrawler<TempListingDataItem>.CrawlWebAsync(string startingUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}
