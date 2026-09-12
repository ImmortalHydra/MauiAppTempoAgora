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

    private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string cidade = Cidade.Text;

                if (string.IsNullOrWhiteSpace(cidade))
                {
                    await DisplayAlert("Atenção", "Digite o nome de uma cidade.", "OK");
                    return;
                }

                Tempo? t = await DataService.GetPrevisao(cidade);

                if (t != null)
                {
                    lbl_res.Text =
                        $"Temperatura mínima: {t.temp_min}°C\n" +
                        $"Temperatura máxima: {t.temp_max}°C\n" +
                        $"Clima: {t.description}\n" +
                        $"Velocidade do vento: {t.speed} m/s\n" +
                        $"Visibilidade: {t.visibility} metros\n" +
                        $"Nascer do sol: {t.sunrise}\n" +
                        $"Pôr do sol: {t.sunset}";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }

}
