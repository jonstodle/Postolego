using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using PocketInterface;
using System.IO;
using Microsoft.Phone.Tasks;

namespace Postolego.Pages {
    public partial class SignInPage : PhoneApplicationPage {
        private enum Elements { SignInButton, LoadingIndicator, ErrorMessage };
        private const string NoNetworkString = "No network connection available";

        public SignInPage() {
            InitializeComponent();
            if((DataContext as PostolegoData).PocketSession == null) {
                GeneratePocketSession();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            LaunchCorrectLoginStage();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);
            (DataContext as PostolegoData).Save();
        }

        private void GeneratePocketSession() {
            var stream = Application.GetResourceStream(new Uri("ConsumerKey.txt", UriKind.Relative));
            using(var reader = new StreamReader(stream.Stream)) {
                (DataContext as PostolegoData).PocketSession = new Pocket(reader.ReadToEnd());
            }
        }

        private void LaunchCorrectLoginStage(bool isRetry = false) {
            if(!(DataContext as PostolegoData).PocketSession.HasLoginUriString) {
                if(isRetry) {
                    StartUserLogin();
                } else {
                    SetVisibleElement(visibleElement: Elements.SignInButton);
                }
            } else if(!(DataContext as PostolegoData).PocketSession.IsAuthenticated) {
                SetVisibleElement("FINALIZING POCKET LOGIN", Elements.LoadingIndicator);
                CompleteUserLogin();
            } else {
                SetVisibleElement("Something has gone wrong with the login process.", Elements.ErrorMessage);
                GeneratePocketSession();
            }
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e) {
            StartUserLogin();
        }

        private async void StartUserLogin() {
            if(NetworkInterface.GetIsNetworkAvailable()) {
                try {
                    SetVisibleElement("COMMUNICATING WITH POCKET", Elements.LoadingIndicator);
                    var uri = await (DataContext as PostolegoData).PocketSession.GenerateUserLoginUriString("postolego:authorized");
                    new WebBrowserTask { Uri = new Uri(uri, UriKind.Absolute) }.Show();
                } catch(WebException ex) {
                    string errorMessage;
                    if(ex.Response.Headers.AllKeys.Contains("X-Error")) {
                        errorMessage = ex.Response.Headers["X-Error"];
                    } else {
                        errorMessage = ex.Message;
                    }
                    SetVisibleElement(errorMessage, Elements.ErrorMessage);
                }
            } else {
                SetVisibleElement(NoNetworkString, Elements.ErrorMessage);
            }
        }

        private async void CompleteUserLogin() {
            if(NetworkInterface.GetIsNetworkAvailable()) {
                try {
                    await (DataContext as PostolegoData).PocketSession.FinalizeUserLogin();
                    if((DataContext as PostolegoData).PocketSession.IsAuthenticated) {
                        NavigationService.Navigate(new Uri("/Pages/MainPage.xaml?RemoveBackEntry", UriKind.Relative));
                    } else {
                        SetVisibleElement("Something went wrong with the final login process.", Elements.ErrorMessage);
                    }
                } catch(WebException ex) {
                    string errorMessage;
                    if(ex.Response.Headers.AllKeys.Contains("X-Error")) {
                        errorMessage = ex.Response.Headers["X-Error"];
                    } else {
                        errorMessage = ex.Message;
                    }
                    SetVisibleElement(errorMessage, Elements.ErrorMessage);
                }
            } else {
                SetVisibleElement(NoNetworkString, Elements.ErrorMessage);
            }
        }

        private void SetVisibleElement(string message = "LOADING", Elements visibleElement = Elements.LoadingIndicator) {
            LoadingIndicator.Content = message;
            ErrorText.Text = message;
            LoginGrid.Visibility = System.Windows.Visibility.Collapsed;
            LoadingIndicator.Visibility = System.Windows.Visibility.Collapsed;
            ErrorPanel.Visibility = System.Windows.Visibility.Collapsed;
            switch(visibleElement) {
                case Elements.SignInButton:
                    LoginGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                case Elements.LoadingIndicator:
                    LoadingIndicator.Visibility = System.Windows.Visibility.Visible;
                    break;
                case Elements.ErrorMessage:
                    ErrorPanel.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void ErrorPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
            e.Handled = true;
            if(NetworkInterface.GetIsNetworkAvailable()) {
                LaunchCorrectLoginStage(true);
            } else {
                SetVisibleElement(NoNetworkString, Elements.ErrorMessage);
            }
        }
    }
}