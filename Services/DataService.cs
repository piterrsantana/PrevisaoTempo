using Newtonsoft.Json.Linq;
using PrevisaoTempo.Models;

namespace PrevisaoTempo.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;
            string chave = "6a2d2c94d965a549b1d6fa799b4b552c";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&lang=pt_br&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                //O HttResponseMessage sendo usado para receber a resposta da requisição HTTP feita para a API do OpenWeatherMap Contém informações sobre:status da resposta, cabeçalhos e o conteúdo retornado pela API.    
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    long sunriseUnix = rascunho["sys"]?["sunrise"]?.Value<long>() ?? 0;
                    long sunsetUnix = rascunho["sys"]?["sunset"]?.Value<long>() ?? 0;

                    DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds(sunriseUnix).LocalDateTime;
                    DateTime sunset = DateTimeOffset.FromUnixTimeSeconds(sunsetUnix).LocalDateTime;

                    t = new Tempo
                    {
                        lat = rascunho["coord"]?["lat"]?.Value<double>(),
                        lon = rascunho["coord"]?["lon"]?.Value<double>(),
                        description = rascunho["weather"]?[0]?["description"]?.ToString(),
                        main = rascunho["weather"]?[0]?["main"]?.ToString(),
                        temp_min = rascunho["main"]?["temp_min"]?.Value<double>(),
                        temp_max = rascunho["main"]?["temp_max"]?.Value<double>(),
                        speed = rascunho["wind"]?["speed"]?.Value<double>(),
                        visibility = rascunho["visibility"]?.Value<int>(),
                        sunrise = sunrise.ToString("HH:mm:ss"),
                        sunset = sunset.ToString("HH:mm:ss")

                    };
                }
            }

            return t;
        }
    }
}