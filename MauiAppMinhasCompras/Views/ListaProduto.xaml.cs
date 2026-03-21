using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        Lista_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Database.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lista.Clear();

            List<Produto> tmp = await App.Database.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        await DisplayAlertAsync("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Utiliza o MenuFlyoutItem ao invés do MenuItem para acessar o BindingContext do item clicado
            MenuFlyoutItem selecionado = sender as MenuFlyoutItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlertAsync(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                // Deleta do banco e da lista para atualizar a tela
                await App.Database.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    // Com CollectionView, utilizei o SelectionChanged para capturar o clique no item
    private async void List_Produtos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            // Pega o primeiro item selecionado
            if (e.CurrentSelection.FirstOrDefault() is Produto p) // Checa se é um Produto para evitar erros caso a seleção seja nula ou de outro tipo
            {
                // Navega para a tela de edição passando o produto selecionado como BindingContext
                await Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p,
                });

                // Limpa a seleção para que o item não fique marcado quando voltar para esta tela
                Lista_produtos.SelectedItem = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}