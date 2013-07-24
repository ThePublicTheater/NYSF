using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
namespace Joes_Pub_MVC_4.Models
{
    public class News
    {
        private List<string> _Urls = new List<string>();

        public int ID { get; set; }

        public int JoomlaID { get; set; }
        public int SectionId { get; set; }
        public int CatId { get; set; }
        public int CreatedById { get; set; }
        public int ModifiedById { get; set; }
        public int VersionNum { get; set; }
        public int Ordering { get; set; }
        public int Hits { get; set; }

        public string Title { get; set; }
        public string Title_alias { get; set; }
        public string IntroText { get; set; }
        public string FullText { get; set; }
        public string UrlsSerialized
        {
            get
            {
                return String.Join(";", _Urls);
            }
            set
            {
                _Urls = value.Split(';').ToList();
            }
        }

        public bool CheckedOut { get; set; }
        public bool State { get; set; }
        public bool AnonAccess { get; set; }
        public bool Important { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CheckedOutTime { get; set; }
        public DateTime Publish { get; set; }
        public DateTime UnPublish { get; set; }

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

    public class NewsComments
    {
        public int ID { get; set; }
        
        public int NewsId { get; set; }
        public int UserId { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Blurb { get; set; }

        public DateTime PostDate { get; set; }
    }

    public class NewsDBcontext : DbContext
    {
        public DbSet<News> MasterNewsListing { get; set; }
        public DbSet<NewsComments> MasterCommentsListing { get; set; }
    }

    public class NewsViewModel
    {
        public News _news { get; set; }
        public int commPage { get; set; }
        public int NumPerPage { get; set; }

        public IPagedList<NewsComments> _commentPages
        {
            get
            {
                return _comments.ToPagedList(commPage, NumPerPage);
            }
        }
        public List<NewsComments> _comments = new List<NewsComments>();

        public NewsViewModel(News Article, int CommentPage)
        {
            _news = Article;
            commPage = CommentPage;
            NewsDBcontext NewsDB = new NewsDBcontext();
            _comments = NewsDB.MasterCommentsListing.Where(s => s.NewsId == _news.JoomlaID).ToList();
        }
    }

    public class NewsListViewModel
    {
        public string Key {get; private set;}
        public DateTime From_Date {get; private set;}
        public DateTime To_Date {get; private set;}
        public int Page { get; private set; }
        public int NumPerPage { get; private set; }
        public List<News> _articles { get; private set; }
        public IPagedList<News> _articlePages
        {
            get
            {
                return _articles.ToPagedList(Page, NumPerPage);
            }
        }
        public NewsListViewModel(int Pagenumber, int NumberArticlesPerPage, DateTime From, DateTime To, string SearchString)
        {
            Key = SearchString;
            From_Date = From;
            To_Date = To;
            Page = Pagenumber;
            NumPerPage = NumberArticlesPerPage;

            NewsDBcontext DB = new NewsDBcontext();

            List<News> temp = new List<News>();
            temp.AddRange(DB.MasterNewsListing.Where(s => (s.Publish >= From_Date && s.Publish <= To_Date)).ToList());
            if (Key != "")
            {
                string[] temparr = Key.Split(' ');
                var res = temp.Where(s => temparr.Any(a => s.Title.Contains(a) || s.Title_alias.Contains(a) || s.IntroText.Contains(a) || s.FullText.Contains(a)));
                if (res != null && res.Count() > 0)
                {
                    temp = res.ToList();
                }
            }
            _articles = temp;
            DB.Dispose();
        }
    }
}