using HataProfileService.Models;

namespace HataProfileService.Repositories;

public interface IProfileRepository
{
    Task<Profile> GetByUserIdAsync(int userId);
    Task AddAsync(Profile profile);
    Task UpdateAsync(Profile profile);
}