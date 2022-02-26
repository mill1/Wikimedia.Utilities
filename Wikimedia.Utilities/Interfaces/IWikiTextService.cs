using System;
using Wikimedia.Utilities.Dtos;

namespace Wikimedia.Utilities.Interfaces
{
    public interface IWikiTextService
    {
        DateTime ResolveDateOfBirth(EntryDto entry, string wikiText);
        string ResolveDescription(string wikiText);
        DateTime ResolveDate(string wikiText, DateTime date);
        string ResolveCauseOfDeath(string wikiText);
        string ResolveCauseOfDeath(EntryDto entry);
        string ResolveKnownFor(string wikiText, string description);
        string GetDaySectionOfMonthList(string wikiText, int day);
        string TrimWikiText(string wikiText, string month, int year);
        string GetNameFromEntryText(string entryText, bool linkedName);
        string GetInformationFromEntryText(string entryText);
        string GetReferencesFromEntryText(string entryText);
        string SanitizeDescription(string description);
        string GetReferenceUrlFromReferenceText(string reference);
        public bool DescriptionContainsCauseOfDeath(string description, string causeOfDeath);
    }
}
