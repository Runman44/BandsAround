using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BandsinTown.Parser
{
    public class ArtistSearch
    {
         [XmlElement("id")]
        public String idFormat
        {
            get { return id; }
            set { id = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
         [XmlIgnore]
        public String id { get; set; }
         [XmlElement("name")]
         public String nameFormat
         {
             get { return name; }
             set { name = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
         }
         [XmlIgnore]
         public String name { get; set; }
       
         [XmlElement("short_bio")]
         public String bioFormat
        {
            get { return bio; }
            set { bio = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
         [XmlIgnore]
         public String bio { get; set; }


         public ArtistSearch()
         { }

    }
}
