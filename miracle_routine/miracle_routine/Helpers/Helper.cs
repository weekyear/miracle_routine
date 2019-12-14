using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Internals;

namespace miracle_routine.Helpers
{
    public class Helper
    {
        public static ObservableCollection<T> ConvertIEnuemrableToObservableCollection<T>(IEnumerable<T> list)
        {
            var collection = new ObservableCollection<T>();

            list.ForEach((item) => collection.Add(item));

            return collection;
        }
    }
}
