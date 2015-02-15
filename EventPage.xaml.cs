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
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace BandsAround
{
    public partial class EventPage : PhoneApplicationPage
    {
        ProgressIndicator _progressIndicator;
        IParser iParser = new XMLParserAdapter();
        Event currentEvent = null;
        Popup my_popup_xaml = new Popup();
        Popup my_popup_delete = new Popup();
        //private InterstitialAd interstitialAd;
        public EventPage()
        {
            InitializeComponent();
            _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = true;
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.Text = "loading";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            GoogleAnalytics.EasyTracker.GetTracker().SendView("EventPage");

            makingAddedPopup();
            makingRemovingPopup();
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

        private void FollowClicked(object sender, EventArgs e)
        {
            try
            {
                ObservableCollection<Event> eventList = new ObservableCollection<Event>();
                eventList = Settings.GetValueOrDefault<ObservableCollection<Event>>("follow", eventList);
                Boolean isPresent = false;
                if (eventList.Count > 0)
                {
                    foreach (Event events in eventList)
                    {
                        if (events.id == currentEvent.id)
                        {
                            isPresent = true;
                            eventList.Remove(events);
                            Settings.AddOrUpdateValue("follow", eventList);
                            Settings.save();

                            if (!my_popup_delete.IsOpen)
                            {
                                my_popup_delete.IsOpen = true;
                            }
                            else
                            {
                                my_popup_delete.IsOpen = false;
                            }
                            break;
                        }
                       
                    }
                }
                if (isPresent == false)
                {
                    eventList.Add(currentEvent);
                    Settings.AddOrUpdateValue("follow", eventList);
                    Settings.save();

                    if (!my_popup_xaml.IsOpen)
                    {

                        my_popup_xaml.IsOpen = true;
                    }
                    else
                    {
                        my_popup_xaml.IsOpen = false;
                    }
                }
                isPresent = false;
            }
            catch (ArgumentException ex)
            {

            }
        }

        private void btn_continue_Click(object sender, EventArgs e)
        {
            my_popup_xaml.IsOpen = false;
            my_popup_delete.IsOpen = false;
        }

        public SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(0xFF, R, G, B));
            return scb;
        }

        private void makingAddedPopup()
        {

            StackPanel skt_pnl_outter2 = new StackPanel();                             // stack panel 
            skt_pnl_outter2.Background = GetColorFromHexa("#15B7C7");

            skt_pnl_outter2.Orientation = System.Windows.Controls.Orientation.Vertical;

            TextBlock txt_blk22 = new TextBlock();                                      // Textblock 2
            txt_blk22.Text = "Following event";
            txt_blk22.TextAlignment = TextAlignment.Center;
            txt_blk22.FontSize = 26;
            txt_blk22.Margin = new Thickness(10, 0, 10, 0);
            txt_blk22.Foreground = new SolidColorBrush(Colors.White);

            skt_pnl_outter2.Children.Add(txt_blk22);

            StackPanel skt_pnl_inner2 = new StackPanel();
            skt_pnl_inner2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            skt_pnl_inner2.Orientation = System.Windows.Controls.Orientation.Horizontal;

            Button btn_continue2 = new Button();                                         // Button continue
            btn_continue2.Content = "continue";
            btn_continue2.Width = 215;
            btn_continue2.Click += new RoutedEventHandler(btn_continue_Click);


            skt_pnl_inner2.Children.Add(btn_continue2);


            skt_pnl_outter2.Children.Add(skt_pnl_inner2);
            skt_pnl_outter2.Width = Application.Current.Host.Content.ActualWidth;


            // Adding border to pup-up
            my_popup_xaml.Child = skt_pnl_outter2;

            this.LayoutRoot.Children.Add(my_popup_xaml);
            my_popup_xaml.IsOpen = false;
        }

        private void makingRemovingPopup()
        {
            StackPanel skt_pnl_outter = new StackPanel();                             // stack panel 
            skt_pnl_outter.Background = GetColorFromHexa("#15B7C7");

            skt_pnl_outter.Orientation = System.Windows.Controls.Orientation.Vertical;

            TextBlock txt_blk2 = new TextBlock();                                      // Textblock 2
            txt_blk2.Text = "Stopped following event";
            txt_blk2.TextAlignment = TextAlignment.Center;
            txt_blk2.FontSize = 26;
            txt_blk2.Margin = new Thickness(10, 0, 10, 0);
            txt_blk2.Foreground = new SolidColorBrush(Colors.White);

            skt_pnl_outter.Children.Add(txt_blk2);

            StackPanel skt_pnl_inner = new StackPanel();
            skt_pnl_inner.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            skt_pnl_inner.Orientation = System.Windows.Controls.Orientation.Horizontal;

            Button btn_continue = new Button();                                         // Button continue
            btn_continue.Content = "continue";
            btn_continue.Width = 215;
            btn_continue.Click += new RoutedEventHandler(btn_continue_Click);


            skt_pnl_inner.Children.Add(btn_continue);


            skt_pnl_outter.Children.Add(skt_pnl_inner);
            skt_pnl_outter.Width = Application.Current.Host.Content.ActualWidth;


            // Adding border to pup-up
            my_popup_delete.Child = skt_pnl_outter;

            this.LayoutRoot.Children.Add(my_popup_delete);
            my_popup_delete.IsOpen = false;
        }





    }






}