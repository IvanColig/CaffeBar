using CaffeBar.Models;

namespace Caffebar.Services
{
    public interface ITableService
    {
        Task<IEnumerable<Table>> GetTablesAsync();
        Task<Table?> GetTableAsync(int id);
        Task<bool> CreateTableAsync(Table newTable);
        Task<bool> UpdateTableAsync(Table updatedTable);
        Task<bool> DeleteTableAsync(int id);
    }
}