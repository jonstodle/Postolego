﻿#pragma checksum "C:\Dev\Postolego\Postolego\Pages\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A68F3CBCEC8C2B8C67088F37D6CEB249"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Postolego.Converters;
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


namespace Postolego.Pages {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ProgressIndicator ProgressBar;
        
        internal Postolego.Converters.TextDependentOnSelectedIndexConverter TDS;
        
        internal System.Windows.DataTemplate TextAndImage;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.Pivot MainPivot;
        
        internal System.Windows.Controls.Button MenuButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Postolego;component/Pages/MainPage.xaml", System.UriKind.Relative));
            this.ProgressBar = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("ProgressBar")));
            this.TDS = ((Postolego.Converters.TextDependentOnSelectedIndexConverter)(this.FindName("TDS")));
            this.TextAndImage = ((System.Windows.DataTemplate)(this.FindName("TextAndImage")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.MainPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("MainPivot")));
            this.MenuButton = ((System.Windows.Controls.Button)(this.FindName("MenuButton")));
        }
    }
}

