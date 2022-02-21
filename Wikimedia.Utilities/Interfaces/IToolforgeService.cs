using Wikimedia.Utilities.Models;

namespace Wikimedia.Utilities.Interfaces
{
    public interface IToolforgeService
    {
        public Wikilinks GetWikilinksInfo(string article);
    }
}
