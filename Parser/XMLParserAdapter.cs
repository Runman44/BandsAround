using BandsAround;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace BandsinTown.Parser
{
    class XMLParserAdapter : IParser
    {

        public async Task<ObservableCollection<ArtistSearch>> getArtistFeed(Uri feed)
        {
            var results = await Utils.downloadFeed(feed);

            XmlSerializer serializer = new XmlSerializer(typeof(Artists));
            StringReader rdr = new StringReader(results);
            Artists resultingMessage = (Artists)serializer.Deserialize(rdr);

            return resultingMessage.eventCollection;
        }

        public async Task<ObservableCollection<EventSearch>> getEventFeed(Uri feed)
        {
            var results = await Utils.downloadFeed(feed);
            
            XmlSerializer serializer = new XmlSerializer(typeof(Events));
            StringReader rdr = new StringReader(results);
            Events resultingMessage = (Events)serializer.Deserialize(rdr);

            return resultingMessage.eventCollection;
        }

        public async Task<Event> getEvent(Uri feed)
        {
            var results = await Utils.downloadFeed(feed);
            
            XmlSerializer serializer = new XmlSerializer(typeof(Event));
            StringReader rdr = new StringReader(results);
            Event resultingMessage = (Event)serializer.Deserialize(rdr);
            rdr.Close();

            return resultingMessage;
        }

        public async Task<Artist> getArtist(Uri feed)
        {
            var results = await Utils.downloadFeed(feed);

            XmlSerializer serializer = new XmlSerializer(typeof(Artist));
            StringReader rdr = new StringReader(results);
            Artist resultingMessage = (Artist)serializer.Deserialize(rdr);
            rdr.Close();

            return resultingMessage;
        }
        
    }
}
