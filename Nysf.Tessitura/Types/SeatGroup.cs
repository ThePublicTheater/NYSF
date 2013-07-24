using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura.Types
{
    public class SeatGroup
    {
        public int Id { get; private set; }
        public Performance Performance { get; private set; }
        public int ZoneId { get; private set; }
        public Seat[] Seats { get; private set; }
    }
}