using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BandsinTown.Parser
{
  
    [XmlRoot("search")]
    public class Artists
    {
        [XmlArray("performers")]
        [XmlArrayItem("performer")]
        public ObservableCollection<ArtistSearch> eventCollection { get; set; }

        public Artists()
        { }
    }
}
