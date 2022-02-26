using System;
using System.Collections.Generic;
using Wikimedia.Utilities.Dtos;

namespace Wikimedia.Utilities.Interfaces
{
    public interface IWikiTextService
    {
        string GetWikiTextDeathsPerMonth(DateTime deathDate, bool removeSublists, string listArticleName = null);
        string GetDaySectionOfMonthList(string wikiText, int day);
        IEnumerable<string> GetDeceasedTextAsList(string daySection);
        DateTime ResolveDateOfBirth(WikipediaListItemDto entry, string wikiText);
        string ResolveDescription(string wikiText);
        DateTime ResolveDate(string wikiText, DateTime date);
        string ResolveCauseOfDeath(string wikiText);
        string ResolveCauseOfDeath(WikipediaListItemDto entry);
        string ResolveKnownFor(string wikiText, string description);        
        string TrimWikiText(string wikiText, string month, int year);
        string GetNameFromEntryText(string entryText, bool linkedName);
        string GetInformationFromEntryText(string entryText);
        string GetReferencesFromEntryText(string entryText);
        string SanitizeDescription(string description);
        string GetReferenceUrlFromReferenceText(string reference);
        public bool DescriptionContainsCauseOfDeath(string description, string causeOfDeath);
    }
}
