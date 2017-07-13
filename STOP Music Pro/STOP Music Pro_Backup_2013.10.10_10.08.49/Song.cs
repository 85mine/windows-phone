using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;

namespace STOP_Music_Pro
{
    class SongMusic
    {
        private static string _name="";
        private static string _artist="";
        private static string _album = "";
        private static BitmapImage _albumcover = new BitmapImage(new Uri("nocover.png", UriKind.RelativeOrAbsolute));

        public static string name
        {
            get
            {
                if (MediaPlayer.State == MediaState.Paused || MediaPlayer.State == MediaState.Playing)
                {
                    _name = MediaPlayer.Queue.ActiveSong.Name;                    
                }

                return _name;
            } 

        }
        public static string artist
        {
            get
            {
                if (MediaPlayer.State == MediaState.Paused || MediaPlayer.State == MediaState.Playing)
                {
                    _artist = MediaPlayer.Queue.ActiveSong.Album.Artist.ToString();
                }
                return _artist;
            } 
        }
        public static string album
        {
            get
            {
                if (MediaPlayer.State == MediaState.Paused || MediaPlayer.State == MediaState.Playing)
                {
                    _album = MediaPlayer.Queue.ActiveSong.Album.Name;
                }
                return _album;
            } 

        }

        public static BitmapImage albumcover
        {
            get
            {
                if (MediaPlayer.State == MediaState.Paused || MediaPlayer.State == MediaState.Playing)
                {
                    Stream stream = MediaPlayer.Queue.ActiveSong.Album.GetAlbumArt();
                    if (stream == null)
                    {
                        return _albumcover;
                    }
                    _albumcover.SetSource(stream);
                }

                return _albumcover;
            }
        }

    }
}
