using MeteoApp.Database;

namespace MeteoApp;

public partial class MeteoListPage : Shell
{
    private LocationDatabase database;
    private List<Entry> entries;
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

    public MeteoListPage()
	{
		InitializeComponent();
        RegisterRoutes();
<<<<<<< HEAD
        GetLocation(); 
=======
>>>>>>> 2aa89f0 (Fix list add and delete actions)
        database = new LocationDatabase();
    }

    protected override async void OnAppearing()
    {
        ActivityIndicator activityIndicator = new ActivityIndicator
        {
            IsRunning = true,
            Color = Colors.Orange
        };

        base.OnAppearing();
        var myPosition = await GetLocation();

         entries = GetEntries();

        BindingContext = new MeteoListViewModel();

        var vm = BindingContext as MeteoListViewModel;

        foreach ( var entry in entries )
        {
            vm.AddCommand.Execute( entry );
        }

        if (myPosition != null)
        {
            vm.AddCommand.Execute(myPosition);
        }

        activityIndicator.IsRunning = false;
    }

    private List<Entry> GetEntries()
    {
        return  database.GetEntries();
    }

    private async Task<Entry> GetLocation()
    {
        Entry location = null;
        try
        {
            var loc = await Geolocation.Default.GetLocationAsync();
            if (loc != null)
            {
                Console.WriteLine($"{loc.Latitude} {loc.Longitude}{loc.Altitude}");

                location.Name = "My position";
                location.Longitude = loc.Longitude;
                location.Latitude = loc.Latitude;

                return location;
            }

        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return location;
    }

    private void RegisterRoutes()
    {
        Routes.Add("entrydetails", typeof(MeteoItemPage));

        foreach (var item in Routes)
        {
            Console.WriteLine($"MeteoListPage: {item}");
            Routing.RegisterRoute(item.Key, item.Value);
        }
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
         Navigation.PushAsync(new MeteoAppAddCity(BindingContext as MeteoListViewModel));
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        var entry = button.BindingContext as Entry;

        var vm = BindingContext as MeteoListViewModel;

        vm.RemoveCommand.Execute(entry);
    }
}