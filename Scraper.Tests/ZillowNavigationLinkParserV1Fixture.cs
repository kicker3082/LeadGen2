using NUnit.Framework;
using Scraper.Core;
using System;
using System.IO;
using System.Linq;

namespace Scraper.Tests
{
    public class ZillowNavigationLinkParserV1Fixture
    {

        readonly static string _testDataFolderPath = TestHelper.GetFullPathToFile(@"TestData");
        [Test]
        public void ctor()
        {
            var parser = new ZillowListingSearchResultsPageParserV1();
            Assert.That(parser, Is.Not.Null);
        }

        [Test]
        public void ParseHtml_HtmlIsNull_Throws()
        {
            var parser = (INavigationLinkParser) new ZillowListingSearchResultsPageParserV1();
            var ex = Assert.Throws<ArgumentNullException>(() => parser.ParseHtml(null)) ;

            Assert.That(ex.ParamName, Is.EqualTo(@"html"));

        }
        [Test]
        public void ParseHtml_HtmlIsEmpty_ReturnZeroLinks()
        {
            var parser = (INavigationLinkParser)new ZillowListingSearchResultsPageParserV1();
            var links = parser.ParseHtml("");

            Assert.That(links.Count(), Is.EqualTo(0));

        }
        [Test]
        public void ParseHtml_HtmlContainsNoLinks_ReturnZeroLinks()
        {
            var parser = (INavigationLinkParser)new ZillowListingSearchResultsPageParserV1();
            var links = parser.ParseHtml(@"<html></html>");

            Assert.That(links.Count(), Is.EqualTo(0));

        }
        [Test]
        public void ParseHtml_LiveZillowPage_ReturnsNextLink()
        {
            var testDataFile = Path.Combine(_testDataFolderPath, @"Amherst Real Estate - Amherst MA Homes For Sale _ Zillow.html");
            var html = File.ReadAllText(testDataFile);
            var parser = (INavigationLinkParser)new ZillowListingSearchResultsPageParserV1();
            var links = parser.ParseHtml(html);
            var hasNext = links.Contains(@"/amherst-ma/2_p/");

            Assert.IsTrue(hasNext);

        }
        
        [Test]
        public void ParseHtml_LiveZillowPage_Return41Links()
        {
            var testDataFile = Path.Combine(_testDataFolderPath, @"Amherst Real Estate - Amherst MA Homes For Sale _ Zillow.html");
            var html = File.ReadAllText(testDataFile);
            var parser = (INavigationLinkParser)new ZillowListingSearchResultsPageParserV1();
            var links = parser.ParseHtml(html);

            Assert.That(links.Count(), Is.EqualTo(41));

        }














    }
}