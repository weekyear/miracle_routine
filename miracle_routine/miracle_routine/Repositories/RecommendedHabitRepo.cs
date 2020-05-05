using miracle_routine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace miracle_routine.Repositories
{
    public class RecommendedHabitRepo : IRecommendedHabitRepo
    {
        public ItemDatabaseGeneric ItemDatabase { get; }
        public RecommendedHabitRepo(ItemDatabaseGeneric itemDatabase)
        {
            //if (Device.RuntimePlatform == "Test") return;
            ItemDatabase = itemDatabase;
        }

        public List<RecommendedHabit> RecommendedHabitsFromDB
        {
            get 
            {
                var _recommendedHabits = GetRecommendedHabits() as List<RecommendedHabit>;
                if (_recommendedHabits.Count == 0)
                {
                    foreach (var recommended in RecommendedHabitList)
                    {
                        SaveRecommendedHabit(recommended);
                    }
                }
                return GetRecommendedHabits() as List<RecommendedHabit>; 
            }
        }

        public RecommendedHabit GetRecommendedHabit(int id)
        {
            return ItemDatabase.GetObject<RecommendedHabit>(id);
        }

        public IEnumerable<RecommendedHabit> GetRecommendedHabits()
        {
            return ItemDatabase.GetObjects<RecommendedHabit>();
        }

        public int SaveRecommendedHabit(RecommendedHabit recommendedHabit)
        {
            return ItemDatabase.SaveObject(recommendedHabit);
        }

        public int DeleteRecommendedHabit(int id)
        {
            return ItemDatabase.DeleteObject<RecommendedHabit>(id);
        }

        public void DeleteAllRecommendedHabits()
        {
            ItemDatabase.DeleteAllObjects<RecommendedHabit>();
        }

        public List<RecommendedHabit> RecommendedHabitList
        {
            get
            {
                return new List<RecommendedHabit>
                {
                    new RecommendedHabit(){ Name = "물 마시기", Description="자는 동안 몸에 쌓인 독소를 제거하고 신진대사 향상, 소화불량 완화에 도움됩니다.", Image = "ic_water.png", Minutes = 1, Seconds = 0 },
                    new RecommendedHabit(){ Name = "차 마시기", Description="당뇨병이나 심혈관질환, 알츠하이머병 예방에도 도움이 됩니다.", Image = "ic_tea.png", Minutes = 3, Seconds = 0 },
                    new RecommendedHabit(){ Name = "이불 개기", Description="이불 정리를 하는지 여부와 자아 성취는 상관관계가 매우 높다고 합니다.", Image = "ic_bed.png", Minutes = 1, Seconds = 0 },
                    new RecommendedHabit(){ Name = "기지개 펴기", Description="혈액 순환을 도와주고 체형 균형에 도움이 됩니다.", Image = "ic_stretching.png", Minutes = 1, Seconds = 0 },
                    new RecommendedHabit(){ Name = "명상", Description="침묵의 시간 동안 마음을 조용히 가라앉히고 내 문제들에 대한 걱정을 멈춥니다.", Image = "ic_meditation.png", Minutes = 10, Seconds = 0 },
                    new RecommendedHabit(){ Name = "일정 세우기", Description="일정을 미리 세운다면 훨씬 탁월한 성과를 이뤄내실거에요.", Image = "ic_todo.png", Minutes = 10, Seconds = 0 },
                    new RecommendedHabit(){ Name = "일기 쓰기", Description="직접 적어보면서 생각을 명확히 정리하고 새로운 영감을 얻게 해줍니다.", Image = "ic_diary.png", Minutes = 10, Seconds = 0 },
                    new RecommendedHabit(){ Name = "아침 독서", Description="독서는 삶을 변화시키는 가장 빠른 길입니다.", Image = "ic_reading.png", Minutes = 30, Seconds = 0 },
                    new RecommendedHabit(){ Name = "무산소 운동", Description="기초대사량을 증가시켜 살이 찌지 않는 체질로 만들어 줍니다.", Image = "ic_exercise.png", Minutes = 2, Seconds = 0 },
                    new RecommendedHabit(){ Name = "조깅하기", Description="신진대사를 크게 증진시키고 노화를 늦춰줍니다. 또 심장 질환도 예방해줍니다.", Image = "ic_jogging.png", Minutes = 40, Seconds = 0 },
                    new RecommendedHabit(){ Name = "과일 챙겨먹기", Description="아침에 사과, 토마토, 블루베리, 감자를 먹는 것이 좋다고 하네요.", Image = "ic_apple.png", Minutes = 5, Seconds = 0 },
                    new RecommendedHabit(){ Name = "약 복용", Description="복용하시는 약이 있다면 잊지 말고 제때 챙겨먹어야 해요!", Image = "ic_pills.png", Minutes = 1, Seconds = 0 },
                    new RecommendedHabit(){ Name = "식사", Description="너무 많이 드시면 이후 일정에 무리를 줍니다. 과식은 삼가주세요! ", Image = "ic_meal.png", Minutes = 30, Seconds = 0 },
                    new RecommendedHabit(){ Name = "샤워", Description="아침 샤워는 몸의 긴장을 완화시키고 스트레스를 줄여주고 창의력을 키워줍니다.", Image = "ic_shower.png", Minutes = 30, Seconds = 0 }
                };
            }
        }
    }
}
