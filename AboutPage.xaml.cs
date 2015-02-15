/*
 * Copyright © 2013 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace BandsAround
{
    /// <summary>
    /// Page for displaying application information.
    /// </summary>
    public partial class AboutPage : PhoneApplicationPage
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutPage()
        {
            InitializeComponent();
            UpdateVersionString();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("AboutPage");
        }

        /// <summary>
        /// Updates VersionText to contain correct version info.
        /// </summary>
        private void UpdateVersionString()
        {
            string appVersion = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            VersionText.Text = "Version: "+ appVersion;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Bands in Town - Feedback";
            emailComposeTask.To = "info@mranderson.nl";

            emailComposeTask.Show();
        }

        private void twitter_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask twitterWebsite = new WebBrowserTask();
            twitterWebsite.Uri = new Uri("https://twitter.com/Bands_in_Town", UriKind.Absolute);
            twitterWebsite.Show();
        }

        private void eventfull_image_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }

        private void eventfull_image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask twitterWebsite = new WebBrowserTask();
            twitterWebsite.Uri = new Uri("http://eventful.com/", UriKind.Absolute);
            twitterWebsite.Show();
        }

        private void rate_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }


    }
}