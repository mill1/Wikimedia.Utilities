namespace Wikimedia.Utilities.Models
{
    public class LinkCount
    {
        public object filelinks { get; set; }
        public object categorylinks { get; set; }
        public Wikilinks wikilinks { get; set; }
        public int redirects { get; set; }
        public Transclusions transclusions { get; set; }
    }

    public class Wikilinks
    {
        public int all { get; set; }
        public int direct { get; set; }
        public int indirect { get; set; }
    }

    public class Transclusions
    {
        public int all { get; set; }
        public int direct { get; set; }
        public int indirect { get; set; }
    }
}
