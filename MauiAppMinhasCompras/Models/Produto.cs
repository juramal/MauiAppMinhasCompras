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
                if (value == null) { 
                    throw new ArgumentException("Descrição não pode ser vazia");
                }
                _descricao = value;
            } 
        }

        double _quantidade;

        public double Quantidade { 
            get => _quantidade;
            set
            {
                if (value == null) { 
                    throw new ArgumentException("Quantidade não pode ser vazia");
                }
                _quantidade = value;
            } 
        }

        double _preco;

        public double Preco { 
            get => _preco; 
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Preço não pode ser vazia");
                }
                _preco = value;
            } 
        }



        public double Total { get => Quantidade * Preco; }

    }
}
