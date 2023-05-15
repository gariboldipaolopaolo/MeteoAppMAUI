using System.Globalization;

namespace MeteoApp;

[QueryProperty(nameof(Entry), "Entry")]
public partial class MeteoItemPage : ContentPage
{
    readonly RestService _restService;
    Entry entry;
    WeatherData weatherData = new();
    public Entry Entry
    {
        get => entry;
        set
        {
            entry = value;
            OnPropertyChanged();
        }
    }

    public MeteoItemPage()
    {
        InitializeComponent();
        _restService = new RestService();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetWeather();
    }

    async void GetWeather(){
            weatherData = await
                _restService.
                GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint));

        BindingContext = weatherData;
    }
    
    string GenerateRequestURL(string endPoint)
    {
        string culture = CultureInfo.CurrentUICulture.ToString();
        int dashIndex = culture.IndexOf('-');
        string requestUri = endPoint;
        requestUri += $"?{$"lat={Entry.Latitude}&lon={Entry.Longitude}"}";
        requestUri += "&units=metric";
        requestUri += $"&lang={culture[..dashIndex]}";
        requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
        return requestUri;
    }
}