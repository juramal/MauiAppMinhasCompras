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
        base.OnAppearing();
        lista.Clear();
        List<Produto> tmp = await App.Db.GetAll();
        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		} catch
		{

		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;

        lista.Clear();
        
        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));

    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            var menuItem = sender as MenuItem;
            var produto = menuItem?.BindingContext as Produto;
            if (produto != null)
            {
                bool confirm = await DisplayAlert("Remover", $"Deseja remover o produto '{produto.Descricao}'?", "Sim", "Não");
                if (confirm)
                {
                    await App.Db.Delete(produto.Id);
                    lista.Remove(produto);
                }
            }
        } catch (Exception ex) 
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao remover o produto: {ex.Message}", "OK");
        }
        
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try 
        {
           Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto { BindingContext = p });


        } catch (Exception ex) 
        {
            DisplayAlert("Erro", $"Ocorreu um erro ao abrir o produto: {ex.Message}", "OK");
        }
    }
}