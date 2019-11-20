using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Creative.System.Core;
using Scraper.Core;

namespace Scraper
{
    /// <summary>
    /// Crawls the web by starting at a root url, discovering further pages to crawl within the content
    /// of the page and continues crawling until the entire graph is completed.
    /// </summary>
    /// <remarks>
    /// Crawling is implemented by iterating over a queue to which newly discovered links for further
    /// crawling are added. This should be more efficient than recursively crawling and is much easier to
    /// implement asynchronously.
    ///
    /// This is the flag indicating that we should stop waiting for more links to appear in the queue.
    /// It is set after any links on the current page are added to the queue. Because there
    /// is only one queue, if there are no more items on it after the current page is parsed and
    /// (zero) discovered links added, there will never be any more links and we're done.
    ///
    /// Even when considering a graph with many leaf nodes - there will always be items in
    /// the queue if any page has not been yet downloaded and parsed, so the flag will be 
    /// remain false. The only time that the queue is truly empty (after the root page has 
    /// been parsed and discovered links added to the queue), is when all lead pages have been 
    /// parsed and contain no additional links. When the last one of these is parsed and 
    /// no links discovered, then we're done.
    /// </remarks>
    public class QueueCrawler : ICrawler
    {
        private readonly IPageLoader _pageLoader;
        readonly AsyncQueue<(string link, CrawlerPageNode parent)> _links;

        public QueueCrawler(IPageLoader pageLoader)
        {
            _links = new AsyncQueue<(string, CrawlerPageNode)>();
            _pageLoader = pageLoader;
        }

        /// <summary>
        /// Crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <returns>An enumeration of <see cref="CrawlerPageNode"/>, each containing the discovered links in the parsed HTML
        /// <exception cref="ArgumentNullException">Throw when the <paramref name="startingUrl"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw when the <paramref name="startingUrl"/> is empty or whitespace.</exception>
        /// <remarks>
        /// User Requirement:
        ///     1 Crawl the website from a starting Url
        ///     1.1 Retrieve the WebResponse
        ///     1.1.1 Interperet web response errors (place in retry queue); or
        ///     1.1.2 Parse the HTML content for navigation links
        ///     1.2 Construct a graph of the HTML pages
        ///     1.2.1 Iterate over discovered navigation links and recursively construct sub-graphs
        ///     1.2.2 Combine the sub-graphs into a complete graph with the starting URL as the root node
        ///     
        /// The resulting set is flat in terms of structure. The Parent property of each 
        /// item maintains the hierarchal relationships.
        /// </remarks>
        IEnumerable<CrawlerPageNode> ICrawler.CrawlWeb(string startingUrl)
        {
            CheckUrl(startingUrl);

            // TODO: Make this not suck so much
            // How can we iterate over an IAsyncEnumerable synchronously but use yield return instead of
            // collecting all of the results before returning?
            // Maybe by using the asynchronous iterator and blocking on it inside a while (with yield return inside)?

            var pageResult = new List<CrawlerPageNode>();
            return Task.Run(async () =>
                {
                    await foreach (var page in ((ICrawler)this).CrawlWebAsync(startingUrl))
                        pageResult.Add(page);
                    return pageResult;
                }
            ).Result;
        }

        /// <summary>
        /// Crawl the graph to discover linked web pages and extract the data items from
        /// each page.
        /// </summary>
        /// <param name="startingUrl">A url to begin crawling.</param>
        /// <returns>A <seealso cref="Task{TResult}"/> with a result that is an enumeration of <see cref="CrawlerPageNode"/>, 
        /// each containing the discovered links in the parsed HTML.
        /// <exception cref="ArgumentNullException">Throw when the <paramref name="startingUrl"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw when the <paramref name="startingUrl"/> is empty or whitespace.</exception>
        /// <remarks>
        /// User Requirement:
        ///     1 Crawl the website from a starting Url
        ///     1.1 Retrieve the WebResponse
        ///     1.1.1 Interperet web response errors (place in retry queue); or
        ///     1.1.2 Parse the HTML content for navigation links
        ///     1.2 Construct a graph of the HTML pages
        ///     1.2.1 Iterate over discovered navigation links and recursively construct sub-graphs
        ///     1.2.2 Combine the sub-graphs into a complete graph with the starting URL as the root node
        ///     
        /// The resulting set is flat in terms of structure. The Parent property of each 
        /// item maintains the hierarchal relationships.
        /// </remarks>

        async IAsyncEnumerable<CrawlerPageNode> ICrawler.CrawlWebAsync(string startingUrl,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            CheckUrl(startingUrl);

            var firstPage = await _pageLoader.LoadPageAsync(startingUrl, null);

            // If the page is null it has already been crawled, don't return it and don't look for links
            if (firstPage == null)
                yield break;

            foreach (var lnk in firstPage.LinksInPage)
                _links.Enqueue((lnk, firstPage));
            yield return firstPage;

            if (_links.Count != 0)
            {
                // NOTE: Iterating over the _links queue will automatically dequeue each item
                // because AsyncQueue is a wrapper for a BufferBlock.
                await foreach (var (link, parent) in _links.WithCancellation(cancellationToken))
                {
                    var page = await _pageLoader.LoadPageAsync(link, parent);
                    // If the page is null it has already been crawled, don't return it and don't look for links
                    if (page == null)
                    {
                        if (_links.Count == 0)
                            break;

                        continue;
                    }

                    foreach (var lnk in page.LinksInPage)
                        _links.Enqueue((lnk, page));

                    yield return page;
                    // Test this after enqueueing any items

                    // If the queue is empty AND this page has no nav links
                    // then we're done

                    if (_links.Count == 0)
                        break;
                }
            }
        }


        /// <summary>
        /// Determine if the starting Url meets the criteria for crawling
        /// </summary>
        /// <param name="startingUrl"></param>
        void CheckUrl(string startingUrl)
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
        }
    }
}
