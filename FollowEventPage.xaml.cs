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
    public partial class FollowEventPage : PhoneApplicationPage
    {
        public FollowEventPage()
        {
            InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("FollowPage");
            ObservableCollection<Event> followList = Settings.GetValueOrDefault<ObservableCollection<Event>>("follow", null);
            followEvents.DataContext = followList;
        }

        private void followEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0)
            {

                var musicEvent = e.AddedItems[0] as Event;
                if (musicEvent != null)
                {
                    NavigationService.Navigate(new Uri("/EventPage.xaml?msg=" + musicEvent.id, UriKind.Relative));
                }

            }
        }
    }
}