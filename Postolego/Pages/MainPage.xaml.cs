using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using Gestures;
using System.Windows.Threading;

namespace Postolego.Pages {
    public partial class MainPage : PhoneApplicationPage {
        public MainPage() {
            InitializeComponent();
            NotificationTimer.Tick += NotificationTimer_Tick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if(NavigationContext.QueryString.Keys.Contains("RemoveBackEntry")) {
                NavigationService.RemoveBackEntry();
            }
        }

        #region Notificaition Window
        DispatcherTimer NotificationTimer = new DispatcherTimer();

        private void ShowNotificationWindow(string header, string message, double visibleTime = 4000) {
            NotificationHeader.Text = header.ToUpper();
            NotificationMessage.Text = message;
            NotificationTimer.Interval = TimeSpan.FromMilliseconds(visibleTime + 200);
            NotificationWindow.IsOpen = true;
            NotificationTimer.Start();
        }

        void NotificationTimer_Tick(object sender, EventArgs e) {
            NotificationTimer.Stop();
            NotificationWindow.IsOpen = false;
        }

        private void NotificationWindow_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
            e.Handled = true;
        }

        private void NotificationWindow_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e) {
            e.Handled = true;
            if(e.IsInertial) {
                var velocity = e.FinalVelocities.LinearVelocity;
                if(GestureHelper.GetDirection(velocity.X, velocity.Y) == GestureHelper.Direction.Right) {
                    NotificationWindow.IsOpen = false;
                }
            }
        }
        #endregion

        private void MenuButton_Click(object sender, RoutedEventArgs e) {
            ShowNotificationWindow("Header", "A short message to all");
        }
    }
}