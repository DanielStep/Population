using Microsoft.EntityFrameworkCore;
using System;

public class DataSeeder
{
    private readonly PopulationDbContext _context;
    public DataSeeder(PopulationDbContext context)
    {
        _context = context;
    }

    public void Seed(Guid factPopulationId)
    {
        var factPopulation = new FactPopulation
        {
            Id = factPopulationId,
            PopulationTime = 2011,
            CensusYear = 2011,
            PopulationValue = 9999
        };

        var dimAge = new DimAge
        {
            Age = "25",
            AgeString = "25 years",
            PopulationId = factPopulationId
        };

        var dimSex = new DimSex
        {
            Sex_ABS = "1",
            Sex = "Male",
            PopulationId = factPopulationId
        };

        var dimRegion = new DimRegion
        {
            StateCode = "1",
            State = "NSW",
            Region = "Sydney - City and Inner South",
            ASGS_2016 = "999999999",
            PopulationId = factPopulationId
        };

        _context.FactPopulation.Add(factPopulation);
        _context.DimAge.Add(dimAge);
        _context.DimSex.Add(dimSex);
        _context.DimRegion.Add(dimRegion);

        _context.SaveChanges();
    }

    public void DeleteSeededData(Guid factPopulationId)
    {
        var regionToDelete = _context.DimRegion.FirstOrDefault(r => r.PopulationId == factPopulationId);
        var sexToDelete = _context.DimSex.FirstOrDefault(s => s.PopulationId == factPopulationId);
        var ageToDelete = _context.DimAge.FirstOrDefault(a => a.PopulationId == factPopulationId);
        var populationToDelete = _context.FactPopulation.FirstOrDefault(p => p.Id == factPopulationId);

        if (regionToDelete != null)
        {
            _context.DimRegion.Remove(regionToDelete);
        }

        if (sexToDelete != null)
        {
            _context.DimSex.Remove(sexToDelete);
        }

        if (ageToDelete != null)
        {
            _context.DimAge.Remove(ageToDelete);
        }

        if (populationToDelete != null)
        {
            _context.FactPopulation.Remove(populationToDelete);
        }

        _context.SaveChanges();
    }
}