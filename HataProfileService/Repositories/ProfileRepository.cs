using HataProfileService.Data;
using HataProfileService.Models;
using Microsoft.EntityFrameworkCore;

namespace HataProfileService.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ProfileContext _context;

    public ProfileRepository(ProfileContext context)
    {
        _context = context;
    }

    public async Task<Profile> GetByUserIdAsync(int userId)
    {
        return await _context.Profiles.SingleOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task AddAsync(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Profile profile)
    {
        _context.Profiles.Update(profile);
        await _context.SaveChangesAsync();
    }
}