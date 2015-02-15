using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BandsinTown.Parser;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace BandsinTown
{
    public partial class ArtistPage : PhoneApplicationPage
    {

        ProgressIndicator _progressIndicator;
        Artist currentArtist;
        IParser iParser = new XMLParserAdapter();
        Popup my_popup_xaml = new Popup();
        Popup my_popup_delete = new Popup();
        public ArtistPage()
        {
            InitializeComponent();
            _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = true;
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.Text = "loading";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            GoogleAnalytics.EasyTracker.GetTracker().SendView("ArtistPage");

            makingAddedPopup();
            makingRemovingPopup();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string id = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out id))
                getArtistData(id);
        }

        private async void getArtistData(String id)
        {
            try
            {
                currentArtist = await iParser.getArtist(new Uri("http://api.eventful.com/rest/performers/get?&app_key=G79FxVMQbxjMTKMk&id=" + id + "&show_events=true"));
 
                artistTitle.Text = currentArtist.name;
                artistGerne.Text = currentArtist.shortBio;
                artistBio.Text = getCleanTextWithoutHtml(currentArtist.longBio);
                artistTumb.Source = new BitmapImage(new Uri(currentArtist.images[0].url));
                artistBioTitle.Text = "Biography";

                artistEvents.DataContext = currentArtist.events;
                _progressIndicator.IsVisible = false;

            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }
        }

        private void artistEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0)
            {

                var musicEvent = e.AddedItems[0] as ArtistEvent;
                if (musicEvent != null)
                {
                    NavigationService.Navigate(new Uri("/EventPage.xaml?msg=" + musicEvent.id, UriKind.Relative));
                }

            }
        }
        private string getCleanTextWithoutHtml(string rssFeed)
        {
            // Remove HTML tags. 
            rssFeed = Regex.Replace(rssFeed, "<[^>]+>", string.Empty);

            return rssFeed;
        }

        private void favoriteClicked(object sender, EventArgs e)
        {
            
            try
            {
                ObservableCollection<Artist> artistList = new ObservableCollection<Artist>();
                artistList = Settings.GetValueOrDefault<ObservableCollection<Artist>>("favorites", artistList);
                Boolean isPresent = false;
                if (artistList.Count > 0)
                {
                    foreach (Artist artist in artistList)
                    {
                        if (artist.id == currentArtist.id)
                        {
                            isPresent = true;
                            artistList.Remove(artist);
                            Settings.AddOrUpdateValue("favorites", artistList);
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
                if (isPresent == false) {
                    artistList.Add(currentArtist);
                    Settings.AddOrUpdateValue("favorites", artistList);
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
            txt_blk22.Text = "Added to favorites";
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
            txt_blk2.Text = "Removed from favorites";
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