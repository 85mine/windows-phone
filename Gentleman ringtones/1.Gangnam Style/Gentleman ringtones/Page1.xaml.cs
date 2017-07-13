﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Gentleman_ringtones
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void sms_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = "";
            smsComposeTask.Body = "Try this ringtone. It's great! \nhttp://www.windowsphone.com/en-us/store/app/gangnam-style-ringtone/c4391a95-c803-4eed-9a3b-0963d004a0b6";
            smsComposeTask.Show();
        }

        private void mail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "";
            emailComposeTask.Body = "Try this ringtone. It's great! \nhttp://www.windowsphone.com/en-us/store/app/gangnam-style-ringtone/c4391a95-c803-4eed-9a3b-0963d004a0b6";
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";
            emailComposeTask.Show();
        }

        private void social_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "Try this ringtone. It's great! \nhttp://www.windowsphone.com/en-us/store/app/gangnam-style-ringtone/c4391a95-c803-4eed-9a3b-0963d004a0b6";
            shareStatusTask.Show();
        }
    }
}