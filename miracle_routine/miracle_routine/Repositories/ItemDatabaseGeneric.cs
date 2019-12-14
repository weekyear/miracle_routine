using miracle_routine.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace miracle_routine.Repositories
{
    public class ItemDatabaseGeneric
    {
        static readonly object locker = new object();
        readonly SQLiteConnection connection;

        public ItemDatabaseGeneric(SQLiteConnection connection)
        {
            this.connection = connection;
            connection.CreateTable<Routine>();
            connection.CreateTable<Habit>();
        }

        public IEnumerable<T> GetObjects<T>() where T : IObject, new()
        {
            lock (locker)
            {
                return (from i in connection.Table<T>() select i).ToList();
            }
        }
        public IEnumerable<T> GetFirstObjects<T>() where T : IObject, new()
        {
            lock (locker)
            {
                return connection.Query<T>($"SELECT * FROM {nameof(T)} WHERE Name = 'First'");
            }
        }

        public T GetObject<T>(int id) where T : IObject, new()
        {
            lock (locker)
            {
                return connection.Table<T>().Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public int SaveObject<T>(T obj) where T : IObject
        {
            lock (locker)
            {
                if (obj.Id != 0)
                {
                    connection.Update(obj);
                }
                else
                {
                    connection.Insert(obj);
                }
                return obj.Id;
            }
        }
        public int DeleteObject<T>(int id) where T : IObject, new()
        {
            lock (locker)
            {
                return connection.Delete<T>(id);
            }
        }
        public void DeleteAllObjects<T>()
        {
            lock (locker)
            {
                connection.DropTable<T>();
                connection.CreateTable<T>();
            }
        }
    }
}
