using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Scraper.Core;

namespace Scraper.Tests
{
    public class PinergyIdxCrawlerFixture
    {
        Mock<INavigationLinkParser> _navLinkParser;
        Mock<IWebClient> _wc;

        [SetUp]
        public void Setup()
        {
            _wc = new Mock<IWebClient>();
            _navLinkParser = new Mock<INavigationLinkParser>();
        }

        [Test]
        public void Ctor()
        {
            new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            Assert.Pass();
        }

        [Test]
        public void Ctor_NullWebClient_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PinergyIdxCrawler(null, _navLinkParser.Object));
            Assert.That(ex.ParamName, Is.EqualTo(@"webClient"));
        }

        [Test]
        public void Ctor_NullNavLinkParser_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PinergyIdxCrawler(_wc.Object, null));
            Assert.That(ex.ParamName, Is.EqualTo(@"navigationLinkParser"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Null_Throws()
        {
            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var ex = Assert.Throws<ArgumentNullException>(() => nav.CrawlWeb(null));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Empty_Throws()
        {
            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => nav.CrawlWeb(""));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_MalformedUrl_Throws()
        {
            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("rt%snhsdn"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be well-formed.\r\nParameter name: startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FtpProtocol_Throws()
        {
            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("ftp://mydomain.com"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be a http or https.\r\nParameter name: startingUrl"));
        }


        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnsOnePage()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");
            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray.Length, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasCorrectUrl()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].PageUrl, Is.EqualTo(@"http://mydomain.com/startingpage"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasNoLinks()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].LinksInPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasNoErrorCode()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].HttpErrorCode, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasBlankContent()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].HTMLContent, Is.Empty);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsNoLinks_ReturnsOnePage()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"This is content without data items or links.");

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            Assert.That(pages.Count, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsNoLinks_ReturnsZeroLinks()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"This is content without data items or links.");

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].LinksInPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_ReturnsTwoPages()
        {
            _navLinkParser.Setup(obj => obj.ParseHtml(It.Is<string>(s => s == @"Page Content; Link: 1"))).Returns(new string[] { @"nav link 1" });
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            Assert.That(pages.Count, Is.EqualTo(2));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_HasOneRootPageWithNullParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.Is<string>(s => s == @"Page Content; Link: 1"))).Returns(new string[] { @"nav link 1" });
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");

            Assert.That(rootPages.Count, Is.EqualTo(1));
            var rootPage = rootPages.First();
            Assert.That(rootPage.Parent, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_RootPageHasOneChild()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.Is<string>(s => s == @"Page Content; Link: 1"))).Returns(new string[] { @"nav link 1" });
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");
            var rootPage = rootPages.First();

            var childPages = rootPage.Children;
            Assert.That(childPages.Count, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsOneLink_LinkedPageContainsNoLinks_ChildHasRootPageAsParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.Is<string>(s => s == @"Page Content; Link: 1"))).Returns(new string[] { @"nav link 1" });
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");
            var rootPage = rootPages.First();
            var childPages = rootPage.Children;
            Assert.That(childPages.All(pg => pg.Parent.PageUrl == @"http://mydomain.com/startingpage"), Is.True);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ReturnsThreePages()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
            {
                if (html == @"Page Content; Link: 1")
                    return new string[] { @"nav link 1", @"nav link 2" };
                else
                    return Enumerable.Empty<string>();
            });

            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else if (url == @"nav link 1")
                    return @"This content contains no links";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            Assert.That(pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_HasOneRootPageWithNullParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
            {
                if (html == @"Page Content; Link: 1")
                    return new string[] { @"nav link 1", @"nav link 2" };
                else
                    return Enumerable.Empty<string>();
            });

            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else if (url == @"nav link 1")
                    return @"This content contains no links";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");

            Assert.That(rootPages.Count, Is.EqualTo(1));
            var rootPage = rootPages.First();
            Assert.That(rootPage.Parent, Is.Null);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_RootPageHasTwoChildPages()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
            {
                if (html == @"Page Content; Link: 1")
                    return new string[] { @"nav link 1", @"nav link 2" };
                else
                    return Enumerable.Empty<string>();
            });

            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else if (url == @"nav link 1")
                    return @"This content contains no links";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");
            var rootPage = rootPages.First();
            var childPages = rootPage.Children;
            Assert.That(childPages.Count, Is.EqualTo(2));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveRootPageAsParent()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
            {
                if (html == @"Page Content; Link: 1")
                    return new string[] { @"nav link 1", @"nav link 2" };
                else
                    return Enumerable.Empty<string>();
            });

            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else if (url == @"nav link 1")
                    return @"This content contains no links";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");
            var rootPage = rootPages.First();
            var childPages = rootPage.Children;
            Assert.That(childPages.All(pg => pg.Parent.PageUrl == @"http://mydomain.com/startingpage"), Is.True);
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveCorrectUrl()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
            {
                if (html == @"Page Content; Link: 1")
                    return new string[] { @"nav link 1", @"nav link 2" };
                else
                    return Enumerable.Empty<string>();
            });

            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else if (url == @"nav link 1")
                    return @"This content contains no links";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");
            var rootPage = rootPages.First();
            var childPages = rootPage.Children.ToArray();
            Assert.That(childPages[0].PageUrl, Is.EqualTo(@"nav link 1"));
            Assert.That(childPages[1].PageUrl, Is.EqualTo(@"nav link 2"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsContent_ContentContainsTwoLinks_ChildPagesHaveNoChildren()
        {
            // A root page is defined as a page with no parent. There should only be one root page in any graph

            _navLinkParser.Setup(obj => obj.ParseHtml(It.IsAny<string>())).Returns<string>(html =>
            {
                if (html == @"Page Content; Link: 1")
                    return new string[] { @"nav link 1", @"nav link 2" };
                else
                    return Enumerable.Empty<string>();
            });

            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns<string>(url =>
            {
                if (url == @"http://mydomain.com/startingpage")
                    return @"Page Content; Link: 1";
                else if (url == @"nav link 1")
                    return @"This content contains no links";
                else
                    return @"This content contains no links";
            });

            var nav = (ICrawler)new PinergyIdxCrawler(_wc.Object, _navLinkParser.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var rootPages = pages.Where(p => p.PageUrl == @"http://mydomain.com/startingpage");
            var rootPage = rootPages.First();
            var childPages = rootPage.Children;
            Assert.That(childPages.All(pg => pg.Children.Count() == 0), Is.True);
        }

        // Continue tests for multiple levels of children

        //[Test]
        //public void CrawlWeb_StartingUrl_FirstPageReturns500_SetsHttpErrorCode()
        //{
        //    var nav = (ICrawler)new PinergyIdxCrawler();
        //    var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
        //    var firstPage = pages.First();
        //    Assert.That(firstPage.HttpErrorCode, Is.EqualTo(500));
        //}
    }
}