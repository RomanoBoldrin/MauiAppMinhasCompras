using SQLite;
using System; // Adicionado para suportar o DateTime

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        double _quantidade;
        double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                // Allow deserialization/ORM to set the property without throwing.
                // Normalize nulls to empty string to avoid null reference issues in bindings.
                _descricao = value?.Trim() ?? string.Empty;
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                // Allow ORM/deserialization to set default numeric values (0).
                _quantidade = value;
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                // Allow ORM/deserialization to set default numeric values (0).
                _preco = value;
            }
        }

        public double Total { get => Quantidade * Preco; }

        // NOVA PROPRIEDADE ADICIONADA
        public DateTime DataCadastro { get; set; }

        // Explicit validation to be called before saving a product.
        public void ValidateForSave()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new Exception("Por favor, preencha a descrição");
            if (Quantidade <= 0)
                throw new Exception("A quantidade deve ser maior que zero");
            if (Preco <= 0)
                throw new Exception("O preço deve ser maior que zero");
        }
    }
}