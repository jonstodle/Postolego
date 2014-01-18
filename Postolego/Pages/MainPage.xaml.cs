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
using PocketInterface;
using System.Collections.ObjectModel;
using Microsoft.Phone.Net.NetworkInformation;

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

        #region Pocket actions
        private async void RetrieveUnread() {
            if(NetworkInterface.GetIsNetworkAvailable()) {
                try {
                    var unreadList = (DataContext as PostolegoData).UnreadList;
                    var returnedList = await (DataContext as PostolegoData).PocketSession.RetrieveItems(PocketInterface.PocketRetrieveItem.States.Unread, Since: (DataContext as PostolegoData).PocketSession.TimeStamp);
                    foreach(var i in returnedList) {
                        unreadList.UpdateOrAdd(i);
                    }
                    for(int i = unreadList.Count - 1; i > -1; i--) {
                        var item = unreadList[i];
                        if(item.Favorite == 1) {
                            (DataContext as PostolegoData).FavoritesList.Add(item);
                        }
                        if(item.Status == 1) {
                            (DataContext as PostolegoData).ArchiveList.Add(item);
                            unreadList.RemoveAt(i);
                        } else if(item.Status == 2) {
                            unreadList.RemoveAt(i);
                        }
                    }
                } catch(WebException ex) {
                    //todo
                }
            }
        }

        private void RetrieveFavorites() {

        }

        private void RetrieveArchive() {

        }

        private void CheckAndPerformActions(ObservableCollection<PocketItem> list) {
            for(int i = list.Count - 1; i > -1; i--) {
                var item = list[i];
                var removeItem = false;
                if(item.Favorite == 1) {
                    (DataContext as PostolegoData).FavoritesList.Add(item);
                }
                if(item.Status == 1) {
                    (DataContext as PostolegoData).ArchiveList.Add(item);
                    removeItem = true;
                } else if(item.Status == 2) {
                    removeItem = true;
                }
            }
        }
        #endregion

        #region List actions

        #endregion

        #region Pivot
        bool[] HasRefreshed = { false, false, false };

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(MainPivot.SelectedIndex == 0 && !HasRefreshed[0]) {
                LoadingLabel.Visibility = System.Windows.Visibility.Visible;
                RetrieveUnread();
                LoadingLabel.Visibility = System.Windows.Visibility.Collapsed;
            } else if(MainPivot.SelectedIndex == 1 && !HasRefreshed[1]) {

            } else if(MainPivot.SelectedIndex == 2 && !HasRefreshed[2]) {

            }
        }
        #endregion

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