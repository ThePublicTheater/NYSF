using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class SyosReservationZone
    {
        public int Id { get; set; }
        public List<SyosReservationSeat> Seats { get; set; }

        public SyosReservationZone(int id)
        {
            Id = id;
            Seats = new List<SyosReservationSeat>();
        }
    }
}