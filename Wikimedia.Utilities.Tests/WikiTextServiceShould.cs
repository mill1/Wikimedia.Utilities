using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using Xunit;

namespace WikipediaDeathsPages.Tests
{
    public class WikiTextServiceShould
    {
        private readonly IWikiTextService wikiTextService;

        public WikiTextServiceShould()
        {
            wikiTextService = new WikiTextService();
        }

        [Theory]
        [InlineData("{{ .. | Death_cause   =    Lobar pneumonia         |         resting_place  =  ..  }} ")]
        [InlineData("{{ .. | death_cause   =    Lobar pneumonia         |         resting_place  =  ..  }} ")]
        [InlineData("{{ .. | death_cause   =  [[Lobar pneumonia]]       |         resting_place  =  ..  }} ")]
        [InlineData("{{ .. | death_cause   =  [[Lobar pneumonia|lung stuff]]   |  resting_place  =  ..  }} ")]
        public void ResolveCauseOfDeathInWPInfobox(string wikiText)
        {
            var actual = wikiTextService.ResolveCauseOfDeath(wikiText);

            Assert.Equal("lobar pneumonia", actual);
        }

        [Theory(DisplayName = "Resolve the description from a Wikipedia article")]
        [InlineData("x was a Japanese actor who appeared..")]
        [InlineData("x was a Japanese actor, who appeared..")]
        [InlineData("x was a Japanese actor best remembered for..")]
        public void ResolveDescriptionInArticle(string wikiText)
        {
            var actual = wikiTextService.ResolveDescription(wikiText);

            Assert.Equal("Japanese actor", actual);
        }

        [Theory(DisplayName = "Resolve American politician as description")]
        [InlineData("American Democratic Party politician")]
        [InlineData("member of the Virginia House of Delegates")]
        [InlineData("U.S. Congressman from Arizona")]
        [InlineData("member of the United States House of Representatives from New York")]
        public void SanitizeDescriptionIntoAmericanPolitician(string description)
        {
            var actual = wikiTextService.SanitizeDescription(description);

            Assert.Equal("American politician", actual);
        }

        [Theory(DisplayName = "Trim Right WikiText")]
        [InlineData("==References==")]
        [InlineData("== References ==")]
        [InlineData("==References ==")]
        [InlineData("== References==")]
        public void TrimRightWikiText(string subtext)
        {
            var month = "May";
            var year = 2004;
            var title = $"=={month} {year}==";

            var text = $"Start\n{title}\nContents\n{subtext}\nEnd";

            var actual = wikiTextService.TrimWikiText(text, month, year);

            Assert.Equal("==May 2004==Contents", actual);
        }
    }
}
