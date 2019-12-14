using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace miracle_routine.Helpers
{
    public class Database : IDatabase
    {
        private string fileName = "MiracleRoutineSQLite.db3";
        public SQLiteConnection DBConnect()
        {
            //if (Device.RuntimePlatform != "Test")
            //{
            //    string path = DependencyService.Get<IDeviceStorageService>().GetFilePath("AlarmsSQLite.db3");
            //    return CreateConnection(path);
            //}

            //TODO::지금은 안드로이드 전용 Path만 설정해놨음. 나중에 방안 찾아야 함 꼭

            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
                return CreateConnection(path);
            }
            catch
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library", fileName);
                return CreateConnection(path);
            }
        }

        private SQLiteConnection CreateConnection(string path)
        {
            return new SQLiteConnection(path);
        }
    }
}
