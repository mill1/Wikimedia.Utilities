using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Net.Http;
using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using Xunit;

namespace Wikimedia.Utilities.Tests
{
    public class WikidataServiceShould
    {
        [Fact(DisplayName = "get a list of items per death data")]
        public void GetListOfDeceased()
        {
            var client = new HttpClient();
            var logger = new NullLogger<WikidataService>();
            IWikidataService wikidataService = new WikidataService(client, logger);

            var deathDate = new DateTime(1997, 3, 28);

            var items = wikidataService.GetItemsPerDeathDate(deathDate);

            Assert.Equal(6, items.ToList().Count);
        }

        [Fact(DisplayName = "get the number of site links of an article")]
        public void GetNumberOfSiteLinks()
        {
            var client = new HttpClient();
            var logger = new NullLogger<WikidataService>();
            IWikidataService wikidataService = new WikidataService(client, logger);

            var item = wikidataService.GetSitelinksResult("Jan Pelleboer");

            Assert.Equal("Jan Pelleboer", item.ArticleName);
            Assert.Equal("Jan Pelleboer", item.Label);
            Assert.Equal(3, item.SiteLinksCount);
            Assert.Equal("http://www.wikidata.org/entity/Q2170771", item.Uri);
        }
    }
}
