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
        try
        {
            // Clear the list first so items don't duplicate when returning to this page
            lista.Clear();

            List<Produto> tmp = await App.Database.GetAll();
            tmp.ForEach(p => lista.Add(p));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", $"Falha ao carregar os produtos: {ex.Message}", "OK");
        }
    }

    private async void ToolbarItem_Clicked_Adicionar(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", $"Algo deu errado ao abrir a tela: {ex.Message}", "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string querry = e.NewTextValue;

            lista.Clear();

            List<Produto> tmp = await App.Database.Search(querry);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", $"Falha ao buscar produtos: {ex.Message}", "OK");
        }
    }

    private async void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);

            string msg = $"O valor total dos produtos é: {soma:C}";

            await DisplayAlertAsync("Soma dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", $"Falha ao calcular a soma: {ex.Message}", "OK");
        }
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