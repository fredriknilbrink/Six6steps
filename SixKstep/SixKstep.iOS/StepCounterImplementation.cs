using System;
using System.Collections.Generic;
using System.Text;
using SixKstep.iOS;
using SixKstep.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(StepCounterImplementation))]
namespace SixKstep.iOS
{
    public class StepCounterImplementation : IStepCounter
    {
        private StepManager _stepManager;
        private StepEventArgs stepArgs = new StepEventArgs();
        private DataEventArgs historyArgs = new DataEventArgs();
        public event EventHandler StepsUpdated;
        public event EventHandler HistoryUpdated;


        // Constructor
        // Creates a new StepManager object and attached the DailyStepCountChaged event
        public StepCounterImplementation()
        {
            _stepManager = new StepManager();
            _stepManager.DailyStepCountChanged += (steps) =>
            {
                if (StepsUpdated != null)
                {
                    stepArgs.data = (int)steps;
                    StepsUpdated(this, stepArgs);
                }
            };
        }
        public void GetHistory(int daysback)
        {
            _stepManager.GetStepsForPreviousDays(daysback);
            _stepManager.HistoryComplete += (history) =>
            {
                if (HistoryUpdated != null)
                {
                    historyArgs.history = (List<PedometerDataStruct>)history;
                    HistoryUpdated(this, historyArgs);
                }
            };
        }


        public void Start()
        {
            _stepManager.Start();
        }

        public void Stop()
        {
            _stepManager.Stop();
        }
    }
}
