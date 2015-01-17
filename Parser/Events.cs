using BandsAround;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BandsinTown
{
    [XmlRoot("search")]
    public class Events
    {
        [XmlArray("events")]
        [XmlArrayItem("event")]
        public ObservableCollection<EventSearch> eventCollection { get; set; }

        public Events()
        { }
    }

}
