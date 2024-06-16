using System.Net.Http.Headers;
using HataProfileService.Models;
using HataProfileService.Repositories;
using HataProfileService.Services;

namespace ProfileService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<ProfileService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _bookingServiceUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(IProfileRepository profileRepository, ILogger<ProfileService> logger, HttpClient httpClient, IConfiguration configuration,  IHttpContextAccessor httpContextAccessor)
        {
            _profileRepository = profileRepository;
            _logger = logger;
            _httpClient = httpClient;
            _bookingServiceUrl = configuration["BookingServiceUrl"];
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Profile> GetProfileAsync(int userId)
        {
            return await _profileRepository.GetByUserIdAsync(userId);
        }

        public async Task AddProfileAsync(Profile profile)
        {
            _logger.LogInformation("Adding profile for user ID: {UserId}", profile.UserId);
            await _profileRepository.AddAsync(profile);
        }

        public async Task UpdateProfileAsync(Profile profile)
        {
            _logger.LogInformation("Updating profile for user ID: {UserId}", profile.UserId);
            await _profileRepository.UpdateAsync(profile);
        }
        
        public async Task<IEnumerable<BookingDto>> GetBookingsForUserAsync(int userId)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"{_bookingServiceUrl}/api/Booking/mybookings");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<BookingDto>>();
        }
    }
}