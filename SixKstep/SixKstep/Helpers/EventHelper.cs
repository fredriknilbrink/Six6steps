using System;
using System.Collections.Generic;
using System.Text;

namespace SixKstep.Helpers
{
   
    public class StepEventArgs : EventArgs
    {
        public int data;
    }

    public class DataEventArgs : EventArgs
    {
        public List<PedometerDataStruct> history;
    }   
}
