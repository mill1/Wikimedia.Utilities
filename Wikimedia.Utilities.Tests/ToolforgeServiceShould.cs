using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Net.Http;
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
            var client = new HttpClient();
            var logger = new NullLogger<ToolforgeService>();
            IToolforgeService toolforgeService = new ToolforgeService(client, logger);

            var info = toolforgeService.GetWikilinksInfo("Hanneke Niens");

            Assert.Equal(13, info.direct);
        }
    }
}
