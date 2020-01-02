using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Creative.System.Core;
using MassIdxIngestion;
using Moq;
using NUnit.Framework;
using Scraper.Core;

    namespace Scraper.Tests
{
    public class PinergyIdxListingRetreiverFixture
    {
        Mock<IFileSystem> _file;

        [SetUp]
        public void Setup()
        {
            _file = new Mock<IFileSystem>();

            //_file.Setup(async f => await f.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>(), It.IsAny<CancellationToken>())
            //.Returns<string, string, Encoding, CancellationToken>((path, contents, encoding, token) =>
            //{
            //    return null;
            //}));
        }

        [Test]
        public void Ctor()
        {
            var ret = new PinergyIdxListingRetreiver();
            Assert.That(ret, Is.Not.Null);
        }

        [Test]
        public async Task RetreiveFilesForDateRange_StartDateOneDayAfterEndDate_Throws()
        {
            var endDate = new DateTime(2019, 5, 24);
            var startDate = endDate.AddDays(1);
            var ret = new PinergyIdxListingRetreiver();

            try
            {
                await foreach (var f in ret.RetreiveFilesForDateRangeAsync(startDate, endDate))
                { }
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.ParamName, Is.EqualTo(@"endDate"));
                Assert.That(ex.Message, Is.EqualTo(@"endDate must be greater than or equal to startDate. (Parameter 'endDate')"));
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task RetreiveFilesForDateRange_StartDateSameMinutesAfterEndDate_ReturnsFourFiles()
        {
            var files = new List<string>();
            var endDate = new DateTime(2019, 5, 24);
            var startDate = endDate.AddMinutes(20);
            var ret = new PinergyIdxListingRetreiver();
            await foreach (var f in ret.RetreiveFilesForDateRangeAsync(startDate, endDate))
            {
                files.Add(f);
            }

            Assert.That(files.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task RetreiveFilesForDateRange_StartDateSameAsEndDate_ReturnsFourFiles()
        {
            var files = new List<string>();
            var endDate = new DateTime(2019, 5, 24);
            var startDate = endDate;
            var ret = new PinergyIdxListingRetreiver();
            await foreach(var f in ret.RetreiveFilesForDateRangeAsync(startDate, endDate))
            {
                files.Add(f);
            }

            Assert.That(files.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task RetreiveFilesForDateRange_StartDateSameAsEndDate_ReturnsFourProperlyNamedFiles()
        {
            var files = new List<string>();
            var endDate = new DateTime(2019, 5, 24);
            var startDate = endDate;
            var ret = new PinergyIdxListingRetreiver();
            await foreach (var f in ret.RetreiveFilesForDateRangeAsync(startDate, endDate))
            {
                files.Add(f);
            }

            files.Sort();

            Assert.That(files[0], Is.EqualTo(@"20190524_all_state_CTG_CC.csv"));
            Assert.That(files[1], Is.EqualTo(@"20190524_all_state_CTG_SF.csv"));
            Assert.That(files[2], Is.EqualTo(@"20190524_all_state_UAG_CC.csv"));
            Assert.That(files[3], Is.EqualTo(@"20190524_all_state_UAG_SF.csv"));
        }

        [Test]
        public async Task RetreiveFilesForDateRange_EndDateOneDayAfterStartDate_ReturnsEightFiles()
        {
            var files = new List<string>();
            var startDate = new DateTime(2019, 5, 24);
            var endDate = startDate.AddDays(1);
            var ret = new PinergyIdxListingRetreiver();
            await foreach (var f in ret.RetreiveFilesForDateRangeAsync(startDate, endDate))
            {
                files.Add(f);
            }

            Assert.That(files.Count, Is.EqualTo(8));
        }

        [Test]
        public async Task RetreiveFilesForDateRange_EndDateOneDayAfterStartDate_ReturnsEightProperlyNamedFiles()
        {
            var files = new List<string>();
            var startDate = new DateTime(2019, 5, 24);
            var endDate = startDate.AddDays(1);
            var ret = new PinergyIdxListingRetreiver();
            await foreach (var f in ret.RetreiveFilesForDateRangeAsync(startDate, endDate))
            {
                files.Add(f);
            }

            files.Sort();

            Assert.That(files[0], Is.EqualTo(@"20190524_all_state_CTG_CC.csv"));
            Assert.That(files[1], Is.EqualTo(@"20190524_all_state_CTG_SF.csv"));
            Assert.That(files[2], Is.EqualTo(@"20190524_all_state_UAG_CC.csv"));
            Assert.That(files[3], Is.EqualTo(@"20190524_all_state_UAG_SF.csv"));

            Assert.That(files[4], Is.EqualTo(@"20190525_all_state_CTG_CC.csv"));
            Assert.That(files[5], Is.EqualTo(@"20190525_all_state_CTG_SF.csv"));
            Assert.That(files[6], Is.EqualTo(@"20190525_all_state_UAG_CC.csv"));
            Assert.That(files[7], Is.EqualTo(@"20190525_all_state_UAG_SF.csv"));
        }
    }
}