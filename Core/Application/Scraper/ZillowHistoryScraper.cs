using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Scraper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper
{
    public class ZillowHistoryScraper
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        /// <remarks> 
        /// 1. apply an x-path query to the html
        /// 2. query will return the price history of the listing as a string IEnumerable
        /// </remarks>
        /// 
        public IEnumerable<string> ScrapeHistory(string html)
        {
            if (html == null)
                throw new ArgumentNullException(nameof(html));
            if (html == "")
                return Enumerable.Empty<string>();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(html);
            var history = document.QuerySelectorAll(@"tr.zsg-table_interactive");
            var result = history.Select(a => a.TextContent);
          
           return result;
           //return null;
        }

   
    }
    class ZillowHIstory
    {

    }
}
