using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SixKstep
{
    public static class Settings
    {
        private static readonly int DailyStepDefault = 6000;
        private static readonly bool LockedInterfaceDefault = false;
        private static readonly int HistoryLengthDefalut = 5;


        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static int DailyStepGoal
        {
            get { return AppSettings.GetValueOrDefault<int>("dailystepgoal_key", DailyStepDefault); }
            set { AppSettings.AddOrUpdateValue<int>("dailystepgoal_key", value); }
        }

        public static bool LockedInterface
        {
            get { return AppSettings.GetValueOrDefault<bool>("lockedinterface_key", LockedInterfaceDefault); }
            set { AppSettings.AddOrUpdateValue<bool>("lockedinterface_key", value); }
        }

        public static string Password
        {
            get { return AppSettings.GetValueOrDefault<string>("password_key", String.Empty); }
            set { AppSettings.AddOrUpdateValue<string>("password_key", value); }
        }

        public static int HistoryLength
        {
            get { return AppSettings.GetValueOrDefault<int>("historylength_key", HistoryLengthDefalut); }
            set { AppSettings.AddOrUpdateValue<int>("historylength_key", value); }
        }

    }
}
