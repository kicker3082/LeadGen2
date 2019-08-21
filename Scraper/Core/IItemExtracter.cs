using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Core
{
    /// <summary>
    /// Implementations process the HTML content to extract zero of more <typeparamref name="T">data items</typeparamref>.
    /// </summary>
    /// <typeparam name="T">The type of the data items that are extracted.</typeparam>
    public interface IItemExtracter<T> where T : class, new() 
    {
        /// <summary>
        /// Asyncronously extract zero or more <typeparamref name="T">data items</typeparamref> from the HTML.
        /// </summary>
        /// <param name="html">The HTML content to be parsed.</param>
        /// <typeparam name="T">The type of the data item that will be extracted from the HTML.</typeparam>
        /// <returns>A <seealso cref="Task{TResult}"/> that resolves to batches of <typeparamref name="T"/>.</returns>
        Task<IEnumerable<T>> ExtractItemsFromHTMLAsync(string html);

        /// <summary>
        /// Extract zero or more <typeparamref name="T">data items</typeparamref> from the HTML.
        /// </summary>
        /// <param name="html">The HTML content to be parsed.</param>
        /// <typeparam name="T">The type of the data item that will be extracted from the HTML.</typeparam>
        /// <returns>An enumeration of <typeparamref name="T"/>.</returns>
        IEnumerable<T> ExtractItemsFromHTML(string html);
    }
}
