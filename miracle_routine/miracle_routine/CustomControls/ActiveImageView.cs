using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.CustomControls
{
    public class ActiveImageView : Image
    {
        public static readonly BindableProperty IsActiveProperty =
               BindableProperty.Create(nameof(IsActive),
                   typeof(bool),
                   typeof(ActiveImageView),
                   false,
                   BindingMode.TwoWay,
                   propertyChanged: IsActivePropertyChanged);

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public static readonly BindableProperty ImageSourceActiveProperty =
            BindableProperty.Create(nameof(ImageSourceActive),
                typeof(string),
                typeof(ActiveImageView),
                "");

        public string ImageSourceActive
        {
            get { return (string)GetValue(ImageSourceActiveProperty); }
            set { SetValue(ImageSourceActiveProperty, value); }
        }

        public static readonly BindableProperty ImageSourceInactiveProperty =
            BindableProperty.Create(nameof(ImageSourceInactive),
                typeof(string),
                typeof(ActiveImageView),
                "");

        public string ImageSourceInactive
        {
            get { return (string)GetValue(ImageSourceInactiveProperty); }
            set { SetValue(ImageSourceInactiveProperty, value); }
        }

        static void IsActivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Property changed implementation goes here
            var image = (ActiveImageView)bindable;

            if (image.IsActive)
            {
                image.Source = image.ImageSourceActive;
            }
            else
            {
                image.Source = image.ImageSourceInactive;
            }
        }
    }
}
