using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Joes_Pub_MVC_4.Models
{
    public static class Utilities
    {
        #region DB ini
        public static ShowDBContext ShowDB = new ShowDBContext();
        public static ArtistDBContext ArtDB = new ArtistDBContext();
        public static NewsDBcontext NewsDB = new NewsDBcontext();
        public static CommentDBContext CommDB = new CommentDBContext();
        public static ProfilesDBContext ProfileDB = new ProfilesDBContext();
        public static GenresDBContext GenreDB = new GenresDBContext();
        public static CacheDBContext CacheDB = new CacheDBContext();
        public static StaffPickDBContext StaffPickDB = new StaffPickDBContext();
        #endregion
    }
}