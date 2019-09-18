using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scraper.Core;
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Scraper
{
    public class ZillowListingSearchResultsPageParserV1 : INavigationLinkParser

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        /// <remarks> 
        /// 1. apply an x-path query to the html
        /// 2. x-path will discover data item links
        /// 3. x-path will discover grid paging links (next link)
        /// 4. combine two sets and return them
        /// </remarks>
        /// 
        IEnumerable<string> INavigationLinkParser.ParseHtml(string html)
        {
            if (html == null)
                throw new ArgumentNullException(nameof(html));
            if (html == "")
                return Enumerable.Empty<string>();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(html);

            // finding the listing links in the page
            var listingSelector = document.QuerySelectorAll(@"a.list-card-link").OfType<IHtmlAnchorElement>();
            var result = listingSelector.Select(a => a.Href);

            // finding the next link in the page
            var zsgPageLinks = document.QuerySelectorAll(@"li.zsg-pagination-next > a").OfType<IHtmlAnchorElement>();
            
            // if next link exists, append it to the end of the listing links
            if (zsgPageLinks.Count() > 0)
            {
                var nextLink = zsgPageLinks.Single(a => a.InnerHtml == "NEXT");
                var targetUrl = nextLink.Href.Replace(@"about://", "");
                result = result.Distinct().Append(targetUrl);
            }
            return result;

        }




    }
}
