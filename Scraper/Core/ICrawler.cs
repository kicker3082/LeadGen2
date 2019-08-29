using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <returns>A <see cref="Task{TResult}"/> that resolves to batches of <see cref="CrawlerPageNode"/>,
        /// each containing the discovered links and the <typeparamref name="T">Data Items</typeparamref> in each page.</returns>
        Task<IEnumerable<CrawlerPageNode>> CrawlWebAsync(string startingUrl);
        /// <summary>
        /// Crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <returns>An enumeration of <see cref="CrawlerPage{T}"/>, each containing the discovered links in each page.</returns>
        IEnumerable<CrawlerPageNode> CrawlWeb(string startingUrl);
    }
}
