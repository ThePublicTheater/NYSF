using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
namespace Joes_Pub_MVC_4.Models
{
    public class Profiles
    {
        private List<int> _GenreIDs = new List<int>();
        public List<int> GenreIDs
        {
            get
            {
                return _GenreIDs;
            }
            set
            {
                _GenreIDs = value;
            }
        }

        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool NyMag { get; set; }
        public bool EmailSub { get; set; }
        public string Bio { get; set; }
        public string GenresSerialized
        {
            get
            {
                return string.Join(";", _GenreIDs);
            }
            set
            {
                List<string> temp = value.Split(';').ToList();
                List<int> tempint = new List<int>();
                foreach (var item in temp)
                {
                    int h = -1;
                    int.TryParse(item, out h);
                    tempint.Add(h);
                }
                _GenreIDs = tempint;
            }
        }
    }
    public class ProfilesDBContext : DbContext
    {
        public DbSet<Profiles> ProfileList { get; set; }
    }

    public class ProfileViewModel
    {
        public Profiles CurUser { get; set; }
        public ProfileCache CurUserCache { get; set; }
        public bool needscaching = false;
        public bool ModelIsValid = false;
        public string[] strings = new string[7];
        public List<NewsComments> NewsComms { get; set; }
        public List<ShowComment> ShowComms { get; set; }
        public List<ArtistComment> ArtistComms { get; set; }
        public List<Comment> GeneralComms { get; set; }
        public List<Show> SuggestedShows = new List<Show>();
        public List<Artist> SuggestedArtists = new List<Artist>();
        public List<Genres> Genres { get; set; }
        public Mplayer SuggestedSongs = new Mplayer(true, true);

        public IPagedList<NewsComments> NewsCommPages
        {
            get
            {
                return NewsComms.ToPagedList(1, 5);
            }
        }
        public IPagedList<ShowComment> ShowCommPages
        {
            get
            {
                return ShowComms.ToPagedList(1, 5);
            }
        }
        public IPagedList<ArtistComment> ArtistCommPages
        {
            get
            {
                return ArtistComms.ToPagedList(1, 5);
            }
        }
        public IPagedList<Comment> GeneralCommPages
        {
            get
            {
                return GeneralComms.ToPagedList(1, 5);
            }
        }
        public IPagedList<Show> SuggestedShowsPages
        {
            get
            {
                return SuggestedShows.ToPagedList(1, 5);
            }
        }
        public IPagedList<Artist> SuggestedArtistsPages
        {
            get
            {
                return SuggestedArtists.ToPagedList(1, 5);
            }
        }

        public ProfileViewModel()
        {
        }
        public ProfileViewModel(string UserName)
        {
            var cacheres = Utilities.CacheDB.ProfileCaches.Where(s => s.Username == UserName);
            if (cacheres != null && cacheres.Count() > 0)
            {
                CurUserCache = cacheres.First(s => s.Username == UserName);
                ModelIsValid = true;
            }
            else
            {
                CurUserCache = new ProfileCache(); 
                needscaching = true;
            }
            List<Profiles> result = Utilities.ProfileDB.ProfileList.Where(s => s.UserName == UserName).ToList();
            CurUser = result[0];
            if (needscaching)
            {
                if (result != null && result.Count > 0)
                {
                    #region 
                    NewsComms = Utilities.NewsDB.MasterCommentsListing.Where(s => s.UserId == CurUser.ID).ToList();
                    if (NewsComms == null)
                    {
                        NewsComms = new List<NewsComments>();
                    }
                    ShowComms = Utilities.ShowDB.MasterShowCommentList.Where(s => s.UserID == CurUser.ID).ToList();
                    if (ShowComms == null)
                    {
                        ShowComms = new List<ShowComment>();
                    }
                    ArtistComms = Utilities.ArtDB.MasterArtistCommentList.Where(s => s.UserID == CurUser.ID).ToList();
                    if (ArtistComms == null)
                    {
                        ArtistComms = new List<ArtistComment>();
                    }
                    GeneralComms = Utilities.CommDB.Comments.Where(s => s.UserID == CurUser.ID).ToList();
                    if (GeneralComms == null)
                    {
                        GeneralComms = new List<Comment>();
                    }
                    List<Genres> tempgenres = new List<Genres>();
                    foreach (var item in CurUser.GenreIDs)
                    {
                        var res = Utilities.GenreDB.GenreList.Where(s => s.JoomlaID == item);
                        tempgenres.AddRange(res);
                    }
                    Genres = tempgenres;
                    List<ShowGenres> tempshows = new List<ShowGenres>();
                    List<ArtistGenres> tempartist = new List<ArtistGenres>();
                    foreach (var item in Genres)
                    {
                        tempshows.AddRange(Utilities.ShowDB.MasterShowGenreList.Where(s => s.GenreID == item.JoomlaID));
                        tempartist.AddRange(Utilities.ArtDB.MasterArtistGenreList.Where(s => s.GenreID == item.JoomlaID));
                    }
                    foreach (var item in tempshows)
                    {
                        SuggestedShows.AddRange(Utilities.ShowDB.MasterShowList.Where(s => s.JoomlaID == item.ShowID));
                        List<ShowMp3s> sRes = Utilities.ShowDB.MasterShowSongList.Where(s => s.ShowID == item.ShowID).ToList();
                        foreach (var song in sRes)
                        {
                            MplayerSong temp = new MplayerSong(song.ID, "../../shows/" + song.ShowID + "/mp3s/", song.Filename);
                            if (!SuggestedSongs.Songs.Any(a => a.Filename == temp.Filename))
                            {
                                SuggestedSongs.Songs.Add(temp);
                            }
                        }
                    }
                    foreach (var item in tempartist)
                    {
                        SuggestedArtists.AddRange(Utilities.ArtDB.MasterArtistList.Where(s => s.JoomlaID == item.ArtistID));
                        List<ArtistMp3s> aRes = Utilities.ArtDB.MasterArtistMp3List.Where(s => s.ArtistID == item.ArtistID).ToList();
                        foreach (var song in aRes)
                        {
                            MplayerSong temp = new MplayerSong(song.ID, "../../artists/" + song.ArtistID + "/mp3s/", song.FileName);
                            if (!SuggestedSongs.Songs.Any(a => a.Filename == temp.Filename))
                            {
                                SuggestedSongs.Songs.Add(temp);
                            }
                        }
                    }
                    ModelIsValid = true;
                    #endregion
                }
                else
                {
                    ModelIsValid = false;
                }
            }
        }

    }
}