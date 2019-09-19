using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Scraper.Core;

namespace Scraper.Tests
{
    public class TestDataComparerFixture
    {
        readonly static string _testDataFolderPath = TestHelper.GetFullPathToFile(@"TestData");
        readonly string[] _testFiles = Directory.GetFiles(_testDataFolderPath);

        [Ignore("Used only for interactive debugging of the file retriever")]
        [Test]
        public void LogInToSite()
        {
            var file = new FileSystem();
            var ret = new PinergyMassListingFileRetriever(file);
            ret.GetFilesFromBoard();



            //var reqCookies = SetInitialCookies();
            //using (var wc = new WebClientEx(reqCookies))
            //{
            //    var loginPageUri = new Uri(@"https://h3w.mlspin.com/signin.asp");
            //    var validateUri = new Uri(@"https://h3w.mlspin.com/validate_new.asp");
            //    var homePageUri = new Uri(@"https://h3w.mlspin.com/homepage.asp");

            //    wc.DownloadFile(loginPageUri, Path.Combine(_testDataFolderPath, Uri.EscapeUriString(loginPageUri.AbsoluteUri).Replace(':', '-').Replace('/', '_')));

            //    var d1 = new Uri(@"http://hl3.mlspin.com");

            //    wc.Headers.Add(@"Cache-Control", @"max-age=0");
            //    wc.Headers.Add(@"Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            //    wc.Headers.Add(@"Accept-Encoding", @"gzip, deflate, br");
            //    wc.Headers.Add(@"Accept-Language", @"en-US,en;q=0.9");
            //    wc.Headers.Add(@"User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36");
            //    wc.Headers.Add(@"DNT", @"1");
            //    wc.Headers.Add(@"Content-Type", @"application/x-www-form-urlencoded");
            //    wc.Headers.Add(@"Content-Length", @"152");
            //    wc.Headers.Add(@"Referer", @"https://h3n.mlspin.com/signin.asp");
            //    wc.Headers.Add(@"Origin", @"https://h3n.mlspin.com");
            //    wc.Headers.Add(@"Sec-Fetch-Mode", @"navigate");
            //    wc.Headers.Add(@"Sec-Fetch-Site", @"same-origin");
            //    wc.Headers.Add(@"Sec-Fetch-User", @"?1");
            //    wc.Headers.Add(@"Upgrade-Insecure-Requests", @"1");
            //    wc.Headers.Add(@"Host", @"h3n.mlspin.com");

            //    var authForm = new NameValueCollection();
            //    authForm.Add(@"dvltccgwvalidhvoxvift", @"DT%3A+9%2F1%2F2019+4%3A20%3A58+AM");
            //    authForm.Add(@"Page_Loaded", @"DT%3A+9%2F1%2F2019+4%3A21%3A48+AM");
            //    authForm.Add(@"user_name", @"cn213390");
            //    authForm.Add(@"pass", @"Storage0%21");
            //    authForm.Add(@"SavePassword", @"Y");


            //    var result = wc.UploadString(validateUri, "POST", authForm.ToString());
            //    var newLoginCookieCred = wc.CookieContainer.GetCookies(validateUri)[@"CookieCred"];

            //    wc.DownloadFile(homePageUri, Path.Combine(_testDataFolderPath, Uri.EscapeUriString(homePageUri.AbsoluteUri).Replace(':', '-').Replace('/', '_')));

            }
        }

    //    CookieContainer SetInitialCookies()
    //    {
    //        var reqCookies = new CookieContainer();
    //        reqCookies.Add(new Cookie(@"__utmz", @"199506417.1526516765.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"_ga", @"GA1.2.166604096.1526516765", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@".MLSPIN", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"contextUserId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsHomeLoad", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKModuleId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKModuleViewId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKOrdinal", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKSkip", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKListingsPageLoadFlag", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKListingsPageLoad", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKViewDetail", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"hdnCKViewId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"loadMapFlag", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"SubscriberId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"SearchContact", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"IgnoreAllFilters", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsHomeSlider", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsHomeTab", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsHomeShowNotes", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsDetailTab", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsActivitySlider", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsSearchShowDetail", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsMatchesSlider", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsMatchesTab", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ContactsHistorySlider", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationLeft", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTopCMA", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationLeftCMA", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"cssMlsId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"amrMLSID", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"amrUNK", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"amrSID", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ASPSESSIONIDAASRSSDR", @"DNGIKADBPJFJJNGEBNNPFELD", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"__RequestVerificationToken_L01MUy5QaW5lcmd50", @"61l5jYvh_HBmZfoz6rlp-pD4srABBuKyFUUJ5i2ReEPv0LNsvgyzow0wPI0Lv6VkVxguVZhw3kVIAo6W4dhxlQX6Acu6sofGmDUh__xsCR41", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"__utma", @"199506417.166604096.1526516765.1567286623.1567324962.41", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"__utmc", @"199506417", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"__utmb", @"199506417.1.10.1567324962", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"NET_SessionId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ASP.", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"H3", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"AgentId", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"LastLoginCookie", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"CookieLastH3Server", @"h3s%2Emlspin%2Ecom", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"LoginCookie", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"CookiePassword", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"CookieAgentId", @"", @"/", @"h3w.mlspin.com"));
    //                                                // DKzB9A1MntTFtnBN27rtdjYyL92YeztntuN22KPr5ZhAoF5P7cvTZDnPIsOLfDnqo2PxNy5
    //        reqCookies.Add(new Cookie(@"CookieCred", @"DKzB9A1MntTFtnBN27rtdjYyL92YeztntuN22KPr5ZhAoF5P7cvTZDnPIsOLfDnqo2PxNy5", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"CookieSavePassword", @"Y", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));
    //        reqCookies.Add(new Cookie(@"ScrollLocationTop", @"", @"/", @"h3w.mlspin.com"));

    //        return reqCookies;
    //    }
    //}
}
