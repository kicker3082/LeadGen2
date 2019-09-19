using System;
using System.Collections.Generic;

namespace Scraper
{

    /// <summary>
    /// Implements the navigation to and retrieval of listings from the PinergyIdx website
    /// </summary>
    /// <remarks>
    /// Steps:
    /// 
    /// POST the user credentials to retreive the authentication cookie and attach to subsequent GETs/POSTs
    /// Iterate over each date within the specified range
    ///     Iterate over each unique combination of Property Type [SF, CC] and Status [CTG, UAG] 
    ///         = ([SF:CTG],[CC:CTG],[SF:AUG],[CC:CTG])
    ///         POST to retreive the listing grid HTML where the Status Date is equal to the date
    ///         Verify that there are fewer than 500 listings
    ///         Parse out the ML Numbers from the listings
    ///         POST to initiate the data download using the ML Numbers
    ///         Write the retrieved data to files named for the combination: YYYYMMDD_all_state_[CTG|UAG]_[CC|SF].csv
    /// </remarks>
    public class PinergyIdxListingRetreiver
    {
        public async IAsyncEnumerable<string> RetreiveFilesForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateNoTime = startDate.Date;
            var endDateNoTime = endDate.Date;
            
            if (endDateNoTime < startDateNoTime)
                throw new ArgumentException("endDate must be greater than or equal to startDate.", nameof(endDate));

            var filterList = new (string status, string propType)[] 
            {
                (@"CTG", @"CC"),
                (@"CTG", @"SF"),
                (@"UAG", @"CC"),
                (@"UAG", @"SF")
            };
            var daysDiff = endDateNoTime.Subtract(startDateNoTime).Days;

            for(var d = 0; d <= daysDiff; d++)
                foreach(var filter in filterList)
                {
                    var statusDate = startDateNoTime.AddDays(d);
                    var filename = string.Format($"{statusDate:yyyyMMdd}_all_state_{filter.status}_{filter.propType}.csv");


                    // await and download the file here!!

                    yield return filename;
                }
        }
    }
}
