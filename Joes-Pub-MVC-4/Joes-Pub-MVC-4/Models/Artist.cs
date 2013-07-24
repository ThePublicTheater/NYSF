using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
namespace Joes_Pub_MVC_4.Models
{
    public class Artist
    {
        public int ID { get; set; }

        public int JoomlaID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Webpage { get; set; }
        public bool Published { get; set; }
        public string TileFilename { get; set; }
        public int CheckedOut { get; set; }
        public DateTime CheckedOutOn { get; set; }
    }
    public class ArtistComment
    {
        public int ID { get; set; }

        public int ArtistID { get; set; }
        public int UserID { get; set; }

        public string Name { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
    public class ArtistGenres
    {
        public int ID { get; set; }

        public int ArtistID { get; set; }
        public int GenreID { get; set; }
    }
    public class ArtistMp3s
    {
        public int ID { get; set; }
        public int ArtistID { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public int Priority { get; set; }
    }
    public class ArtistPhotos
    {
        public int ID { get; set; }
        public int ArtistID { get; set; }
        public string Caption { get; set; }
        public string FileName { get; set; }
        public int Priority { get; set; }
    }
    public class ArtistVideos
    {
        public int ID { get; set; }
        public int ArtistID { get; set; }
        public string Caption { get; set; }
        public int Priority { get; set; }
        public string YouTubeLink { get; set; }
    }

    public class ArtistDBContext : DbContext
    {
        public DbSet<Artist> MasterArtistList { get; set; }
        public DbSet<ArtistComment> MasterArtistCommentList { get; set; }
        public DbSet<ArtistGenres> MasterArtistGenreList { get; set; }
        public DbSet<ArtistMp3s> MasterArtistMp3List { get; set; }
        public DbSet<ArtistPhotos> MasterArtistPhotoList { get; set; }
        public DbSet<ArtistVideos> MasterArtistVideoList { get; set; }
    }

    public class ArtistViewModel
    {
        public Artist _artist { get; set; }
        public int commPage { get; set; }
        public int numPics
        {
            get
            {
                int temp = 0;
                foreach (var item in _showsfull)
                {
                    temp += item._showpics.Count;
                }
                temp += _pics.Count;
                return temp;
            }
        }
        public int numSongs
        {
            get
            {
                int temp = 0;
                foreach (var item in _showsfull)
                {
                    temp += item._songs.Count;
                }
                temp += _songs.Count;
                return temp;
            }
        }
        public int numVids
        {
            get
            {
                int temp = 0;
                foreach (var item in _showsfull)
                {
                    temp += item._vids.Count;
                }
                temp += _vids.Count;
                return temp;
            }
        }
        public bool hasupcoming
        {
            get
            {
                return _prods.Count > 0;
            }
        }

        public  Joes_Pub_MVC_4.Models.ArtistDBContext ArtDB = new ArtistDBContext();
        public Joes_Pub_MVC_4.Models.ShowDBContext ShowDB = new ShowDBContext();
        public  PagedList.IPagedList<ArtistComment> _commentPages
        {
            get
            {
                return _comments.ToPagedList(commPage, 10);
            }
        }
        public  List<ArtistComment> _comments
        {
            get
            {
                List<ArtistComment> temp = ArtDB.MasterArtistCommentList.Where(s => s.ArtistID == _artist.JoomlaID).ToList();
                if (temp != null)
                {
                    return temp;
                }
                else return new List<ArtistComment>();
            }
        }
        public List<ArtistComment> _recentComments
        {
            get
            {
                List<ArtistComment> temp = _comments.OrderByDescending(s => s.PostDate).ToList();
                if (temp == null || temp.Count == 0)
                    return new List<ArtistComment>();
                if (temp.Count > 3)
                {
                    temp.RemoveRange(4, temp.Count - 4);
                    return temp;
                }
                else return temp;
            }
        }
        public  List<ArtistGenres> _genres
        {
            get
            {
                var temp = ArtDB.MasterArtistGenreList.Where(s => s.ArtistID == _artist.JoomlaID);
                if(temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ArtistGenres>();
            }
        }
        public  List<ArtistMp3s> _songs
        {
            get
            {
                var temp = ArtDB.MasterArtistMp3List.Where(s => s.ArtistID == _artist.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ArtistMp3s>();
            }
        }
        public  List<ArtistPhotos> _pics
        {
            get
            {
                var temp = ArtDB.MasterArtistPhotoList.Where(s => s.ArtistID == _artist.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ArtistPhotos>();
            }
        }
        public  List<ArtistVideos> _vids
        {
            get
            {
                var temp = ArtDB.MasterArtistVideoList.Where(s => s.ArtistID == _artist.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ArtistVideos>();
            }
        }
        
        public List<Show> _shows
        {
            get
            {
                List<ShowArtists> result1 = ShowDB.MasterShowArtistList.Where(s => s.ArtistID == _artist.JoomlaID).ToList();
                ShowDB.Dispose();
                ShowDB = new ShowDBContext();
                List<Show> tempshows = new List<Show>();
                foreach (var item in result1)
                {
                    tempshows.AddRange(ShowDB.MasterShowList.Where(s => s.JoomlaID == item.ShowID));
                }
                if (tempshows != null)
                {
                    return tempshows;
                }
                else return new List<Show>();
            }
        }
        public List<ShowViewModel> _showsfull
        {
            get
            {
                List<ShowViewModel> tempmod = new List<ShowViewModel>();
                foreach (var item in _shows)
                {
                    ShowViewModel mod = new ShowViewModel();
                    mod.commpage = 1;
                    mod._show = item;
                    tempmod.Add(mod);
                }
                return tempmod;
            }
        }
        public List<Nysf.Tessitura.Production> _prods
        {
            get
            {
                List<Nysf.Tessitura.Production> tempprods = new List<Nysf.Tessitura.Production>();
                foreach (var item in _shows)
                {
                    tempprods.Add(Nysf.Tessitura.WebClient.GetProduction(item.TessProdID));    
                }
                return tempprods;
            }
        }


        public void DisposeDB()
        {
            ShowDB.Dispose();
            ArtDB.Dispose();
        }
    }
}