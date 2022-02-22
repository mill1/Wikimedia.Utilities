namespace Wikimedia.Utilities.Models.WikidataQueries.SitelinksOfArticle
{


    public class ResultSitelinksOfArticle
    {
        public Head head { get; set; }
        public Results results { get; set; }
    }

    public class Head
    {
        public string[] vars { get; set; }
    }

    public class Results
    {
        public Binding[] bindings { get; set; }
    }

    public class Binding
    {
        public Article article { get; set; }
        public Item item { get; set; }
        public Sitelinks sitelinks { get; set; }
        public Itemlabel itemLabel { get; set; }
    }

    public class Article
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Item
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Sitelinks
    {
        public string datatype { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Itemlabel
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

}
