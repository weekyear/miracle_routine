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

            SoundSettingSwitch.IsToggled = Preferences.Get("IsSound", false);
            VibrationSettingSwitch.IsToggled = Preferences.Get("IsVibrate", false);
        }

        private void SoundSettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("IsSound", e.Value);
        }

        private void VibrationSettingSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("IsVibrate", e.Value);
        }
    }
}