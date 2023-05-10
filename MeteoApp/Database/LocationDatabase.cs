using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApp.Database
{
    public static class Constants
    {
        public const string DatabaseFilename = "location.db";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }

    public class LocationDatabase
    {
        private readonly SQLiteConnection _database;

        public LocationDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "location.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Entry>();
        }

        public List<Entry> GetEntries()
        {
            return _database.Table<Entry>().ToList();
        }

        public int SaveEntry(Entry entry)
        {
            return _database.Insert(entry);
        }

        public int DeleteEntry(Entry entry)
        {
            return _database.Delete(entry);
        }
    }
}
