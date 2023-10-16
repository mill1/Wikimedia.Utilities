namespace Wikimedia.Utilities.Interfaces
{
    public interface IWikipediaWebClient
    {
        /// <summary>
        /// Retrieve the contents of the article as wikitext , also known as wiki markup or wikicode.
        /// </summary>
        /// <param name="article"></param>
        /// <param name="redirectedArticleName"></param>
        /// <returns></returns>
        string GetWikiTextArticle(string article, out string redirectedArticleName);

        /// <summary>
        /// Retrieve the number of direct links to an article on the English Wikipedia.
        /// "direct links" means links only from outside the article within the article namespace, excluding user pages.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        int GetWikimediaSearchDirectLinkCount(string article);

        /// <summary>
        /// Retrieve the number of direct links to an article on the English Wikipedia.
        /// "direct" in Toolforge Link Count means "not through a redirect", not "not through a transclusion".
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        int GetToolforgeDirectLinkCount(string article);
    }
}
