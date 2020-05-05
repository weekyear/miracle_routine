using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace miracle_routine.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HabitImageGrid : ContentView
    {
        private static string image_string;
        public static event Action<string> OnHabitImageSeleted;
        
        public void HabitImageSeleted()
        {
            OnHabitImageSeleted?.Invoke(image_string);
        }

        public static readonly BindableProperty ImageListProperty =
               BindableProperty.Create(
                   "Days",
                   typeof(List<string>),
                   typeof(DaysOfWeekSelectionView),
                   new List<string>());

        public List<string> ImageList
        {
            get { return (List<string>)GetValue(ImageListProperty); }
            set { SetValue(ImageListProperty, value); }
        }

        public string Image_0 => ImageList[0];
        public string Image_1 => ImageList[1];
        public string Image_2 => ImageList[2];
        public string Image_3 => ImageList[3];
        public string Image_4 => ImageList[4];
        public string Image_5 => ImageList[5];
        public string Image_6 => ImageList[6];

        public HabitImageGrid()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            var imageSource = image.Source as FileImageSource;
            image_string = imageSource.File;
            HabitImageSeleted();
        }
    }
}