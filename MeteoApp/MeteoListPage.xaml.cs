using Android.Util;
using CommunityToolkit.Maui.Alerts;
using MeteoApp.Database;
using Plugin.LocalNotification;

namespace MeteoApp;

public partial class MeteoListPage : Shell
{
    private LocationDatabase database;
    private List<Entry> entries;
    private static Location tempLocation;
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

    public MeteoListPage()
	{
		InitializeComponent();
        RegisterRoutes();
        database = new LocationDatabase();
        Thread thread = new Thread(TrackUserLocation); 
        thread.Start();
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

    static async void TrackUserLocation() { 
        while(true)
        {
            var loc = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (tempLocation != null && tempLocation.Latitude.ToString("N2") != loc.Latitude.ToString("N2") && tempLocation.Longitude.ToString("N2") != loc.Longitude.ToString("N2"))
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1000,
                    Subtitle = "Current location info",
                    Title = $"Your location as changed. The new coordinates are: Latitude {loc.Latitude}, Longitude {loc.Longitude}"
                };

                await LocalNotificationCenter.Current.Show(request);
            }
            tempLocation = loc;
            Thread.Sleep(120000);
        }
    }

    private List<Entry> GetEntries()
    {
        return  database.GetEntries();
    }

    private async Task<Entry> GetLocation()
    {
        Entry location = new Entry();
        try
        {
            var loc = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (loc != null)
            {
                location.Name = "My position";
                location.Longitude = loc.Longitude;
                location.Latitude = loc.Latitude;

                return location;
            }

        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return null;
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

        zlistView.SelectedItem = null;
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

        var message = "City removed!";
        var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
        toast.Show();
    }
}