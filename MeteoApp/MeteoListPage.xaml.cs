using MeteoApp.Database;
using System.Collections.Generic;

namespace MeteoApp;

public partial class MeteoListPage : Shell
{
    private LocationDatabase database;
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

    public MeteoListPage()
	{
		InitializeComponent();
        RegisterRoutes();
        GetLocation();
        database = new LocationDatabase();

        var entries = GetEntries();
        BindingContext = new MeteoListViewModel(entries);

    }

    private List<Entry> GetEntries()
    {
        return database.GetEntries();
    }

    private async void GetLocation()
    {
        try
        {
            var loc = await Geolocation.Default.GetLocationAsync();
            if (loc != null)
            {
                Console.WriteLine($"{loc.Latitude} {loc.Longitude}{loc.Altitude}");
                database.SaveEntry(new Entry
                {
                    Name = "My position",
                    Longitude = loc.Longitude,
                    Latitude = loc.Latitude,
                });
            }

        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void RegisterRoutes()
    {
        Routes.Add("entrydetails", typeof(MeteoItemPage));

        foreach (var item in Routes)
            Routing.RegisterRoute(item.Key, item.Value);
    }

    private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            Entry entry = e.SelectedItem as Entry;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Entry", entry }
            };

            Shell.Current.GoToAsync($"entrydetails", navigationParameter);
        }
    }

    private void OnItemAdded(object sender, EventArgs e)
    {
         Navigation.PushAsync(new MeteoAddCity());
    }
}