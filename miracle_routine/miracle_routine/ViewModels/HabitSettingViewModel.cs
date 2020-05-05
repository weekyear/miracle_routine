using miracle_routine.Helpers;
using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace miracle_routine.ViewModels
{
    public class HabitSettingViewModel : BaseViewModel
    {
        public HabitSettingViewModel(INavigation navigation, Habit habit) : base(navigation)
        {
            Habit = new Habit(habit);

            ConstructCommand();
        }

        private void ConstructCommand()
        {
            DeleteCommand = new Command(async () => await Delete());
            SaveCommand = new Command(async () => await Save());
            CloseCommand = new Command(async () => await ClosePopup());
        }

        #region PROPERTY
        public Habit Habit { get; set; }

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
                return new List<HabitImages>
                {
                    new HabitImages { "ic_water.png", "ic_tea.png", "ic_bed.png", "ic_stretching.png", "ic_meditation.png", "ic_todo.png", "ic_diary.png" },
                    new HabitImages { "ic_reading.png", "ic_exercise.png", "ic_jogging.png", "ic_apple.png", "ic_pills.png", "ic_meal.png", "ic_shower.png" },
                    new HabitImages { "ic_baby.png", "ic_barber.png", "ic_basket.png", "ic_bath.png", "ic_check.png", "ic_drive.png", "ic_dryer.png" },
                    new HabitImages { "ic_game.png", "ic_libstick.png", "ic_sun_2.png", "ic_moon.png", "ic_shoping.png", "ic_trash.png", "ic_waste.png" },
                    new HabitImages { "ic_pets.png", "ic_plant.png", "ic_phone.png", "ic_notebook.png", "ic_music.png", "ic_bus.png", "ic_train.png" },
                    new HabitImages { "ic_bank.png", "ic_church.png", "ic_library.png", "ic_facebook.png", "ic_instagram.png", "ic_twitter.png", "" }
                };
            }
        }

        public List<Habit> RecommendedHabitList
        {
            get
            {
                return new List<Habit>
                {
                    new Habit(){ Name = "물 마시기", Description="자는 동안 몸에 쌓인 독소를 제거하고 신진대사 향상, 소화불량 완화에 도움됩니다.", Image = "ic_water.png", Minutes = 1, Seconds = 0 },
                    new Habit(){ Name = "차 마시기", Description="당뇨병이나 심혈관질환, 알츠하이머병 예방에도 도움이 됩니다.", Image = "ic_tea.png", Minutes = 3, Seconds = 0 },
                    new Habit(){ Name = "이불 개기", Description="이불 정리를 하는지 여부와 자아 성취는 상관관계가 매우 높다고 합니다.", Image = "ic_bed.png", Minutes = 1, Seconds = 0 },
                    new Habit(){ Name = "기지개 펴기", Description="혈액 순환을 도와주고 체형 균형에 도움이 됩니다.", Image = "ic_stretching.png", Minutes = 1, Seconds = 0 },
                    new Habit(){ Name = "명상", Description="침묵의 시간 동안 마음을 조용히 가라앉히고 내 문제들에 대한 걱정을 멈춥니다.", Image = "ic_meditation.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "일정 세우기", Description="일정을 미리 세운다면 훨씬 탁월한 성과를 이뤄내실거에요.", Image = "ic_todo.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "일기 쓰기", Description="직접 적어보면서 생각을 명확히 정리하고 새로운 영감을 얻게 해줍니다.", Image = "ic_diary.png", Minutes = 10, Seconds = 0 },
                    new Habit(){ Name = "아침 독서", Description="독서는 삶을 변화시키는 가장 빠른 길입니다.", Image = "ic_reading.png", Minutes = 30, Seconds = 0 },
                    new Habit(){ Name = "무산소 운동", Description="기초대사량을 증가시켜 살이 찌지 않는 체질로 만들어 줍니다.", Image = "ic_exercise.png", Minutes = 2, Seconds = 0 },
                    new Habit(){ Name = "조깅하기", Description="신진대사를 크게 증진시키고 노화를 늦춰줍니다. 또 심장 질환도 예방해줍니다.", Image = "ic_jogging.png", Minutes = 40, Seconds = 0 },
                    new Habit(){ Name = "과일 챙겨먹기", Description="아침에 사과, 토마토, 블루베리, 감자를 먹는 것이 좋다고 하네요.", Image = "ic_apple.png", Minutes = 5, Seconds = 0 },
                    new Habit(){ Name = "약 복용", Description="복용하시는 약이 있다면 잊지 말고 제때 챙겨먹어야 해요!", Image = "ic_pills.png", Minutes = 1, Seconds = 0 },
                    new Habit(){ Name = "식사", Description="너무 많이 드시면 이후 일정에 무리를 줍니다. 과식은 삼가주세요! ", Image = "ic_meal.png", Minutes = 30, Seconds = 0 },
                    new Habit(){ Name = "샤워", Description="아침 샤워는 몸의 긴장을 완화시키고 스트레스를 줄여주고 창의력을 키워줍니다.", Image = "ic_shower.png", Minutes = 30, Seconds = 0 }
                };
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
                AddHabitList(Habit);
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
                    $"습관 삭제",
                    $"습관을 삭제하시겠습니까?", null,
                    async () =>
                    {
                        RemoveHabitList(Habit);

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

        private void AddHabitList(Habit habit)
        {
            var Habits = RoutineSettingViewModel.habits;
            if (habit.Index == -1)
            {
                habit.Index = Habits.Count;
                Habits.Add(habit);
            }
            else
            {
                var oldHabit = Habits.FirstOrDefault(h => h.Index == habit.Index);
                int i = Habits.IndexOf(oldHabit);
                Habits.Remove(oldHabit);
                Habits.Insert(i, habit);
            }
        }

        private void RemoveHabitList(Habit habit)
        {
            var Habits = RoutineSettingViewModel.habits;
            if (habit.Index == -1)
            {
                return;
            }
            else
            {
                var oldHabit = Habits.FirstOrDefault(h => h.Index == habit.Index);
                int i = Habits.IndexOf(oldHabit);
                Habits.Remove(oldHabit);

                RoutineSettingViewModel.HabitsForDelete.Add(oldHabit);
            }
        }
        #endregion
    }
}
