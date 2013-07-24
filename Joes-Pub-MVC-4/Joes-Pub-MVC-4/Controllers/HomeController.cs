using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
namespace Joes_Pub_MVC_4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_New()
        {
            return View("Index_New");
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Information()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult Support()
        {
            return View();
        }

        public ActionResult Press()
        {
            return View();
        }

        public string UpdateGenComment(Models.Comment Comm)
        {
            Comm.PostDate = DateTime.Now;
            Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
            Comm.Name = Mod.CurUser.FirstName + " " + Mod.CurUser.LastName;
            Comm.UserID = Mod.CurUser.ID;
            Comm.Anonymous = false;
            Models.Utilities.CommDB.Comments.Add(Comm);
            Models.Utilities.CommDB.SaveChanges();
            Mod.GeneralComms = Models.Utilities.CommDB.Comments.Where(s => s.UserID == Mod.CurUser.ID).ToList();
            List<string> temp = new List<string>();
            for (int i = 1; i < Mod.GeneralCommPages.PageCount; i++)
            {
                temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_GeneralComms.cshtml", Mod.GeneralComms.ToPagedList(i, 5)));
            }
            Mod.CurUserCache.generalcommentcache = temp;
            Models.Utilities.CacheDB.Entry(Mod.CurUserCache).State = System.Data.EntityState.Modified;
            Models.Utilities.CacheDB.SaveChanges();
            return ControllerContext.RenderPartialViewToString("~/Views/Shared/_Comments.cshtml", null);
        }
        /*
         *             List<string> strs = new List<string>();
            int n = 0;
            using (MySqlConnection conn = new MySqlConnection("server=localhost;User Id=root;database=joespub;password=Diamond's1;Allow Zero Datetime=true;Treat Tiny As Boolean=False;Convert Zero Datetime=true;"))
            {
                    conn.Open();
                    MySqlCommand sql = new MySqlCommand("SELECT * FROM jos_shows ORDER BY showDate DESC", conn);
                    MySqlDataReader rdr = sql.ExecuteReader();
                    while (rdr.Read())
                    {
                        Models.Show newshow = new Models.Show();
                        newshow.ID = (int)rdr[0];
                        string h = rdr[1].ToString();
                        string g = h;
                        if (rdr[1] != null && rdr[1].ToString() != "")
                        {
                            newshow.TessProdID = (int)rdr["tessi_prod_no"];
                        }
                        else newshow.TessProdID = -1;
                        newshow.Title = rdr[6].ToString();
                        newshow.ShortTitle = rdr[7].ToString();
                        newshow.subtitle = rdr[8].ToString();
                        newshow.description = rdr[9].ToString();
                        newshow.ShowDate = DateTime.Parse( rdr[10].ToString());
                        if (newshow.ShowDate.Year == 0001)
                        {
                            newshow.ShowDate = new DateTime(2004,1,1);
                        }
                        if (rdr[11].ToString() == "1")
                        {
                            newshow.Private = 1;
                        }
                        else newshow.Private = 0;
                        if (rdr[12].ToString() == "0")
                        {
                            newshow.Published = 0;
                        }
                        else newshow.Published = 1;
                        newshow.PriceInWords = rdr[13].ToString();
                        newshow.TileFilename = rdr[14].ToString();
                        Models.Utilities.ShowDB.MasterShowList.Add(newshow);
                        Models.Utilities.ShowDB.SaveChanges();
                        n++;
                    }
            }
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
 
         * 
         * 
        #region parse stuff
        List<string> StringArrayRaw = new List<string>();
        List<Models.ShowVideos> ShowArtistList = new List<Models.ShowVideos>();
        private void ParseFile()
        {
            string line;
            int ind;
            StreamReader fs = new StreamReader("C:\\Users\\Christian\\Desktop\\shows_vids.txt");
            line = fs.ReadToEnd();
            while (line.Length > 0)
            {
                ind = line.IndexOf("[:]");
                char[] subline = new char[ind];
                line.CopyTo(0, subline, 0, ind);
                StringArrayRaw.Add(new string(subline));
                line = line.Remove(0, ind + 3);

            }
            fs.Close();
        }
        private void ParseList3()
        {
            char[] tempchar;
            int ind;
            List<char> temp2char;

            int tempInt;
            for (int i = 0; i < StringArrayRaw.Count; i++)
            {
                Models.ShowVideos temp = new Models.ShowVideos();
                //JoomlaID
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                        temp.JoomlaID = -1;
                    }
                    else
                    {
                        temp.JoomlaID = tempInt;
                    }
                }
                //ShowID
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test2 = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test2)
                    {
                        temp.ShowID = -1;
                    }
                    else
                    {
                        temp.ShowID = tempInt;
                    }
                }
                //caption
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.Caption = new string(temp2char.ToArray());
                }
                //caption
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.YoutubeLink = new string(temp2char.ToArray());
                }
                //priority
                tempchar = new char[StringArrayRaw[i].Length];
                StringArrayRaw[i].CopyTo(0, tempchar, 0, StringArrayRaw[i].Length);
                StringArrayRaw[i] = StringArrayRaw[i].Remove(0, StringArrayRaw[i].Length);
                temp2char = tempchar.ToList();
                temp2char.RemoveAt(0);
                temp2char.RemoveAt(temp2char.Count - 1);
                bool test3 = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                if (!test3)
                {
                    temp.Priority = -1;
                }
                else
                {
                    temp.Priority = tempInt;
                }
                ShowArtistList.Add(temp);
            }
        }
        
        private void ParseList4()
        {
            char[] tempchar;
            int ind;
            List<char> temp2char;

            int tempInt;
            for (int i = 0; i < StringArrayRaw.Count; i++)
            {
                Models.Show temp = new Models.Show();
                //JoomlaID
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                        temp.JoomlaID = -1;
                    }
                    else
                    {
                        temp.JoomlaID = tempInt;
                    }
                }
                //TessProdNo
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                        temp.TessProdID = -1;
                    }
                    else
                    {
                        temp.TessProdID = tempInt;
                    }
                }
                //Title
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.Title = new string(temp2char.ToArray());
                }
                //ShortTitle
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.ShortTitle = new string(temp2char.ToArray());
                }
                //subtitle
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.subtitle = new string(temp2char.ToArray());
                }
                //description
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.description = new string(temp2char.ToArray());
                }
                //showDate
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    DateTime tempdate;
                    bool test = DateTime.TryParse(new string(temp2char.ToArray()), out tempdate);
                    if (!test)
                    {
                        temp.ShowDate = new DateTime(2007, 1, 1, 1, 1, 0);
                    }
                    else
                    {
                        temp.ShowDate = tempdate;
                    }
                }
                //private
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                        temp.Private = -1;
                    }
                    else
                    {
                        temp.Private = tempInt;
                    }
                }
                //Published
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                        temp.Published = -1;
                    }
                    else
                    {
                        temp.Published = tempInt;
                    }
                }
                //Price in words
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.PriceInWords = new string(temp2char.ToArray());
                }
                //TileFilname
                tempchar = new char[StringArrayRaw[i].Length];
                StringArrayRaw[i].CopyTo(0, tempchar, 0, StringArrayRaw[i].Length);
                StringArrayRaw[i] = StringArrayRaw[i].Remove(0, StringArrayRaw[i].Length);
                temp2char = tempchar.ToList();
                temp2char.RemoveAt(0);
                temp2char.RemoveAt(temp2char.Count - 1);
                temp.TileFilename = new string(temp2char.ToArray());
                ShowList2.Add(temp);
            }
        }
        

        
        private void ParseList2()
        {
            char[] tempchar;
            int ind;
            List<char> temp2char;

            int tempInt;
            for (int i = 0; i < StringArrayRaw.Count; i++)
            {
                Models.ArtistGenres temp = new Models.ArtistGenres();
                //JoomlaID
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                    }
                    else
                    {
                    }
                }
                //ArtistID
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test)
                    {
                        temp.ArtistID = -1;
                    }
                    else
                    {
                        temp.ArtistID = tempInt;
                    }
                }
                //GenreID
                    tempchar = new char[StringArrayRaw[i].Length];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, StringArrayRaw[i].Length);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, StringArrayRaw[i].Length);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test2 = Int32.TryParse(new string(temp2char.ToArray()), out tempInt);
                    if (!test2)
                    {
                        temp.GenreID = -1;
                    }
                    else
                    {
                        temp.GenreID = tempInt;
                    }
                GenreList.Add(temp);
            }
        }
       
        private void ParseList()
        {
            char[] tempchar;
            int ind;
            List<char> temp2char;

            int tempJoomlaID;
            DateTime tempCreatedOn;
            int tempCreatedBy;
            DateTime tempUpdatedOn;
            int tempUpdatedBy;
            bool tempPublished;
            int tempCheckOut;
            DateTime tempCheckedOutOn;
            for (int i = 0; i < StringArrayRaw.Count; i++)
            {
                Models.Artist temp = new Models.Artist();
                //JoomlaID
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempJoomlaID);
                    if (!test)
                    {
                        temp.JoomlaID = -1;
                    }
                    else
                    {
                        temp.JoomlaID = tempJoomlaID;
                    }
                }
                //CreatedOn
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = DateTime.TryParse(new string(temp2char.ToArray()), out tempCreatedOn);
                    if (!test)
                    {
                        temp.CreatedOn = new DateTime(2007, 1, 1, 1, 0, 0);
                    }
                    else
                    {
                        temp.CreatedOn = tempCreatedOn;
                    }
                }
                //CreatedBy
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempCreatedBy);
                    if (!test)
                    {
                        temp.CreatedBy = -1;
                    }
                    else
                    {
                        temp.CreatedBy = tempCreatedBy;
                    }
                }
                //UpdatedOn
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = DateTime.TryParse(new string(temp2char.ToArray()), out tempUpdatedOn);
                    if (!test)
                    {
                        temp.UpdatedOn = new DateTime(2007, 1, 1, 1, 0, 0);
                    }
                    else
                    {
                        temp.UpdatedOn = tempUpdatedOn;
                    }
                }
                //UpdatedBy
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = Int32.TryParse(new string(temp2char.ToArray()), out tempUpdatedBy);
                    if (!test)
                    {
                        temp.UpdatedBy = -1;
                    }
                    else
                    {
                        temp.UpdatedBy = tempUpdatedBy;
                    }
                }
                //Name
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.Name = new string(temp2char.ToArray());
                }
                //ShortName
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.ShortName = new string(temp2char.ToArray());
                }
                //Subtitle
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.Subtitle = new string(temp2char.ToArray());
                }
                //Description
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.Description = new string(temp2char.ToArray());
                }
                //Webpage
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.Webpage = new string(temp2char.ToArray());
                }
                //Published
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    bool test = bool.TryParse(new string(temp2char.ToArray()), out tempPublished);
                    if (!test)
                    {
                        temp.Published = true;
                    }
                    else
                    {
                        temp.Published = tempPublished;
                    }
                }
                //TileFilename
                ind = StringArrayRaw[i].IndexOf(":|:");
                if (ind > 0)
                {
                    tempchar = new char[ind];
                    StringArrayRaw[i].CopyTo(0, tempchar, 0, ind);
                    StringArrayRaw[i] = StringArrayRaw[i].Remove(0, ind + 3);
                    temp2char = tempchar.ToList();
                    temp2char.RemoveAt(0);
                    temp2char.RemoveAt(temp2char.Count - 1);
                    temp.TileFilename = new string(temp2char.ToArray());
                }
                //CheckedOut
                temp.CheckedOut = 0;
                temp.CheckedOutOn = new DateTime(2007, 1, 1, 1, 0, 0);
                ArtistList.Add(temp);
            }
        }
        private void NewsParseFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\Christian\\Desktop\\file.txt"))
                {
                    String line = sr.ReadToEnd();
                    while (line.Length >= 0)
                    {
                        int ind = line.IndexOf("},{");
                        if (ind != -1)
                        {
                            char[] h = new char[ind];
                            line.CopyTo(0, h, 0, ind);
                            NewsMasterList.Add(h);
                            line = line.Remove(0, ind + 3);
                        }
                        else
                        {
                            char[] h = new char[line.Length];
                            line.CopyTo(0, h, 0, line.Length);
                            NewsMasterList.Add(h);
                            break;
                        }
                    }
                }
            }
            catch (Exception err)
            {

            }
        }
        private void NewsParseList()
        {
            #region TEMP-VARS
            int startInd = 0;
            int normalizedStartInd = 0;
            int endInd = 0;
            int normalizedEndInd = 0;
            int diff = 0;

            int tempInt = 0;
            bool tempBool = false;
            DateTime tempDate = new DateTime();

            string startParam;
            string endParam;
            char[] result;
            bool parseResult;
            #endregion
            ArticleMasterList.Clear();
            foreach (char[] item in NewsMasterList)
            {
                string NewsArticle = new string(item);
                Models.News tempNews = new Models.News();
                #region JOOMLA-ID
                startParam = "\"id\":";
                endParam = "\"title\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.JoomlaID = tempInt;
                }
                else
                {
                    tempNews.JoomlaID = -1;
                }
                #endregion

                #region TITLE
                startParam = "\"title\":";
                endParam = "\"title_alias\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                tempNews.Title = new string(result);
                #endregion

                #region TITLE-ALIAS
                startParam = "\"title_alias\":";
                endParam = "\"introtext\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                tempNews.Title_alias = new string(result);
                #endregion

                #region INTROTEXT
                startParam = "\"introtext\":";
                endParam = "\"fulltext\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                tempNews.IntroText = new string(result);
                #endregion

                #region FULLTEXT
                startParam = "\"fulltext\":";
                endParam = "\"state\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                tempNews.FullText = new string(result);
                #endregion

                #region STATE
                startParam = "\"state\":";
                endParam = "\"sectionid\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = bool.TryParse(new string(result), out tempBool);
                if (parseResult)
                {
                    tempNews.State = tempBool;
                }
                else
                {
                    tempNews.State = false;
                }
                #endregion

                #region SECTION-ID
                startParam = "\"sectionid\":";
                endParam = "\"mask\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.SectionId = tempInt;
                }
                else
                {
                    tempNews.SectionId = -1;
                }
                #endregion

                #region CAT-ID
                startParam = "\"catid\":";
                endParam = "\"created\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.CatId = tempInt;
                }
                else
                {
                    tempNews.CatId = -1;
                }
                #endregion

                #region CREATED-DATE
                startParam = "\"created\":";
                endParam = "\"created_by\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = DateTime.TryParse(new string(result), out tempDate);
                if (parseResult)
                {
                    tempNews.CreatedDate = tempDate;
                }
                else
                {
                    tempNews.CreatedDate = new DateTime(2000, 1, 1);
                }
                #endregion

                #region CREATED-BY-ID
                startParam = "\"created_by\":";
                endParam = "\"created_by_alias\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.CreatedById = tempInt;
                }
                else
                {
                    tempNews.CreatedById = -1;
                }
                #endregion

                #region MODIFIED-DATE
                startParam = "\"modified\":";
                endParam = "\"modified_by\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = DateTime.TryParse(new string(result), out tempDate);
                if (parseResult)
                {
                    tempNews.ModifiedDate = tempDate;
                }
                else
                {
                    tempNews.ModifiedDate = new DateTime(2000, 1, 1);
                }
                #endregion

                #region MODIFIED-BY-ID
                startParam = "\"modified_by\":";
                endParam = "\"checked_out\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.ModifiedById = tempInt;
                }
                else
                {
                    tempNews.ModifiedById = -1;
                }
                #endregion

                #region ISCHECKED-OUT
                startParam = "\"checked_out\":";
                endParam = "\"checked_out_time\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = bool.TryParse(new string(result), out tempBool);
                if (parseResult)
                {
                    tempNews.CheckedOut = tempBool;
                }
                else
                {
                    tempNews.CheckedOut = false;
                }
                #endregion

                #region CHECKED-OUT-DATE
                startParam = "\"checked_out_time\":";
                endParam = "\"publish_up\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = DateTime.TryParse(new string(result), out tempDate);
                if (parseResult)
                {
                    tempNews.CheckedOutTime = new DateTime(2000, 1, 1);
                }
                else
                {
                    tempNews.CheckedOutTime = new DateTime(2000, 1, 1);
                }
                #endregion

                #region PUBLISH-DATE
                startParam = "\"publish_up\":";
                endParam = "\"publish_down\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = DateTime.TryParse(new string(result), out tempDate);
                if (parseResult)
                {
                    tempNews.Publish = tempDate;
                }
                else
                {
                    tempNews.Publish = new DateTime(2000, 1, 1);
                }
                #endregion

                #region UNPUBLISH-DATE
                startParam = "\"publish_down\":";
                endParam = "\"images\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = DateTime.TryParse(new string(result), out tempDate);
                if (parseResult)
                {
                    tempNews.UnPublish = new DateTime(2000,1,1);
                }
                else
                {
                    tempNews.UnPublish = new DateTime(2000, 1, 1);
                }
                #endregion

                #region VERSION-NUM
                startParam = "\"version\":";
                endParam = "\"parentid\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.VersionNum = tempInt;
                }
                else
                {
                    tempNews.VersionNum = -1;
                }
                #endregion

                #region ORDERING
                startParam = "\"ordering\":";
                endParam = "\"metakey\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.Ordering = tempInt;
                }
                else
                {
                    tempNews.Ordering = -1;
                }
                #endregion

                #region ACCESS
                startParam = "\"access\":";
                endParam = "\"hits\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.IndexOf(endParam);
                normalizedEndInd = (endInd - 2);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = bool.TryParse(new string(result), out tempBool);
                if (parseResult)
                {
                    tempNews.AnonAccess = tempBool;
                }
                else
                {
                    tempNews.AnonAccess = false;
                }
                #endregion

                #region HITS
                startParam = "\"hits\":";
                startInd = NewsArticle.IndexOf(startParam);
                endInd = NewsArticle.LastIndexOf("\"");
                normalizedEndInd = (endInd);
                normalizedStartInd = (startInd + startParam.Length + 1);
                diff = normalizedEndInd - normalizedStartInd;
                result = new char[diff];
                NewsArticle.CopyTo(normalizedStartInd, result, 0, diff);
                parseResult = Int32.TryParse(new string(result), out tempInt);
                if (parseResult)
                {
                    tempNews.Hits = tempInt;
                }
                else
                {
                    tempNews.Hits = -1;
                }
                #endregion

                ArticleMasterList.Add(tempNews);
            }
        }
        private void AddtoDB()
        {
            Models.NewsDBcontext Ndb = new Models.NewsDBcontext();
            int count = 0;
            foreach (Models.News item in ArticleMasterList)
            {
                Ndb.MasterNewsListing.Add(item);
                Ndb.SaveChanges();
                count++;
            }
        }
        #endregion
        */
    }
}
