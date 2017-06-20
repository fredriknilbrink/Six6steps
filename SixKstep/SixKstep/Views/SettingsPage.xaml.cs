using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SixKstep
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public event EventHandler SettingsAltered;

        private string strLockapp = "[LÅS APPEN]";
        private string strUnlockapp = "APPEN LÅST -- LÅS UPP";
        public bool lockInterface;

        public SettingsPage()
        {
            InitializeComponent();
            entGoal.Text = Settings.DailyStepGoal.ToString();
            entHistoryLength.Text = Settings.HistoryLength.ToString();
            entPassword.Text = Settings.Password;
            butInvisible.Text = Settings.LockedInterface ? strUnlockapp : strLockapp;
            butInvisible.Clicked += ButInvisible_Clicked;
            butDone.Clicked += ButDone_Clicked;
            SetButtonInterface(Settings.LockedInterface);
        }

        public void SetButtonInterface(bool isLocked)
        {
            butInvisible.TextColor = Color.Black;
            butInvisible.BackgroundColor = isLocked ? Color.Red : Color.Green;
            butInvisible.Text = isLocked ? strUnlockapp : strLockapp;
        }

        private void ButInvisible_Clicked(object sender, EventArgs e)
        {
            lockInterface = lockInterface ? false : true;
            SetButtonInterface(lockInterface);
        }

        // Check validity of input and save values
        private void ButDone_Clicked(object sender, EventArgs e)
        {
            bool validInput = true;
            int goal = 0;
            int history = 0;

            try
            {
                goal = int.Parse(entGoal.Text);
                history = int.Parse(entHistoryLength.Text);
            }
            catch (Exception)
            {
                validInput = false;
            }

            if (!validInput || goal < 1 || history < 1)
            {
                DisplayAlert("Inmatning ogiltig", "Justera inmatningen och försök igen", "OK");
                return;
            }

            if (history > 7)
            {
                DisplayAlert("Inmatning ogiltig", "Historiken är max 7 dagar", "OK");
                return;
            }


            if (entPassword.Text == string.Empty && lockInterface)
            {
                DisplayAlert("Ogiltigt lösenord", "Lösenordet får inte vara blankt om du ska låsa appen", "OK");
                return;
            }

            Settings.HistoryLength = history;
            Settings.LockedInterface = lockInterface;
            Settings.Password = entPassword.Text;
            Settings.DailyStepGoal = goal;

            SettingsAltered?.Invoke(this, EventArgs.Empty);
        }
    }
}
