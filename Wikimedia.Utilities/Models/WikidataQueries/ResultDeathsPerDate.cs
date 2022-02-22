using System;

namespace Wikimedia.Utilities.Models.WikidataQueries.DeathsPerDate
{

    public class ResultDeathsPerDate
    {
        public Head head { get; set; }
        public Results results { get; set; }
    }

    public class Head
    {
        public string[] vars { get; set; }
    }

    public class Results
    {
        public Binding[] bindings { get; set; }
    }

    public class Binding
    {
        public Item item { get; set; }
        public Dob dob { get; set; }
        public Dod dod { get; set; }
        public Dod_Refs dod_refs { get; set; }
        public Cod cod { get; set; }
        public Articlename articlename { get; set; }
        public Sl sl { get; set; }
        public Itemlabel itemLabel { get; set; }
        public Itemdescription itemDescription { get; set; }
        public Mod mod { get; set; }
    }

    public class Item
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Dob
    {
        public string datatype { get; set; }
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Dod
    {
        public string datatype { get; set; }
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Dod_Refs
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Cod
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Articlename
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Sl
    {
        public string datatype { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Itemlabel
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Itemdescription
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Mod
    {
        public string xmllang { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
}
