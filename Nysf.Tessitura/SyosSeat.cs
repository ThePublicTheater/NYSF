using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class SyosSeat
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public int LocationId { get; private set; }
        public SyosSeatStatus Status { get; private set; }

        public SyosSeat(int id, int number, int xposition, int yposition, SyosSeatStatus status)
        {
            Id = id;
            Number = number;
            Status = status;
            LocationId = Int32.Parse(xposition.ToString("D3") + yposition.ToString("D3"));
        }
    }
}