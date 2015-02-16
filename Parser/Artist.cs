using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace BandsinTown.Parser
{
    [XmlRoot("performer")]
    public class Artist
    {
        [XmlAttribute("id")]
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
        public String shortBioFormat
        {
            get { return shortBio.ToString(); }
            set { shortBio = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
        [XmlIgnore]
        public String shortBio { get; set; }
        [XmlElement("long_bio")]
        public String longBioFormat
        {
            get { return longBio; }
            set { longBio = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
        [XmlIgnore]
        public String longBio { get; set; }

        [XmlArray("images")]
        [XmlArrayItem("image")]
        public List<Image> images {get; set;}

        [XmlArray("events")]
        [XmlArrayItem("event")]
        public ObservableCollection<ArtistEvent> events { get; set; }
        
         public Artist()
         { }


         public BitmapImage imageList()
         {
             return new BitmapImage(new Uri(images[0].url));
         }
    }

    [XmlRoot("medium")]
    public class Image {

        [XmlElement("url")]
        public String urlFormat
        {
            get { return url; }
            set { url = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
        [XmlIgnore]
        public String url { get; set; }
    }

    public class ArtistEvent
    {
        [XmlElement("id")]
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

        [XmlElement("location")]
        public String locationFormat
        {
            get { return location; }
            set { location = string.IsNullOrEmpty(value) != true ? value : "unknown"; }
        }
        [XmlIgnore]
        public String location { get; set; }
    }
}
