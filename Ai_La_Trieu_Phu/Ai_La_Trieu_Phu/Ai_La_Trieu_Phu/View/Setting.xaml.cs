using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Ai_La_Trieu_Phu.View
{
    public partial class Setting : PhoneApplicationPage
    {
        public Setting()
        {
            InitializeComponent();
            if (IsolatedStorageSettings.ApplicationSettings["key"].ToString() == "0")
            {
                rd1.IsChecked = true;
            }
            else
                rd2.IsChecked = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("key"))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("key");
                if (rd1.IsChecked == true)
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("key", "0");
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("key", "1");
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
            }
            else
            {
                if (rd1.IsChecked == true)
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("key", "0");
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("key","1");
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
            }    
            
            base.OnBackKeyPress(e);
        }
    }
}