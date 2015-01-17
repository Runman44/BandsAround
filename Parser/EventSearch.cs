using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BandsAround
{
    public class EventSearch
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
         [XmlElement("venue_address")]
        public String venueAddressFormat
        {
            get { return venueAddress; }
            set { venueAddress = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
         [XmlIgnore]
        public String venueAddress { get;set;  }
         [XmlElement("city_name")]
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

         public EventSearch()
         { }

        public override string ToString()
        {
            return venueAddress + " - " + startTime.ToShortDateString();
        }
    }
}
