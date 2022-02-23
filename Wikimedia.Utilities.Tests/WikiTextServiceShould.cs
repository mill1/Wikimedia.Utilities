using Wikimedia.Utilities.Services;
using Xunit;

namespace WikipediaDeathsPages.Tests
{
    public class WikiTextServiceShould
    {
        private readonly WikiTextService wikiTextService;

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

    }
}
