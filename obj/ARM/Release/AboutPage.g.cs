﻿#pragma checksum "C:\Users\denni_000\Desktop\BandsAround\AboutPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EF024E365C011F126A17265A1FE44D3D"
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


namespace BandsAround {
    
    
    public partial class AboutPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock VersionText;
        
        internal System.Windows.Controls.Button email;
        
        internal System.Windows.Controls.Button twitter;
        
        internal System.Windows.Controls.Button rate;
        
        internal System.Windows.Controls.Image eventfull_image;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Bands%20in%20Town;component/AboutPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.VersionText = ((System.Windows.Controls.TextBlock)(this.FindName("VersionText")));
            this.email = ((System.Windows.Controls.Button)(this.FindName("email")));
            this.twitter = ((System.Windows.Controls.Button)(this.FindName("twitter")));
            this.rate = ((System.Windows.Controls.Button)(this.FindName("rate")));
            this.eventfull_image = ((System.Windows.Controls.Image)(this.FindName("eventfull_image")));
        }
    }
}
