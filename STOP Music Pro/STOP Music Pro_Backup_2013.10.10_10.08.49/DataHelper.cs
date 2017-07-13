using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TIMER;
using System.Diagnostics;

namespace STOP_Music_Pro
{
    class DataHelper
    {
        private const string ConnectionString = @"isostore:/TIMER.sdf";
        public static string FileName = "TIMER.sdf";
        private TIMERDataContext dc = ((App)App.Current).DbDC;
        public bool CreateDatabase()
        {
            bool created = false;
            using (var context = new TIMERDataContext(ConnectionString))
            {
                if (!context.DatabaseExists())
                {
                    string[] names = this.GetType().Assembly.GetManifestResourceNames();
                    string name = names.Where(n => n.EndsWith(FileName)).FirstOrDefault();
                    if (name != null)
                    {
                        using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
                        {
                            if (resourceStream != null)
                            {
                                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                                {
                                    using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(FileName, FileMode.Create, myIsolatedStorage))
                                    {
                                        using (BinaryWriter writer = new BinaryWriter(fileStream))
                                        {
                                            long length = resourceStream.Length;
                                            byte[] buffer = new byte[32];
                                            int readCount = 0;
                                            using (BinaryReader reader = new BinaryReader(resourceStream))
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
                                created = true;
                            }
                            else
                            {
                                context.CreateDatabase();
                                created = true;
                            }
                        }
                    }
                    else
                    {
                        context.CreateDatabase();
                        created = true;
                    }
                }
            }
            return created;
        }
        int? cachedCount = null;
        public int Count
        {
            get
            {
                if (!cachedCount.HasValue)
                {
                    var wordcount = dc.TIMERs.Count();
                    cachedCount = wordcount;
                }
                return cachedCount.Value;
            }
        }
        public static void DeleteDatabase()
        {

            using (var context = new TIMERDataContext(ConnectionString))
            {
                if (context.DatabaseExists())
                {
                    // delete database if it exist
                    context.DeleteDatabase();
                }
            }
        }
        public static void AddTIMER(TIMER.TIMER1 _timer)
        {
            using (var context = new TIMERDataContext(ConnectionString))
            {
                if (context.DatabaseExists())
                {
                    try
                    {
                        DeleteTIMER(_timer, _timer.Name);
                        context.TIMERs.InsertOnSubmit(_timer);
                        context.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        public static IList<TIMER.TIMER1> GetTIMER()
        {
            IList<TIMER.TIMER1> Timers;
            using (var context = new TIMERDataContext(ConnectionString))
            {
                IQueryable<TIMER.TIMER1> query = from jn in context.TIMERs select jn;
                Timers = query.ToList();

            }
            return Timers;
        }

        public static bool DeleteTIMER(TIMER.TIMER1 _timers, string _nameTimer)
        {
            using (var context = new TIMERDataContext(ConnectionString))
            {
                if (context.DatabaseExists())
                {
                    IQueryable<TIMER.TIMER1> items = from timer in context.TIMERs where timer.Name==_nameTimer  select timer;
                    foreach (var item in items)
                    {
                        if (item.Name != null)
                        {
                            context.TIMERs.DeleteOnSubmit(item);
                            context.SubmitChanges();
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
