using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.TicketScanning
{
	public class FacilityCollection : IEnumerable
	{
        private Facility[] facilities;

		public FacilityCollection()
		{
			facilities = new Facility[0];
		}

		public FacilityCollection(Facility[] facilities)
		{
			this.facilities = facilities;
		}

        public Facility this[int i]
        {
            get
            {
				return facilities[i];
            }
        }

        public int Count
        {
            get
            {
				return facilities.Length;
            }
        }

		public IEnumerator GetEnumerator()
        {
			return new Enumerator(facilities);
        }

        private class Enumerator : IEnumerator
        {
            private Facility[] facilities;
            private int position = -1;

            public Enumerator(Facility[] facilities)
            {
				this.facilities = facilities;
            }

            public bool MoveNext()
            {
                position++;
				return position < facilities.Length;
            }

            public void Reset()
            {
                position = -1;
            }

            public object Current
            {
                get
                {
                    try
                    {
						return facilities[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
	}
}