using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Internals;

namespace miracle_routine.Helpers
{
    public class Helper
    {
        public static OrderableCollection<T> ConvertIEnuemrableToObservableCollection<T>(IEnumerable<T> list)
        {
            var collection = new OrderableCollection<T>();

            list.ForEach((item) => collection.Add(item));

            return collection;
        }
    }
}
