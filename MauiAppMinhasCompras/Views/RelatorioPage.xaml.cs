using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class RelatorioPage : ContentPage
    {
        public RelatorioPage()
        {
            InitializeComponent();

            // Define intervalo padrão: de 30 dias atrás até hoje
            dtp_inicio.Date = DateTime.Today.AddDays(-30);
            dtp_fim.Date = DateTime.Today;
        }

        private async void btn_filtrar_Clicked(object sender, EventArgs e)
        {
            // Validação: Data Inicial não pode ser maior que Final
            if (dtp_inicio.Date > dtp_fim.Date)
            {
                await DisplayAlertAsync("Atenção", "A data inicial não pode ser maior que a data final.", "OK");
                return;
            }

            try
            {
                // --- REUTILIZANDO A FONTE DE DADOS EXISTENTE ---
                List<Produto> todosProdutos = await App.Database.GetAll();

                // --- APLICANDO O FILTRO POR DATA (LINQ) ---
                // Comparamos apenas a parte .Date para ignorar horas, se houver
                var produtosFiltrados = todosProdutos
                    .Where(p => p.DataCadastro.Date >= dtp_inicio.Date && p.DataCadastro.Date <= dtp_fim.Date)
                    .OrderByDescending(p => p.DataCadastro) // Mostra os mais recentes primeiro
                    .ToList();

                // Atualiza o ItemsSource do CollectionView
                cv_relatorio.ItemsSource = produtosFiltrados;

                // Validação: Se não houver resultados, exibe mensagem
                if (!produtosFiltrados.Any())
                {
                    await DisplayAlertAsync("Aviso", "Nenhum produto encontrado no período selecionado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", $"Ocorreu um erro ao buscar o relatório: {ex.Message}", "OK");
            }
        }
    }
}