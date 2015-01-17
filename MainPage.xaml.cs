/*
 * Copyright © 2013 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using BandsinTown;
using BandsinTown.Parser;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using Nokia.Music;
using Nokia.Music.Tasks;
using Nokia.Music.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Windows.Devices.Geolocation;

namespace BandsAround
{
    /// <summary>
    /// Main Page of the application with the Map and Pushpin controls.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {

        // Maximum number of Pushpins on the map at the same time
        private const int MAX_ARTISTS_COUNT = 2;

        // Geolocator for getting device's current location.
        private Geolocator geoLocator = null;

        IParser iParser = new XMLParserAdapter();
        ProgressIndicator _progressIndicator;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage()
        {
                this.InitializeComponent();
                _progressIndicator = new ProgressIndicator();
                _progressIndicator.IsVisible = true;
                _progressIndicator.IsIndeterminate = true;
                _progressIndicator.Text = "loading";
                SystemTray.SetProgressIndicator(this, _progressIndicator);
                this.InitializeMap(this.Map);
                this.Loaded += MainPage_Loaded;
                
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Map.Layers.Clear();
        }
        /// <summary>
        /// Centers the map on current location at application launch.
        /// Subsequently only makes the map visible.
        /// </summary>
        /// <param name="sender">This page</param>
        /// <param name="e">Event arguments</param>
        private async void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Make Map visible again after returning from about page.
            Map.Visibility = Visibility.Visible;
            if (Settings.GetValueOrDefault<Boolean>("location", true))
            {
                //Do Something
            
                if (geoLocator == null)
                {
                    try
                    {
                    geoLocator = new Geolocator();
               
               
                        Geoposition pos = await geoLocator.GetGeopositionAsync();

                        if (pos != null && pos.Coordinate != null)
                        {
                            // make pin here
                            Map.SetView(new GeoCoordinate(pos.Coordinate.Latitude, pos.Coordinate.Longitude), 10);

                            MapLayer layer1 = new MapLayer();

                            Pushpin pushpin1 = new Pushpin();
                            pushpin1.GeoCoordinate = new GeoCoordinate(pos.Coordinate.Latitude, pos.Coordinate.Longitude);
                            pushpin1.Content = "You are here!";
               
                            MapOverlay overlay1 = new MapOverlay();
                            overlay1.Content = pushpin1;
                            overlay1.GeoCoordinate = new GeoCoordinate(pos.Coordinate.Latitude, pos.Coordinate.Longitude);
                            layer1.Add(overlay1);

                            Map.Layers.Add(layer1);

                            GetNearbyArtists();
                        }
                    }
                    catch (Exception /*ex*/)
                    {
                        // Couldn't get current location. Location may be disabled.
                        MessageBox.Show("Current location cannot be obtained. It is "
                                      + "recommended that location service is turned "
                                      + "on when using Bands in Town.\n"
                                      + "\nNow centering on London.");
                        Map.Center = new GeoCoordinate(51.51, -0.12);
                        Map.SetView(new GeoCoordinate(51.51, -0.12), 10);
                        GetNearbyArtists();
                    }

                }
            }
            else
            {
                MessageBox.Show("Current location cannot be obtained. It is "
                                 + "recommended that location service is turned "
                                 + "on when using Bands in Town.\n"
                                 + "\nNow centering on London.");
                Map.Center = new GeoCoordinate(51.51, -0.12);
                Map.SetView(new GeoCoordinate(51.51, -0.12), 10);
                GetNearbyArtists();
            }
        }


        /// <summary>
        /// Open Nokia Music application to show selected artist.
        /// </summary>
        /// <param name="sender">Selected Pushpin</param>
        /// <param name="e">Event arguments</param>
        private void pushpintap(object sender, MouseButtonEventArgs e)    
          {
            //Messagebox are what ever    
              Pushpin cls = sender as Pushpin;

              NavigationService.Navigate(new Uri("/EventPage.xaml?msg=" + cls.Tag, UriKind.Relative));   
          }
            
        


        /// <summary>
        /// Navigates to About page. Hides the map to avoid flickering
        /// pushpins while navigating between pages.
        /// </summary>
        /// <param name="sender">About menu item</param>
        /// <param name="e">Event arguments</param>
        private void AboutClicked(object sender, EventArgs e)
        {
            Map.Visibility = Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            Map.Visibility = Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/SearchEvent.xaml", UriKind.Relative));
        }
        /// <summary>
        /// Requests nearby artists after map has been moved to a new position.
        /// </summary>
        /// <param name="sender">Map</param>
        /// <param name="e">Event arguments</param>
        private void OnResolveCompleted(object sender, MapResolveCompletedEventArgs e)
        {
            _progressIndicator.IsVisible = true;
            GetNearbyArtists();
        }

        private void createPinInMap(List<EventSearch> events)
        {
            // The pushpin to add to the map.
            for (int i = 0; i < events.Count; i++)
            {
                MapLayer pushpin_layer = new MapLayer();

                Pushpin pushpin1 = new Pushpin();
                pushpin1.GeoCoordinate = new GeoCoordinate(events[i].latitude, events[i].longitude); 
                pushpin1.Content = events[i].title;
                pushpin1.MouseLeftButtonUp += new MouseButtonEventHandler(pushpintap);
                pushpin1.Tag = events[i].id;
                MapOverlay overlay1 = new MapOverlay();
                overlay1.Content = pushpin1;
                overlay1.GeoCoordinate = new GeoCoordinate(events[i].latitude, events[i].longitude); 
                pushpin_layer.Add(overlay1);
                
                Map.Layers.Add(pushpin_layer);
            }
            _progressIndicator.IsVisible = false;
        }

        /// <summary>
        /// Requests artists and bands originating nearby map center.
        /// Each artist/band is represented on the map as Pushpin.
        /// </summary>
        /// <param name="sender">Refresh menu item</param>
        /// <param name="e">Event arguments</param>
        private ReverseGeocodeQuery reverseGeocode = null;
        private  void GetNearbyArtists()
        {
            try
            {
                if (reverseGeocode == null || !reverseGeocode.IsBusy)
                {
                    reverseGeocode = new ReverseGeocodeQuery();
                    reverseGeocode.GeoCoordinate = new GeoCoordinate(Map.Center.Latitude, Map.Center.Longitude);
                    reverseGeocode.QueryCompleted += reverseGeocode_QueryCompleted;
                     reverseGeocode.QueryAsync();
                }
            }
            catch (Exception /*ex*/)
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private async void getLocationData(String cityName)
        {
            try
            {
                string when = Settings.GetValueOrDefault<string>("when", "Next+30+days");
                string distance = Settings.GetValueOrDefault<string>("distance", "20");
                //Map.Layers.Clear();
                ObservableCollection<EventSearch> list = await iParser.getEventFeed(new Uri("http://api.eventful.com/rest/events/search?&app_key=G79FxVMQbxjMTKMk&location=" + cityName + "&category=music&within="+ distance +"&date=" + when));
                List<EventSearch> newEvents = new List<EventSearch>();

                for (int i = 0; i < list.Count; i++)
                {
                    newEvents.Add(list[i]);
                }
                createPinInMap(newEvents);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        void reverseGeocode_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    MapAddress geoAddress = e.Result[0].Information.Address;
                    String city = geoAddress.City;
                    getLocationData(city);
                }
            }
        }

        #region Map Helpers

        // These helpers enable the Pushpin control from Windows Phone Toolkit
        // to be used in representing the artists and bands on the Map control.
        private void InitializeMap(Map map)
        {
            ObservableCollection<DependencyObject> children = MapExtensions.GetChildren(map);
            IEnumerable<FieldInfo> runtimeFields = this.GetType().GetRuntimeFields();

            foreach (DependencyObject i in children)
            {
                SetChildItemField(i, runtimeFields);
            }
        }

        private void SetChildItemField(DependencyObject obj, IEnumerable<FieldInfo> fields)
        {
            var info = obj.GetType().GetProperty("Name");

            if (info != null)
            {
                string name = (string)info.GetValue(obj);

                if (name != null)
                {
                    foreach (FieldInfo j in fields)
                    {
                        if (j.Name == name)
                        {
                            j.SetValue(this, obj);
                            break;
                        }
                    }
                }
            }

            Panel panelItem = obj as Panel;
            if (panelItem != null)
            {
                foreach (UIElement child in panelItem.Children)
                {
                    SetChildItemField(child, fields);
                }
            }
        }
        #endregion

        private void Map_Loaded_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "45d6df3b-3921-4cec-926c-b1e4a8450d82";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "X_8v2WA-V_HnHKILzFouXg";
        

        }

        private void SettingsClicked(object sender, EventArgs e)
        {
            Map.Visibility = Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
      
        }

    }
}