﻿#pragma checksum "D:\Documents\Workspaces\Visual Studio Projects\BandsAround\ArtistPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "328349D4595F3E93716163738837969F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BandsinTown {
    
    
    public partial class ArtistPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Image artistTumb;
        
        internal System.Windows.Controls.TextBlock artistTitle;
        
        internal System.Windows.Controls.TextBlock artistGerne;
        
        internal System.Windows.Controls.TextBlock artistBioTitle;
        
        internal System.Windows.Controls.TextBlock artistBio;
        
        internal System.Windows.Controls.ListBox artistEvents;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Bands%20in%20Town;component/ArtistPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.artistTumb = ((System.Windows.Controls.Image)(this.FindName("artistTumb")));
            this.artistTitle = ((System.Windows.Controls.TextBlock)(this.FindName("artistTitle")));
            this.artistGerne = ((System.Windows.Controls.TextBlock)(this.FindName("artistGerne")));
            this.artistBioTitle = ((System.Windows.Controls.TextBlock)(this.FindName("artistBioTitle")));
            this.artistBio = ((System.Windows.Controls.TextBlock)(this.FindName("artistBio")));
            this.artistEvents = ((System.Windows.Controls.ListBox)(this.FindName("artistEvents")));
        }
    }
}
