using System;
using System.Collections.Generic;
using System.Text;

namespace SixKstep
{
    public interface IStepCounter
    {
        event EventHandler StepsUpdated;
        event EventHandler HistoryUpdated;
        void GetHistory(int daysback);
        void Start();
        void Stop();
    }
}
