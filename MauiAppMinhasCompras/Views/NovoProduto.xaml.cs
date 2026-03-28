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
       // Validação dos campos obrigatórios
	   if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
		   string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
		   string.IsNullOrWhiteSpace(txt_preco.Text))
	   {
		   await DisplayAlert("Erro", "Preencha todos os campos!", "OK");
		   return;
	   }

	   if (!double.TryParse(txt_quantidade.Text, out double quantidade) ||
		   !double.TryParse(txt_preco.Text, out double preco))
	   {
		   await DisplayAlert("Erro", "Quantidade e Preço devem ser números válidos!", "OK");
		   return;
	   }

	   try
	   {
		   Produto p = new Produto
		   {
			   Descricao = txt_descricao.Text,
			   Quantidade = quantidade,
			   Preco = preco
		   };

		   await App.Db.Insert(p);
		   await DisplayAlert("Sucesso!", "Registro Inserido", "OK");

	   }
	   catch (Exception ex)
	   {
		   await DisplayAlert("Ops", ex.Message, "OK");
	   }
    }
}