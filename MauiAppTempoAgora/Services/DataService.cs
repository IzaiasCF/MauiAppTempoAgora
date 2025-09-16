using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "aeb865d798149506a8e4abcf17d44ce5";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp;

                try
                {
                    resp = await client.GetAsync(url);
                }
                catch (HttpRequestException)
                {
                    throw new Exception("Sem conexão com a internet.");
                }

                if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                resp.EnsureSuccessStatusCode();

                string json = await resp.Content.ReadAsStringAsync();
                var rascunho = JObject.Parse(json);

                var sunriseUnix = (long?)rascunho["sys"]?["sunrise"];
                var sunsetUnix = (long?)rascunho["sys"]?["sunset"];

                string sunriseStr = sunriseUnix.HasValue
                    ? DateTimeOffset.FromUnixTimeSeconds(sunriseUnix.Value)
                                     .ToLocalTime()
                                     .ToString("HH:mm"): "";

                string sunsetStr = sunsetUnix.HasValue
                    ? DateTimeOffset.FromUnixTimeSeconds(sunsetUnix.Value)
                                     .ToLocalTime()
                                     .ToString("HH:mm"): "";

                t = new Tempo
                {
                    lat = (double?)rascunho["coord"]?["lat"] ?? 0,
                    lon = (double?)rascunho["coord"]?["lon"] ?? 0,
                    description = (string?)rascunho["weather"]?[0]?["description"] ?? "",
                    main = (string?)rascunho["weather"]?[0]?["main"] ?? "",
                    temp_min = (double?)rascunho["main"]?["temp_min"] ?? 0,
                    temp_max = (double?)rascunho["main"]?["temp_max"] ?? 0,
                    speed = (double?)rascunho["wind"]?["speed"] ?? 0,
                    visibility = (int?)rascunho["visibility"] ?? 0,
                    sunrise = sunriseStr,
                    sunset = sunsetStr
                };
            }
            return t;
        }
    }
}