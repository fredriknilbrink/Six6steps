using System;
using System.Collections.Generic;
using System.Text;
using SixKstep.iOS.Helpers;
using SixKstep.Helpers;
using CoreMotion;
using Foundation;

namespace SixKstep.iOS
{
    class StepManager
    {
        public delegate void DailyStepCountChangedEventHandler(nint stepCount);
        public event DailyStepCountChangedEventHandler DailyStepCountChanged;

        public delegate void HistoryCompleteChangedEventHandler(List<PedometerDataStruct> history);
        public event HistoryCompleteChangedEventHandler HistoryComplete;

        CMPedometer pedometer;
        public nint totalSteps = 0;
        public int numberofDaysinHistory = 0;
        public int totaldaysProcessed = 0;
        private List<PedometerDataStruct> historicalData;

        // Contructor create the Pedometer object 
        public StepManager()
        {
            this.pedometer = new CMPedometer();
            historicalData = new List<PedometerDataStruct>();

        }

        // This method is called to start Pedometer updates
        public void Start()
        {
            this.pedometer.StartPedometerUpdates(new NSDate(), this.UpdatePedometerData);
            GetStepsToday();
        }

        // This method is called to stop Pedometer updates
        public void Stop()
        {
            this.pedometer.StopPedometerUpdates();
        }

        // Gets 
        public async void GetStepsToday()
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now;

            var data = await this.pedometer.QueryPedometerDataAsync(DateHelper.ToNSDate(start), DateHelper.ToNSDate(end));
            this.GetDailyPedometerData(data, null);
            //  RequestAccess();
        }

        public async void GetStepsForPreviousDays(int numberOfdaysBackInTime)
        {
            DateTime start;
            DateTime end;

            historicalData.Clear();
            totaldaysProcessed = 0;
            numberofDaysinHistory = numberOfdaysBackInTime;

            for (int daysBack = 1; daysBack <= numberOfdaysBackInTime; daysBack++)
            {
                start = DateTime.Now.Date.AddDays(-daysBack);
                end = DateTime.Now.Date.AddDays(-daysBack + 1);
                var data = await this.pedometer.QueryPedometerDataAsync(DateHelper.ToNSDate(start), DateHelper.ToNSDate(end));
                this.GetTimeRangePedometerData(data, null);
            }
        }

        // Gets historical data and fills the List structure with data
        // When all historical data is collected the HistoryComplete event is invoked
        private void GetTimeRangePedometerData(CMPedometerData data, NSError error)
        {
            if (error == null)
            {
                try
                {
                    totalSteps = int.Parse(data.NumberOfSteps.ToString());
                    totaldaysProcessed++;
                }
                catch (Exception)
                {
                    totalSteps = 0;
                }
                historicalData.Add(new PedometerDataStruct(DateHelper.ToDateTime(data.StartDate), (int)totalSteps, (double)data.Distance));

                // Now we got all data, invoke event
                if (totaldaysProcessed == numberofDaysinHistory)
                {
                    HistoryComplete?.Invoke(historicalData);
                }

            }
        }

        // The update for daily steps. When steps are detected the DailyStepCountChanged is invoked
        private void GetDailyPedometerData(CMPedometerData data, NSError error)
        {
            if (error == null)
            {
                try
                {
                    totalSteps = int.Parse(data.NumberOfSteps.ToString());
                }
                catch (Exception)
                {
                    totalSteps = 0;
                }
                DailyStepCountChanged?.Invoke(totalSteps);
            }
        }

        // Fires when a new set of steps are taken
        private void UpdatePedometerData(CMPedometerData data, NSError error)
        {
            if (error == null)
            {
                GetStepsToday();
            }

        }
    }
}
