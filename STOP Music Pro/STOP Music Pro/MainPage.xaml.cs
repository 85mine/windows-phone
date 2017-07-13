using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using TIMER;
using Telerik.Windows.Controls;

namespace STOP_Music_Pro
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer = new DispatcherTimer();
        DataHelper TIMER = new DataHelper();
        ProgressIndicator progressIndicator = new ProgressIndicator()
        {

            IsVisible = true,
            IsIndeterminate = false,
            Text = "😊 tap on CD icon to STOP MUSIC"
        };
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
			//Shows the rate reminder message, according to the settings of the RateReminder.
            (App.Current as App).rateReminder.Notify();
            this.Width = System.Windows.Application.Current.Host.Content.ActualWidth;
            this.Height = System.Windows.Application.Current.Host.Content.ActualHeight;
            //FrameworkDispatcher.Update();
            ImageBrush _imageBrush = new ImageBrush();
            _imageBrush.ImageSource = SongMusic.albumcover;
            album_img.Stroke = _imageBrush;
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }

		void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
		   // TIMER.CreateDatabase();
            savetoIsolated("pintimer.png");
            timer.Interval = TimeSpan.FromSeconds(1.5);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                namesong.Text = SongMusic.name;
                artsong.Text = SongMusic.artist;
                ImageBrush _imageBrush = new ImageBrush();
                _imageBrush.ImageSource = SongMusic.albumcover;
                if (SongMusic.albumcover == null)
                {
                    _imageBrush.ImageSource = new BitmapImage(new Uri("nocover.png", UriKind.RelativeOrAbsolute));
                }
                album_img.Stroke = _imageBrush;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CLGT"+ ex.Message);
            }

        }

		private void rate_Click(object sender, System.EventArgs e)
		{
			// TODO: Add event handler implementation here.
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
		}

        private void pin_bar_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Would you like auto STOP Music when you tap on tile ?", "Notice",MessageBoxButton.OKCancel)==MessageBoxResult.OK)
            {
                ShellTile oTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("pinstop".ToString()));

                if (oTile != null && oTile.NavigationUri.ToString().Contains("pinstop"))
                {
                    FlipTileData oFliptile = new FlipTileData();
                    oFliptile.SmallBackgroundImage = new Uri("160.png", UriKind.Relative);
                    oFliptile.BackgroundImage = new Uri("512.png", UriKind.Relative);
                    oTile.Update(oFliptile);
                    MessageBox.Show("Tile Data successfully update.");
                }
                else
                {
                    // once it is created flip tile
                    // Uri tileUri = new Uri("ms-settings-lock:",UriKind.RelativeOrAbsolute);
                    Uri tileUri = new Uri("/Page1.xaml?tile=pinstop", UriKind.Relative);
                    ShellTileData tileData = this.CreateSTOPTileData();
                    ShellTile.Create(tileUri, tileData, false);
                }
            }            
        }
        private ShellTileData CreateSTOPTileData()
        {
            return new FlipTileData()
            {
                SmallBackgroundImage = new Uri("160.png", UriKind.Relative),
                BackgroundImage = new Uri("512.png", UriKind.Relative)
            };
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            Storyboard3.Begin();
        }

        private void what_snew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            MessageBox.Show("Version 1.3.0.0 : Added timer \nVersion 1.2.5.0 : Implement clear history \nVersion 1.2.0.0 : Change themes \nVersion 1.1.0.0 : Fix error can't stop music");
        }
        private void album_img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            try
            {
                Song track = Song.FromUri("no song playing", new Uri("cut.mp3", UriKind.Relative));
                MediaPlayer.Play(track);
                MediaPlayer.Stop();
                Storyboard3.Stop();
                progressIndicator.Text = "Music Stopped";
            }
            catch (Exception)
            {
                Application.Current.Terminate();
            }
        }

        private void clear_bar_Click(object sender, System.EventArgs e)
        {
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            progressIndicator.IsIndeterminate = true;
            progressIndicator.Text = "Clearing...";
            DispatcherTimer timer=new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s1, e1) =>
                {
                    clear_history();
                    timer.Stop();
                };
            timer.Start();
        }

        private void clear_history()
        {
            //this.clear_bar.IsEnabled = false;
            for (int i = 0; i < 40; i++)
            {
                try
                {
                    //create a snapshot of the page title
                    StreamResourceInfo sri = Application.GetResourceStream(new Uri("cut.png", UriKind.Relative));
                    MediaHistoryItem mediaHistoryItem = new MediaHistoryItem();
                    //<hubTileImageStream> must be a valid ImageStream.
                    mediaHistoryItem.ImageStream = sri.Stream;
                    mediaHistoryItem.Source = "";
                    mediaHistoryItem.Title = "STOP Music";
                    mediaHistoryItem.PlayerContext.Add("STOP Music" + i.ToString(), "STOP Music");
                    MediaHistory mediaHistory = MediaHistory.Instance;
                    mediaHistory.WriteRecentPlay(mediaHistoryItem);         
                    if (i == 26)
                    {
                        ApplicationBar.Mode=ApplicationBarMode.Default;
                        progressIndicator.Text = "History cleared";
                        progressIndicator.IsIndeterminate = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :(, please try again ! "+ex.Message);
                }
            }

        }

        private void donate_bar_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
            marketplaceDetailTask.ContentIdentifier = "4ae8ac78-2bf1-47bc-88d4-d77d66b8fc9b";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;
            marketplaceDetailTask.Show();
        }

        private DispatcherTimer coutdown_timer = new DispatcherTimer{Interval = TimeSpan.FromSeconds(1.00)};
        private int totalsecond;
        private ToastPrompt toast = new ToastPrompt();

        private void start_timer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            totalsecond = 0;
            start_timer.Visibility = Visibility.Collapsed;
            stop_timer.Visibility=Visibility.Visible;
            pin_timer.Visibility = Visibility.Collapsed;
            int second = timewheel.selectedSecond;
            int min = timewheel.selectedMinute;
            int hour = timewheel.selectedHour;
            totalsecond = second + min*60 + hour*3600;

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
                            Storyboard3.Stop();
                            progressIndicator.Text = "Music Stopped";
                        }
                        catch (Exception)
                        {
                            Application.Current.Terminate();
                        }
                        coutdown_timer.Stop();
                        coutdown_timer = null;
                        timewheel.setTime(0,30,0);
                        start_timer.Visibility = Visibility.Visible;
                        stop_timer.Visibility = Visibility.Collapsed;
                        pin_timer.Visibility=Visibility.Visible;
                        if (autoclose.IsChecked==true)
                        {
                            //Application.Current.Terminate();
                        }
                        else
                        {
                            VibrateController.Default.Start(TimeSpan.FromMilliseconds(500));
                        }
                    }

                };
            toast.Title = "Warning !";
            toast.Message = "Please don't close this app and run it under \nlockscreen while TIMER is activated 🙅";
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            toast.ImageSource = new BitmapImage(new Uri("Warning.png", UriKind.RelativeOrAbsolute));
            toast.Show();

            if (!coutdown_timer.IsEnabled)
            {
                coutdown_timer.Start();
            }

        }

        private void stop_timer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if (coutdown_timer.IsEnabled)
            {
                coutdown_timer.Stop();
                coutdown_timer = null;
                toast.Title = "Notice";
                toast.Message = "Now, you can close this app 🙆";
                toast.ImageSource = new BitmapImage(new Uri("Warning.png", UriKind.RelativeOrAbsolute));
                toast.Show();
                start_timer.Visibility = Visibility.Visible;
                stop_timer.Visibility = Visibility.Collapsed;
                pin_timer.Visibility = Visibility.Visible;
            }
        }

        private Popup popup = new Popup();

        private void quickset_timer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            quicksetcontrol quickset=new quicksetcontrol(); 
            popup.Child = quickset;
            popup.IsOpen = true;
            ApplicationBar.Opacity = 0;
            popup.Closed += (s1, e1) =>
            {
                // Add you code here to do something 
                // when the Popup is closed
                ApplicationBar.Opacity = 75;
            };
        }

        private void pin_timer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string pintimer = (timewheel.selectedHour < 10 ? '0' + timewheel.selectedHour.ToString() : timewheel.selectedHour.ToString())
                            + (timewheel.selectedMinute < 10 ? '0' + timewheel.selectedMinute.ToString() : timewheel.selectedMinute.ToString())
                            + (timewheel.selectedSecond < 10 ? '0' + timewheel.selectedSecond.ToString() : timewheel.selectedSecond.ToString());
            string timercurrent=(timewheel.selectedHour < 10 ? '0' + timewheel.selectedHour.ToString() : timewheel.selectedHour.ToString())+":"
                            + (timewheel.selectedMinute < 10 ? '0' + timewheel.selectedMinute.ToString() : timewheel.selectedMinute.ToString())+":"
                            + (timewheel.selectedSecond < 10 ? '0' + timewheel.selectedSecond.ToString() : timewheel.selectedSecond.ToString());

            if (MessageBox.Show("Would you like pin " + timercurrent +  " as TIMER to start screen ?", "QUICK SET TIMER", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ShellTile oTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(pintimer.ToString()));
                if (oTile != null && oTile.NavigationUri.ToString().Contains(pintimer))
                {
                    FlipTileData oFliptile = new FlipTileData();
                    oFliptile.SmallBackgroundImage = new Uri(ImageTiles.image(pintimer, "pintimer.png"),
                                                             UriKind.RelativeOrAbsolute);
                    oFliptile.BackgroundImage = new Uri(ImageTiles.image(pintimer, "pintimer.png"),
                                                        UriKind.RelativeOrAbsolute);
                    oFliptile.WideBackgroundImage
                    oTile.Update(oFliptile);
                    MessageBox.Show("Tile Data successfully update.");
                }
                else
                {
                    Uri tileUri = new Uri("/autotimer.xaml?pintimer=" + pintimer, UriKind.Relative);
                    ShellTileData tileData = new FlipTileData
                    {
                        SmallBackgroundImage = new Uri(ImageTiles.image(pintimer, "pintimer.png"), UriKind.RelativeOrAbsolute),
                        BackgroundImage = new Uri(ImageTiles.image(pintimer, "pintimer.png"), UriKind.RelativeOrAbsolute)
                    };

                    DataHelper.AddTIMER(new TIMER1 { Name = pintimer });
                    ShellTile.Create(tileUri, tileData, false);
                }
                if (
                    MessageBox.Show("Sorry, this feature only available on DONATE version. Would you like get it ?",
                                    "DONATE", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    MarketplaceDetailTask donate_pro = new MarketplaceDetailTask();
                    donate_pro.ContentIdentifier = "4ae8ac78-2bf1-47bc-88d4-d77d66b8fc9b";
                    donate_pro.ContentType = MarketplaceContentType.Applications;
                    donate_pro.Show();
                }
            }      
        }

        private void pivotlayout_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			if (pivotlayout.SelectedItem == pivot_timer)
            {
                ApplicationBar.Mode=ApplicationBarMode.Minimized;
                progressIndicator.Text = "😊 tap on 00:30:00 to set timer";
            }
            else if (pivotlayout.SelectedItem == pivot_about)
            {
                progressIndicator.Text = "😊 this app very useful ?";
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
            else if (pivotlayout.SelectedItem == pivot_stopmusic)
            {
                progressIndicator.Text = "😊 tap on CD icon to STOP MUSIC";
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //if (popup.IsOpen)
            //{
            //    if (popup.Child.Visibility == Visibility.Visible)
            //    {
            //        //Close the PopUp Window
            //        popup.IsOpen = false;
            //        //Keep the back button from 
            //        //navigating away from the current page
            //        e.Cancel = true;
            //        ApplicationBar.Opacity = 75;
            //    }
            //}
            //else
            //{
            //    //There is no PopUp open, use the back button normally
            //    base.OnBackKeyPress(e);
            //}

                e.Cancel = true;
                Application.Current.Terminate();
        }

        private void savetoIsolated(string _file)
        {
            string FileName = _file;

            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(new Uri(FileName, UriKind.Relative));

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(FileName))
                {
                    myIsolatedStorage.DeleteFile(FileName);
                }

                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(FileName, FileMode.Create, myIsolatedStorage))
                {
                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                    {
                        Stream resourceStream = streamResourceInfo.Stream;
                        long length = resourceStream.Length;
                        byte[] buffer = new byte[32];
                        int readCount = 0;
                        using (BinaryReader reader = new BinaryReader(streamResourceInfo.Stream))
                        {
                            // read file in chunks in order to reduce memory consumption and increase performance
                            while (readCount < length)
                            {
                                int actual = reader.Read(buffer, 0, buffer.Length);
                                readCount += actual;
                                writer.Write(buffer, 0, actual);
                            }
                        }
                    }
                }
            }

        }

        private void autoclose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            autoclose.IsChecked = false;
        	if (
                    MessageBox.Show("Sorry, this feature only available on DONATE version. Would you like get it ?",
                                    "DONATE", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    MarketplaceDetailTask donate_pro = new MarketplaceDetailTask();
                    donate_pro.ContentIdentifier = "4ae8ac78-2bf1-47bc-88d4-d77d66b8fc9b";
                    donate_pro.ContentType = MarketplaceContentType.Applications;
                    donate_pro.Show();
                }
        }

        private void donate_bar_Click_1(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
			MarketplaceDetailTask donate_pro=new MarketplaceDetailTask();
            donate_pro.ContentIdentifier = "4ae8ac78-2bf1-47bc-88d4-d77d66b8fc9b";
            donate_pro.ContentType=MarketplaceContentType.Applications;
            donate_pro.Show();
        }

        private void getmore_bt_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceSearchTask getmore = new MarketplaceSearchTask();
            getmore.SearchTerms = "Zodiac Inc.";
            getmore.ContentType=MarketplaceContentType.Applications;
            getmore.Show();
        }

        private void share_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            NavigationService.Navigate(new Uri("/Page2.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}
