using miracle_routine.Resources;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.CustomControls
{
    public class DoneButton : Button
    {
        public static readonly BindableProperty IsNotDoneProperty =
            BindableProperty.Create(nameof(IsNotDone),
                typeof(bool),
                typeof(DoneButton),
                false,
                BindingMode.TwoWay);

        public bool IsNotDone
        {
            get
            {
                return (bool)GetValue(IsNotDoneProperty);
            }
            set
            {
                SetValue(IsNotDoneProperty, value);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (IsNotDone)
            {
                Text = StringResources.Next;
                TextColor = (Color)App.Current.Resources["PrimaryDark"];
            }
            else
            {
                Text = StringResources.Complete;
                TextColor = (Color)App.Current.Resources["Accent"];
            }
        }
    }
}
