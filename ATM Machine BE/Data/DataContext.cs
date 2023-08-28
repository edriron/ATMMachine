using ATM_Machine_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM_Machine_BE.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // connection string configuration
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=atmdb;Trusted_Connection=true;Trusted_Connection=true;TrustServerCertificate=true;");
        }
    }
}
