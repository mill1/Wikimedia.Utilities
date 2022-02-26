using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Wikimedia.Utilities.Dtos;
using Wikimedia.Utilities.Exceptions;
using Wikimedia.Utilities.ExtensionMethods;
using Wikimedia.Utilities.Interfaces;

namespace Wikimedia.Utilities.Services
{
    // https://en.wikipedia.org/wiki/Help:Wikitext
    public class WikiTextService : IWikiTextService
    {
        private const int InitialMaxLengthDescription = 500;
        private const int InitialPosEnd = 100000;
        private const string EntryDelimiter = "~!&#~[[";

        public string GetWikiTextDeathsPerMonth(DateTime deathDate, bool removeSublists, string listArticleName = null)
        {
            string text;
            string month = deathDate.ToString("MMMM", new CultureInfo("en-US"));
            string articleName = listArticleName == null ? $"Deaths in {month} {deathDate.Year}" : listArticleName;

            text = new WikipediaWebClient().GetWikiTextArticle(articleName, out string redirectedArticle);

            if (redirectedArticle != null)
                // No deaths per month article yet. Redirected to Year page
                return null;

            text = TrimWikiText(text, month, deathDate.Year);

            if (text.Contains("* "))
                // Let's see how this goes.. Voordeel: ook evt. ** [[. Nadeel: mogelijk in referenties
                throw new InvalidWikipediaPageException($"Invalid markup style found: '* '. Fix the article");

            if (removeSublists)
                text = RemoveSubLists(text);
            
            // Flatten the sublists
            text = text.Replace("**[[", "*[[");
            text = text.Replace("*[[", EntryDelimiter); // See GetDeceasedTextAsList(); solves hassle with "M*A*S*H, "NOC*NSF, * in references etc.

            if (text.Contains("* "))
                throw new InvalidWikipediaPageException($"Invalid markup style found: '* '. Fix the article");

            //  'alphabetisation' tags: aanpassen in wiki:
            // FOUT: *<!-- M -->[[Mei Baojiu]], 82
            // GOED: <!-- M -->*[[Mei Baojiu]], 82
            // see https://en.wikipedia.org/w/index.php?title=Deaths_in_April_2016&oldid=prev&diff=1048507267

            return text;
        }

        public string GetDaySectionOfMonthList(string wikiText, int day)
        {
            string daySection = wikiText;
            int pos;

            //Trim left
            pos = Math.Max(daySection.IndexOf($"==={day}==="), daySection.IndexOf($"=== {day} ==="));

            if (pos == -1)
                throw new InvalidWikipediaPageException($"Invalid day section header found. Day: {day}, Text: '{wikiText.Substring(0, 16)}...'");

            daySection = daySection.Substring(pos);

            // Trim right
            pos = Math.Max(daySection.IndexOf($"==={day + 1}==="), daySection.IndexOf($"=== {day + 1} ==="));

            if (pos == -1) // we reached the end
                return daySection;
            else
                return daySection.Substring(0, pos);
        }

        public IEnumerable<string> GetDeceasedTextAsList(string daySection)
        {
            string[] array = daySection.Split(EntryDelimiter);

            IEnumerable<string> deceasedWikiText = array.Select(e => "[[" + e);

            return deceasedWikiText.Skip(1);
        }

        private string RemoveSubLists(string text)
        {
            if (!text.Contains("**[["))
                return text;

            text = text.Replace("**[[", "~~[[");

            var entries = text.Split('*').Skip(1).ToList();

            entries.ForEach(entry =>
            {
                if (entry.Substring(0, 2) != "[[")
                    text = RemoveSubList(entry, text);
            });

            return text;
        }

        private string RemoveSubList(string subList, string text)
        {
            int pos = subList.IndexOf("==");

            if (pos == -1)
                throw new InvalidWikipediaPageException($"Invalid markup found: no section found after sub list. Fix the article");

            subList = subList.Substring(0, pos);

            return text.Replace($"*{subList}", string.Empty);
        }

        public string GetReferenceUrlFromReferenceText(string reference)
        {
            if (reference == null)
                return null;

            string string1 = "[http";
            int posStart = reference.IndexOf(string1);

            if (posStart == -1)
            {
                string1 = "url=http";
                posStart = reference.IndexOf(string1);
            }

            if (posStart == -1)
                return null;

            string string2 = ResolveUrlEndPosition(reference, posStart);
            const int trunc = 5; // <ref> or something else
            posStart = reference.IndexOf("http");
            reference = reference.Substring(posStart - trunc);
            string1 = reference.Substring(0, trunc);

            return reference.ValueBetweenTwoStrings(string1, string2).Trim();
        }

        private string ResolveUrlEndPosition(string reference, int posStart)
        {
            // 'Loop through possible strings following an url in a reference
            var refEnds = new[] { " ", "|", "]", "}}" };
            string string2 = null;

            int posEnd = -1;
            foreach (var refEnd in refEnds)
            {
                int posCandidate = reference.IndexOf(refEnd, posStart + 1);

                if (posCandidate != -1)
                {
                    if (posEnd == -1)
                    {
                        posEnd = posCandidate;
                        string2 = refEnd;
                    }
                    else
                    {
                        if (posCandidate < posEnd)
                        {
                            posEnd = posCandidate;
                            string2 = refEnd;
                        }
                    }
                }
            }

            if (string2 == null)
                throw new InvalidWikipediaPageException($"No valid value found following url. Reference: {reference}");

            return string2;
        }

        public string SanitizeDescription(string description)
        {
            if (IsAmericanPoliticianExclusively(description))
                return "American politician";

            description = description.Replace("one of the most ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("eminent ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("prominent ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("renowned ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("famous ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("well-known ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("celebrated ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("distinguished ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("noted ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("notable ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("leading ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("prolific ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("virtuoso ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("acknowledged ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("influential ", string.Empty, StringComparison.Ordinal);
            description = description.Replace("former ", string.Empty, StringComparison.Ordinal);

            if (description.Contains("football", StringComparison.OrdinalIgnoreCase))
                description = SanitizeFootballDescription(description);

            description = description.Replace("cricketer", "cricket player", StringComparison.Ordinal);

            if (!description.Contains("professional wrestl", StringComparison.Ordinal)) // wrestler / wrestling
                description = description.Replace("professional ", string.Empty);

            // character actor
            description = description.Replace("character act", "act"); // actor / actress

            // Remove "(1900 – 1996)" or "(1900-1996)"      [\s]*  betekent 0 of meer spaties
            Regex regex = new Regex(@"\s{1}\(\d{4}[\s]*[-–—][\s]*\d{4}\)");

            var res = regex.Match(description);

            if (res.Success)
                description = description.Replace(res.Value, string.Empty);

            return description.TruncLastPoint().CapitalizeFirstLetter().Trim();
        }

        private bool IsAmericanPoliticianExclusively(string description)
        {
            if (description.Contains(" and ")) // was an American attorney and politician
                return false;

            // 1. 
            var adjectives = new List<string> { "U.S.", "United States", "Republican", "Democratic", "American" };
            var nouns = new List<string> { "representative", "congressman", "congresswoman", "politician" }; // NB: excluding 'member' and 'official'

            foreach (var adjective in adjectives)
                foreach (var noun in nouns)
                    if (description.Contains(adjective) && description.Contains(noun, StringComparison.OrdinalIgnoreCase))
                        return true;

            // 2.
            if (description.Contains("Maryland House of Delegates") || description.Contains("Virginia House of Delegates") || description.Contains("West Virginia House of Delegates"))
                return true;

            var bodies = new List<string> { "House of Representatives", "General Assembly", "Legislature", "congressional district", "Senate" };

            foreach (var state in GetUSStates())
                foreach (var body in bodies)
                    if (description.Contains(state) && description.Contains(body, StringComparison.OrdinalIgnoreCase))
                        return true;

            return false;
        }

        private string SanitizeFootballDescription(string description)
        {
            if (description.Contains("Canadian Football", StringComparison.OrdinalIgnoreCase) && !(description.Contains("coach", StringComparison.OrdinalIgnoreCase) || description.Contains("referee", StringComparison.OrdinalIgnoreCase)))
                return "Canadian football player";

            if (description.Contains("American", StringComparison.OrdinalIgnoreCase) && !(description.Contains("coach", StringComparison.OrdinalIgnoreCase) || description.Contains("referee", StringComparison.OrdinalIgnoreCase)))
                return "American gridiron football player";

            if (description.Contains("rugby league football player", StringComparison.OrdinalIgnoreCase))
                return "rugby player";

            description = description.Replace("association football", "football", StringComparison.Ordinal);
            description = description.Replace("footballer", "football player", StringComparison.Ordinal);

            return description;
        }

        public string ResolveDescription(string wikiText)
        {
            wikiText = RemoveReferences(wikiText);

            string description = GetInitialDescription(wikiText);

            if (description == null)
                return null;

            description = description.Replace("U.S. ", "American ", StringComparison.OrdinalIgnoreCase); // because of the end candidate '.'
            description = description.Replace("United States ", "American ", StringComparison.OrdinalIgnoreCase); // while we're at it.. (not the best location I admit.

            // Trucate string;  [,] [perhaps/probably] [best] known [mostly] for  ..  etc.            
            string[] endCandidates = new string[] { "Infobox", "infobox", "{|", "{{", " who ", " whose ", " notable ", " noted ", " known ", " better ", " spanning ", " originally ",
                                                    " widely ",
                                                    " responsible ", " remembered ",  " best ", " most ", " perhaps ", " reputed ", " born ", " considered ", " particularly ", "." };

            int posEnd = GetPositionDescriptionEnd(description, endCandidates);

            if (posEnd == InitialPosEnd)
                throw new InvalidWikipediaPageException($"None of the {endCandidates.Length} 'description end' candidates found (including '.') within {InitialMaxLengthDescription} chars from 'description start'. Change the opening sentence of the article. Description: \r\n{description}");

            description = description.Substring(0, posEnd);

            // No more pipes present (removed references, infobox and tables : {| class="wikitable sortable")
            return RemoveWikiLinks(description);
        }

        private int GetPositionDescriptionEnd(string description, string[] endCandidates)
        {
            int pos = InitialPosEnd;

            foreach (var endCandidate in endCandidates)
            {
                var values = new string[] { endCandidate, $",{endCandidate}" };

                foreach (var value in values)
                {
                    if (description.Contains(value))
                    {
                        int candidate = description.IndexOf(value);
                        if (candidate < pos)
                            pos = candidate;
                    }
                }
            }
            return pos;
        }

        private string GetInitialDescription(string wikiText)
        {
            string[] descriptionStarts = new string[] { " was a ", " was an ", " was the ", " was one of ", " was " }; // " was " als LAATSTE

            int pos = GetPositioninWikiText(wikiText, descriptionStarts);

            if (pos == -1)
                return null;

            return wikiText.Substring(pos, Math.Min(InitialMaxLengthDescription, wikiText.Length - pos));
        }

        private int GetPositioninWikiText(string wikiText, string[] descriptionStarts)
        {
            int posStart = InitialPosEnd;
            string startText = "";

            foreach (var (start, pos) in from start in descriptionStarts
                                         where wikiText.Contains(start)
                                         let pos = wikiText.IndexOf(start)
                                         where pos < posStart
                                         select (start, pos))
            {
                startText = start;
                posStart = pos;
            }

            if (startText == " was one of ")
                posStart = posStart + " was ".Length;  // include 'one of' in description
            else
                posStart = posStart + startText.Length;

            return posStart;
        }

        public string ResolveKnownFor(string wikiText, string description)
        {
            if (description == null)
                return null;

            if (description.Contains("baseball", StringComparison.OrdinalIgnoreCase) || description.Contains("MLB"))
                return "Baseball";
            else if (description.Contains("American football", StringComparison.OrdinalIgnoreCase) || description.Contains("Canadian football", StringComparison.OrdinalIgnoreCase) || description.Contains("gridiron football") || description.Contains("NFL"))
                return "American football";
            else if (description.Contains("Australian rules football", StringComparison.OrdinalIgnoreCase) || description.Contains("Gaelic football", StringComparison.OrdinalIgnoreCase))
                return null; // too many and too few            
            else if (wikiText.Contains("olympi", StringComparison.OrdinalIgnoreCase))   // From the description it's not always evident if it's an olympian -> look in article
                return "Olympics";
            else if (description.Contains("basketball", StringComparison.OrdinalIgnoreCase) || description.Contains("NBA"))
                return "Basketball";
            else if (description.Contains("hockey", StringComparison.OrdinalIgnoreCase) && !description.Contains("field hockey", StringComparison.OrdinalIgnoreCase) || description.Contains("NHL"))
                return "Hockey";
            else if (description.Contains("football", StringComparison.OrdinalIgnoreCase)) // Other types football of football have been addressed. After olympics because of quality football site
                return "Association football";
            else if (description.Contains("cycli", StringComparison.OrdinalIgnoreCase) || wikiText.Contains("bicycle", StringComparison.OrdinalIgnoreCase))
                return "Cyclist";
            else if (description.Contains("golf", StringComparison.OrdinalIgnoreCase))
                return "Golfer";

            return null;
        }

        public DateTime ResolveDateOfBirth(WikipediaListItemDto entry, string wikiText)
        {
            // Resolving the DoB is only of use when there are multiple entries regarding the same wikidata item otherwise we don't really care. No feature creep.

            if (entry.WikidataItem.DateOfBirth == DateTime.MinValue)
                return DateTime.MinValue; // Too much work for now to resolve the DoB in an article without knowing the DoB. Tip: zie dev branch mbt 1995 fix. Hoewel..

            return ResolveDate(wikiText, (DateTime)entry.WikidataItem.DateOfBirth);
        }

        public DateTime ResolveDate(string wikiText, DateTime date)
        {
            string monthName = date.ToString("MMMM", new CultureInfo("en-US"));

            // [\s]*  betekent 0 of meer spaties. 0{0,1} betekent 1 of geen nul. See https://regex101.com/ 0*
            string[] expressions =  {
                $@"{monthName}[\s]*{date.Day},[\s]*{date.Year}",
                $@"{date.Day}[\s]*{monthName}[\s]*{date.Year}",
                @"\|[\s]*" + date.Year + @"[\s]*\|[\s]*0{0,1}" + date.Month + @"[\s]*\|[\s]*0{0,1}" + date.Day + @"[\s]*"
            };

            foreach (var expression in expressions)
            {
                Regex regex = new Regex(expression);

                if (regex.IsMatch(wikiText))
                    return date;
            }

            return DateTime.MinValue;
        }

        private string Trunc(string text, string search)
        {
            if (!text.Contains(search))
                return text;

            return text.Substring(0, text.IndexOf(search));
        }

        private string RemoveWikiLinks(string text)
        {
            while (text.Contains("|"))
            {
                string removedLinkPart = RemoveLinkPartFromWikilink(text);

                if (removedLinkPart == text)
                    break;

                text = removedLinkPart;
            }
            text = text.Replace("[[", string.Empty);
            text = text.Replace("]]", string.Empty);

            return text;
        }

        private string RemoveLinkPartFromWikilink(string text)
        {
            int pos2 = text.IndexOf("|");

            int pos1 = text.LastIndexOf("[[", pos2);

            if (pos1 == -1)
                return text;

            pos1 = pos1 + "[[".Length;

            return text.Replace(text.Substring(pos1, pos2 - pos1 + 1), string.Empty);
        }

        private string RemoveReferences(string wikiText)
        {
            while (wikiText.Contains("<ref"))
            {
                string endRef = "</ref>";

                int pos1 = wikiText.IndexOf("<ref");
                int pos2 = wikiText.IndexOf(endRef, pos1 + 1);

                int pos2Alt = wikiText.IndexOf("/>", pos1 + 1);
                if (pos2Alt != -1 && pos2Alt < pos2)
                {
                    pos2 = pos2Alt;
                    endRef = "/>";
                }

                if (pos2 == -1)
                    break;

                wikiText = wikiText.Substring(0, pos1) + wikiText.Substring(pos2 + endRef.Length);
            }
            return wikiText;
        }

        public string GetNameFromEntryText(string entryText, bool linkedName)
        {
            string namePart = entryText.Substring("[[".Length, entryText.IndexOf("]]") - "]]".Length);
            int pos = namePart.IndexOf('|');
            string name;

            if (pos < 0)
                name = namePart;
            else
            {
                if (linkedName)
                    name = namePart.Substring(0, pos);
                else
                    name = namePart.Substring(pos + "|".Length);
            }

            name = CheckRedirection(linkedName, name);

            return name;
        }

        private string CheckRedirection(bool linkedName, string name)
        {
            string redirectedArticleName;

            //if linked name make sure it is not a redirect.
            if (linkedName)
            {
                new WikipediaWebClient().GetWikiTextArticle(name, out redirectedArticleName);

                if (redirectedArticleName != null)
                    return redirectedArticleName;
            }
            return name;
        }

        public string GetInformationFromEntryText(string entryText)
        {
            string info = entryText.Substring(entryText.IndexOf("]]") + "]]".Length);

            // Loose the first comma
            info = info.Substring(1).Trim();

            int posRef = info.IndexOf("<ref>");

            if (posRef < 0)
                return info;
            else
                return info.Substring(0, posRef);
        }

        public string GetReferencesFromEntryText(string entryText)
        {
            if (entryText.Contains("<ref "))
                throw new InvalidWikipediaPageException("Entry contains a named reference. Correct the entry");

            int pos = entryText.IndexOf("<ref>");

            if (pos == -1)
                return null;

            return entryText.Substring(pos);
        }

        public string TrimWikiText(string wikiText, string month, int year)
        {
            string trimmedText = wikiText;
            int pos;

            //Trim left
            pos = Math.Max(trimmedText.IndexOf($"=={month} {year}=="), trimmedText.IndexOf($"== {month} {year} =="));

            if (pos == -1)
                throw new InvalidWikipediaPageException($"Not found:  ==[]{ month } { year}[]== ");

            trimmedText = trimmedText.Substring(pos);

            // Trim right
            pos = Math.Max(trimmedText.IndexOf("==References=="), trimmedText.IndexOf("== References =="));

            if (pos == -1)
                throw new InvalidWikipediaPageException($"Not found:  ==[]References[]== ");

            trimmedText = trimmedText.Substring(0, pos);

            // Loose '\n'
            trimmedText = trimmedText.Replace("\n", "");

            return trimmedText;
        }

        public string ResolveCauseOfDeath(string wikiText)
        {
            // Alleen i.g.v. Infobox: cause of death (waarschijnlijk overgenomen door Wikidata, maar goed)
            string causeOfDeath = null;
            // [\s]*  betekent 0 of meer spaties
            Regex regex = new Regex(@"\|[\s]*[Dd]eath_cause[\s]*=[\s]*");

            var res = regex.Match(wikiText);

            if (res.Success)
            {
                wikiText = wikiText.Replace(Environment.NewLine, string.Empty);

                // bijv. res is '| death_cause   = '
                int posStart = wikiText.IndexOf(res.ToString()) + res.ToString().Length;
                int posSecondPipe = wikiText.IndexOf("|", posStart); // First pipe is part of res :)

                if (posSecondPipe == -1)
                    return null;

                // [[Hal Robson]] Because of regex.Match existence of the infobox is implied.
                if (wikiText.IndexOf("}}", posStart) < posSecondPipe)
                    return null;

                // What is the meaning of this pipe? wikilink or listbox separator?
                int posLinkStart = wikiText.IndexOf("[[", posSecondPipe);
                int posLinkEnd = wikiText.IndexOf("]]", posSecondPipe);

                if (posLinkStart < posLinkEnd)
                    //  | [[ ... ]]    -> pipe is a listbox separator. 
                    causeOfDeath = RemoveWikiLinks(wikiText.Substring(posStart, posSecondPipe - posStart));
                else
                    //  | ]] ... [[    -> pipe is part of a wiki-link
                    causeOfDeath = RemoveWikiLinks(wikiText.Substring(posStart, posSecondPipe - posStart).Trim());

                causeOfDeath = Trunc(causeOfDeath, "<ref");
                causeOfDeath = causeOfDeath.Trim().ToLower();
                return causeOfDeath == string.Empty ? null : causeOfDeath;
            }
            return causeOfDeath;
        }

        public string ResolveCauseOfDeath(WikipediaListItemDto entry)
        {
            string causeOfDeath;

            if (entry.WikidataItem.CauseOfDeath == null && entry.WikipediaArticle.CauseOfDeath == null)
                return string.Empty;

            if (entry.WikidataItem.CauseOfDeath == null && entry.WikipediaArticle.CauseOfDeath != null)
                causeOfDeath = ", " + entry.WikipediaArticle.CauseOfDeath;
            else
                //favor wikidata in case both are available
                causeOfDeath = ", " + entry.WikidataItem.CauseOfDeath;

            causeOfDeath = causeOfDeath.Replace("death from ", string.Empty);
            causeOfDeath = causeOfDeath.Replace("hanging", "suicide");
            causeOfDeath = causeOfDeath.Replace("falling", "suicide");
            causeOfDeath = causeOfDeath.Replace("executed", "execution");
            causeOfDeath = causeOfDeath.Replace("leukaemia", "leukemia");
            causeOfDeath = causeOfDeath.Replace("drowning", "drowned");
            causeOfDeath = causeOfDeath.Replace("alzheimer", "Alzheimer");
            causeOfDeath = causeOfDeath.Replace("parkinson", "Parkinson");
            causeOfDeath = causeOfDeath.Replace("myocardial infarction", "heart attack");
            causeOfDeath = causeOfDeath.Replace("cardiac arrest", "heart attack");
            causeOfDeath = causeOfDeath.Replace("heart problems", "heart failure");

            return causeOfDeath;
        }

        // Check if the existing entry already states the cod. Also see previous method.
        public bool DescriptionContainsCauseOfDeath(string description, string causeOfDeath)
        {
            if (description.Contains(causeOfDeath.Replace(", ", ""), StringComparison.OrdinalIgnoreCase))
                return true;

            if (new[] {
            "cancer",
            "heart attack",
            "failure", // heart.. respiratory.. organ.. liver 
            "complications ",
            "pneumonia",
            "suicide",
            "accident", // car .. traffic.. domestic
            }.Any(s => description.Contains(s)))
                return true;

            if (new[] {
            "leukemia",
            "drowned",
            "killed",
            "shot",
            "murdered",
            "homicide",
            "stabbed",
            "collision",
            "crash",  // plane.. car.. automobile 
            "Alzheimer",
            "Parkinson",
            "stroke",
            "disease", // heart.. cardiovascular
            "aneurysm",
            "melanoma",
            "AIDS",
            "lymphoma", // Hodgkin.. Non-Hodgkin
            "execution",
            "overdose",
            "cirrhosis",
            "hemorrhage",
            "emphysema",
            "ailment", // lung ..
            "embolism",
            }.Any(s => description.Contains(s)))
                return true;

            return false;
        }

        public List<string> GetUSStates()
        {
            return new List<string>
            {
                "Alabama",
                "Alaska",
                "Arizona",
                "Arkansas",
                "California",
                "Colorado",
                "Connecticut",
                "Delaware",
                "Florida",
                "Georgia",
                "Hawaii",
                "Idaho",
                "Illinois",
                "Indiana",
                "Iowa",
                "Kansas",
                "Kentucky",
                "Louisiana",
                "Maine",
                "Maryland",
                "Massachusetts",
                "Michigan",
                "Minnesota",
                "Mississippi",
                "Missouri",
                "Montana",
                "Nebraska",
                "Nevada",
                "New Hampshire",
                "New Jersey",
                "New Mexico",
                "New York",
                "North Carolina",
                "North Dakota",
                "Ohio",
                "Oklahoma",
                "Oregon",
                "Pennsylvania",
                "Rhode Island",
                "South Carolina",
                "South Dakota",
                "Tennessee",
                "Texas",
                "Utah",
                "Vermont",
                "Virginia",
                "Washington",
                "West Virginia",
                "Wisconsin",
                "Wyoming"
            };
        }
    }
}
