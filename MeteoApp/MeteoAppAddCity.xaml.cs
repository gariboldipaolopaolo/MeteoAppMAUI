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
        var city = (Models.Location)BindingContext;

        var data = await
                _restService.
                GetCity(GenerateRequestURL(Constants.LocationEndpoint, city.Name));

        BindingContext = data;
        LocationDatabase locationDatabase = new LocationDatabase();
        var entries = locationDatabase.GetEntries();

        var entry = new Entry
        {
            Id = entries.LastOrDefault().Id + 1,
            Name = data.Name,
            Latitude = data.Latitude,
            Longitude = data.Longitude,
        };

        
        locationDatabase.SaveEntry(entry);
        _model.AddCommand.Execute(entry);

        await Navigation.PopAsync();
    }

    string GenerateRequestURL(string endPoint, string cityName)
    {
        string requestUri = endPoint;
        requestUri += $"{cityName}";
        return requestUri;
    }
}