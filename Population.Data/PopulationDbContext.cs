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

    public DbSet<SA4PopulationData> SA4PopData { get; set; }
    public DbSet<SA4PopulationAgeDiffData> SA4PopulationAgeDiffData { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SA4PopulationData>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<SA4PopulationAgeDiffData>(entity =>
        {
            entity.HasNoKey();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DbConnection"));
    }
}
