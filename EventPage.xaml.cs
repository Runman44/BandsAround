using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;
using Windows.System;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using System.Globalization;
using System.Text.RegularExpressions;
using vservWindowsPhone;
using Nokia.Music.Tasks;
using BandsinTown;
using BandsinTown.Parser;
using GoogleAds;

namespace BandsAround
{
    public partial class EventPage : PhoneApplicationPage
    {
        ProgressIndicator _progressIndicator;
        IParser iParser = new XMLParserAdapter();
        Event currentEvent = null;
        //private InterstitialAd interstitialAd;
        public EventPage()
        {
            InitializeComponent();
            _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = true;
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.Text = "loading";
            SystemTray.SetProgressIndicator(this, _progressIndicator);

            //interstitialAd = new InterstitialAd("ca-app-pub-4050305580977890/3477777966");
            //AdRequest adRequest = new AdRequest();
            // Enable test ads.
           // adRequest.ForceTesting = true;

            //interstitialAd.ReceivedAd += OnAdReceived;
            //interstitialAd.FailedToReceiveAd += onAdFailed;
            //interstitialAd.LoadAd(adRequest);
        }
        /*
        private void onAdFailed(object sender, AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Ad failed");
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Ad received successfully");
            //interstitialAd.ShowAd();
        }
         */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string id = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out id))
                getEventData(id);
        }

        private async void getEventData(String id)
        {
            try
            {
                currentEvent = await iParser.getEvent(new Uri("http://api.eventful.com/rest/events/get?&app_key=G79FxVMQbxjMTKMk&id=" + id));

                //textBox
                title.Text = currentEvent.title;
                time.Text = currentEvent.startTime.ToString("g", DateTimeFormatInfo.InvariantInfo);
                venue.Text = currentEvent.venueName;
                city.Text = currentEvent.cityName;
                price.Text = string.IsNullOrEmpty(currentEvent.price) != true ? "The price is " + currentEvent.price + " bucks!" : "The price is unknown";

                MainPage_Loaded();
                _progressIndicator.IsVisible = false;

            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }
        }

        private void CalendarClicked(object sender, EventArgs e)
        {
            if (currentEvent != null)
            {
                SaveAppointmentTask saveAppointmentTask = new SaveAppointmentTask();


                if (currentEvent.startTime == DateTime.MinValue)
                {
                    saveAppointmentTask.StartTime = currentEvent.startTime;
                    saveAppointmentTask.EndTime = currentEvent.startTime.AddHours(2);
                }
                else
                {
                    saveAppointmentTask.StartTime = DateTime.Now.AddHours(2);
                    saveAppointmentTask.EndTime = DateTime.Now.AddHours(3);
                }
                saveAppointmentTask.Subject = currentEvent.title;
                saveAppointmentTask.Location = currentEvent.cityName;
                saveAppointmentTask.Details = currentEvent.venueName;
                saveAppointmentTask.IsAllDayEvent = false;
                saveAppointmentTask.Reminder = Reminder.FifteenMinutes;
                saveAppointmentTask.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;

                saveAppointmentTask.Show();
            }
        }

        private void MainPage_Loaded()
        {
            // Make Map visible again after returning from about page.
            Map.Visibility = Visibility.Visible;



            if (currentEvent != null)
            {

                // make pin here
                Map.Center = new GeoCoordinate(currentEvent.latitude, currentEvent.longitude);
                Map.SetView(new GeoCoordinate(currentEvent.latitude, currentEvent.longitude), 15);

                MapLayer layer1 = new MapLayer();

                Pushpin pushpin1 = new Pushpin();
                pushpin1.GeoCoordinate = new GeoCoordinate(currentEvent.latitude, currentEvent.longitude);
                pushpin1.Content = currentEvent.title;

                MapOverlay overlay1 = new MapOverlay();
                overlay1.Content = pushpin1;
                overlay1.GeoCoordinate = new GeoCoordinate(currentEvent.latitude, currentEvent.longitude);
                layer1.Add(overlay1);

                Map.Layers.Add(layer1);

            }

            else
            {
                // Couldn't get current location. Location may be disabled.
                MessageBox.Show("Event location cannot be obtained."
                              + "\nNow centering on London.");
                Map.Center = new GeoCoordinate(51.51, -0.12);
                Map.SetView(new GeoCoordinate(51.51, -0.12), 10);
            }

        }

        private void Map_Loaded_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "45d6df3b-3921-4cec-926c-b1e4a8450d82";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "X_8v2WA-V_HnHKILzFouXg";
        }

        private void SharedClicked(object sender, EventArgs e)
        {
            if (currentEvent != null)
            {
                ShareStatusTask shareStatusTask = new ShareStatusTask();

                shareStatusTask.Status = "Just looked at " + currentEvent.title + ". And planning to go! Thanks to Bands in Town App";

                shareStatusTask.Show();
            }
        }

        private void InfoClicked(object sender, EventArgs e)
        {
            try
            {
                if (currentEvent != null)
                {
                    WebBrowserTask eventWebsite = new WebBrowserTask();
                    eventWebsite.Uri = new Uri(currentEvent.url, UriKind.Absolute);
                    eventWebsite.Show();
                }
            }
            catch (Exception we)
            {
                Console.Write(we.Message);
                MessageBox.Show("There is no website for this event available.");
            }
        }

        private void MoreClicked(object sender, EventArgs e)
        {
            try
            {
                if (currentEvent != null)
                {
                    MusicSearchTask task = new MusicSearchTask();
                    task.SearchTerms = currentEvent.title;
                    task.Show();
                }
            }
            catch (Exception we)
            {
                Console.Write(we.Message);
                MessageBox.Show("There is no website for this event available.");
            }
        }

        private void NavigateClicked(object sender, EventArgs e)
        {
            if(currentEvent != null)
                Windows.System.Launcher.LaunchUriAsync(new Uri("ms-drive-to:?destination.latitude= " + currentEvent.latitude.ToString(CultureInfo.InvariantCulture) + "&destination.longitude=" + currentEvent.longitude.ToString(CultureInfo.InvariantCulture) + "&destination.name=" + currentEvent.title));
        }





    }






}