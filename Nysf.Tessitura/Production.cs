using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class Production
    {
        

        private int id;
        private string title;
        private string synopsis;
     

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "The production ID must be positive or zero.");
                else
                    id = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(
                        "A production must have a name.");
                if (value.Length > 30)
                    throw new ArgumentException(
                        "The production's name must not exceed 30 characters.");
                title = value;
            }
        }

        public string Synopsis { get; set; }

        /// <remarks>
        ///     This is based on whether prices have been configured for web sale for the next
        ///     performance.  If performances other than the very next do have prices configured,
        ///     it won't be reflected in this Nysf.Tessitura.Production class.
        /// </remarks>
        public bool IsOnSale { get; set; }

        public bool IsSoldOut { get; set; }


        public Performance[] Performances
        {
            get
            {
                List<Performance> perfs = new List<Performance>(WebClient.GetPerformances());
                for (int i = perfs.Count-1; i >= 0; i--)
                {
                    if (perfs[i].ProductionId != id)
                        perfs.RemoveAt(i);
                }
                return perfs.ToArray();
            }
        }

        public Production(int id, string title, string synopsis, bool isOnSale, bool isSoldOut)
        {
            Id = id;
            Title = title;
            Synopsis = synopsis;
            IsOnSale = isOnSale;
            IsSoldOut = isSoldOut;
           
            
        }
       
    }
}