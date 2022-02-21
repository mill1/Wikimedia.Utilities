using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Models;

namespace Wikimedia.Utilities.Services
{
    public class ToolforgeService : IToolforgeService
    {
        private readonly HttpClient client;
        private readonly ILogger<ToolforgeService> logger;

        public ToolforgeService(HttpClient client, ILogger<ToolforgeService> logger)
        {
            this.logger = logger;
            this.client = client;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "C# Application");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Wikilinks GetWikilinksInfo(string article)
        {
            try
            {
                const string NamespaceArticle = "0";
                article = article.Replace(" ", "_");

                string uri = $@"https://linkcount.toolforge.org/api/?page={article}&namespaces={NamespaceArticle}&project=en.wikipedia.org";
                var jsonString = client.GetStringAsync(uri).Result;
                var result = JsonConvert.DeserializeObject<LinkCount>(jsonString);

                return result.wikilinks;
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
