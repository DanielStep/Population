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
        var factPopulation = _context.FactPopulation.Find(factPopulationId);
        if (factPopulation == null)
        {
            factPopulation = new FactPopulation();

            var dimSex = _context.DimSex.FirstOrDefault(x => x.Sex_ABS == "1");
            var dimAge = _context.DimAge.FirstOrDefault(x => x.Age == "10");

            var dimRegion = new DimRegion { StateCode = "1", State = "New South Wales", Region = "NSW Test Region", ASGS_2016 = "9999" };
            _context.DimRegion.Add(dimRegion);

            if (dimSex != null && dimAge != null && dimRegion != null)
            {
                factPopulation.Id = factPopulationId;
                factPopulation.PopulationTime = 2011;
                factPopulation.CensusYear = 2011;
                factPopulation.PopulationValue = 999;
                factPopulation.DimSexFk = dimSex.Id;
                factPopulation.DimAgeFk = dimAge.Id;
                factPopulation.DimRegionFk = dimRegion.Id;

                _context.FactPopulation.Add(factPopulation);
            }

            _context.SaveChanges();
        }
    }

    public void DeleteSeededData(Guid factPopulationId)
    {
        var factPopulation = _context.FactPopulation.Find(factPopulationId);
        if (factPopulation != null)
        {
            _context.FactPopulation.Remove(factPopulation);
            var dimRegion = _context.DimRegion.FirstOrDefault(dr => dr.Id == factPopulation.DimRegionFk);
            if (dimRegion != null)
            {
                _context.DimRegion.Remove(dimRegion);
            }

            _context.SaveChanges();
        }
    }
}