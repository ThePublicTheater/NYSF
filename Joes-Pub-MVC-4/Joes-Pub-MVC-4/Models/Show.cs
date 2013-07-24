using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using Nysf.Tessitura;
namespace Joes_Pub_MVC_4.Models
{
    public class Show
    {
        public int ID { get; set; }

        public int JoomlaID { get; set; }
        public int TessProdID { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public DateTime ShowDate { get; set; }
        public int Private { get; set; }
        public int Published { get; set; }
        public string PriceInWords { get; set; }

        public string TileFilename { get; set; }

        //public Show(Performance p)
        //{
        //    this.description=p.
        //}
    }
    public class ShowComment
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ShowID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }
    }
    public class ShowArtists
    {
        public int ID { get; set; }
        public int JoomlaID { get; set; }
        public int ShowID { get; set; }
        public int ArtistID { get; set; }
        public int Priority { get; set; }
    }
    public class ShowGenres
    {
        public int ID { get; set; }
        public int JoomlaID { get; set; }
        public int ShowID { get; set; }
        public int GenreID { get; set; }
    }
    public class ShowMp3s
    {
        public int ID { get; set; }
        public int JoomlaID { get; set; }
        public int ShowID { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
    public class ShowPhotos
    {
        public int ID { get; set; }
        public int JoomlaID { get; set; }
        public int ShowID { get; set; }
        public int Priority { get; set; }
        public string Caption { get; set; }
        public string Filename { get; set; }
    }
    public class ShowVideos
    {
        public int ID { get; set; }
        public int JoomlaID { get; set; }
        public int ShowID { get; set; }
        public string Caption { get; set; }
        public string YoutubeLink { get; set; }
        public int Priority { get; set; }
    }

    public class ShowDBContext : DbContext
    {
        public DbSet<Show> MasterShowList { get; set; }
        public DbSet<ShowArtists> MasterShowArtistList { get; set; }
        public DbSet<ShowGenres> MasterShowGenreList { get; set; }
        public DbSet<ShowMp3s> MasterShowSongList { get; set; }
        public DbSet<ShowPhotos> MasterShowPhotoList { get; set; }
        public DbSet<ShowVideos> MasterShowVideoList { get; set; }
        public DbSet<ShowComment> MasterShowCommentList { get; set; }
    }

    public class ShowViewModel
    {
        public Show _show { get; set; }
        public int commpage { get; set; }
        public int numPics
        {
            get
            {
                int temp = 0;
                foreach (var item in _artistsfull)
                {
                    temp += item._pics.Count;
                }
                temp += _showpics.Count;
                return temp;
            }
        }
        public int numSongs
        {
            get
            {
                int temp = 0;
                foreach (var item in _artistsfull)
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
                foreach (var item in _artistsfull)
                {
                    temp += item._vids.Count;
                }
                temp += _vids.Count;
                return temp;
            }
        }
        public int numPastShows
        {
            get
            {
                return _PastShows.Count;
            }
        }
        public bool hasupcoming
        {
            get
            {
                return _prod == null;
            }
        }

        public Joes_Pub_MVC_4.Models.ShowDBContext ShowDB = new ShowDBContext();
        public ArtistDBContext ArtDB = new ArtistDBContext();
        public Nysf.Tessitura.Production _prod
        {
            get
            {
                if (_show.TessProdID == -1)
                {
                    return new Nysf.Tessitura.Production(1 , "Not Found", "Not Found", false, false);
                }
                var result = Nysf.Tessitura.WebClient.GetProduction(_show.TessProdID + 1);
                if (result != null)
                {
                    return result;
                }
                else return new Nysf.Tessitura.Production(1 , "Not Found", "Not Found", false, false);
            }
        }
        public PagedList.IPagedList<ShowComment> _commentPages
        {
            get
            {
                return _comments.ToPagedList(commpage, 10);
            }
        }
        public List<ShowComment> _comments
        {
            get
            {
                var temp = ShowDB.MasterShowCommentList.Where(s => s.ShowID == _show.ID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ShowComment>();
            }
        }
        public List<ShowComment> _recentComments
        {
            get
            {
                List<ShowComment> temp = _comments.OrderByDescending(s => s.PostDate).ToList();
                if (temp == null || temp.Count == 0)
                    return new List<ShowComment>();
                if (temp.Count > 3)
                {
                    temp.RemoveRange(4, temp.Count - 4);
                    return temp;
                }
                else return temp;
            }
        }
        public List<ShowGenres> _genres
        {
            get
            {
                var temp = ShowDB.MasterShowGenreList.Where(s => s.ShowID == _show.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ShowGenres>();
            }
        }
        public List<ShowMp3s> _songs
        {
            get
            {
                var temp = ShowDB.MasterShowSongList.Where(s => s.ShowID == _show.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ShowMp3s>();
            }
        }
        public List<ShowPhotos> _showpics
        {
            get
            {
                var temp = ShowDB.MasterShowPhotoList.Where(s => s.ShowID == _show.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ShowPhotos>();
            }
        }
        public List<ShowVideos> _vids
        {
            get
            {
                var temp = ShowDB.MasterShowVideoList.Where(s => s.ShowID == _show.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ShowVideos>();
            }
        }
        public List<Show> _PastShows
        {
            get
            {
                if (_show.TessProdID != -1)
                {
                    List<Show> temp = ShowDB.MasterShowList.Where(s => s.TessProdID == _show.TessProdID).ToList();
                    if (temp != null && temp.Count > 0)
                    {
                        return temp;
                    }
                    else return new List<Show>();
                }
                else return new List<Show>();
            }
        }

        public List<ShowArtists> _artists
        {
            get
            {
                var temp = ShowDB.MasterShowArtistList.Where(s => s.ShowID == _show.JoomlaID);
                if (temp != null)
                {
                    return temp.ToList();
                }
                else return new List<ShowArtists>();
            }
        }
        public List<ArtistViewModel> _artistsfull
        {
            get
            {
                List<Artist> temp = new List<Artist>();
                List<ArtistViewModel> tempmod = new List<ArtistViewModel>();
                foreach (var item in _artists)
                {
                    temp.AddRange(ArtDB.MasterArtistList.Where(s => s.JoomlaID == item.ArtistID).ToList());
                }
                foreach (var item in temp)
                {
                    ArtistViewModel mod = new ArtistViewModel();
                    mod.commPage = 1;
                    mod._artist = item;
                    tempmod.Add(mod);
                }
                return tempmod;
            }
        }

        public void DisposeDB()
        {
            ShowDB.Dispose();
            ArtDB.Dispose();
        }
    }

    public class ShowListViewModel
    {
        public List<Show> ShowResults = new List<Show>();
        public List<Nysf.Tessitura.Performance> Perfs = new List<Nysf.Tessitura.Performance>();
        public IPagedList<Show> ShowPages
        {
            get
            {
                return ShowResults.ToPagedList(Page , NumPerPage);
            }
        }
        public IPagedList<Nysf.Tessitura.Performance> PerfPages
        {
            get
            {
                return Perfs.ToPagedList(PerfPage, NumPerPage);
            }
        }
        public DateTime From_date { get; set; }
        public DateTime To_date { get; set; }
        public string Key { get; set; }
        public string Genre { get; set; }
        public string Artistname { get; set; }
        public int PerfPage { get; set; }
        public int Page { get; set; }
        public int NumPerPage { get; set; }

        public ShowListViewModel(DateTime? From = null, DateTime? To = null, string SearchString = null, string Artist = null, string Genre = null, int UpcomingShowPage = 1, int PastShowPage = 1, int NumPerLoad = 15)
        {
            if (From != null)
            {
                From_date = From.Value;
            }
            else From_date = DateTime.MinValue;
            if (To != null)
            {
                To_date = To.Value;
            }
            else To_date = DateTime.MaxValue;

            Key = SearchString;
            PerfPage = UpcomingShowPage;
            Page = PastShowPage;
            NumPerPage = NumPerLoad;
            Artistname = Artist;
            List<Show> result = Utilities.ShowDB.MasterShowList.Where(s =>
                (s.ShowDate <= To_date && s.ShowDate >= From_date) &&
                (string.IsNullOrEmpty(Key) ? true : s.Title.Contains(Key) || s.description.Contains(Key) || s.ShortTitle.Contains(Key) || s.subtitle.Contains(Key))).ToList();
            if (!string.IsNullOrEmpty(Artistname))
            {
                var result2 = Utilities.ArtDB.MasterArtistList.Where(s => s.Name.Contains(Artistname));
                List<ShowArtists> result3 = new List<ShowArtists>();
                foreach (var item in result2)
                {
                    result3.AddRange(Utilities.ShowDB.MasterShowArtistList.Where(s => s.ArtistID == item.JoomlaID));
                }
                result = result.Where(s => result3.Any(a => a.ShowID == s.JoomlaID)).ToList();
            }
            if (!string.IsNullOrEmpty(Genre))
            {
                var result4 = Utilities.GenreDB.GenreList.Where(s => s.Name.Contains(Genre)).ToList();
                List<ShowGenres> result5 = new List<ShowGenres>();
                foreach (var item in result4)
                {
                    result5.AddRange(Utilities.ShowDB.MasterShowGenreList.Where(s => s.GenreID == item.JoomlaID));
                }
                result = result.Where(s => result5.Any(a => a.ShowID == s.JoomlaID)).ToList();
            }
            ShowResults = result;
            Perfs = Nysf.Tessitura.WebClient.GetPerformances(From_date, To_date, Nysf.Types.Organization.JoesPub).ToList();
        }
    }
}