using System;
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
            smsComposeTask.Body = "Try this ringtone. It's great! \nhttp://www.windowsphone.com/en-us/store/app/wrecking-ball-ringtone/b1d20662-d367-4f65-8c4d-aededea26792";
            smsComposeTask.Show();
        }

        private void mail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "";
            emailComposeTask.Body = "Try this ringtone. It's great! \nhttp://www.windowsphone.com/en-us/store/app/wrecking-ball-ringtone/b1d20662-d367-4f65-8c4d-aededea26792";
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";
            emailComposeTask.Show();
        }

        private void social_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "Try this ringtone. It's great! \nhttp://www.windowsphone.com/en-us/store/app/wrecking-ball-ringtone/b1d20662-d367-4f65-8c4d-aededea26792";
            shareStatusTask.Show();
        }
    }
}