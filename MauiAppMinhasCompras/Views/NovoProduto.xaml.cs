using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

	private async void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			Produto produto = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_valorUnitario.Text)
            };

            await App.Database.Insert(produto);
			await DisplayAlertAsync("Sucesso!", "Registro inserido.", "OK");
			await Navigation.PopAsync();
        }
		catch (Exception ex)
		{
			await DisplayAlertAsync("Ops", $"Algo deu errado: {ex.Message}", "OK");
		}
	}
}