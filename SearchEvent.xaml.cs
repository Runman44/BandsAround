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
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using BandsinTown.Parser;
using System.Windows.Media;
using BandsinTown;
using GoogleAds;

namespace BandsAround
{
    public partial class SearchEvent : PhoneApplicationPage
    {
        ProgressIndicator _progressIndicator;
        ObservableCollection<EventSearch> list = null;

        IParser iParser = new XMLParserAdapter();
        public SearchEvent()
        {
            InitializeComponent();
            _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = false;
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.Text = "loading";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            GoogleAnalytics.EasyTracker.GetTracker().SendView("SearchPage");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String text_search = textbox_search.Text;
                if (text_search != null)
                {
                    _progressIndicator.IsVisible = true;
                    list = null;
                    ObservableCollection<EventSearch> sortedList = null;
                    string when = Settings.GetValueOrDefault<string>("when", "Next+30+days");
                    string distance = Settings.GetValueOrDefault<string>("distance", "20");

                    list = await iParser.getEventFeed(new Uri("http://api.eventful.com/rest/events/search?&app_key=G79FxVMQbxjMTKMk&location=" + text_search + "&category=music&within=" + distance + "&date=" + when));
                    sortedList = new ObservableCollection<EventSearch>(list.OrderBy(Event => Event.startTime));
                    searchEvents.DataContext = sortedList;
                    _progressIndicator.IsVisible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, please try again");
                _progressIndicator.IsVisible = false;
            }
        }

        private async void Artist_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String text_search = artist_search.Text;
                if (text_search != null)
                {
                    _progressIndicator.IsVisible = true;
                    ObservableCollection<ArtistSearch> list = null;
                    //ObservableCollection<ArtistFeed> sortedList = null;

                    list = await iParser.getArtistFeed(new Uri("http://api.eventful.com/rest/performers/search?&app_key=G79FxVMQbxjMTKMk&keywords=" + text_search));
                   // sortedList = new ObservableCollection<ArtistFeed>(list));
                    searchArtists.DataContext = list;
                    _progressIndicator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, please try again");
                _progressIndicator.IsVisible = false;
            }
        }

        private void searchArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var musicEvent = e.AddedItems[0] as ArtistSearch;
                if (musicEvent != null)
                {
                    NavigationService.Navigate(new Uri("/ArtistPage.xaml?msg=" + musicEvent.id, UriKind.Relative));
                }
            }
        }

        private void searchEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (e.AddedItems.Count > 0)
            {
                
                var musicEvent = e.AddedItems[0] as EventSearch;
                if (musicEvent != null)
                {
                    NavigationService.Navigate(new Uri("/EventPage.xaml?msg="+ musicEvent.id, UriKind.Relative));
                }
                    
            }
        }

        private void Sorting_Name_Click(object sender, RoutedEventArgs e)
        {
            if (list != null)
            {
                ObservableCollection<EventSearch> sortedList = new ObservableCollection<EventSearch>(list.OrderBy(Event => Event.title));
                searchEvents.DataContext = sortedList;
            }
        }

        private void Sorting_Date_Click(object sender, RoutedEventArgs e)
        {
            if (list != null)
            {
                ObservableCollection<EventSearch> sortedList = new ObservableCollection<EventSearch>(list.OrderBy(Event => Event.startTime));
                searchEvents.DataContext = sortedList;
            }
        }
    }
}