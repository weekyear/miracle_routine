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
        
        public static List<HabitImages> HabitImageList
        {
            get
            {
                return new List<HabitImages>
                {
                    new HabitImages { "ic_miracle_routine_mini.png", "ic_water.png", "ic_tea.png", "ic_bed.png", "ic_stretching.png", "ic_meditation.png", "ic_todo.png"  },
                    new HabitImages { "ic_diary.png", "ic_reading.png", "ic_exercise.png", "ic_jogging.png", "ic_apple.png", "ic_pills.png", "ic_meal.png"  },
                    new HabitImages { "ic_baby.png", "ic_barber.png", "ic_basket.png", "ic_bath.png", "ic_check.png", "ic_drive.png", "ic_dryer.png" },
                    new HabitImages { "ic_game.png", "ic_libstick.png", "ic_sun_2.png", "ic_moon.png", "ic_shoping.png", "ic_trash.png", "ic_waste.png" },
                    new HabitImages { "ic_shower.png", "ic_plant.png", "ic_phone.png", "ic_notebook.png", "ic_music.png", "ic_bus.png", "ic_train.png" },
                    new HabitImages { "ic_bank.png", "ic_church.png", "ic_library.png", "ic_facebook.png", "ic_instagram.png", "ic_twitter.png", "ic_pets.png" },
                    new HabitImages { "ic_briefcase.png", "ic_contacts.png", "ic_idea.png", "ic_lock.png", "ic_camera.png", "ic_boxing.png", "ic_swimming.png" },
                    new HabitImages { "ic_weight.png", "ic_soccer.png", "ic_dancing.png", "ic_cycling.png", "ic_hospital.png", "ic_cake.png", "ic_brocolli.png" }
                };
            }
        }
    }
}
