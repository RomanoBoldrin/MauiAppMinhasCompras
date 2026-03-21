using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto; // Pega o produto que foi passado como parâmetro na navegação

            Produto produto = new Produto // Pega os dados do formulário e cria um novo produto para atualizar o registro no banco de dados
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_valorUnitario.Text)
            };

            await App.Database.Update(produto);
            await DisplayAlertAsync("Sucesso!", "Registro Atualizado.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", $"Algo deu errado: {ex.Message}", "OK");
        }
    }
}