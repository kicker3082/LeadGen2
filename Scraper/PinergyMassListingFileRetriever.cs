using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Creative.System.Core;
using Scraper.Core;

namespace Scraper
{

    public class PinergyMassListingFileRetriever : IDisposable
    {
        readonly IWebClient _webClient;
        WebPage _lastWebPage;
        WebPage _lastSearchWebPage;
        WebPage _lastSearchResultsPage;
        string _urlHost;

        readonly IEnumerable<string> _searchNames = new[]
        {
            @"Boston_1_Pending",
            @"Boston_2_Pending",
            @"ToBolton2_Pending",
            @"ToCambridge_Pending",
            @"ToCohasset2_Pending",
            @"ToFoxboro2_Pending",
            @"ToHanover2_Pending",
            @"ToHull2_Pending",
            @"ToLowell_Pending",
            @"ToMarlboro2_Pending",
            @"ToNeedham_Pending",
            @"ToNorfolk2_Pending",
            @"ToPlainville2_Pending",
            @"ToRevere2_Pending",
            @"ToSomerset2_Pending",
            @"ToStoneham_Pending",
            @"ToUxbridge2_Pending",
            @"ToUxbridge2_Pending",
            @"ToWeston_Pending",
            @"ToWinthrop2_Pending",
            @"ToWoburn_Pending",
            @"ToWrentham2_Pending"

        };

#pragma warning disable IDE1006 // Naming Styles
        const string download_fields =
#pragma warning restore IDE1006 // Naming Styles
            @"PAll.ACRE,PALL.ADULT_COMMUNITY,PAll.ALERT_COMMENTS,SF.AMENITIES,PAll.ANT_SOLD_DATE,SF.APPLIANCES,PAll.AREA,PAll.ASSESSMENTS,PAll.BASEMENT,SF.BASEMENT_FEATURE,SF.BEACH_DESCRIPTION,SF.BEACH_MILES_TO,SF.BEACH_OWNERSHIP,PAll.BEACHFRONT_FLAG,SF.BED2_DIMEN,SF.BED2_DSCRP,SF.BED2_LEVEL,SF.BED3_DIMEN,SF.BED3_DSCRP,SF.BED3_LEVEL,SF.BED4_DIMEN,SF.BED4_DSCRP,SF.BED4_LEVEL,SF.BED5_DIMEN,SF.BED5_DSCRP,SF.BED5_LEVEL,PAll.BLOCK,PAll.BOOK,SF.BTH1_DIMEN,SF.BTH1_DSCRP,SF.BTH1_LEVEL,SF.BTH2_DIMEN,SF.BTH2_DSCRP,SF.BTH2_LEVEL,SF.BTH3_DIMEN,SF.BTH3_DSCRP,SF.BTH3_LEVEL,PAll.BUYER_BROKER_COMP,PAll.CERTIFICATE_NUMBER,SF.COLOR,PALL.COMP_BASED_ON,SF.CONSTRUCTION,PALL.CONTINGENCY_TYPE,SF.COOLING,SF.COOLING_ZONES,PALL.OFFER_TIME,SF.DIN_DIMEN,SF.DIN_DSCRP,SF.DIN_LEVEL,PAll.DIRECTION,PAll.DISCLOSURE,PALL.DISCLOSURES,PALL.DPR_Flag,SF.ELECTRIC_FEATURE,SF.ENERGY_FEATURES,PAll.ENTRY_ONLY,PALL.EXCLUSIONS,SF.EXTERIOR,SF.EXTERIOR_FEATURES,SF.FACING_DIRECTION,SF.FAM_DIMEN,SF.FAM_DSCRP,SF.FAM_LEVEL,PAll.FIN_CODE,PAll.FIRE_PLACES,PAll.FIRM_RMK1,SF.FLOORING,SF.FOUNDATION,SF.FOUNDATION_SIZE,PAll.FRONTAGE,SF.GARAGE_PARKING,SF.GARAGE_SPACES,PAll.GRADE_SCHOOL,SF.GREEN_CERTIFICATION,SF.GREEN_CERTIFIED,SF.HANDICAP_ACCESS,SF.HANDICAP_AMENITIES,SF.HEAT_ZONES,SF.HEATING,PALL.HERS_COMPLETION_DATE,PALL.HERS_INDEX_SCORE,PAll.HIGH_SCHOOL,SF.HOA_FEE,SF.HOME_OWN_ASSOCIATION,SF.HOT_WATER,PALL.HOW_SHOWN_BB,PALL.HOW_SHOWN_FB,PALL.HOW_SHOWN_SB,SF.INSULATION_FEATURE,SF.INTERIOR_FEATURES,SF.KIT_DIMEN,SF.KIT_DSCRP,SF.KIT_LEVEL,SF.LAUNDRY_DIMEN,SF.LAUNDRY_DSCRP,SF.LAUNDRY_LEVEL,SF.LEAD_PAINT,PAll.LENDER_OWNED,PAll.LIST_AGENT,MEMBER.LIST_AGENT_NAME,MEMBER.LIST_AGENT_PHONE,PAll.LIST_DATE,PAll.LIST_DATE_RCVD,PAll.LIST_NO,PAll.LIST_OFFICE,OFFICE.LIST_OFFICE_NAME,OFFICE.LIST_OFFICE_PHONE,PAll.LIST_PRICE,PAll.LISTING_AGREEMENT,PAll.LISTING_ALERT,SF.LIV_DIMEN,SF.LIV_DSCRP,SF.LIV_LEVEL,PAll.LOT,SF.LOT_DESCRIPTION,PALL.LOT_SIZE,PAll.MAIN_LO,PAll.MAIN_SO,PAll.MAP,PAll.MARKET_TIME,PAll.MARKET_TIME_BROKER,PAll.MARKET_TIME_PROPERTY,PAll.MASTER_BATH,SF.MBR_DIMEN,SF.MBR_DSCRP,SF.MBR_LEVEL,PAll.MIDDLE_SCHOOL,PALL.NEIGHBORHOOD,PAll.NO_BEDROOMS,PAll.NO_FULL_BATHS,PAll.NO_HALF_BATHS,PAll.NO_ROOMS,PAll.OFF_MKT_DATE,PALL.OFFER_DATE,PAll.ORIG_PRICE,SF.OTH1_DIMEN,SF.OTH1_DSCRP,SF.OTH1_LEVEL,SF.OTH1_ROOM_NAME,SF.OTH2_DIMEN,SF.OTH2_DSCRP,SF.OTH2_LEVEL,SF.OTH2_ROOM_NAME,SF.OTH3_DIMEN,SF.OTH3_DSCRP,SF.OTH3_LEVEL,SF.OTH3_ROOM_NAME,SF.OTH4_DIMEN,SF.OTH4_DSCRP,SF.OTH4_LEVEL,SF.OTH4_ROOM_NAME,SF.OTH5_DIMEN,SF.OTH5_DSCRP,SF.OTH5_LEVEL,SF.OTH5_ROOM_NAME,SF.OTH6_DIMEN,SF.OTH6_DSCRP,SF.OTH6_LEVEL,SF.OTH6_ROOM_NAME,PAll.OTHER_AGENT_COMP,PAll.PAGE,PAll.PARCEL_NUMBER,SF.PARKING_FEATURE,SF.PARKING_SPACES,PAll.PHOTO_DATE,PAll.PHOTO_MASK,PAll.PRICE_PER_SQFT,PAll.PROP_TYPE,PAll.REMARKS,SF.REQD_OWN_ASSOCIATION,SF.ROAD_TYPE,SF.ROOF_MATERIAL,PAll.SALE_AGENT,MEMBER.SALE_AGENT_NAME,MEMBER.SALE_AGENT_PHONE,PAll.SALE_OFFICE,OFFICE.SALE_OFFICE_NAME,OFFICE.SALE_OFFICE_PHONE,PAll.SALE_PRICE,PAll.SELLER_CONCESSIONS_AT_CLOSING,PAll.SELLER_DISCOUNT_PTS,PAll.SELLING_BROKER_COMP,PAll.SETTLED_DATE,SF.SEWAGE_DISTRICT,SF.SEWER,SF.SF_TYPE,PAll.SHORT_SALE_LENDER_APP_REQD,PAll.SHOW_INSTRUCTION,PAll.SOLD_VS_RENT,PAll.SQUARE_FEET,PALL.SQUARE_FEET_DISCLOSURES,PALL.SQUARE_FEET_INCL_BASE,PAll.SQUARE_FEET_SOURCE,PAll.STATUS,PAll.STATUS_DATE,PAll.STREET_NAME,PAll.STREET_NO,SF.STYLE,PAll.TAX_YEAR,PAll.TAXES,PAll.TEAM_MEMBER,SF.TERMS_FEATURE,SF.TITLE5,PAll.TOWN_NUM,SF.UFFI,PAll.UNIT_NO,PAll.UPDATE_DATE,SF.UTILITY_CONNECTIONS,SF.WARRANTY,SF.WATER,SF.WATERFRONT,PAll.WATERFRONT_FLAG,SF.WATERVIEW_FEATURES,PALL.WATERVIEW_FLAG,PAll.YEAR_BUILT,PAll.YEAR_BUILT_DESCRP,PAll.YEAR_BUILT_SOURCE,SF.YEAR_ROUND,PAll.ZIP_CODE,PAll.ZIP_CODE_4,PAll.ZONING,";

        public PinergyMassListingFileRetriever(IFileSystem file)
        {
            _webClient = new WebClient(file);
        }

        public void GetFilesFromBoard()
        {
            var signInPage = _lastWebPage = _webClient.GetPage(new Uri(@"https://h3w.mlspin.com/signin.asp"));

            _urlHost = signInPage.Uri.Host;

            var formDataValues = new NameValueCollection
            {
                {@"rjkienamvalidvkjxouwr", @"DT: 7/22/2016 3:27:38 PM"},
                {@"Page_Loaded", @"DT: 7/22/2016 3:27:38 PM"},
                {@"user_name", @"cn213390"},
                {@"pass", @"Storage0!"},
                {@"SavePassword", @"Y"},
                //{@"signin", @"Sign In"},
            };
            var formData = GenericASPWebPostBackData.FromPage(signInPage, $@"https://{_urlHost}/validate_new.asp",
                formDataValues);

            var loggedInPage = _lastWebPage = _webClient.Post(new Uri($@"https://{_urlHost}/validate_new.asp"), formData);

            var dupeLogIndindicator = loggedInPage.Document.TextContent.Contains(@"Login Violation Notice");

            // If we have received the log in violation page, one more get request is needed to get to the logged-in page
            if (dupeLogIndindicator)
                loggedInPage = _lastWebPage = _webClient.GetPage(new Uri($@"https://{_urlHost}/home.asp"));

            var searchPageLinks = loggedInPage.Document.QuerySelectorAll(@".//a[@title=' Search MLS Listings ']");
            var searchListingsNoMapRelativeUrl = searchPageLinks[0].Attributes["href"].Value;

            // Go to the search page
            var searchPageFrameSet =
                _lastWebPage = _webClient.GetPage(new Uri($@"https://{_urlHost}{searchListingsNoMapRelativeUrl}"));

            var searchPageTopFrameNode = searchPageFrameSet.Document.QuerySelector(@".//frame[@name='TopFrame']");
            var searchPageUrl = searchPageTopFrameNode.Attributes[@"src"].Value;

            var searchPage = _lastWebPage = _webClient.GetPage(new Uri($@"https://{_urlHost}/search/{searchPageUrl}"));

            // Sort through the options in the drop-down
            var searchOption = searchPage.Document.QuerySelector(@".//select[@name='SavedSearchId']");

            var searchItems = searchOption.QuerySelectorAll(@"./option")?.Select(n => new
            {
                Name = n.NextSibling?.TextContent?.Trim('\r', '\n', '\t'),
                Value = n.Attributes[@"value"]?.Value
            }).Where(n => _searchNames.Contains(n.Name));

            // pare down the list to the preset searches
            Debug.Assert(searchItems != null, "searchItems != null");

            var searches = searchItems.Where(s => _searchNames.Contains(s.Name));

            // iterate the searches and execute each one
            foreach (var search in searches)
            {
                // construct the url
                var searchUrl = $@"/search/editsavedsearch.asp?sid=&cid=&savedsearchid={search.Value}";
                var searchUri = new Uri($@"https://{_urlHost}{searchUrl}");

                var searchResultPage = _lastSearchWebPage = _webClient.GetPage(searchUri);

                // Load the hidden inputs into the appropriate post values

                var hiddenInputs = searchResultPage.Document.QuerySelectorAll(@".//input[@type='hidden']");
                var values = hiddenInputs.Select(n => new
                {
                    Name = n.Attributes["name"].Value,
                    Value = n.Attributes["value"].Value
                });

                // Post the values to /search/results.asp

                var searchFormCollection = new NameValueCollection();
                foreach (var formValue in values)
                    searchFormCollection.Add(formValue.Name, formValue.Value);

                var searchResultsUri = new Uri($@"https://{_urlHost}/search/results.asp");
                var searchFormData = GenericASPWebPostBackData.FromPage(searchResultPage, searchResultsUri.AbsoluteUri, searchFormCollection);


                var lastSearchResults = _lastSearchResultsPage = _webClient.Post(searchResultsUri, searchFormData);

                // Compile a list of the returned items

                var searchResultsListingItemNodes = lastSearchResults.Document.QuerySelectorAll(@".//input[@name='marked']");
                var listingNumbers = searchResultsListingItemNodes.Select(n => n.Attributes["value"].Value).ToArray();
                var sb = new StringBuilder();

                foreach (var ml in listingNumbers)
                    sb.Append(ml).Append(',');

                var mls = sb.ToString().TrimEnd(',');

                var submitDownloadUri = new Uri($@"https://{_urlHost}/search/Submit_Download.asp");
                // TODO: Start here
                //var submittedDownload = _webClient.GetPageWithAdditionalCookies(submitDownloadUri,);

                var downloadFormCollection = new NameValueCollection
                {
                    { @"mls", mls },
                    { @"Prop_Type", @"CC,RN,SF" },
                    { @"DownloadFields", download_fields },
                    { @"Delimiter", @"comma_csv" },
                    { @"HeaderRow", "Y" }
                };

                var downloadUri = new Uri($@"https://{_urlHost}/search/Download_Reports.asp");
                var downloadFormData = GenericASPWebPostBackData.FromPage(lastSearchResults, downloadUri.AbsoluteUri, downloadFormCollection);

                var domain = $@"https://{downloadUri.Host}";
                var referer = $@"{domain}/search/Download_Select.asp";

                var headerItems = new Dictionary<string, string>
                {
                    {@"Cache-Control", @"max-age=0"},
                    {@"Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"},
                    {@"Accept-Encoding", @"gzip, deflate, br"},
                    {@"Accept-Language", @"en - US,en; q = 0.8,es; q = 0.6"},
                    {@"User-Agent", @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36"},
                    {@"Referer", referer},
                    {@"Origin", domain},
                    {@"Upgrade-Insecure-Requests", @"1"},
                    {@"Host", downloadUri.Host},
                };

                var fileContents = _webClient.PostRaw(downloadUri, downloadFormData, headerItems, true);
                break;

            }


        }


        #region IDisposable

        private bool _alreadyDisposed;

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isInFinalizer)
        {
            if (_alreadyDisposed)
                return;

            // If this is being called from the finalizer, do not attempt to
            // dispose of managed objects. Only unmanaged resources should be
            // released.
            if (!isInFinalizer)
                DisposeManagedObjects();

            ReleaseUnmanagedResources();

            _alreadyDisposed = true;
        }

        private void DisposeManagedObjects()
        {
            // Dispose managed objects here
            if (_webClient != null && _lastWebPage != null && !string.IsNullOrWhiteSpace(_urlHost))
                _webClient.GetPage(new Uri($@"https://{_urlHost}/logout.asp"));


        }

        private void ReleaseUnmanagedResources()
        {
            // Release unmanaged resources here
        }

        ~PinergyMassListingFileRetriever()
        {
            Dispose(true);
        }

        #endregion

    }
}
