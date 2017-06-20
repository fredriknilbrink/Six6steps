using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SixKstep.Helpers;

namespace SixKstep
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            DependencyService.Get<IStepCounter>().HistoryUpdated += HistoryPage_historyUpdated;
        }

        public void UpdateHistoryLength()
        {
            DependencyService.Get<IStepCounter>().GetHistory(Settings.HistoryLength);
        }

        private void HistoryPage_historyUpdated(object sender, EventArgs e)
        {
            var scrollView = new ScrollView();
            var stackLayout = new StackLayout { BackgroundColor = Color.AliceBlue };
            List<PedometerDataStruct> history = ((DataEventArgs)e).history;
            int numberofelements = Settings.HistoryLength;
            if (numberofelements > history.Count)
                numberofelements = history.Count;


            var boxWidth = 100;
            var boxHeight = 10;


            Grid topGrid = new Grid
            {

                Margin = 20,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = 20 },

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = new GridLength(boxWidth, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }

                }
            };
            topGrid.Children.Add(new Label { Text = "Datum", Margin = 0, FontSize = 25 }, 0, 0);
            topGrid.Children.Add(new Label { Text = "Mål", Margin = 0, FontSize = 25 }, 1, 0);
            topGrid.Children.Add(new Label { Text = "Antal", Margin = 0, FontSize = 25, HorizontalOptions = LayoutOptions.End }, 2, 0);



#if __IOS__
            topGrid.Children.Add(new Label { Text = DateTime.Now.ToShortDateString(), Margin = 0, FontSize = 20, TextColor = Color.Transparent }, 0, 0);
#endif

#if __ANDROID__
            topGrid.Children.Add(new Label { Text = DateTime.Now.ToShortDateString(), Margin = 0, FontSize = 20, TextColor = Color.Transparent }, 0, 0);
#endif


#if __MOBILE__
            topGrid.Children.Add(new Label { Text = DateTime.Now.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern), Margin = 0, FontSize = 20, TextColor = Color.Transparent }, 0, 0);
#endif


            stackLayout.Children.Add(topGrid);

            for (int i = 0; i < numberofelements; i++)
            {
                // new grid with 3 columns
                Grid mainGrid = new Grid
                {
                    Margin = 20,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 20 },
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = new GridLength(boxWidth, GridUnitType.Absolute) },

                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }

                    }
                };


                var temp_width = boxWidth * history[i].steps / Settings.DailyStepGoal;
                if (temp_width > boxWidth) temp_width = boxWidth;

                // column 0 - date

#if __IOS__
                mainGrid.Children.Add(new Label { Text = history[i].day.ToShortDateString(), Margin = 0, FontSize = 20 }, 0, 0);
#endif

#if __ANDROID__
                mainGrid.Children.Add(new Label { Text = history[i].day.ToShortDateString(), Margin = 0, FontSize = 20 }, 0, 0);
#endif


#if __MOBILE__
                history[i].day.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
#endif



                // column 1 - progress
                //StackLayout sl = new StackLayout {  };

                Grid subGrid = new Grid { BackgroundColor = Color.Black, Padding = 0.5, HeightRequest = 10 };


                BoxView b1 = new BoxView { Color = Color.White, HeightRequest = boxHeight, WidthRequest = boxWidth, HorizontalOptions = LayoutOptions.StartAndExpand };
                BoxView b2 = new BoxView { Color = Color.Green, HeightRequest = boxHeight, WidthRequest = temp_width, HorizontalOptions = LayoutOptions.StartAndExpand };
                Label l1 = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12, TextColor = Color.Black, Text = ((int)(100 * (double)history[i].steps / (double)Settings.DailyStepGoal)).ToString() + "%" };

                subGrid.Children.Add(b1);
                subGrid.Children.Add(b2);
                subGrid.Children.Add(l1);

                mainGrid.Children.Add(subGrid, 1, 0);
                // column 2 - image

                // column 3 - steps
                mainGrid.Children.Add(new Label { Text = history[i].steps.ToString(), Margin = 0, FontSize = 20, HorizontalOptions = LayoutOptions.End }, 2, 0);
                // Accomodate iPhone status bar.               
                stackLayout.Children.Add(mainGrid);

            }
            Content = new ScrollView { Content = stackLayout };
        }
    }
}
