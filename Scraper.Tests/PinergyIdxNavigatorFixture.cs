using System;
using NUnit.Framework;
using Scraper.Core;

namespace Scraper.Tests
{
    public class PinergyIdxNavigatorFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Ctor()
        {
            var nav = new PinergyIdxNavigator();
            Assert.Pass();
        }

        [Test]
        public void CrawlWeb_StartingUrl_Null_Throws()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator();
            var ex = Assert.Throws<ArgumentNullException>(() => nav.CrawlWeb(null));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }

        [Test]
        public void CrawlWeb_StartingUrl_Empty_Throws()
        {
            var nav = (ICrawler<TempListingDataItem>)new PinergyIdxNavigator();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => nav.CrawlWeb(""));
            Assert.That(ex.ParamName, Is.EqualTo(@"startingUrl"));
        }
    }
}