using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object? sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(Cidade.Text);

                    if (t != null)
                    {
                        string resultado =
                            $"Latitude: {t.lat}\n" +
                            $"Longitude: {t.lon}\n" +
                            $"Nascer do Sol: {t.sunrise}\n" +
                            $"Pôr do Sol: {t.sunset}\n" +
                            $"Temperatura Máxima: {t.temp_max}°C\n" +
                            $"Temperatura Mínima: {t.temp_min}°C\n" +
                            $"Velocidade do Vento: {t.speed} m/s\n" +
                            $"Condição: {t.description}";

                        lbl_res.Text = resultado;
                    }
                    else
                    {
                        lbl_res.Text = "Não foi possível obter a previsão do tempo para a cidade informada.";
                    }
                }
                else
                {
                    lbl_res.Text = "Informe uma cidade para consultar a previsão do tempo.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Ocorreu um erro",
                    ex.Message,
                    "OK");
            }
        }
    }
}