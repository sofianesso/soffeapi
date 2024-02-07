using Microsoft.EntityFrameworkCore;
using myOrderApi.Data;

namespace myOrderApi
{
    // En generisk repository-klass som hanterar grundläggande CRUD-operationer för en specifik modell.
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        // Hämtar alla objekt av typ T.
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        // Hämtar ett specifikt objekt av typ T baserat på dess ID.
        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Lägger till ett nytt objekt av typ T i databasen.
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Uppdaterar ett befintligt objekt av typ T i databasen.
        public async Task UpdateAsync(string id, T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        // Tar bort ett specifikt objekt av typ T från databasen.
        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
