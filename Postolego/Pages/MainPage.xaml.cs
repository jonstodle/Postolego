﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Postolego.Pages {
    public partial class MainPage : PhoneApplicationPage {
        public MainPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if(NavigationContext.QueryString.Keys.Contains("RemoveBackEntry")) {
                NavigationService.RemoveBackEntry();
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e) {

        }
    }
}