﻿namespace Wikimedia.Utilities.Models
{
    public class WikimediaSearchResult
    {
        public bool batchcomplete { get; set; }
        public Continue _continue { get; set; }
        public Query query { get; set; }
    }

    public class Continue
    {
        public int sroffset { get; set; }
        public string _continue { get; set; }
    }

    public class Query
    {
        public Searchinfo searchinfo { get; set; }
        public Search[] search { get; set; }
    }

    public class Searchinfo
    {
        public int totalhits { get; set; }
    }

    public class Search
    {
        public int ns { get; set; }
        public string title { get; set; }
        public int pageid { get; set; }
        public int size { get; set; }
    }
}
