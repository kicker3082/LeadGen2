using System.Collections.Generic;
using System.Threading;

namespace Scraper.Core
{
    /// <summary>
    /// Implementations of this interface recursively crawl a graph of web pages to extract links and
    /// data items from the page.
    /// </summary>
    public interface ICrawler
    {
        /// <summary>
        /// Asyncronously crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <param name="cancellationToken">A token that will </param>
        /// <returns>An enumeration of <see cref="CrawlerPageNode"/> that asynchronously stream back to the caller,
        /// each containing the discovered links in each page.</returns>
        IAsyncEnumerable<CrawlerPageNode> CrawlWebAsync(string startingUrl, CancellationToken cancellationToken);
        /// <summary>
        /// Crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <returns>An enumeration of <see cref="CrawlerPage{T}"/>, each containing the discovered links in each page.</returns>
        IEnumerable<CrawlerPageNode> CrawlWeb(string startingUrl);
    }
}
