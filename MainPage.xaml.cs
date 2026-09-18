using PrevisaoTempo.Models;
using PrevisaoTempo.Services;

namespace PrevisaoTempo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica a conectividade do dispositivo antes de tentar a chamada de rede
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlertAsync("Sem Conexão", "Não há conexão com a internet. Verifique a sua rede.", "OK");
                    return;
                }

                // Garante que o campo de texto da cidade não está vazio ou nulo
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Latitude: {t.lat} \n" +
                                                 $"Longitude: {t.lon} \n" +
                                                 $"Descrição: {t.description} \n" +
                                                 $"Nascer do Sol: {t.sunrise} \n" +
                                                 $"Por do Sol: {t.sunset} \n" +
                                                 $"Temp Máx: {t.temp_max}º \n" +
                                                 $"Temp Min: {t.temp_min}º \n" +
                                                 $"Visibilidade: {t.visibility}m \n" +
                                                 $"Velocidade do Vento: {t.speed}km/h\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        // Exibe mensagem amigável quando a API retorna que a cidade não existe (t == null)
                        await DisplayAlertAsync("Ops", "Não foi possível localizar a cidade digitada. Verifique o nome e tente novamente.", "OK");
                    }
                }
                else
                {
                    await DisplayAlertAsync("Ops", "Preencha o nome da cidade", "OK");
                }
            }
            catch (HttpRequestException)
            {
                // Captura exceções da camada de transporte HTTP (falhas físicas de rede ou problemas DNS durante a requisição)
                await DisplayAlertAsync("ERRO", "Falha ao comunicar com o servidor. Verifique a sua conexão.", "OK");
            }
            catch (Exception ex)
            {
                // Captura qualquer outra falha não esperada no código
                await DisplayAlertAsync("Ops", ex.Message, "OK");
            }
        }
    }
}