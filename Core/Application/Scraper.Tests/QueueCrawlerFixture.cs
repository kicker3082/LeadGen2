using System;
using System.Linq;
using System.Threading.Tasks;
using Creative.System.Core.Web;
using Moq;
using NUnit.Framework;
using Scraper.Core;

namespace Scraper.Tests
{
    public abstract class QueueCrawlerFixtureBase
    {
        protected Mock<INavigationLinkParser> NavLinkParser;
        protected Mock<IWebClient> Wc;
        protected PageLoader Loader;
        protected PageLoaderDecoratorWithNoDupeUrls NoDupeLoader;

        protected static readonly string NoLinksStartingUrl = @"http://mydomain.com/startingpage";
        protected static readonly string NoContentUrl = NoLinksStartingUrl + @"/nocontent";
        protected static readonly string OneLinkStartingUrl = NoLinksStartingUrl + @"/1";
        protected static readonly string TwoLinkStartingUrl = NoLinksStartingUrl + @"/2";

        protected readonly string NoLinksContent = @"This content contains no links";
        protected readonly string OneLinkContent = @"Page Content; Link: 1";
        protected readonly string TwoLinksContent = @"Page Content; Link: 1; Link 2";
        protected readonly string Link1Url = @"nav link 1";
        protected readonly string Link2Url = @"nav link 1";

        [SetUp]
        public void Setup()
        {
            Wc = new Mock<IWebClient>();
            NavLinkParser = new Mock<INavigationLinkParser>();

            Wc.Setup(obj => obj.DownloadStringTaskAsync(It.IsAny<string>())).Returns<string>(url => Task.FromResult(
                url == NoContentUrl ? string.Empty :
                url == NoLinksStartingUrl ? NoLinksContent :
                url == OneLinkStartingUrl ? OneLinkContent :
                url == TwoLinkStartingUrl ? TwoLinksContent :
                url == Link1Url ? NoLinksContent :
                url == Link2Url ? NoLinksContent : null
            ));

            NavLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
                html == NoLinksContent ? Enumerable.Empty<string>() :
                html == OneLinkContent ? new[] { Link1Url } :
                html == TwoLinksContent ? new[] { Link1Url, Link2Url } : Enumerable.Empty<string>()
            );
            Loader = new PageLoader(Wc.Object, NavLinkParser.Object);
            NoDupeLoader = new PageLoaderDecoratorWithNoDupeUrls(Loader);
        }
    }
    public class QueueCrawlerWithTimingFixture : QueueCrawlerFixtureBase
    {
        [Test]
        public async void InterceptCrawlAsync()
        {
            var crawler = (ICrawler)new QueueCrawler(Loader);
            await foreach (var p in crawler.CrawlWebAsync(TwoLinkStartingUrl))
            {

            };

        }
    }

    public class QueueCrawlerFixture : QueueCrawlerFixtureBase
    {

        [Test]
        public void Ctor()
        {
            new QueueCrawler(Loader);
            Assert.Pass();
        }

        [Test]
        public void CrawlWeb_StartingUrl_Null_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var ex = Assert.Throws<ArgumentNullException>(() => nav.CrawlWeb(null));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Empty_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => nav.CrawlWeb(""));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_MalformedUrl_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("rt%snhsdn"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be well-formed. (Parameter 'startingUrl')"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FtpProtocol_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("ftp://mydomain.com"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be a http or https. (Parameter 'startingUrl')"));
        }


        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnsOnePage()
        {
            Wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray.Length, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasCorrectUrl()
        {
            Wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].PageUrl, Is.EqualTo(NoContentUrl));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasNoLinks()
        {
            Wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].LinksInPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasNoErrorCode()
        {
            Wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].HttpErrorCode, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasBlankContent()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].HTMLContent, Is.Empty);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsNoLinks_ReturnsOnePage()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoLinksStartingUrl);
            Assert.That(pages.Count, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_WithNoDupeLoader_StartingUrl_DuplicateFirstPageReturnsContent_ContentContainsNoLinks_ReturnsOnlyOnePage()
        {
            {
                var nav = (ICrawler)new QueueCrawler(Loader);
                var pages = nav.CrawlWeb(NoLinksStartingUrl);
                var pages2 = nav.CrawlWeb(NoLinksStartingUrl);
                Assert.That(pages.Count, Is.EqualTo(1));
                Assert.That(pages2.Count, Is.EqualTo(1));
            }

            {
                var nav = (ICrawler)new QueueCrawler(NoDupeLoader);
                var pages = nav.CrawlWeb(NoLinksStartingUrl);
                var pages2 = nav.CrawlWeb(NoLinksStartingUrl);
                Assert.That(pages.Count, Is.EqualTo(1));
                Assert.That(pages2.Count, Is.EqualTo(0));
            }

        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsNoLinks_ReturnsZeroLinks()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(NoLinksStartingUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].LinksInPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_ReturnsTwoPages()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(OneLinkStartingUrl);
            Assert.That(pages.Count, Is.EqualTo(2));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_HasOneRootPageWithNullParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(OneLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == OneLinkStartingUrl);

            Assert.That(rootPages.Count, Is.EqualTo(1));
            var rootPage = rootPages.First();
            Assert.That(rootPage.Parent, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_RootPageHasOneChild()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(OneLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == OneLinkStartingUrl);
            var rootPage = rootPages.First();

            var childPages = pages.Where(pg => pg.Parent?.PageUrl == OneLinkStartingUrl);
            Assert.That(childPages.Count, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_ChildHasRootPageAsParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(OneLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == OneLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == OneLinkStartingUrl);
            Assert.That(childPages.All(pg => pg.Parent?.PageUrl == OneLinkStartingUrl), Is.True);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ReturnsThreePages()
        {
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(TwoLinkStartingUrl);
            Assert.That(pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_HasOneRootPageWithNullParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(TwoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == TwoLinkStartingUrl);

            Assert.That(rootPages.Count, Is.EqualTo(1));
            var rootPage = rootPages.First();
            Assert.That(rootPage.Parent, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_RootPageHasTwoChildPages()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(TwoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == TwoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == TwoLinkStartingUrl);
            Assert.That(childPages.Count, Is.EqualTo(2));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveRootPageAsParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(TwoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == TwoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == TwoLinkStartingUrl);
            Assert.That(childPages.All(pg => pg.Parent?.PageUrl == TwoLinkStartingUrl), Is.True);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveCorrectUrl()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(TwoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == TwoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == TwoLinkStartingUrl).ToArray();
            Assert.That(childPages[0].PageUrl, Is.EqualTo(Link1Url));
            Assert.That(childPages[1].PageUrl, Is.EqualTo(Link2Url));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveNoChildren()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(Loader);
            var pages = nav.CrawlWeb(TwoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == TwoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPageUrls = pages.Where(pg => pg.Parent?.PageUrl == TwoLinkStartingUrl).Select(pg => pg.PageUrl);
            Assert.That(pages.Any(pg => childPageUrls.Contains(pg.Parent?.PageUrl)), Is.False);
        }

        // Continue tests for multiple levels of children

        //[Test]
        //public void CrawlWeb_StartingUrl_FirstPageReturns500_SetsHttpErrorCode()
        //{
        //    var nav = (ICrawler)new PinergyIdxCrawler();
        //    var pages = nav.CrawlWeb(_startingUrl);
        //    var firstPage = pages.First();
        //    Assert.That(firstPage.HttpErrorCode, Is.EqualTo(500));
        //}
    }
}