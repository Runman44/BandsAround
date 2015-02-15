using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BandsinTown.Parser
{
    [XmlRoot("event")]
    public class Event
    {
        [XmlAttribute("id")]
        public String idFormat
        {
            get { return id; }
            set { id = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
        [XmlIgnore]
        public String id { get; set; }

         [XmlElement("title")]
        public String titleFormat
        {
            get { return title; }
            set { title = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
         [XmlIgnore]
         public String title { get; set; }
         [XmlElement("start_time")]
         public String startTimeFormat
         {
             get { return startTime.ToString(); }
             set { startTime = DateTime.Parse(value, CultureInfo.InvariantCulture); }
         }
        [XmlIgnore]
        public DateTime startTime { get; set; }
         [XmlElement("venue_name")]
        public String venueNameFormat
        {
            get { return venueName; }
            set { venueName = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
         [XmlIgnore]
        public String venueName{ get;set;  }
         [XmlElement("city")]
         public String cityNameFormat
         {
             get { return cityName; }
             set { cityName = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
         }
         [XmlIgnore]
         public String cityName { get;set; }
        [XmlIgnore]
        public double latitude {get;set;}
        [XmlElement("latitude")]
        public String latitudeFormat
        {
            get { return latitude.ToString(); }
            set { latitude = double.Parse(value, CultureInfo.InvariantCulture); }
        }
        [XmlIgnore]
        public double longitude { get; set; }

         [XmlElement("longitude")]
         public String longitudeFormat 
        {
            get { return longitude.ToString(); }
            set { longitude = double.Parse(value, CultureInfo.InvariantCulture); }
        }
         [XmlIgnore]
         public string price { get; set; }

         [XmlElement("price")]
         public String priceFormat
         {
             get { return price.ToString(); }
             set { price = Regex.Match(value, @"\d+").Value; }               
         }
         [XmlElement("url")]
         public String urlFormat
         {
             get { return url; }
             set { url = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
         }
         [XmlIgnore]
         public String url { get; set; }

         public Event()
         { }
    }
}
