using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
    }

	protected async override void OnAppearing()
	{
		List<Produto> tmp = await App.Database.GetAll();
		tmp.ForEach(p => lista.Add(p));
    }

    private void ToolbarItem_Clicked_Adicionar(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		} catch (Exception ex)
		{
			DisplayAlertAsync("Ops", $"Algo deu errado: {ex.Message}", "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		string querry = e.NewTextValue;

		lista.Clear();

		List<Produto> tmp = await App.Database.Search(querry);

		tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O valor total dos produtos é: {soma:C}";

		DisplayAlertAsync("Soma dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked_Remover(object sender, EventArgs e)
    {
        try
        {
            // Identifica o componente que disparou o clique
            var swipeItem = sender as SwipeItem;

            // Extrai o Produto do BindingContext
            if (swipeItem?.BindingContext is Produto produtoParaRemover)
            {
                bool confirmacao = await DisplayAlertAsync("Remover Produto",
                                                    $"Tem certeza que deseja remover '{produtoParaRemover.Descricao}'?",
                                                    "Sim", "Não");

                if (confirmacao)
                {
                    // Remove o produto do Banco de Dados
                    await App.Database.Delete(produtoParaRemover.Id);

                    // Remove o produto da lista visível na tela
                    lista.Remove(produtoParaRemover);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", $"Algo deu errado ao remover: {ex.Message}", "OK");
        }
    }
}