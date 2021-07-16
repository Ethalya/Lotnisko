using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineReservationSystems
{
    public class Test
    {
        public bool Test1(int candidate)
        {
            if (candidate == 1)
            {
                return false;
            }
            throw new NotImplementedException("Please create a test first.");
        }
        public bool Test2(int candidate)
        {
            if (candidate == 23)
            {
                return true;
            }
            throw new NotImplementedException("Please create a test first.");
        }
    }
}
