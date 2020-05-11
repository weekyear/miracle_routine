using miracle_routine.Helpers;
using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class RecommendedHabitSettingViewModel : BaseViewModel
    {
        public RecommendedHabitSettingViewModel(INavigation navigation, RecommendedHabit habit) : base(navigation)
        {
            Habit = new RecommendedHabit(habit);

            ConstructCommand();
        }

        private void ConstructCommand()
        {
            DeleteCommand = new Command(async () => await Delete());
            SaveCommand = new Command(async () => await Save());
            CloseCommand = new Command(async () => await ClosePopup());
        }

        #region PROPERTY
        public RecommendedHabit Habit { get; set; }

        public string Image
        {
            get { return Habit.Image; }
            set
            {
                if (Habit.Image == value) return;
                Habit.Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public string Name
        {
            get { return Habit.Name; }
            set
            {
                if (Habit.Name == value) return;
                Habit.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Minutes
        {
            get { return Habit.Minutes; }
            set
            {
                if (Habit.Minutes == value) return;
                Habit.Minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public int Seconds
        {
            get
            {
                if (Habit.Seconds >= 10)
                {
                    return Habit.Seconds / 10;
                }
                return Habit.Seconds;
            }
            set
            {
                if (Habit.Seconds == value) return;
                Habit.Seconds = value;
                OnPropertyChanged(nameof(Seconds));
            }
        }

        public string Description
        {
            get
            {
                return Habit.Description;
            }
            set
            {
                if (Habit.Description == value) return;
                Habit.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public List<HabitImages> HabitImageList
        {
            get
            {
                return HabitImages.HabitImageList;
            }
        }

        public Command DeleteCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command CloseCommand { get; set; }

        #endregion

        #region METHOD
        private async Task Save()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                if (Seconds < 10)
                {
                    Seconds = Seconds * 10;
                }
                if (Habit.Id == 0) Habit.Index = App.RecommendedHabitRepo.RecommendedHabitsFromDB.Count;
                App.RecommendedHabitRepo.SaveRecommendedHabit(Habit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                await ClosePopup();
                IsBusy = false;
            }
        }

        private async Task Delete()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                DependencyService.Get<MessageBoxService>().ShowConfirm(
                    $"추천 습관 삭제",
                    $"추천 습관을 삭제하시겠습니까?", null,
                    async () =>
                    {
                        if (Habit.Id != 0) App.RecommendedHabitRepo.DeleteRecommendedHabit(Habit.Id);

                        await ClosePopup();
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync(true);
        }
        #endregion
    }
}
