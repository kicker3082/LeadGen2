using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Scraper.Core;

namespace Scraper.Tests
{
    public class PinergyIdxNavigatorFixture
    {
        Mock<IWebClient> _wc;

        [SetUp]
        public void Setup()
        {
            _wc = new Mock<IWebClient>();
        }

        [Test]
        public void Ctor()
        {
            new PinergyIdxNavigator(_wc.Object);
            Assert.Pass();
        }

        [Test]
        public void Ctor_NullWebClient_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PinergyIdxNavigator(null));
            Assert.That(ex.ParamName, Is.EqualTo(@"webClient"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Null_Throws()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator(_wc.Object);
            var ex = Assert.Throws<ArgumentNullException>(() => nav.CrawlWeb(null));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Empty_Throws()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator(_wc.Object);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => nav.CrawlWeb(""));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_MalformedUrl_Throws()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator(_wc.Object);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("rt%snhsdn"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be well-formed.\r\nParameter name: startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FtpProtocol_Throws()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator(_wc.Object);
            var ex = Assert.Throws<ArgumentException>(() => nav.CrawlWeb("ftp://mydomain.com"));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
            Assert.That(ex.Message, Is.EqualTo("startingUrl must be a http or https.\r\nParameter name: startingUrl"));
        }


        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnsOnePage()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator(_wc.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray.Length, Is.EqualTo(1));
        }

        [Test]
        public void CrawlWeb_StartingUrl_FirstPageReturnsNoContent_ReturnedPageHasCorrectUrl()
        {
            _wc.Setup(obj => obj.DownloadString(It.IsAny<string>())).Returns(@"");

            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator(_wc.Object);
            var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
            var pagesArray = pages.ToArray();
            Assert.That(pagesArray[0].PageUrl, Is.EqualTo(@"http://mydomain.com/startingpage"));
        }

        //[Test]
        //public void CrawlWeb_StartingUrl_FirstPageReturns500_SetsHttpErrorCode()
        //{
        //    var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator();
        //    var pages = nav.CrawlWeb(@"http://mydomain.com/startingpage");
        //    var firstPage = pages.First();
        //    Assert.That(firstPage.HttpErrorCode, Is.EqualTo(500));
        //}
    }
}