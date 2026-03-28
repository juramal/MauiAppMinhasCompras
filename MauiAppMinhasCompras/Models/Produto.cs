using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao { 
            get => _descricao;
            set
            {
                try
                {
                    if (value == null)
                    {
                        throw new ArgumentException("Descrição não pode ser vazia");
                    }
                    _descricao = value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao definir Descricao: {ex.Message}", ex);
                }
            }
        }

        double _quantidade;

        public double Quantidade { 
            get => _quantidade;
            set
            {
                try
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Quantidade não pode ser negativa");
                    }
                    _quantidade = value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao definir Quantidade: {ex.Message}", ex);
                }
            }
        }

        double _preco;

        public double Preco { 
            get => _preco;
            set
            {
                try
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Preço não pode ser negativo");
                    }
                    _preco = value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao definir Preco: {ex.Message}", ex);
                }
            }
        }



        public double Total { get => Quantidade * Preco; }

    }
}
