using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace miracle_routine.Helpers
{
    public static class ResourcesHelper
    {
        public const string DynamicBackgroundColor = nameof(DynamicBackgroundColor);
        public const string DynamicSecondaryBackgroundColor = nameof(DynamicSecondaryBackgroundColor);
        public const string DynamicPrimaryColor = nameof(DynamicPrimaryColor);
        public const string DynamicPrimaryDarkColor = nameof(DynamicPrimaryDarkColor);
        public const string DynamicAccentColor = nameof(DynamicAccentColor);
        public const string DynamicIconColor = nameof(DynamicIconColor);


        public const string DynamicPrimaryTextColor = nameof(DynamicPrimaryTextColor);
        public const string DynamicMediumGrayTextColor = nameof(DynamicMediumGrayTextColor);

        public const string DynamicNavigationBarColor = nameof(DynamicNavigationBarColor);
        public const string DynamicBarTextColor = nameof(DynamicBarTextColor);

        public static void SetDynamicResource(string targetResourceName, string sourceResourceName)
        {
            if (!Application.Current.Resources.TryGetValue(sourceResourceName, out var value))
            {
                throw new InvalidOperationException($"key {sourceResourceName} not found in the resource dictionary");
            }

            Application.Current.Resources[targetResourceName] = value;
        }

        public static void SetDynamicResource<T>(string targetResourceName, T value)
        {
            Application.Current.Resources[targetResourceName] = value;
        }

        public static void SetLightMode()
        {
            SetDynamicResource(DynamicBackgroundColor, "LightThemeBackgroundColor");
            SetDynamicResource(DynamicSecondaryBackgroundColor, "LightThemeSecondaryBackgroundColor");
            SetDynamicResource(DynamicPrimaryColor, "LightThemePrimary");
            SetDynamicResource(DynamicPrimaryDarkColor, "LightThemePrimaryDark");
            SetDynamicResource(DynamicAccentColor, "LightThemeAccent");
            SetDynamicResource(DynamicIconColor, "LightThemeIcon");

            SetDynamicResource(DynamicPrimaryTextColor, "LightThemePrimaryTextColor");
            SetDynamicResource(DynamicMediumGrayTextColor, "LightThemeMediumGrayTextColor");

            SetDynamicResource(DynamicNavigationBarColor, "LightThemePrimary");
            SetDynamicResource(DynamicBarTextColor, "LightThemeBarTextColor");
        }

        public static void SetDarkMode()
        {
            SetDynamicResource(DynamicBackgroundColor, "DarkThemeBackgroundColor");
            SetDynamicResource(DynamicSecondaryBackgroundColor, "DarkThemeSecondaryBackgroundColor");
            SetDynamicResource(DynamicPrimaryColor, "DarkThemePrimary");
            SetDynamicResource(DynamicPrimaryDarkColor, "DarkThemePrimaryDark");
            SetDynamicResource(DynamicAccentColor, "DarkThemeAccent");
            SetDynamicResource(DynamicIconColor, "DarkThemeIcon");

            SetDynamicResource(DynamicPrimaryTextColor, "DarkThemePrimaryTextColor");
            SetDynamicResource(DynamicMediumGrayTextColor, "DarkThemeMediumGrayTextColor");

            SetDynamicResource(DynamicNavigationBarColor, "DarkThemePrimary");
            SetDynamicResource(DynamicBarTextColor, "DarkThemeBarTextColor");
        }
    }
}
