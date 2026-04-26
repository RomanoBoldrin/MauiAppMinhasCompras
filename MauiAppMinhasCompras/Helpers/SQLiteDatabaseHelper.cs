using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path) {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto produto) { 
            return _conn.InsertAsync(produto);
        }

        public Task<int> Update(Produto produto) {
            // Use the built-in UpdateAsync to properly update the record and return affected rows
            return _conn.UpdateAsync(produto);
        }

        public Task<int> Delete(int id) {
            return _conn.Table<Produto>().DeleteAsync(p => p.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string querry) {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + querry + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
