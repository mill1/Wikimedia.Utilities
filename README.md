# Wikimedia.Utilities
Utility library that contains generic functionality for my Wikimedia and Wikidata projects.

### Personal library
As per 2023 I created four repositories that assists me in my endeavour to improve the quality of the [Deaths per month articles](https://en.wikipedia.org/wiki/Lists_of_deaths_by_year) on Wikipedia.
These repo's (web applications and console utilities) make use of this library to access common functionality. An example is fetching the raw wikitext of a Wikipedia article.

The utility offers four types of services:
* Wikitext service: functionality directed at text manipulation and resolving data from the [wikitext](https://en.wikipedia.org/wiki/Help:Wikitext) of a Wikipedia page
* WikipediaWebClient: a Wikipedia WebClient wrapper
* WikidataService: contains features that fetch data from Wikidata and resolves and manipulates its content
* ToolforgeService: service that accesses toolforge.org in order to fetch [information on incoming links](https://linkcount.toolforge.org/?project=en.wikipedia.org&page=Clayton+Townsend) regarding a Wikipedia article.

### Nuget
This utility as is available [here](https://www.nuget.org/packages/Wikimedia.Utilities) as a NuGet package.
