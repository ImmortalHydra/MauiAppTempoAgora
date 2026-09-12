using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            string chaveApi = "d7d88ba4b7d687136b070725d47cf447";

        string url =
            $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(cidade)}&units=metric&appid={chaveApi}&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    // Cidade não encontrada
                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Cidade não encontrada.");
                    }

                    // Outros erros da API
                    if (!resp.IsSuccessStatusCode)
                    {
                        throw new Exception("Erro ao consultar a previsão do tempo.");
                    }

                    string json = await resp.Content.ReadAsStringAsync();

                    JObject rascunho = JObject.Parse(json);

                    DateTime sunrise = DateTimeOffset
                        .FromUnixTimeSeconds((long)rascunho["sys"]!["sunrise"]!)
                        .ToLocalTime()
                        .DateTime;

                    DateTime sunset = DateTimeOffset
                        .FromUnixTimeSeconds((long)rascunho["sys"]!["sunset"]!)
                        .ToLocalTime()
                        .DateTime;

                    Tempo t = new Tempo
                    {
                        lat = (double)rascunho["coord"]!["lat"]!,
                        lon = (double)rascunho["coord"]!["lon"]!,

                        description = (string)rascunho["weather"]![0]!["description"]!,
                        main = (string)rascunho["weather"]![0]!["main"]!,

                        temp_min = (double)rascunho["main"]!["temp_min"]!,
                        temp_max = (double)rascunho["main"]!["temp_max"]!,

                        speed = (double)rascunho["wind"]!["speed"]!,
                        visibility = (int)rascunho["visibility"]!,

                        sunrise = sunrise.ToString("HH:mm:ss"),
                        sunset = sunset.ToString("HH:mm:ss")
                    };

                    return t;
                }
                catch (HttpRequestException)
                {
                    // Problema de conexão com a internet
                    throw new Exception("Sem conexão com a internet.");
                }
            }
        }
    }

}
