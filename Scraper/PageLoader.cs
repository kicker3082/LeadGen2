using System;
using System.Threading.Tasks;
using Scraper.Core;

namespace Scraper
{
    public class PageLoader : IPageLoader
    {
        IWebClient _webClient;
        INavigationLinkParser _navigationLinkParser;

        public PageLoader(IWebClient webClient, INavigationLinkParser navigationLinkParser)
        {
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
            _navigationLinkParser = navigationLinkParser ?? throw new ArgumentNullException(nameof(navigationLinkParser));
        }
        /// <summary>
        /// Recursively capture the page content and links
        /// </summary>
        /// <param name="url">The url to start from</param>
        /// <returns>A set of all of the child pages</returns>
        async Task<CrawlerPageNode> IPageLoader.LoadPageAsync(string url, CrawlerPageNode parentPage)
        {
            var pageText = await _webClient.DownloadStringTaskAsync(url);
            // Make sure to add the startingUrl to the set of visited pages so we don't wrap around to the top
            // page.

            var thisPage = new CrawlerPageNode
            {
                PageUrl = url,
                HTMLContent = pageText,
                Parent = parentPage
            };

            var navLinks = _navigationLinkParser.ParseHtml(pageText);
            thisPage.LinksInPage = navLinks;

            return thisPage;
        }
    }
}
