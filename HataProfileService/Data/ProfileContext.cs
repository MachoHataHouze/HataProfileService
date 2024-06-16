using HataProfileService.Models;
using Microsoft.EntityFrameworkCore;

namespace HataProfileService.Data;

public class ProfileContext : DbContext
{
    public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }

    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>().HasKey(p => p.Id);
        base.OnModelCreating(modelBuilder);
    }
}