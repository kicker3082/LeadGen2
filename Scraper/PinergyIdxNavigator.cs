using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scraper.Core;

namespace Scraper
{
    public class PinergyIdxCrawler : ICrawler
    {
        private readonly IWebClient _webClient;
        private readonly INavigationLinkParser _navigationLinkParser;

        public PinergyIdxCrawler(
            IWebClient webClient,
            INavigationLinkParser navigationLinkParser)
        {
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
            _navigationLinkParser = navigationLinkParser ?? throw new ArgumentNullException(nameof(navigationLinkParser));
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
            return ((ICrawler)this).CrawlWebAsync(startingUrl).Result;
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

        async Task<IEnumerable<CrawlerPageNode>> ICrawler.CrawlWebAsync(string startingUrl)
        {
            CheckUrl(startingUrl);
            return await Task.Run(() => CreateGraphFromPage(startingUrl, null));
        }

        /// <summary>
        /// Recursively capture the page content and links
        /// </summary>
        /// <param name="startingUrl">The url to start from</param>
        /// <returns>A set of all of the child pages</returns>
        IEnumerable<CrawlerPageNode> CreateGraphFromPage(string startingUrl, CrawlerPageNode parentPage)
        {
            var pageText = _webClient.DownloadString(startingUrl);
            var navLinkItems = _navigationLinkParser.ParseHtml(pageText);

            var thisPage = new CrawlerPageNode
            {
                PageUrl = startingUrl,
                HTMLContent = pageText,
                LinksInPage = navLinkItems,
                Parent = parentPage
            };

            var childPages = new List<CrawlerPageNode>();

            foreach (var childLink in navLinkItems)
            {
                childPages.AddRange(CreateGraphFromPage(childLink, thisPage));
            }
            thisPage.Children = childPages;
            return childPages.Prepend(thisPage);
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
