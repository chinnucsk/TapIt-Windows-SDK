﻿#pragma checksum "D:\TapIt-Project\tapit-SVN\trunk\TapIt-WP8-TestApp\TapIt-WP8-TestApp\InterstitialAdPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4B1D2CC60C3A56BD057801F5FB60D464"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
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


namespace TapIt_WP8_TestApp {
    
    
    public partial class InterstitialAdPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage InterstitialPage;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button loadBtn;
        
        internal System.Windows.Controls.ProgressBar progressring;
        
        internal System.Windows.Controls.Button showBtn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TapIt-WP8-TestApp;component/InterstitialAdPage.xaml", System.UriKind.Relative));
            this.InterstitialPage = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("InterstitialPage")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.loadBtn = ((System.Windows.Controls.Button)(this.FindName("loadBtn")));
            this.progressring = ((System.Windows.Controls.ProgressBar)(this.FindName("progressring")));
            this.showBtn = ((System.Windows.Controls.Button)(this.FindName("showBtn")));
        }
    }
}

