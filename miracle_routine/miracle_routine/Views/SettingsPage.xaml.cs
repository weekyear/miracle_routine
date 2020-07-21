using miracle_routine.Helpers;
using miracle_routine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace miracle_routine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            DarkModeSettingSwitch.IsToggled = Preferences.Get("IsDarkTheme", false);
            SoundSettingSwitch.IsToggled = Preferences.Get("IsSound", false);
            VibrationSettingSwitch.IsToggled = Preferences.Get("IsVibrate", false);
            AutoFlipHabitSettingSwitch.IsToggled = Preferences.Get("IsAutoFlipHabit", false);
        }

        private void SoundSettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("IsSound", e.Value);
        }

        private void VibrationSettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("IsVibrate", e.Value);
        }
        
        private void AutoFlipHabitSettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("IsAutoFlipHabit", e.Value);
        }

        private void DarkModeSettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("IsDarkTheme", e.Value);

            if (e.Value)
            {
                ResourcesHelper.SetDarkMode();
            }
            else
            {
                ResourcesHelper.SetLightMode();
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert(StringResources.IconCopyright, "https://icons8.com", StringResources.OK);
        }
    }
}