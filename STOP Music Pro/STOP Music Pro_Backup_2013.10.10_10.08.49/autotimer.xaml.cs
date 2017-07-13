using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;

namespace STOP_Music_Pro
{
    public partial class autotimer : PhoneApplicationPage
    {
        public autotimer()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = "";
            int hours;
            int min;
            int seconds;
            if (NavigationContext.QueryString.TryGetValue("pintimer", out msg))
            {
                hours = int.Parse(msg.Substring(0, 2));
                min = int.Parse(msg.Substring(2, 2));
                seconds = int.Parse(msg.Substring(4, 2));
                timewheel.setTime(hours,min,seconds);
            }
        }

        private void home_bar_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
            NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
        }

        private DispatcherTimer coutdown_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.00) };
        private int totalsecond;
        private ToastPrompt toast = new ToastPrompt();
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            totalsecond = 0;
            int second = timewheel.selectedSecond;
            int min = timewheel.selectedMinute;
            int hour = timewheel.selectedHour;
            totalsecond = second + min * 60 + hour * 3600;
            CheckandDelPinTimer();
            if (coutdown_timer == null)
            {
                coutdown_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.00) };
            }
            coutdown_timer.Tick += (s1, e1) =>
            {
                if (totalsecond > 0)
                {
                    totalsecond--;
                    hour = totalsecond / 3600;
                    min = (totalsecond % 3600) / 60;
                    second = (totalsecond % 3600) % 60;
                    timewheel.setTime(hour, min, second);
                }

                if (totalsecond <= 0)
                {
                    try
                    {
                        Song track = Song.FromUri("no song playing", new Uri("cut.mp3", UriKind.Relative));
                        MediaPlayer.Play(track);
                        MediaPlayer.Stop();
                        Application.Current.Terminate();
                    }
                    catch (Exception)
                    {
                        Application.Current.Terminate();
                    }
                }

            };
            toast.Title = "Warning !";
            toast.Message = "Please don't close this app and run it under\nlockscreen while TIMER is activated\nThis app will be closed after music is stopped 🙅";
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            toast.ImageSource = new BitmapImage(new Uri("Warning.png", UriKind.RelativeOrAbsolute));
            toast.Show();

            if (!coutdown_timer.IsEnabled)
            {
                coutdown_timer.Start();
            }
        }

        private void CheckandDelPinTimer()
        {
            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetUserStoreForApplication();

            foreach (var trigger in DataHelper.GetTIMER())
            {
                ShellTile oTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(trigger.Name));

                if (oTile == null) //|| oTile.NavigationUri.ToString().Contains(listpin[0, i]))
                {
                    try
                    {
                        isolatedFile.DeleteFile("/Shared/ShellContent/" + trigger.Name + ".png");
                        DataHelper.DeleteTIMER(trigger, trigger.Name);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }
    }
}