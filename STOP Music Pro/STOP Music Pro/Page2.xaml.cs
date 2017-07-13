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

namespace STOP_Music_Pro
{
    public partial class Page2 : PhoneApplicationPage
    {
        public Page2()
        {
            InitializeComponent();
        }
 private void sms_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = "";
            smsComposeTask.Body = "Try STOP Music. It's great! \nhttp://www.windowsphone.com/en-us/store/app/stop-music/5af99f41-879e-4233-b2d6-8936247669d2";
            smsComposeTask.Show();
        }

        private void mail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "";
            emailComposeTask.Body = "Try STOP Music. It's great! \nhttp://www.windowsphone.com/en-us/store/app/stop-music/5af99f41-879e-4233-b2d6-8936247669d2";
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";
            emailComposeTask.Show();

        }

        private void social_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            ShareStatusTask shareStatusTask = new ShareStatusTask();

            shareStatusTask.Status = "Try STOP Music. It's great! \nhttp://www.windowsphone.com/en-us/store/app/stop-music/5af99f41-879e-4233-b2d6-8936247669d2";

            shareStatusTask.Show();

        }
    }
}