using NZWalks.API.Models.Domain;
using NZWalks.API.Data;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Repositories
{
    public class WalkDifficuiltyRepository : IWalkDifficuiltyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficuiltyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id= Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);   
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficuilty=await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficuilty != null)
            {
                nZWalksDbContext.WalkDifficulty.Remove(existingWalkDifficuilty);
                await nZWalksDbContext.SaveChangesAsync();
            }
            return null;


        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
           return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync (x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficuilty= await nZWalksDbContext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficuilty==null)
            {
                return null;
            }
            
            existingWalkDifficuilty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficuilty;
        }
    }
}
