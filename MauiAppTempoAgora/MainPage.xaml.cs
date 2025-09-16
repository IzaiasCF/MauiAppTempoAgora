using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Net.Http;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_cidade.Text))
            {
                lbl_res.Text = "Preencha a cidade:";
                return;
            }

            try
            {
                Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                if (t == null)
                {
                    await DisplayAlert("Cidade não encontrada",
                        "O nome da cidade não foi encontrado. Verifique e tente novamente.",
                        "OK");
                    lbl_res.Text = "";
                    return;
                }

                string dados_previsao = "";

                dados_previsao =
                    $"Latitude: {t.lat}\n" +
                    $"Longitude: {t.lon}\n" +
                    $"Nascer do Sol: {t.sunrise}\n" +
                    $"Por do Sol: {t.sunset}\n" +
                    $"Temperatura Máxima: {t.temp_max}\n" +
                    $"Temperatura Mínima: {t.temp_min}\n" +
                    $"Descrição do Clima: {t.description}\n" +
                    $"Velocidade do Vento: {t.speed} \n" +
                    $"Visibilidade: {t.visibility} \n";

                lbl_res.Text = dados_previsao;
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Sem conexão",
                    "Não foi possível conectar à internet. Verifique sua rede e tente novamente.",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }
    }
}
