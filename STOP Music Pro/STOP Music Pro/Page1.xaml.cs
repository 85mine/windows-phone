using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace STOP_Music_Pro
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
            FrameworkDispatcher.Update();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("tile", out msg))
            {
                if (msg == "pinstop")
                {
                    try
                    {
                        Song track = Song.FromUri("STOP Music", new Uri("cut.mp3", UriKind.Relative));
                        MediaPlayer.Play(track);
                        MediaPlayer.Stop();
                        Application.Current.Terminate();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error, please try again :(");
                    }

                }

            }
        }
    }
}