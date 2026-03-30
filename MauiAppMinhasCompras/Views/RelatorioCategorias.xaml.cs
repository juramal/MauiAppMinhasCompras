using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategorias : ContentPage
{
	public RelatorioCategorias()
	{
		InitializeComponent();
	}

	public RelatorioCategorias(List<Produto> produtos)
	{
		InitializeComponent();
		CarregarRelatorio(produtos);
	}

	private void CarregarRelatorio(List<Produto> produtos)
	{
		// Agrupar produtos por categoria e calcular totais
		var relatorio = produtos
			.Where(p => !string.IsNullOrWhiteSpace(p.Categoria))
			.GroupBy(p => p.Categoria)
			.Select(g => new
			{
				Categoria = g.Key,
				QuantidadeProdutos = g.Count(),
				TotalCategoria = g.Sum(p => p.Total)
			})
			.OrderByDescending(r => r.TotalCategoria)
			.ToList();

		collectionViewRelatorio.ItemsSource = relatorio;

		// Calcular e exibir total geral
		double totalGeral = relatorio.Sum(r => r.TotalCategoria);
		lblTotalGeral.Text = $"Total Geral: {totalGeral:C}";
	}

	private async void OnFecharClicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}
}