using CommunityToolkit.Maui.Alerts;
using MeteoApp.Database;

namespace MeteoApp;

public partial class MeteoAppAddCity : ContentPage
{
    RestService _restService;
    MeteoListViewModel _model;
    public MeteoAppAddCity(MeteoListViewModel vm)
	{
		InitializeComponent();
        _restService = new RestService();
        BindingContext = new Models.Location();
        _model = vm;
    }

    private async void OnCityAdd(object sender, EventArgs e)
    {
        string message = "";

        var city = (Models.Location)BindingContext;

        var data = await
                _restService.
                GetCity(GenerateRequestURL(Constants.LocationEndpoint, city.Name));

        if(data == null)
        {
            message = "City not found!";
            var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
            await toast.Show();

            return;
        }

        BindingContext = data;

        LocationDatabase locationDatabase = new LocationDatabase();

        var remoteEntries = locationDatabase.GetEntries();
        var matchedEntries = remoteEntries.Where(q => q.Name.ToLower() == data.Name.ToLower());

        if(matchedEntries.Any())
        {

            message = "City already inserted!";
            var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
            await toast.Show();

            return;
        }

        var entry = new Entry
        {
            Name = data.Name,
            Latitude = data.Latitude,
            Longitude = data.Longitude,
        };

        
        locationDatabase.SaveEntry(entry);
        _model.AddCommand.Execute(entry);

        await Navigation.PopAsync();

        message = "City inserted!";
        var toast1 = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
        await toast1.Show();
    }

    string GenerateRequestURL(string endPoint, string cityName)
    {
        string requestUri = endPoint;
        requestUri += $"{cityName}";
        return requestUri;
    }
}