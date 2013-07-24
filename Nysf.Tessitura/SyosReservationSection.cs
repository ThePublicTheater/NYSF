using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class SyosReservationSection
    {
        public int Id { get; set; }
        public List<SyosReservationZone> Zones { get; private set; }

        public SyosReservationSection(int id)
        {
            Id = id;
            Zones = new List<SyosReservationZone>();
        }
    }
}