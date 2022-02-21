using Microsoft.Extensions.Logging;
using Wikimedia.Utilities.Interfaces;

namespace Wikimedia.Utilities.Services
{
    public class WikidataService: IWikidataService
    {
        private readonly ILogger<WikidataService> logger;

        public WikidataService(ILogger<WikidataService> logger)
        {
            this.logger = logger;
        }

        public void MyMethod()
        {
            throw new System.NotImplementedException();
        }
    }
}
