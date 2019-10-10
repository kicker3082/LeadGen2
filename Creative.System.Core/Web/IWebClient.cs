using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Creative.System.Core.Web
{

    public interface IWebClient
    {
        /// <summary>
        /// Downloads the resource with the specified URI to a local file.
        /// </summary>
        /// <param name="uri">The address from which to download data.</param>
        /// <param name="filename">The name of the local file that is to receive the data.</param>
        void DownloadFile(Uri uri, string filename);
        /// <summary>
        /// Downloads the html at the specified address into a file.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="filename">The name of the file that will contain the
        ///     downloaded html.</param>
        IWebPage DownloadPageIntoFile(Uri uri, string filename);

        /// <summary>
        /// GETs a webpage at the specified uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        IWebPage GetPage(Uri uri);

        /// <summary>
        /// GETs a webpage at the specified uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        IWebPage GetPageWithAdditionalCookies(Uri uri, CookieCollection cookies);

        /// <summary>
        /// GET's a webpage without blocking. Page is available in the Document property.
        /// </summary>
        /// <param name="uri"></param>
        Task<IWebPage> GetPageAsync(Uri uri);

        /// <summary>
        /// POSTs to the specified uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postBackData"></param>
        /// <returns></returns>
        IWebPage Post(Uri uri, IWebPostbackData postBackData);

        string PostRaw(Uri uri, IWebPostbackData postbackData);

        IWebClient WithAdditionalCookies(CookieCollection cookies);
        string PostRaw(Uri uri, IWebPostbackData postbackData, IDictionary<string, string> additionalHeaders, bool keepAlive);

        //
        // Summary:
        //     Downloads the requested resource as a System.String. The resource to download
        //     is specified as a System.String containing the URI.
        //
        // Parameters:
        //   address:
        //     A System.String containing the URI to download.
        //
        // Returns:
        //     A System.String containing the requested resource.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The address parameter is null.
        //
        //   T:System.Net.WebException:
        //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.
        //     -or- An error occurred while downloading the resource.
        //
        //   T:System.NotSupportedException:
        //     The method has been called simultaneously on multiple threads.
        string DownloadString(string address);
        //
        // Summary:
        //     Downloads the requested resource as a System.String. The resource to download
        //     is specified as a System.Uri.
        //
        // Parameters:
        //   address:
        //     A System.Uri object containing the URI to download.
        //
        // Returns:
        //     A System.String containing the requested resource.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The address parameter is null.
        //
        //   T:System.Net.WebException:
        //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.
        //     -or- An error occurred while downloading the resource.
        //
        //   T:System.NotSupportedException:
        //     The method has been called simultaneously on multiple threads.
        string DownloadString(Uri address);
        //
        // Summary:
        //     Downloads the resource as a System.String from the URI specified as an asynchronous
        //     operation using a task object.
        //
        // Parameters:
        //   address:
        //     The URI of the resource to download.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task`1. The task object representing the asynchronous
        //     operation. The System.Threading.Tasks.Task`1.Result property on the task object
        //     returns a System.Byte array containing the downloaded resource.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The address parameter is null.
        //
        //   T:System.Net.WebException:
        //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.
        //     -or- An error occurred while downloading the resource.
        Task<string> DownloadStringTaskAsync(string address);
        //
        // Summary:
        //     Downloads the resource as a System.String from the URI specified as an asynchronous
        //     operation using a task object.
        //
        // Parameters:
        //   address:
        //     The URI of the resource to download.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task`1. The task object representing the asynchronous
        //     operation. The System.Threading.Tasks.Task`1.Result property on the task object
        //     returns a System.Byte array containing the downloaded resource.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The address parameter is null.
        //
        //   T:System.Net.WebException:
        //     The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.
        //     -or- An error occurred while downloading the resource.
        Task<string> DownloadStringTaskAsync(Uri address);

    }
}
