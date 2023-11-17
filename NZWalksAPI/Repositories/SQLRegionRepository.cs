using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid Id)
        {
            var existingRegion= await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if(existingRegion == null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetByIdAsync(Guid Id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region?> UpdateAsync(Guid Id, Region region)
        {
            var existingDomain= await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id== Id);
            if (existingDomain ==null)
            {
                return null;
            }
            
            existingDomain.Code = region.Code;
            existingDomain .Name = region.Name;
            existingDomain .RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingDomain;
        }
    }
}
