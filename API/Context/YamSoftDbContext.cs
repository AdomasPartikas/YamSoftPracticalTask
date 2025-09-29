using Microsoft.EntityFrameworkCore;
using YamSoft.API.Entities;

namespace YamSoft.API.Context;

public class YamSoftDbContext(DbContextOptions<YamSoftDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}