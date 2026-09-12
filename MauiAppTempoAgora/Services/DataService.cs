using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chaveApi = "d7d88ba4b7d687136b070725d47cf447";

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(cidade)}&units=metric&appid={chaveApi}&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime sunrise = DateTimeOffset
                        .FromUnixTimeSeconds((long)rascunho["sys"]!["sunrise"]!)
                        .LocalDateTime;

                    DateTime sunset = DateTimeOffset
                        .FromUnixTimeSeconds((long)rascunho["sys"]!["sunset"]!)
                        .LocalDateTime;

                    t = new Tempo()
                    {
                        lat = (double?)rascunho["coord"]?["lat"],
                        lon = (double?)rascunho["coord"]?["lon"],
                        description = (string?)rascunho["weather"]?[0]?["description"],
                        main = (string?)rascunho["weather"]?[0]?["main"],
                        temp_min = (double?)rascunho["main"]?["temp_min"],
                        temp_max = (double?)rascunho["main"]?["temp_max"],
                        speed = (double?)rascunho["wind"]?["speed"],
                        visibility = (int?)rascunho["visibility"],
                        sunrise = sunrise.ToString("HH:mm:ss"),
                        sunset = sunset.ToString("HH:mm:ss")
                    };
                }
            }

            return t;
        }
    }
}