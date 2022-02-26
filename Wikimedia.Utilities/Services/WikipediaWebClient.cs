using Wikimedia.Utilities.Exceptions;
using Wikimedia.Utilities.Interfaces;

namespace Wikimedia.Utilities.Services
{
    public class WikipediaWebClient : IWikipediaWebClient
    {
        public string GetWikiTextArticle(string article, out string redirectedArticleName)
        {
            string wikiText = FetchWikiTextArticle(article);

            if (wikiText.Contains("#REDIRECT"))
            {
                redirectedArticleName = GetRedirectArticleName(article, wikiText);
                wikiText = FetchWikiTextArticle(redirectedArticleName);
            }
            else
                redirectedArticleName = null;

            // Sanitize (wip)
            wikiText = wikiText.Replace("&nbsp;", " ");

            return wikiText;
        }

        private string FetchWikiTextArticle(string article)
        {
            const string UrlWikipediaRawBase = "https://en.wikipedia.org/w/index.php?action=raw&title=";
            string uri = UrlWikipediaRawBase + article.Replace(" ", "_");

            try
            {
                return new System.Net.WebClient().DownloadString(uri);
            }
            catch (System.Net.WebException) // article does not exist (anymore) in Wikipedia
            {
                var message = $"{article}: FAIL: no such wikipedia article";
                throw new WikipediaPageNotFoundException(message);
            }
        }

        private string GetRedirectArticleName(string article, string wikiText)
        {
            int pos = wikiText.IndexOf("[[");

            if (pos == -1)
                throw new InvalidWikipediaPageException($"{article}: #REDIRECT without '[['!");

            string redirectPage = wikiText.Substring(pos + 2);
            pos = redirectPage.IndexOf("]]");

            return redirectPage.Substring(0, pos);
        }
    }
}
