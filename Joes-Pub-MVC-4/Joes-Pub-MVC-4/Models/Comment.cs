using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Joes_Pub_MVC_4.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Anonymous { get; set; }
        public DateTime PostDate { get; set; }
        public string Message { get; set; }

    }

    public class CommentDBContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
    }
}