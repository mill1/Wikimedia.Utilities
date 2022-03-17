using System;
using System.Linq;
using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using Xunit;

namespace Wikimedia.Utilities.Tests
{
    public class WikidataServiceShould
    {
        [Fact(DisplayName = "get a list of items per death data")]
        public void GetListOfDeceasedAllField()
        {
            IWikidataService wikidataService = new WikidataService();

            var deathDate = new DateTime(1997, 3, 28);

            var items = wikidataService.GetItemsPerDeathDate(deathDate, false);

            Assert.Equal(6, items.ToList().Count);
        }

        [Fact(DisplayName = "get a list of items per death data, core fields only")]
        public void GetListOfDeceasedCoreFields()
        {
            IWikidataService wikidataService = new WikidataService();

            var deathDate = new DateTime(1997, 3, 28);

            var items = wikidataService.GetItemsPerDeathDate(deathDate, true);

            Assert.Null(items.First().DateOfBirth);
            Assert.Equal(6, items.ToList().Count);
        }

        [Fact(DisplayName = "get the number of site links of an article")]
        public void GetNumberOfSiteLinks()
        {
            IWikidataService wikidataService = new WikidataService();

            var item = wikidataService.GetSitelinksResult("Jan Pelleboer");

            Assert.Equal("Jan Pelleboer", item.ArticleName);
            Assert.Equal("Jan Pelleboer", item.Label);
            Assert.Equal(3, item.SiteLinksCount);
            Assert.Equal("http://www.wikidata.org/entity/Q2170771", item.Uri);
        }
    }
}
