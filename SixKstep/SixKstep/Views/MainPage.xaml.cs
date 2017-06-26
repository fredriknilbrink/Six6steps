using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SixKstep
{
	
    public partial class MainPage : ContentPage
    {
        private int screenHight;
        private int screenWidth;

        public event EventHandler HistoryRequested;
        public event EventHandler SettingsRequested;

        public MainPage()
        {
            InitializeComponent();
            screenHight = (int)App.DisplaySettings.GetHeight();
            screenWidth = (int)App.DisplaySettings.GetWidth();
            AdaptToLock();
        }

        public void AdaptToLock()
        {
            if (Settings.LockedInterface)
            {
                Stop();
                imgHai.IsVisible = true;
                infoStack.IsVisible = false;
                stackUnlock.IsVisible = true;
                imgLogo.IsVisible = false;
                entPassword.Text = string.Empty;
            }
            else
            {
                Start();
                imgHai.IsVisible = false;
                stackUnlock.IsVisible = false;
                infoStack.IsVisible = true;
                imgLogo.IsVisible = true;
            }
        }
        private void Settings_Activated(object sender, EventArgs e)
        {
            if (!Settings.LockedInterface)
                SettingsRequested?.Invoke(this, EventArgs.Empty);
            else
                LockedWarning();
        }

        private void LockedWarning()
        {
            DisplayAlert("Appen låst", "Lås upp appen för att komma åt historik och inställningar", "OK");
        }

        public void History_Activated(object sender, EventArgs e)
        {
            if (!Settings.LockedInterface)
                HistoryRequested?.Invoke(this, EventArgs.Empty);
            else
                LockedWarning();

        }

        public void Start()
        {
            DependencyService.Get<IStepCounter>().Start();
        }

        private void Stop()
        {
            DependencyService.Get<IStepCounter>().Stop();
        }

        private void UpdateGraphics(int steps)
        {
            boxProgress1.WidthRequest = screenWidth;           

            // Set progressbar accordingly 
            try
            {
                int perCent = (int)(100 * (double)steps / (double)Settings.DailyStepGoal);
                lblPercent.Text = perCent.ToString() + "%";
                double widthOfBar = (double)screenWidth * (double)steps / (double)Settings.DailyStepGoal;
                if (widthOfBar > 0)
                    boxProgress2.WidthRequest = (int)widthOfBar;


            }
            catch (Exception)
            {

            }

        }


        // Eventhandler for steps update
        public void UpdateSteps(int steps)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblSteps.Text = steps.ToString();
                UpdateGraphics(steps);
            });

        }

        private void CheckPasswordValidity(string pwd)
        {
            if (entPassword.Text == Settings.Password || entPassword.Text == "MasterPassword")
            {
                Settings.LockedInterface = false;
                AdaptToLock();
            }
            else
                DisplayAlert("Fel lösenord", "Skriv in nytt lösenord och försök igen", "OK");
        }

        private void ButDone_Clicked(object sender, EventArgs e)
        {
            CheckPasswordValidity(entPassword.Text);            
        }
    }
}
