using System.Collections.Generic;

namespace Scraper
{
    /// <summary>
    /// Implementations comprehend the passed in html and are able to generate <typeparamref name="T">data items</typeparamref> from it.
    /// </summary>
    /// <typeparam name="T">The data item type.</typeparam>
    public interface IDataItemParser<T>
    {
        /// <summary>
        /// Interpret the html of a page and parse out zero or more <typeparamref name="T"/> items
        /// </summary>
        /// <param name="html">The raw html to be parsed.</param>
        /// <returns>A set of zero or more <typeparamref name="T">data items</typeparamref></returns>
        IEnumerable<T> ParseHtml(string html);
    }
}
