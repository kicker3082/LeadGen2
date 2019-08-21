using System.Collections.Generic;

namespace Scraper.Core
{
    /// <summary>
    /// Implementations process HTML content to extract zero or more urls as defined
    /// by the implementation.
    /// </summary>
    public interface ILinkExtracter
    {
        /// <summary>
        /// Parse the passed HTML and return zero or more links contained within the HTML.
        /// </summary>
        /// <param name="html">The HTML content to be parsed.</param>
        /// <returns>A set or zero or more links contained within the HTML.</returns>
        IEnumerable<string> ExtractLinksFromHTML(string html);
    }
}
