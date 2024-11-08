# Wikimedia.Utilities
Utility library that contains generic functionality for my Wikipedia and Wikidata projects.

### Personal library
As per 2023 I created four repositories that assist me in my endeavour to improve the quality of the [Deaths per month articles](https://en.wikipedia.org/wiki/Lists_of_deaths_by_year) on Wikipedia.
These repo's (web applications and console utilities) make use of this library to access common functionality. An example is fetching the raw [wikitext](https://en.wikipedia.org/wiki/Help:Wikitext) of a Wikipedia article.

The utility offers four types of services:
* Wikitext service: functionality directed at text manipulation and resolving data from the wikitext of a Wikipedia page
* WikipediaWebClient: a WebClient wrapper for Wikipedia-related requests
* WikidataService: contains features that fetch data from Wikidata and resolves and manipulates its content

### NuGet
This utility as is available [here](https://www.nuget.org/packages/Wikimedia.Utilities) as a NuGet package.
