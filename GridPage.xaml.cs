using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BandsinTown
{
    public partial class GridPage : PhoneApplicationPage
    {
        public GridPage()
        {
            InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("GridPage");
        }

        private void Events_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchEvent.xaml", UriKind.Relative));
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FavoritePage.xaml", UriKind.Relative));
        }

        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FollowEventPage.xaml", UriKind.Relative));
        }
    }
}