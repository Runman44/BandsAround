using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using BandsinTown.Parser;

namespace BandsinTown
{
    public partial class FavoritePage : PhoneApplicationPage
    {
        public FavoritePage()
        {
            InitializeComponent();
            ObservableCollection<Artist> favoriteList = Settings.GetValueOrDefault<ObservableCollection<Artist>>("favorites", null);
            favoriteArtists.DataContext = favoriteList;
        }


        private void favoriteArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0)
            {

                var musicEvent = e.AddedItems[0] as Artist;
                if (musicEvent != null)
                {
                    NavigationService.Navigate(new Uri("/ArtistPage.xaml?msg=" + musicEvent.id, UriKind.Relative));
                }

            }
        }
    }
}