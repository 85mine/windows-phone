using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace STOP_Music_Pro
{
    public partial class quicksetcontrol : UserControl
    {
        public quicksetcontrol()
        {
            InitializeComponent();
            this.Width = System.Windows.Application.Current.Host.Content.ActualWidth;
            this.Height = System.Windows.Application.Current.Host.Content.ActualHeight;
        }
    }
}
