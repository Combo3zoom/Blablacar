using Microsoft.EntityFrameworkCore;
using TestRegister.Models;

namespace TestRegister.Context;

public class Context: DbContext
{
    public DbSet<RegisterUser> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-OAMN13B;Database=Blalacar;Trusted_Connection=True");
    }
}