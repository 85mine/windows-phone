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
using System.IO;
using Ai_La_Trieu_Phu.Model;

namespace Ai_La_Trieu_Phu.View
{
    public partial class LoadQuestion : PhoneApplicationPage
    {
        public LoadQuestion()
        {
            InitializeComponent();
            highScore.ItemsSource = ConnectDatabase.GetHighScore();
        }
    }
}