using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using Xunit;

namespace Wikimedia.Utilities.Tests
{
    public class WikipediaWebClientShould
    {
        [Fact(DisplayName = "get the number of direct links to an article")]
        public void GetNumberOfDirectLinks()
        {
            IWikipediaWebClient clientService = new WikipediaWebClient();

            var text = clientService.GetWikiTextArticle("User:Mill_1/Lesley_Cunliffe", out string r);

            Assert.Equal(3913, text.Length);
        }
    }
}
