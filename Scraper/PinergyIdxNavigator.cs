using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Scraper.Core;

namespace Scraper
{
    public class PinergyIdxCrawler : ICrawler
    {
        private readonly IWebClient _webClient;
        private readonly INavigationLinkParser _navigationLinkParser;
        AsyncQueue<(string link, CrawlerPageNode parent)> _links;
        bool _isDone;

        public PinergyIdxCrawler(
            IWebClient webClient,
            INavigationLinkParser navigationLinkParser)
        {
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
            _navigationLinkParser = navigationLinkParser ?? throw new ArgumentNullException(nameof(navigationLinkParser));

            _links = new AsyncQueue<(string, CrawlerPageNode)>();
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
        ///     1 Crawl the PinergyIdx website from a starting Url
        ///     1.1 Retrieve the WebResponse
        ///     1.1.1 Interperet web response errors (place in retry queue); or
        ///     1.1.2 Parse the HTML content for navigation links
        ///     1.2 Construct a graph of the HTML pages
        ///     1.2.1 Iterate over discovered navigation links and recursively construct sub-graphs
        ///     1.2.2 Combine the sub-graphs into a complete graph with the starting URL as the root node
        ///     
        /// The resulting set is flat in terms of structure, but the Parent and Children properties of each 
        /// item maintains the hierarchal relationships.
        /// </remarks>
        IEnumerable<CrawlerPageNode> ICrawler.CrawlWeb(string startingUrl)
        {
            CheckUrl(startingUrl);
            return (IEnumerable<CrawlerPageNode>)((ICrawler)this).CrawlWebAsync(startingUrl);
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
        ///     1 Crawl the PinergyIdx website from a starting Url
        ///     1.1 Retrieve the WebResponse
        ///     1.1.1 Interperet web response errors (place in retry queue); or
        ///     1.1.2 Parse the HTML content for navigation links
        ///     1.2 Construct a graph of the HTML pages
        ///     1.2.1 Iterate over discovered navigation links and recursively construct sub-graphs
        ///     1.2.2 Combine the sub-graphs into a complete graph with the starting URL as the root node
        ///     
        /// The resulting set is flat in terms of structure, but the Parent and Children properties of each 
        /// item maintains the hierarchal relationships.
        /// </remarks>

        async IAsyncEnumerable<CrawlerPageNode> ICrawler.CrawlWebAsync(string startingUrl, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            CheckUrl(startingUrl);

            cancellationToken.ThrowIfCancellationRequested();

            var firstPage = await LoadPageAsync(startingUrl, null);
            yield return firstPage;

            await foreach(var lnk in _links.WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return await LoadPageAsync(lnk.link, lnk.parent);

                if (_isDone)
                    break;
            }
        }

        /// <summary>
        /// Recursively capture the page content and links
        /// </summary>
        /// <param name="startingUrl">The url to start from</param>
        /// <returns>A set of all of the child pages</returns>
        async Task<CrawlerPageNode> LoadPageAsync(string startingUrl, CrawlerPageNode parentPage)
        {
            var pageText = await _webClient.DownloadStringTaskAsync(startingUrl);
            var navLinks = _navigationLinkParser.ParseHtml(pageText);

            foreach (var lnk in navLinks)
                _links.Enqueue((lnk, parentPage));

            // Test this after enqueueing any items

            // If the queue is empty AND this page has no nav links
            // then we're done
            _isDone = _links.Count == 0;

            var thisPage = new CrawlerPageNode
            {
                PageUrl = startingUrl,
                HTMLContent = pageText,
                LinksInPage = navLinks,
                Parent = parentPage
            };

            return thisPage;
        }


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
