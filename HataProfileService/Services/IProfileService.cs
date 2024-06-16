using HataProfileService.Models;

namespace HataProfileService.Services;

public interface IProfileService
{
    Task<Profile> GetProfileAsync(int userId);
    Task AddProfileAsync(Profile profile);
    Task UpdateProfileAsync(Profile profile);
    Task <IEnumerable<BookingDto>>GetBookingsForUserAsync(int userId);
}