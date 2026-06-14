using FinCoreBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinCoreBank.Infrastructure.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(
        DbContextOptions<BankingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions =>
        Set<Transaction>();

        public DbSet<User> Users =>
    Set<User>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Amount)
            .HasPrecision(18,2);
    }
}