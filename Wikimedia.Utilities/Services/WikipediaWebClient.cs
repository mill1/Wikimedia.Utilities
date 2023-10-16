using System.Net.Http.Headers;
using System.Net.Http;
using Wikimedia.Utilities.Exceptions;
using Wikimedia.Utilities.Interfaces;
using Newtonsoft.Json;
using Wikimedia.Utilities.Models;

namespace Wikimedia.Utilities.Services
{
    public class WikipediaWebClient : IWikipediaWebClient
    {
        private const string NamespaceArticle = "0";

        private readonly HttpClient client;

        public WikipediaWebClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "C# Application");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Retrieve the contents of the article as wikitext , also known as wiki markup or wikicode.
        /// </summary>
        /// <param name="article"></param>
        /// <param name="redirectedArticleName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve the number of direct links to an article on the English Wikipedia.
        /// "direct links" means links only from outside the article within the article namespace, excluding user pages.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public int GetWikimediaSearchDirectLinkCount(string article)
        {
            const int searchResultLimit = 1;

            article = $"%22{article.Replace(" ", "+")}%22";

            string uri = $@"https://en.wikipedia.org/w/api.php?action=query&format=json&list=search&srnamespace={NamespaceArticle}&srlimit={searchResultLimit}&utf8=1&formatversion=2&srprop=size&srsearch=linksto%3A{article}+insource%3A{article}";
            var jsonString = client.GetStringAsync(uri).Result;
            var result = JsonConvert.DeserializeObject<WikimediaSearchResult>(jsonString);

            return result.query.searchinfo.totalhits;
        }

        /// <summary>
        /// Retrieve the number of direct links to an article on the English Wikipedia.
        /// "direct" in Toolforge Link Count means "not through a redirect", not "not through a transclusion".
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public int GetToolforgeDirectLinkCount(string article)
        {            
            article = article.Replace(" ", "_");

            string uri = $@"https://linkcount.toolforge.org/api/?page={article}&namespaces={NamespaceArticle}&project=en.wikipedia.org";
            var jsonString = client.GetStringAsync(uri).Result;
            var result = JsonConvert.DeserializeObject<ToolforgeLinkCount>(jsonString);

            return result.wikilinks.direct;
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
