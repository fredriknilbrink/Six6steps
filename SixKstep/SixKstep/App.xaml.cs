using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixKstep.Helpers;


using Xamarin.Forms;

namespace SixKstep
{
       public partial class App : Application
        {

            SixKstep.MainPage mainPage;
            SixKstep.SettingsPage settingsPage;
            SixKstep.HistoryPage historyPage;

            private int todaysSteps = 0;

            public static IDisplaySettings DisplaySettings { get; private set; }
            public static void Init(IDisplaySettings displaySettings)
            {
                App.DisplaySettings = displaySettings;
            }

            public App()
            {

                DependencyService.Get<IStepCounter>().StepsUpdated += App_stepsUpdated;
                //Settings page
                settingsPage = new SettingsPage();
                settingsPage.SettingsAltered += SettingsPage_SettingsAltered;
                settingsPage.Appearing += SettingsPage_Appearing;

                // History page
                historyPage = new HistoryPage();
                historyPage.Appearing += HistoryPage_Appearing;

                // Main page
                mainPage = new MainPage();
                mainPage.HistoryRequested += MainPage_HistoryRequested;
                mainPage.SettingsRequested += MainPage_SettingsRequested;
                mainPage.Appearing += MainPage_Appearing;

                MainPage = new NavigationPage(mainPage);
            }

            private void SettingsPage_Appearing(object sender, EventArgs e)
            {
                settingsPage.lockInterface = Settings.LockedInterface;
                settingsPage.SetButtonInterface(settingsPage.lockInterface);
            }

            private void App_stepsUpdated(object sender, EventArgs e)
            {
                StepEventArgs stepstaken = (StepEventArgs)e;
                todaysSteps = stepstaken.data;
                mainPage.UpdateSteps(todaysSteps);
            }

            private void HistoryPage_Appearing(object sender, EventArgs e)
            {
                historyPage.UpdateHistoryLength();
            }

            private async void SettingsPage_SettingsAltered(object sender, EventArgs e)
            {
                await MainPage.Navigation.PopAsync();
            }

            private void MainPage_Appearing(object sender, EventArgs e)
            {
                mainPage.UpdateSteps(todaysSteps);
                mainPage.AdaptToLock();
            }

            private void MainPage_SettingsRequested(object sender, EventArgs e)
            {
                MainPage.Navigation.PushAsync(settingsPage);
            }

            private void MainPage_HistoryRequested(object sender, EventArgs e)
            {
                MainPage.Navigation.PushAsync(historyPage);
            }


            protected override void OnStart()
            {
                // Handle when your app starts
            }

            protected override void OnSleep()
            {
                // Handle when your app sleeps
            }

            protected override void OnResume()
            {
                // Handle when your app resumes
            }
        }
    }

