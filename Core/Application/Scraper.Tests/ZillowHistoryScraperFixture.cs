using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Scraper.Core;
using System.IO;
using System.Linq;

namespace Scraper.Tests
{
    class ZillowHistoryScraperFixture
    {
        readonly static string _testDataFolderPath = TestHelper.GetFullPathToFile(@"TestData");
        [Test]
        public void ctor()
        {
            var scraper = new ZillowHistoryScraper();
            Assert.That(scraper, Is.Not.Null);
        }

        [Test]
        public void ScrapeHtml_HtmlIsNull_Throws()
        {
            var scraper = new ZillowHistoryScraper();
            var ex = Assert.Throws<ArgumentNullException>(() => scraper.ScrapeHistory(null));

            Assert.That(ex.ParamName, Is.EqualTo(@"html"));

        }
        [Test]
        public void ScrapeHtml_HtmlIsEmpty_ReturnEmptyStringIEnumerable()
        {
            var scraper = new ZillowHistoryScraper();
            var history = scraper.ScrapeHistory("");

            Assert.That(history.Count(), Is.EqualTo(0));

        }

        [Test]
        public void ScrapeHtml_LiveZillowPage_Return11Items()
        {
            var testDataFile = Path.Combine(_testDataFolderPath, @"57 Harkness Rd, Amherst, MA 01002 _ Zillow.html");
            var html = File.ReadAllText(testDataFile);
            var scraper = new ZillowHistoryScraper();
            var history = scraper.ScrapeHistory(html);
            
            Assert.That(history.Count(), Is.EqualTo(7));
            

        }
    }
}
