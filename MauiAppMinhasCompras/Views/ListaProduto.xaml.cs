using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;


public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	List<Produto> listaTodosProdutos = new List<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarProdutos();
        CarregarCategorias();
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
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp;

            if (string.IsNullOrWhiteSpace(q))
            {
                tmp = await App.Db.GetAll();
            }
            else
            {
                tmp = await App.Db.Search(q);
            }

            // Aplicar filtro de categoria se houver uma selecionada
            string categoriaSelecionada = picker_categoria.SelectedItem as string;
            if (!string.IsNullOrEmpty(categoriaSelecionada) && categoriaSelecionada != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            listaTodosProdutos = tmp;
            tmp.ForEach(i => lista.Add(i));
        } catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }

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

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await CarregarProdutos();
            CarregarCategorias();
        } 
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        } 
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async Task CarregarProdutos()
    {
        lista.Clear();
        listaTodosProdutos = await App.Db.GetAll();
        listaTodosProdutos.ForEach(i => lista.Add(i));
    }

    private void CarregarCategorias()
    {
        try
        {
            // Obter categorias únicas dos produtos
            var categorias = listaTodosProdutos
                .Where(p => !string.IsNullOrWhiteSpace(p.Categoria))
                .Select(p => p.Categoria)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Adicionar "Todas" no início
            categorias.Insert(0, "Todas as Categorias");

            picker_categoria.ItemsSource = categorias;
            picker_categoria.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", $"Erro ao carregar categorias: {ex.Message}", "OK");
        }
    }

    private void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string categoriaSelecionada = picker_categoria.SelectedItem as string;

            lista.Clear();

            if (string.IsNullOrEmpty(categoriaSelecionada) || categoriaSelecionada == "Todas as Categorias")
            {
                // Mostrar todos os produtos
                listaTodosProdutos.ForEach(i => lista.Add(i));
            }
            else
            {
                // Filtrar por categoria selecionada
                var produtosFiltrados = listaTodosProdutos
                    .Where(p => p.Categoria == categoriaSelecionada)
                    .ToList();

                produtosFiltrados.ForEach(i => lista.Add(i));
            }

            // Aplicar também o filtro de busca se houver texto no SearchBar
            if (!string.IsNullOrWhiteSpace(txt_search.Text))
            {
                var produtosFiltrados = lista
                    .Where(p => p.Descricao.ToLower().Contains(txt_search.Text.ToLower()))
                    .ToList();

                lista.Clear();
                produtosFiltrados.ForEach(i => lista.Add(i));
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", $"Erro ao filtrar por categoria: {ex.Message}", "OK");
        }
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            // Verificar se há produtos
            if (listaTodosProdutos == null || listaTodosProdutos.Count == 0)
            {
                await DisplayAlert("Aviso", "Não há produtos cadastrados para gerar o relatório.", "OK");
                return;
            }

            // Verificar se há produtos com categorias
            var produtosComCategoria = listaTodosProdutos
                .Where(p => !string.IsNullOrWhiteSpace(p.Categoria))
                .ToList();

            if (produtosComCategoria.Count == 0)
            {
                await DisplayAlert("Aviso", "Não há produtos com categorias cadastradas.", "OK");
                return;
            }

            // Abrir página de relatório como modal
            var relatorioPage = new RelatorioCategorias(listaTodosProdutos);
            await Navigation.PushModalAsync(new NavigationPage(relatorioPage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao abrir relatório: {ex.Message}", "OK");
        }
    }
}