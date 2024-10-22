using Microsoft.EntityFrameworkCore;
using UMD.Models;

namespace UMD.Data;

public class UMDContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}