using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PocketInterface;
using System.IO;
using Microsoft.Phone.Tasks;

namespace Postolego.Pages {
    public partial class SignInPage : PhoneApplicationPage {
        public SignInPage() {
            InitializeComponent();
            if((DataContext as PostolegoData).PocketSession == null) {
                var stream = Application.GetResourceStream(new Uri("ConsumerKey.txt", UriKind.Relative));
                using(var reader = new StreamReader(stream.Stream)) {
                    (DataContext as PostolegoData).PocketSession = new Pocket(reader.ReadToEnd());
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if(!(DataContext as PostolegoData).PocketSession.HasLoginUriString) {
                SetLoadingIndicator(visible: false);
            } else if(!(DataContext as PostolegoData).PocketSession.IsAuthenticated) {
                CompleteUserLogin();
            }
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e) {
            StartUserLogin();
        }

        private async void StartUserLogin() {
            try {
                var uri = await (DataContext as PostolegoData).PocketSession.GenerateUserLoginUriString("postolego:authorization");
                new WebBrowserTask { Uri = new Uri(uri, UriKind.Absolute) }.Show();
            } catch(WebException ex) {
            }
        }

        private async void CompleteUserLogin() {
            await (DataContext as PostolegoData).PocketSession.FinalizeUserLogin();
            if((DataContext as PostolegoData).PocketSession.IsAuthenticated) {

            } else {

            }
        }

        private void SetLoadingIndicator(string message = "LOADING", bool visible = true) {
            LoadingIndicator.Content = message;
            LoadingIndicator.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}