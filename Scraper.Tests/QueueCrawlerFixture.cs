using System;
using System.Linq;
using System.Threading.Tasks;
using Creative.System.Core.Web;
using Moq;
using NUnit.Framework;
using Scraper.Core;

namespace Scraper.Tests
{
    public class QueueCrawlerWithTimingFixture
    {
    }

    public class QueueCrawlerFixture
    {
        Mock<INavigationLinkParser> _navLinkParser;
        Mock<IWebClient> _wc;
        PageLoader _loader;
        PageLoaderDecoratorWithNoDupeUrls _noDupeLoader;

        static readonly string _noLinksStartingUrl = @"http://mydomain.com/startingpage";
        static readonly string _noContentUrl = _noLinksStartingUrl  + @"/nocontent";
        static readonly string _oneLinkStartingUrl = _noLinksStartingUrl + @"/1";
        static readonly string _twoLinkStartingUrl = _noLinksStartingUrl + @"/2";

        readonly string _noLinksContent = @"This content contains no links";
        readonly string _oneLinkContent = @"Page Content; Link: 1";
        readonly string _twoLinksContent = @"Page Content; Link: 1; Link 2";
        readonly string _link1Url = @"nav link 1";
        readonly string _link2Url = @"nav link 1";



        [SetUp]
        public void Setup()
        {
            _wc = new Mock<IWebClient>();
            _navLinkParser = new Mock<INavigationLinkParser>();

            _wc.Setup(obj => obj.DownloadStringTaskAsync(It.IsAny<string>())).Returns<string>(url => Task.FromResult(
                url == _noContentUrl ? string.Empty :
                url == _noLinksStartingUrl ? _noLinksContent :
                url == _oneLinkStartingUrl ? _oneLinkContent :
                url == _twoLinkStartingUrl ? _twoLinksContent : 
                url == _link1Url ? _noLinksContent :
                url == _link2Url ? _noLinksContent :null
            ));

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
                html == _noLinksContent ? Enumerable.Empty<string>() :
                html == _oneLinkContent ? new[] { _link1Url } :
                html == _twoLinksContent ? new[] { _link1Url, _link2Url } : Enumerable.Empty<string>()
            );
            _loader = new PageLoader(_wc.Object, _navLinkParser.Object);
            _noDupeLoader = new PageLoaderDecoratorWithNoDupeUrls(_loader);
        }

        [Test]
        public void Ctor()
        {
            new QueueCrawler(_loader);
            Assert.Pass();
        }

        [Test]
        public void CrawlWeb_StartingUrl_Null_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var ex = Assert.Throws<ArgumentNullException>(() => nav.CrawlWeb(null));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Empty_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => nav.CrawlWeb(""));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_MalformedUrl_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("rt%snhsdn"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be well-formed. (Parameter 'startingUrl')"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FtpProtocol_Throws()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("ftp://mydomain.com"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be a http or https. (Parameter 'startingUrl')"));
        }


        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnsOnePage()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray.Length, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasCorrectUrl()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].PageUrl, Is.EqualTo(_noContentUrl));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasNoLinks()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].LinksInPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasNoErrorCode()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].HttpErrorCode, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasBlankContent()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noContentUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].HTMLContent, Is.Empty);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsNoLinks_ReturnsOnePage()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noLinksStartingUrl);
            Assert.That(pages.Count, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_WithNoDupeLoader_StartingUrl_DuplicateFirstPageReturnsContent_ContentContainsNoLinks_ReturnsOnlyOnePage()
        {
            {
                var nav = (ICrawler)new QueueCrawler(_loader);
                var pages = nav.CrawlWeb(_noLinksStartingUrl);
                var pages2 = nav.CrawlWeb(_noLinksStartingUrl);
                Assert.That(pages.Count, Is.EqualTo(1));
                Assert.That(pages2.Count, Is.EqualTo(1));
            }

            {
                var nav = (ICrawler)new QueueCrawler(_noDupeLoader);
                var pages = nav.CrawlWeb(_noLinksStartingUrl);
                var pages2 = nav.CrawlWeb(_noLinksStartingUrl);
                Assert.That(pages.Count, Is.EqualTo(1));
                Assert.That(pages2.Count, Is.EqualTo(0));
            }

        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsNoLinks_ReturnsZeroLinks()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_noLinksStartingUrl);
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].LinksInPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_ReturnsTwoPages()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_oneLinkStartingUrl);
            Assert.That(pages.Count, Is.EqualTo(2));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_HasOneRootPageWithNullParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_oneLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _oneLinkStartingUrl);

            Assert.That(rootPages.Count, Is.EqualTo(1));
            var rootPage = rootPages.First();
            Assert.That(rootPage.Parent, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_RootPageHasOneChild()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_oneLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _oneLinkStartingUrl);
            var rootPage = rootPages.First();

            var childPages = pages.Where(pg => pg.Parent?.PageUrl == _oneLinkStartingUrl);
            Assert.That(childPages.Count, Is.EqualTo(1));
        }     

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_ChildHasRootPageAsParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_oneLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _oneLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == _oneLinkStartingUrl);
            Assert.That(childPages.All(pg => pg.Parent?.PageUrl == _oneLinkStartingUrl), Is.True);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ReturnsThreePages()
        {
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_twoLinkStartingUrl);
            Assert.That(pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_HasOneRootPageWithNullParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_twoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _twoLinkStartingUrl);

            Assert.That(rootPages.Count, Is.EqualTo(1));
            var rootPage = rootPages.First();
            Assert.That(rootPage.Parent, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_RootPageHasTwoChildPages()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_twoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _twoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == _twoLinkStartingUrl);
            Assert.That(childPages.Count, Is.EqualTo(2));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveRootPageAsParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_twoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _twoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == _twoLinkStartingUrl);
            Assert.That(childPages.All(pg => pg.Parent?.PageUrl == _twoLinkStartingUrl), Is.True);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveCorrectUrl()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_twoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _twoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPages = pages.Where(pg => pg.Parent?.PageUrl == _twoLinkStartingUrl).ToArray();
            Assert.That(childPages[0].PageUrl, Is.EqualTo(_link1Url));
            Assert.That(childPages[1].PageUrl, Is.EqualTo(_link2Url));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveNoChildren()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph
            var nav = (ICrawler)new QueueCrawler(_loader);
            var pages = nav.CrawlWeb(_twoLinkStartingUrl);
            var rootPages = pages.Where(p => p.PageUrl == _twoLinkStartingUrl);
            var rootPage = rootPages.First();
            var childPageUrls = pages.Where(pg => pg.Parent?.PageUrl == _twoLinkStartingUrl).Select(pg => pg.PageUrl);
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