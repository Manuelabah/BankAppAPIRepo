using BankAPPAPICoreDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPPAPICore.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public virtual DbSet<UserModel> UserModels { get; set; }
        public virtual DbSet<AccountModel> AccountModels { get; set; }
        public virtual DbSet<TransactionModel> TransactionModels { get; set; }
    }
}
