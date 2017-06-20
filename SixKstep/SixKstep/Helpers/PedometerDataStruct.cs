using System;
using System.Collections.Generic;
using System.Text;

namespace SixKstep.Helpers
{
    
        public struct PedometerDataStruct
        {
            public DateTime day;
            public int steps;
            public double distance;

            public PedometerDataStruct(DateTime dt, int s, double dist)
            {
                steps = s;
                day = dt;
                distance = dist;
            }

        }
    
}
