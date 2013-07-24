using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Joes_Pub_MVC_4.Models
{
    public class Genres
    {
        public int ID { get; set; }
        public int JoomlaID { get; set; }
        public string Name { get; set; }
        public int Published { get; set; }
    }

    public class GenresDBContext : DbContext
    {
        public DbSet<Genres> GenreList { get; set; }
    }
}