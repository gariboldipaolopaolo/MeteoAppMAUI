using MeteoApp.Database;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeteoApp
{
    public class MeteoListViewModel : BaseViewModel
    {

        LocationDatabase database;
        ObservableCollection<Entry> _entries;

        public ObservableCollection<Entry> Entries
        {
            get { return _entries; }
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }

        public Command<Entry> RemoveCommand
        {
            get
            {
                return new Command<Entry>((entry) =>
                {
                    Entries.Remove(entry);
                    var remoteEntries = database.GetEntries().Where(q => q.Name.ToLower() == entry.Name.ToLower());

                    foreach(var remoteEntry in remoteEntries)
                        database.DeleteEntry(remoteEntry);
                });
            }
        }

        public Command<Entry> AddCommand
        {
            get
            {
                return new Command<Entry>((entry) =>
                {
                    Entries.Insert(0,entry);
                    //database.SaveEntry(entry);
                });
            }
        }

        public MeteoListViewModel()
        {
            Entries = new ObservableCollection<Entry>();
            database = new LocationDatabase();
        }
    }
}
