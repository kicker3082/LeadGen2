using System.Threading.Tasks;
using Scraper.Core;

namespace Scraper.Core
{
    public interface IPageLoader
    {
        Task<CrawlerPageNode> LoadPageAsync(string url, CrawlerPageNode parentPage);
    }
}
