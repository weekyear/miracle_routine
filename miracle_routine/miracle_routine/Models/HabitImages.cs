using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Models
{
    public class HabitImages : List<string>
    {
        public List<string> ImageList
        {
            get { return this; }
        }

        public string Image_0 => ImageList[0];
        public string Image_1 => ImageList[1];
        public string Image_2 => ImageList[2];
        public string Image_3 => ImageList[3];
        public string Image_4 => ImageList[4];
        public string Image_5 => ImageList[5];
        public string Image_6 => ImageList[6];
    }
}
