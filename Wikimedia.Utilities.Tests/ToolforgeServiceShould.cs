using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using Xunit;

namespace Wikimedia.Utilities.Tests
{
    public class ToolforgeServiceShould
    {
        [Fact(DisplayName = "get the number of direct links to an article")]
        public void GetNumberOfDirectLinks()
        {
            IToolforgeService toolforgeService = new ToolforgeService();

            var info = toolforgeService.GetWikilinksInfo("Hanneke Niens");

            Assert.Equal(13, info.direct);
        }
    }
}
