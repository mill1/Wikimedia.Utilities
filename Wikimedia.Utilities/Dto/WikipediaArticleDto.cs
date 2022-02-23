using System;

namespace Wikimedia.Utilities.Dtos
{
    public class WikipediaArticleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public int LinksToArticleCount { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string CauseOfDeath { get; set; }
    }
}
