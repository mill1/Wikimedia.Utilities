using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Models.WikidataQueries.SitelinksOfArticle;
using Wikimedia.Utilities.Services;
using Xunit;

namespace Wikimedia.Utilities.Tests
{
    public class WikipediaWebClientShould
    {
        [Fact(DisplayName = "get the contents of an article as wikitext")]
        public void GetWikiTextOfAnArticle()
        {
            IWikipediaWebClient client = new WikipediaWebClient();

            var text = client.GetWikiTextArticle("User:Mill_1/Lesley_Cunliffe", out string r);

            Assert.Equal(3916, text.Length);
        }

        [Fact(DisplayName = "get the number of direct links from Wikimedia Search")]
        public void GetWikimediaSearchDirectLinkCount()
        {
            IWikipediaWebClient client = new WikipediaWebClient();

            var count = client.GetWikimediaSearchDirectLinkCount("Kudrat Singh");

            Assert.Equal(2, count);
        }

        [Fact(DisplayName = "get the number of direct links from Wikimedia Search")]
        public void GetToolforgeDirectLinkCount()
        {
            IWikipediaWebClient client = new WikipediaWebClient();

            var count = client.GetToolforgeDirectLinkCount("Kudrat Singh");

            Assert.Equal(680, count);
        }
    }
}
