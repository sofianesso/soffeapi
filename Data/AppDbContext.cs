using Microsoft.EntityFrameworkCore;

namespace myOrderApi.Data
{
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurera Restaurant-modellen för Cosmos DB
            modelBuilder.Entity<Restaurant>()
                .ToContainer("Restaurants") // Namnet på containern i Cosmos DB
                .HasPartitionKey(r => r.Id); 

            
        }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Restaurant> Restaurants {get; set;}
    


    public class MyDataService
{
    private readonly AppDbContext _context;

    public MyDataService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}

}

}
