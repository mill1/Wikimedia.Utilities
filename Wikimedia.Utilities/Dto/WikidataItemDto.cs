using System;

namespace Wikimedia.Utilities.Dtos
{
    public class WikidataItemDto
    {
        public string ArticleName { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public int SiteLinksCount { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string DateOfDeathRefs { get; set; }
        public string CauseOfDeath { get; set; }
        public string MannerOfDeath { get; set; }
    }
}
