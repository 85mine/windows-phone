using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using Database;

namespace Ai_La_Trieu_Phu.Model
{
    class ConnectDatabase
    {
        private const string ConnectionString = "Data Source = isostore:/ALTP.sdf";

        public ConnectDatabase()
        {
            using (var context = new Database.ALTPContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                    context.CreateDatabase();
            }
        }

        public static void MoveReferenceDatabase()
        {

            // Obtain the virtual store for the application.
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a stream for the file in the installation folder.
            using (Stream input = Application.GetResourceStream(new Uri("ALTP.sdf", UriKind.Relative)).Stream)
            {
                // Create a stream for the new file in isolated storage.
                using (IsolatedStorageFileStream output = iso.CreateFile("ALTP.sdf"))
                {
                    // Initialize the buffer.
                    byte[] readBuffer = new byte[4096];
                    int bytesRead = -1;

                    // Copy the file from the installation folder to isolated storage. 
                    while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        output.Write(readBuffer, 0, bytesRead);
                    }
                }
            }
        }

        public static IList<Database.Question> GetQuestionLevel(int level)
        {
            IList<Database.Question> quesList = null;
            using (var context = new Database.ALTPContext(ConnectionString))
            {
                IQueryable<Database.Question> t = from ques in context.Questions where ques.QUESTION_NO == level select ques;
                quesList = t.ToList();
            }
            return quesList;
        }

        public static void SaveScore(Database.HighScore hs)
        {
            using (var context = new Database.ALTPContext(ConnectionString))
            {
                context.HighScores.InsertOnSubmit(hs);
                context.SubmitChanges();
            }
        }

        public static ObservableCollection<Database.HighScore> GetHighScore()
        {
            var highList = new ObservableCollection<Database.HighScore>();
            using (var context = new Database.ALTPContext(ConnectionString))
            {
                var t = from h in context.HighScores orderby h.Score descending select h;
                if (t != null)
                {
                    foreach (var item in t)
                    {
                        highList.Add(item as Database.HighScore);
                    }
                }
                else highList = null;
            }
            return highList;
        }
    }
}
