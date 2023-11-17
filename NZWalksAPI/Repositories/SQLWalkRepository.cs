using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        public async Task<Walk> Createasync(Walk walk)
        {
             await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            //throw new NotImplementedException();
            var existingwalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingwalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingwalk);
            await dbContext.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
            //throw new NotImplementedException();
            var existingWalk= await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) 
            {
                return null;
            }
            return existingWalk;
        }

        public async Task<List<Walk>> GetWAlkAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            var walks= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn)== false && string.IsNullOrWhiteSpace(filterQuery)== false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (bool)isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }

                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (bool)isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            int pagination = (pageNumber - 1) * pageSize;

            return await walks.Skip(pagination).Take(pageSize).ToListAsync();
            //return await walks.ToListAsync();
            //return (await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync());
            // walkDomain;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingwalkdomain= await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
            if(existingwalkdomain == null)
            {
                return null;
            }
            existingwalkdomain.Name = walk.Name;
            existingwalkdomain.LengthInKm =walk .LengthInKm;
            existingwalkdomain.Description = walk.Description;
            existingwalkdomain.DifficultyId = walk.DifficultyId;
            existingwalkdomain.RegionId=walk.RegionId;
            await dbContext.SaveChangesAsync();
            return existingwalkdomain;
            //existingwalkdomain.
        }
    }
}
