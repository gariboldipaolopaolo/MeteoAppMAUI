namespace MeteoApp;

[QueryProperty(nameof(Entry), "Entry")]
public partial class MeteoItemPage : ContentPage
{
    RestService _restService;
    Entry entry;
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

    async void OnGetWeatherButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace("lat=46.5386&lon=10.1357"))
        {
            WeatherData weatherData = await
                _restService.
                GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint));

            BindingContext = weatherData;
        }
    }
    string GenerateRequestURL(string endPoint)
    {
        string requestUri = endPoint;
        requestUri += $"?q={"lat=46.5386&lon=10.1357"}";
        requestUri += "&units=metric";
        requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
        return requestUri;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}