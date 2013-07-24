using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Joes_Pub_MVC_4.Models;
namespace Joes_Pub_MVC_4.Models
{
    public class Mplayer
    {
        public List<MplayerSong> Songs = new List<MplayerSong>();
        public Mplayer()
        {
            List<ShowMp3s> sRes = Utilities.ShowDB.MasterShowSongList.ToList();
            foreach (var item in sRes)
            {
                MplayerSong temp = new MplayerSong(item.ID,"../../shows/" + item.ShowID + "/mp3s/", item.Filename);
                if (!Songs.Any(a => a.Filename == temp.Filename))
                {
                    Songs.Add(temp);
                }
            }
            List<ArtistMp3s> aRes = Utilities.ArtDB.MasterArtistMp3List.ToList();
            foreach (var item in aRes)
            {
                MplayerSong temp = new MplayerSong(item.ID, "../../artists/" + item.ArtistID + "/mp3s/", item.FileName);
                if (!Songs.Any(a => a.Filename == temp.Filename))
                {
                    Songs.Add(temp);
                }
            }

        }
        public Mplayer(bool Shows, bool Artists)
        {

        }
        public Mplayer(Show _show)
        {

        }
        public Mplayer(Artist _artist)
        {

        }
    }

    public class MplayerSong
    {
        public int ID { get; set; }
        public string FullPath { get; set; }
        public string Filename { get; set; }
        public MplayerSong(int id, string fullpath, string filename)
        {
            ID = id;
            FullPath = fullpath;
            Filename = filename;
        }
    }
}