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

            UnreadPivotItem.Header = new PostolegoPivotHeader { HeaderText = "UNREAD", IsLoading = false };
            FavoritesPivotItem.Header = new PostolegoPivotHeader { HeaderText = "FAVORITES", IsLoading = false };
            ArchivePivotItem.Header = new PostolegoPivotHeader { HeaderText = "ARCHIVE", IsLoading = false };

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
                            (DataContext as PostolegoData).FavoritesList.UpdateOrAdd(item);
                        }
                        if(item.Status == 1) {
                            (DataContext as PostolegoData).ArchiveList.UpdateOrAdd(item);
                            unreadList.RemoveAt(i);
                        } else if(item.Status == 2) {
                            unreadList.RemoveAt(i);
                        }
                    }
                    HasRefreshed[0] = true;
                } catch(WebException ex) {
                    ShowNotificationWindow((string)App.Current.Resources["GenericErrorHeader"], "Postolego was unable to refresh the unread.\nError message: " + ex.Message, tapFunction: RetrieveUnread);
                }
            } else {
                ShowNotificationWindow((string)App.Current.Resources["NoNetworkHeader"], (string)App.Current.Resources["NoNetworkMessage"]);
            }
        }

        private async void RetrieveFavorites() {
            if(NetworkInterface.GetIsNetworkAvailable()) {
                try {
                    var favoritesList = (DataContext as PostolegoData).FavoritesList;
                    var returnedList = await (DataContext as PostolegoData).PocketSession.RetrieveItems(PocketRetrieveItem.States.All, PocketRetrieveItem.Favorites.Favorited);
                    foreach(var i in returnedList) {
                        favoritesList.UpdateOrAdd(i);
                    }
                    for(int i = favoritesList.Count - 1; i > -1; i--) {
                        var item = favoritesList[i];
                        if(item.Status == 1) {
                            (DataContext as PostolegoData).ArchiveList.UpdateOrAdd(item);
                        }
                        if(item.Status == 2 || item.Favorite == 0) {
                            favoritesList.RemoveAt(i);
                        }
                    }
                    HasRefreshed[1] = true;
                } catch(WebException ex) {
                    ShowNotificationWindow((string)App.Current.Resources["GenericErrorHeader"], "Postolego was unable to refresh the favorites.\nError message: " + ex.Message, tapFunction: RetrieveFavorites);
                }
            } else {
                ShowNotificationWindow((string)App.Current.Resources["NoNetworkHeader"], (string)App.Current.Resources["NoNetworkMessage"]);
            }
        }

        private async void RetrieveArchive() {
            if(NetworkInterface.GetIsNetworkAvailable()) {
                try {
                    var archiveList = (DataContext as PostolegoData).ArchiveList;
                    var returnedList = await (DataContext as PostolegoData).PocketSession.RetrieveItems(PocketRetrieveItem.States.Archive, Count: 30);
                    foreach(var i in returnedList) {
                        archiveList.UpdateOrAdd(i);
                    }
                    for(int i = archiveList.Count - 1; i > -1; i--) {
                        var item = archiveList[i];
                        if(item.Favorite == 1) {
                            (DataContext as PostolegoData).FavoritesList.UpdateOrAdd(item);
                        }
                        if(item.Status == 0) {
                            (DataContext as PostolegoData).UnreadList.UpdateOrAdd(item);
                            archiveList.RemoveAt(i);
                        } else if(item.Status == 2) {
                            archiveList.RemoveAt(i);
                        }
                    }
                    HasRefreshed[2] = true;
                } catch(WebException ex) {
                    ShowNotificationWindow((string)App.Current.Resources["GenericErrorHeader"], "Postolego was unable to refresh the archive.\nError message: " + ex.Message, tapFunction: RetrieveArchive);
                }
            } else {
                ShowNotificationWindow((string)App.Current.Resources["NoNetworkHeader"], (string)App.Current.Resources["NoNetworkMessage"]);
            }
        }
        #endregion

        #region List actions

        #endregion

        #region Pivot
        bool[] HasRefreshed = { false, false, false };

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(MainPivot.SelectedIndex == 0 && !HasRefreshed[0]) {
                LoadingLabel.Text = "LOADING UNREAD";
                LoadingLabel.Visibility = System.Windows.Visibility.Visible;
                RetrieveUnread();
                LoadingLabel.Visibility = System.Windows.Visibility.Collapsed;
            } else if(MainPivot.SelectedIndex == 1 && !HasRefreshed[1]) {
                LoadingLabel.Text = "LOADING FAVORITES";
                LoadingLabel.Visibility = System.Windows.Visibility.Visible;
                RetrieveFavorites();
                LoadingLabel.Visibility = System.Windows.Visibility.Collapsed;
            } else if(MainPivot.SelectedIndex == 2 && !HasRefreshed[2]) {
                LoadingLabel.Text = "LOADING ARCHIVE";
                LoadingLabel.Visibility = System.Windows.Visibility.Visible;
                RetrieveArchive();
                LoadingLabel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        #endregion

        #region Notificaition Window
        private DispatcherTimer NotificationTimer = new DispatcherTimer();
        private Action TapFunction;

        private void ShowNotificationWindow(string header, string message, double visibleTime = 4000, Action tapFunction = null) {
            TapFunction = tapFunction;
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
            if(TapFunction != null) TapFunction();
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
            //ShowNotificationWindow("Header", "A short message to all");
            //(UnreadPivotItem.Header as PostolegoPivotHeader).IsLoading = !(UnreadPivotItem.Header as PostolegoPivotHeader).IsLoading;
        }
    }
}