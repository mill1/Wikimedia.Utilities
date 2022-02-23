using Wikimedia.Utilities.Dtos;

namespace Wikimedia.Utilities.Dtos
{
    public class EntryDto
    {
        public string Id { get; set; } // ID is gelijk voor WP- en Wikidata items
        public string WikiText { get; set; }
        public int NotabilityScore { get; set; }
        public string ReferenceUrl { get; set; }
        public string KnownFor { get; set; }
        public bool KeepExisting { get; set; }
        public WikidataItemDto WikidataItem { get; set; }
        public WikipediaArticleDto WikipediaArticle { get; set; }
    }
}
