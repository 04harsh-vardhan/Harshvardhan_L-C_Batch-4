using System.Net.Http.Json;

await GeoLocation.GetUserResponse();
Console.ReadKey();
public static class GeoLocation
{
    public static async Task GetUserResponse()
    {
        Console.WriteLine("Enter Your City Name To Get Co-ordinates");
        string cityName = Console.ReadLine();
        OpenWeatherGeocoding openWeatherGeocoding = new OpenWeatherGeocoding();
        Coordinates coordinates = await openWeatherGeocoding.GetCoordinates(cityName);
        Console.WriteLine("Latitude " + coordinates.Latitude);
        Console.WriteLine("Longitude " + coordinates.Longitude);
    }
}

public class OpenWeatherGeocoding
{
    private const int _limit = 1;
    private const string API_KEY = "";
    private string GetUrl(string cityName)
    {
        return $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit={_limit}&appid={API_KEY}";
    }
    public async Task<Coordinates> GetCoordinates(string cityName)
    {
        Coordinates coordinates = new();
        try
        {
            HttpClient client = new HttpClient();
            List<Location> location = await client.GetFromJsonAsync<List<Location>>(GetUrl(cityName));
            coordinates.Latitude = location[0].Lat;
            coordinates.Longitude = location[0].Lon;
            return coordinates;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return coordinates;
        }
    }
    private class Location
    {
        public string Name { get; set; }

        public Dictionary<string, string> LocalNames { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public string Country { get; set; }

        public string State { get; set; }
    }

}

public class Coordinates
{
    public double? Longitude { get; set; } = 0;
    public double? Latitude { get; set; } = 0;
}