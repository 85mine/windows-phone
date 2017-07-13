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
using AMobiSDK;
using Microsoft.Phone.Controls;
using System.Threading;
using System.IO.IsolatedStorage;
using Ai_La_Trieu_Phu.Model;
using Coding4Fun.Toolkit.Controls;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Ai_La_Trieu_Phu
{
    public partial class MainPage : PhoneApplicationPage
    {
        AboutPrompt pr = new AboutPrompt();
        RadioButton rb1 = new RadioButton();
        RadioButton rb2 = new RadioButton();
        ListBox list = new ListBox();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            if (!store.FileExists("ALTP.sdf"))
            {
                ConnectDatabase.MoveReferenceDatabase();
            }
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings["key"].ToString() == "0")
            {
                nhac_nen.Volume = 1;
            }
            else
            {
                nhac_nen.Volume = 0;
            }
            Storyboard_logo.Begin();
            Storyboard_khoidong.Begin();
            
        }

        private void batdau_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Storyboard_khoidong.Stop();
            batdau.Opacity = 1;
            caidat.Opacity = 0.4;
            gioithieu.Opacity = 0.4;
            diemcao.Opacity = 0.4;
            Microsoft.Phone.Reactive.Scheduler.Dispatcher.Schedule(OnTimerDone, TimeSpan.FromSeconds(0.5));  
        }

        private void OnTimerDone()
        {
            nhac_nen.Stop();
            NavigationService.Navigate(new Uri("/View/PlayPage.xaml", UriKind.Relative));
        }

        private void caidat_MouseEnter_1(object sender, MouseEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Setting.xaml",UriKind.Relative));
        }

        private void gioithieu_MouseEnter_1(object sender, MouseEventArgs e)
        {
            AboutPrompt prompt = new AboutPrompt();
            prompt.Title = "Ai là triệu phú";
            prompt.VersionNumber = "Phiên bản 1.0";
            prompt.Body = "Gameshow Ai là triệu phú phiên bản cho \nsmartphone trên nền tảng Windows Phone."
                +"\nChúc các bạn chơi game vui vẻ.";
            prompt.Show();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            MessageBoxResult res = MessageBox.Show("Bạn có muốn thoát chương trình?", "Thoát game?", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                e.Cancel = false;
                base.OnBackKeyPress(e);
            }
        }

        private void diemcao_MouseEnter_1(object sender, MouseEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Diemcao.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            AdManager.setWidgetCode("1aaad03c180dff50ebc1277cd78414c2");
            AdManager advestisement=new AdManager();
            advestisement.OnNavigationTo(this,null,null,banner320x50);
        }
    }
}