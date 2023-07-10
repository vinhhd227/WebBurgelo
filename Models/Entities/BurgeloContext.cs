using Microsoft.EntityFrameworkCore;

namespace WebBurgelo.Models;

public class BurgeloContext : DbContext
{
    public BurgeloContext(DbContextOptions<BurgeloContext> options) : base(options)
    {
        //....
    }
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderDetailModel>(entity =>
        {
            entity.HasOne(od => od.order).WithMany();
        });
    }
    public DbSet<ProductModel> products { get; set; }
    public DbSet<CategoryModel> categories { get; set; }
    // public DbSet<ContactModel> contacts { get; set; }
    public DbSet<SubscribeModel> subscribes { get; set; }
    public DbSet<AccountModel> accounts { get; set; }
    public DbSet<UserModel> users { get; set; }
    public DbSet<RoleModel> roles { get; set; }
    public DbSet<OrderModel> orders { get; set; }
    // public DbSet<PaymentModel> payments { get; set; }
    public DbSet<DeliveryModel> deliveries { get; set; }
    public DbSet<OrderDetailModel> orderDetails { get; set; }
    public DbSet<VerifyEmailModel> verifyEmails { get; set; }
}