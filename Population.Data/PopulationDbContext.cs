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

        modelBuilder.Entity<DimAge>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            entity.HasOne(d => d.Population)
                .WithOne(p => p.Age)
                .HasForeignKey<DimAge>(d => d.PopulationId);
        });

        modelBuilder.Entity<DimSex>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            entity.HasOne(d => d.Population)
                .WithOne(p => p.Sex)
                .HasForeignKey<DimSex>(d => d.PopulationId);
        });

        modelBuilder.Entity<DimRegion>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            entity.HasOne(d => d.Population)
                .WithOne(p => p.Region)
                .HasForeignKey<DimRegion>(d => d.PopulationId);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DbConnection"));
    }
}
