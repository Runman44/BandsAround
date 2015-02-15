using BandsAround;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandsinTown.Parser
{
    interface IParser
    {
        Task<ObservableCollection<EventSearch>> getEventFeed(Uri feed);

        Task<Event> getEvent(Uri feed);

        Task<ObservableCollection<ArtistSearch>> getArtistFeed(Uri feed);

        Task<Artist> getArtist(Uri feed);


    }
}
