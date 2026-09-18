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
                // 1. Valida se o utilizador está sem conexão com a internet
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlertAsync("Sem Conexão", "Não há conexão com a internet. Verifique a sua rede.", "OK");
                    return;
                }

                // 2. Valida se o campo da cidade foi preenchido
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Latitude: {t.lat} \n" +
                                                 $"Longitude: {t.lon} \n" +
                                                 $"Descrição: {t.description} \n" +
                                                 $"Nascer do Sol: {t.sunrise}h \n" +
                                                 $"Por do Sol: {t.sunset}h \n" +
                                                 $"Temp Máx: {t.temp_max}º \n" +
                                                 $"Temp Min: {t.temp_min}º \n" +
                                                 $"Visibilidade: {t.visibility}m \n" +
                                                 $"Velocidade do Vento: {t.speed}km/h\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        // Mensagem específica quando a cidade não é encontrada
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
                // Tratamento de falhas de requisição HTTP (ex: falhas de rede durante o envio)
                await DisplayAlertAsync("ERRO", "Falha ao comunicar com o servidor. Verifique a sua conexão.", "OK");
            }
            catch (Exception ex)
            {
                // Outros erros genéricos
                await DisplayAlertAsync("Ops", ex.Message, "OK");
            }
        }
    }
}