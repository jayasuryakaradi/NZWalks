﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionResopitory :IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLRegionResopitory(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            dbContext.SaveChanges();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
           return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(region=>region.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion=await dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
            if (existingRegion == null)
            {
                return null;    
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingRegion;

                
        }
        public async Task<Region?> DeleteAsync(Guid id){
            var existingRegion=await dbContext.Regions.FirstOrDefaultAsync(region=> region.Id == id);
            if (existingRegion != null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
