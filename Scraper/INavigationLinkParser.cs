using System.Collections.Generic;

namespace Scraper
{
    /// <summary>
    /// Inplementations comprehend the passed in html and generate a set of urls for further navigation
    /// </summary>
    public interface INavigationLinkParser
    {
        /// <summary>
        /// Interpret the html of a page and parse out zero or more navigation link urls
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        IEnumerable<string> ParseHtml(string html);
    }
}
