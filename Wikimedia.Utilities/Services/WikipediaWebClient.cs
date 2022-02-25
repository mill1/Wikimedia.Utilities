using Microsoft.Extensions.Logging;
using System;
using Wikimedia.Utilities.Exceptions;
using Wikimedia.Utilities.Interfaces;

namespace Wikimedia.Utilities.Services
{
    public class WikipediaWebClient : IWikipediaWebClient
    {
        private readonly ILogger<WikipediaWebClient> logger;        

        public WikipediaWebClient(ILogger<WikipediaWebClient> logger)
        {
            this.logger = logger;            
        }

        public string GetWikiTextArticle(string article, out string redirectedArticleName)
        {
            string wikiText = FetchWikiTextArticle(article);

            if (wikiText.Contains("#REDIRECT"))
            {
                redirectedArticleName = GetRedirectArticleName(wikiText);
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
            catch (System.Net.WebException e) // article does not exist (anymore) in Wikipedia
            {
                var message = $"{article}: FAIL: no such wikipedia article";
                logger.LogError(message, e);
                throw new WikipediaPageNotFoundException(message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                throw;
            }
        }

        private string GetRedirectArticleName(string wikiText)
        {            
            int pos = wikiText.IndexOf("[[");
            string redirectPage = wikiText.Substring(pos + 2);
            pos = redirectPage.IndexOf("]]");

            return redirectPage.Substring(0, pos);
        }
    }
}
