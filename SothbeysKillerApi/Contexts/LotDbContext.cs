using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Entity;

namespace SothbeysKillerApi.Contexts;

public class LotDbContext(DbContextOptions<LotDbContext> options) : DbContext(options)
{
    public DbSet<Lot> Lots { get; set; }
}