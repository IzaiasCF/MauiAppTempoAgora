using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System;
using System.Net.Http;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
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

        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = 
                    new GeolocationRequest(
                        GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10)
                    );

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null) 
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude}";

                    lbl_coords.Text = local_disp;
                }
                else 
                {
                    lbl_coords.Text = "Nenhuma localização";
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não suporta", fnsEx.Message, "Ok");
            }
            catch (FeatureNotEnabledException fnsEx)
            {
                await DisplayAlert("Erro: Localização desabilitada", fnsEx.Message, "Ok");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão da localização", pEx.Message, "Ok");
            }
            catch (Exception ex) 
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
            }
        }
    }
}
