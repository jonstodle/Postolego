using Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace Postolego {
    public class NotificationWindow : RadWindow {
        private Action TapFunction;

        public NotificationWindow(string header, string message, double width, Action tapFunction, double visibleTime = 4000) {
            var openAnimation = new RadMoveAndFadeAnimation();
            openAnimation.MoveAnimation.StartPoint = new Point(-200, 0);
            openAnimation.MoveAnimation.EndPoint = new Point(0, 0);
            openAnimation.FadeAnimation.StartOpacity = 0;
            openAnimation.FadeAnimation.EndOpacity = 1;
            openAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            var closeAnimation = new RadMoveAndFadeAnimation();
            closeAnimation.MoveAnimation.StartPoint = new Point(0, 0);
            closeAnimation.MoveAnimation.EndPoint = new Point(200, 0);
            closeAnimation.FadeAnimation.StartOpacity = 1;
            closeAnimation.FadeAnimation.EndOpacity = 0;
            closeAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            var headerBlock = new TextBlock { Text = header.ToUpper(), FontSize = (double)App.Current.Resources["PhoneFontSizeMedium"], FontWeight = FontWeights.SemiBold, TextWrapping = TextWrapping.Wrap };
            var messageBlock = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap };

            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(24, 12, 24, 6);
            stackPanel.Children.Add(headerBlock);
            stackPanel.Children.Add(messageBlock);

            var grid = new Grid();
            grid.Background = (Brush)App.Current.Resources["PhoneAccentBrush"];
            grid.Width = width;
            grid.Children.Add(stackPanel);

            var closeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(visibleTime + 200) };
            closeTimer.Tick += closeTimer_Tick;

            this.Placement = PlacementMode.TopCenter;
            this.OpenAnimation = openAnimation;
            this.CloseAnimation = closeAnimation;
            this.Content = grid;
            this.TapFunction = tapFunction;
            this.Tap += NotificationWindow_Tap;
            this.ManipulationCompleted += NotificationWindow_ManipulationCompleted;
            this.IsOpen = true;
            closeTimer.Start();
        }

        void closeTimer_Tick(object sender, EventArgs e) {
            (sender as DispatcherTimer).Stop();
            this.IsOpen = false;
        }

        void NotificationWindow_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e) {
            e.Handled = true;
            System.Diagnostics.Debug.WriteLine("Manipulation");
            if(e.IsInertial) {
                var velocity = e.FinalVelocities.LinearVelocity;
                if(GestureHelper.GetDirection(velocity.X, velocity.Y) == GestureHelper.Direction.Right) {
                    this.IsOpen = false;
                }
            }
        }

        void NotificationWindow_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
            e.Handled = true;
            TapFunction();
        }
    }
}
