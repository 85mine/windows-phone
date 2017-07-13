using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;

namespace Music
{
    public partial class MainPage : PhoneApplicationPage
    {
        private int _width = 480;

        private int _height = 800;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

			//Shows the rate reminder message, according to the settings of the RateReminder.
            (App.Current as App).rateReminder.Notify();
            this.Width = System.Windows.Application.Current.Host.Content.ActualWidth;
            this.Height = System.Windows.Application.Current.Host.Content.ActualHeight;
            _width = (int)this.Width;
            _height = (int) this.Height;
        }

        private void next_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            MediaPlayer.MoveNext();
        }

        private void back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            MediaPlayer.MovePrevious(); 
        }

        private void stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            try
            {
                var mediaElement = new MediaElement();
                mediaElement.Source = new Uri("cut.mp3", UriKind.Relative);
                mediaElement.Position = new TimeSpan(0);
                LayoutRoot.Children.Add(mediaElement); //Add to visual tree
                mediaElement.Play();
                mediaElement.Stop();
            }
            catch (Exception)
            {
                Application.Current.Terminate();
            }
        }

        private void Play_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }
    }
}
