using Caffebar.Services;
using CaffeBar.Data;
using CaffeBar.Models;

namespace CaffeBar.Services
{
    public class TableService : ITableService
    {
        private readonly CaffeBarDbContext _context;
        
        public TableService(CaffeBarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Table>> GetTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table?> GetTableAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<bool> CreateTableAsync(Table newTable)
        {
            if (newTable == null)
            {
                throw new ArgumentNullException("Table cannot be null.");
            }

            if (newTable.Seats <= 0)
            {
                throw new ArgumentException("Invalid number of seats.");
            }

            try{
                _context.Tables.Add(newTable);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTableAsync(Table updatedTable)
        {
            if (updatedTable == null)
            {
                throw new ArgumentNullException("Table cannot be null.");
            }

            if (updatedTable.Seats <= 0)
            {
                throw new ArgumentException("Invalid number of seats.");
            }

            try
            {
                _context.Tables.Update(updatedTable);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid table ID.");
            }

            var table = await _context.Tables.FindAsync(id);

            if (table == null)
            {
                return false;
            }
            
            try {
                _context.Tables.Remove(table);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}