namespace Wikimedia.Utilities.Interfaces
{
    public interface IWikipediaWebClient
    {
        string GetWikiTextArticle(string article, out string redirectedArticleName);
    }
}
