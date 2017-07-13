using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Gentleman_ringtones
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SaveRingtoneTask saveringtone;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            savetoiso();
        }

		void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

		private void psy_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
            saveringtone = new SaveRingtoneTask();
            saveringtone.Source = new Uri("isostore:/psy.mp3");
            saveringtone.DisplayName = "Ringa Linga ringtone";
            saveringtone.Completed += saveringtone_Completed;
            saveringtone.Show();
		}

        void saveringtone_Completed(object sender, TaskEventArgs e)
        {
            switch (e.TaskResult)
            {
                //Logic for when the ringtone was saved successfully
                case TaskResult.OK:
                    MessageBox.Show("Ringtone saved.");
                    break;

                //Logic for when the task was cancelled by the user
                case TaskResult.Cancel:
                    MessageBox.Show("Save cancelled.");
                    break;

                //Logic for when the ringtone could not be saved
                case TaskResult.None:
                    MessageBox.Show("Ringtone could not be saved.");
                    break;
            }

        }

        private void savetoiso()
        {
            string FileName = "psy.mp3";

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

        private void rate_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
			MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        private void getmore_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            MarketplaceSearchTask searchTask = new MarketplaceSearchTask();
            searchTask.SearchTerms = "DamSoft";
            searchTask.Show();
        }

        private void donate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "58bb9007-cba9-45c3-ae32-b9ed75914c04";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }

        private void share_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			NavigationService.Navigate(new Uri("/Page1.xaml",UriKind.RelativeOrAbsolute));
        }
    }
}
