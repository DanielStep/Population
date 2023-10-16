using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Population.Domain;
using System;
using System.Collections.Generic;

public class PopulationDbContext : DbContext
{
    private readonly IConfiguration _config;
    public PopulationDbContext(DbContextOptions<PopulationDbContext> options, IConfiguration config)
    {
        _config = config;
    }

    //Keyless Entities
    public DbSet<SA4Population> SA4PopData { get; set; }
    public DbSet<SA4PopulationAgeDiff> SA4PopulationAgeDiffData { get; set; }

    //Entities
    public DbSet<FactPopulation> FactPopulation { get; set; }
    public DbSet<DimAge> DimAge { get; set; }
    public DbSet<DimSex> DimSex { get; set; }
    public DbSet<DimRegion> DimRegion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SA4Population>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<SA4PopulationAgeDiff>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<FactPopulation>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        });

        modelBuilder.Entity<DimSex>().HasKey(s => s.Id);
        modelBuilder.Entity<DimAge>().HasKey(a => a.Id);
        modelBuilder.Entity<DimRegion>().HasKey(r => r.Id);
        modelBuilder.Entity<FactPopulation>().HasKey(p => p.Id);

        modelBuilder.Entity<FactPopulation>()
            .HasOne(p => p.DimSex)
            .WithMany(s => s.Populations)
            .HasForeignKey(p => p.DimSexFk);

        modelBuilder.Entity<FactPopulation>()
            .HasOne(p => p.DimAge)
            .WithMany(a => a.Populations)
            .HasForeignKey(p => p.DimAgeFk);

        modelBuilder.Entity<FactPopulation>()
            .HasOne(p => p.DimRegion)
            .WithMany(r => r.Populations)
            .HasForeignKey(p => p.DimRegionFk);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DbConnection"));
    }
}
