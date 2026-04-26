using MauiAppMinhasCompras.Models;
using System;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();

        // Define a data de hoje como padrão ao abrir a tela
        dtp_dataCadastro.Date = DateTime.Today;
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validações básicas antes de tentar converter
            if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
                string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
                string.IsNullOrWhiteSpace(txt_valorUnitario.Text))
            {
                await DisplayAlertAsync("Atenção", "Preencha todos os campos.", "OK");
                return;
            }

            Produto produto = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_valorUnitario.Text),
                DataCadastro = dtp_dataCadastro.Date ?? DateTime.Today
            };

            // Validate before persisting
            produto.ValidateForSave();

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