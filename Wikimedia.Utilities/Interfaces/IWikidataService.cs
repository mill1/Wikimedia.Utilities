using System;
using System.Collections.Generic;
using Wikimedia.Utilities.Dtos;

namespace Wikimedia.Utilities.Interfaces
{
    public interface IWikidataService
    {
        WikidataItemDto GetSitelinksResult(string article);
        IEnumerable<WikidataItemDto> GetItemsPerDeathDate(DateTime deathDate);
        DateTime ResolveDateOfBirth(WikidataItemDto wikiDataItemDto);
        string ResolveItemLabel(WikidataItemDto wikiDataItemDto);
        string ResolveItemDescription(WikidataItemDto wikiDataItemDto);
        string ResolveItemCauseOfDeath(WikidataItemDto wikiDataItemDto);
        string ResolveBiolink(WikidataItemDto wikiDataItemDto);
        string ResolveAge(WikidataItemDto wikiDataItemDto);
        string SanitizeDateOfDeathReferences(WikidataItemDto wikiDataItemDto);
    }
}
