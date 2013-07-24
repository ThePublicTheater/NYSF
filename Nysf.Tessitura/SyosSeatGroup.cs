using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class SyosSeatGroup
    {
        public int Id { get; private set; }
        public SyosSeat[] Seats { get; private set; }

        public SyosSeatGroup(int id, SyosSeat[] seats)
        {
            Id = id;
            Seats = seats;
        }
    }
}