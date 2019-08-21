using System;
using System.Threading.Tasks;

namespace Scraper
{
    public interface IWebClient
    {
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
