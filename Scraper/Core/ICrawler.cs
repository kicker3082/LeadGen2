using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Core
{
    /// <summary>
    /// Implementations of this interface recursively crawl a graph of web pages to extract links and
    /// data items from the page.
    /// </summary>
    public interface ICrawler<T>
    {
        /// <summary>
        /// Asyncronously crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <typeparam name="T">The type of the Data Item that the crawler extracts from the crawled pages.</typeparam>
        /// <returns>A <see cref="Task{TResult}"/> that resolves to batches of <see cref="CrawlerPage{T}"/>,
        /// each containing the discovered links and the <typeparamref name="T">Data Items</typeparamref> in each page.</returns>
        Task<IEnumerable<CrawlerPage<T>>> CrawlWebAsync(string startingUrl);
        /// <summary>
        /// Crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <typeparam name="T">The type of the Data Item that the crawler extracts from the crawled pages.</typeparam>
        /// <returns>An enumeration of <see cref="CrawlerPage{T}"/>, each containing the discovered links and the
        /// <typeparamref name="T">Data Items</typeparamref> in each page.</returns>
        IEnumerable<CrawlerPage<T>> CrawlWeb(string startingUrl);
    }
}
