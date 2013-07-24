using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Joes_Pub_MVC_4.Models
{
    public class CalendarCache
    {
        public int ID { get; set; }
        public string cachedpage { get; set; }
        public DateTime CachedDate { get; set; }
        public DateTime DeleteDate { get; set; }
        public DateTime CachedMonth { get; set; }
    }
    public class ProfileCache
    {
        private List<string> _sshowscache = new List<string>();
        private List<string> _sartistcache = new List<string>();

        public int ID { get; set; }
        public string Username { get; set; }
        public DateTime CachedDate { get; set; }
        public DateTime DeleteDate { get; set; }

        public string playercache { get; set; }

        public string serializedss
        {
            get
            {
                return string.Join("{|}", _sshowscache);
            }
            set
            {
                List<string> templist = new List<string>();
                string h = value;
                int ind = 0;
                ind = h.IndexOf("{|}");
                while (ind != -1)
                {
                    char[] j = new char[ind];
                    h.CopyTo(0, j, 0, ind - 1);
                    templist.Add(new string(j));
                    h = h.Remove(0, ind + 3);
                    ind = h.IndexOf("{|}");
                }
                templist.Add(h);
                _sshowscache = templist;
            }
        }
        public string serializedsa
        {
            get
            {
                return string.Join("{|}", _sartistcache);
            }
            set
            {
                List<string> templist = new List<string>();
                string h = value;
                int ind = 0;
                ind = h.IndexOf("{|}");
                while (ind != -1)
                {
                    char[] j = new char[ind];
                    h.CopyTo(0, j, 0, ind - 1);
                    templist.Add(new string(j));
                    h = h.Remove(0, ind + 3);
                    ind = h.IndexOf("{|}");
                }
                templist.Add(h);
                _sartistcache = templist;
            }
        }

        public string serializednc { get; set; }
        public string serializedsc { get; set; }
        public string serializedac { get; set; }
        public string serializedgc { get; set; }

        public List<string> sshowscache
        {
            get
            {
                return _sshowscache;
            }
            set
            {
                _sshowscache = value;
            }
        }
        public List<string> sartistcache
        {
            get
            {
                return _sartistcache;
            }
            set
            {
                _sartistcache = value;
            }
        }
        public List<string> newscommentcache
        {
            get
            {
                return serializednc.Split(new char[] { ':', ';', ':' }).ToList();
            }
            set
            {
                serializednc = String.Join(":;:", value);
            }
        }
        public List<string> showcommentcache
        {
            get
            {
                return serializedsc.Split(new char[] { ':', ';', ':' }).ToList();
            }
            set
            {
                serializedsc = String.Join(":;:", value);
            }
        }
        public List<string> artistcommentcache
        {
            get
            {
                return serializedac.Split(new char[] { ':', ';', ':' }).ToList();
            }
            set
            {
                serializedac = String.Join(":;:", value);
            }
        }
        public List<string> generalcommentcache
        {
            get
            {
                return serializedgc.Split(new char[] { ':', ';', ':' }).ToList();
            }
            set
            {
                serializedgc = String.Join(":;:", value);
            }
        }
    }

    public class HomeCache
    {
        public int ID { get; set; }
    }
    public class CacheDBContext : DbContext
    {
        public DbSet<CalendarCache> CalendarCaches { get; set; }
        public DbSet<ProfileCache> ProfileCaches { get; set; }
    }
}