using System.Collections.Generic;
using System.Threading.Tasks;
using Scraper.Core;

namespace Scraper
{
    public class PageLoaderDecoratorWithNoDupeUrls : IPageLoader
    {
        private readonly IPageLoader _baseLoader;
        HashSet<string> _alreadyLoadedUrls;

        public PageLoaderDecoratorWithNoDupeUrls(IPageLoader baseLoader) {
            _baseLoader = baseLoader;
            _alreadyLoadedUrls = new HashSet<string>();
        }

        async Task<CrawlerPageNode> IPageLoader.LoadPageAsync(string url, CrawlerPageNode parentPage)
        {
            if (_alreadyLoadedUrls.Contains(url))
                return null;

            var page = await _baseLoader.LoadPageAsync(url, parentPage);

            _alreadyLoadedUrls.Add(url);
            return page;
        }
    }
}
