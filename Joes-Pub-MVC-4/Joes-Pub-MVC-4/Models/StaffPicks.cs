using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
namespace Joes_Pub_MVC_4.Models
{
    public class StaffPick
    {
        private List<string> _Urls = new List<string>();

        public int ID { get; set; }
        public int Catagory { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UnPublishDate { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }

        public string SerializedUrls
        {
            get
            {
                return string.Join(";", _Urls);
            }
            set
            {
                _Urls = value.Split(';').ToList();
            }
        }

        public List<string> Urls
        {
            get
            {
                return _Urls;
            }
            set
            {
                _Urls = value;
            }
        }
    }

    public class StaffPickComments
    {
        public int ID { get; set; }
        public int PickID { get; set; }
        public int UserID { get; set; }
        public DateTime PostDate { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class StaffPickSongs
    {
        public int ID { get; set; }
        public int PickID { get; set; }
        public string Loc { get; set; }
    }

    public class StaffPickVideos
    {
        public int ID { get; set; }
        public int PickID { get; set; }
        public string Loc { get; set; }
    }

    public class StaffPickPictures
    {
        public int ID { get; set; }
        public int PickID { get; set; }
        public string Loc { get; set; }
    }

    public class StaffPickDBContext : DbContext
    {
        public DbSet<StaffPick> Picks { get; set; }
        public DbSet<StaffPickComments> PickComments { get; set; }
        public DbSet<StaffPickPictures> PickPictures { get; set; }
        public DbSet<StaffPickSongs> PickSongs { get; set; }
        public DbSet<StaffPickVideos> PickVideos { get; set; }
    }

    public class SPlistViewModel
    {
        public List<SPViewModel> _Picks = new List<SPViewModel>();
        public IPagedList<SPViewModel> _PickPages;
        public int _Page = 1;
        public SPlistViewModel(List<SPViewModel> picks, int? page)
        {
            if (page.HasValue)
            {
                _Page = page.Value;
            }
            _Picks = picks;
            _PickPages = _Picks.ToPagedList(_Page, 10);
        }
    }
    public class SPViewModel
    {
        public StaffPick _Pick = new StaffPick();
        public List<StaffPickComments> _Comms = new List<StaffPickComments>();
        public int Page = 1;
        public IPagedList<StaffPickComments> _CommsPages;
        public List<StaffPickPictures> _Pics = new List<StaffPickPictures>();
        public List<StaffPickSongs> _Songs = new List<StaffPickSongs>();
        public List<StaffPickVideos> _Vids = new List<StaffPickVideos>();
        public SPViewModel(StaffPick pick, int? _page)
        {
            _Pick = pick;
            _Comms = Utilities.StaffPickDB.PickComments.Where(s => s.PickID == _Pick.ID).ToList();
            if (_page.HasValue)
            {
                Page = _page.Value;
            }
            _CommsPages = _Comms.ToPagedList( Page, 10);
            _Pics = Utilities.StaffPickDB.PickPictures.Where(s => s.PickID == _Pick.ID).ToList();
            _Songs = Utilities.StaffPickDB.PickSongs.Where(s => s.PickID == _Pick.ID).ToList();
            _Vids = Utilities.StaffPickDB.PickVideos.Where(s => s.PickID == _Pick.ID).ToList();
        }
    }
}