using System.Security.Claims;
using HataProfileService.Models;
using HataProfileService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(ProfileDto profileDto)
        {
            var profile = new Profile
            {
                UserId = profileDto.UserId,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                ContactInfo = profileDto.ContactInfo,
                ProfilePicture = profileDto.ProfilePicture
            };

            await _profileService.AddProfileAsync(profile);
            return CreatedAtAction(nameof(GetProfile), new { userId = profile.UserId }, profile);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "nameid claim not found" });
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "Invalid nameid value" });
            }

            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            return Ok(profile);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateDto profileUpdateDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "nameid claim not found" });
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "Invalid nameid value" });
            }

            if (userId != profileUpdateDto.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingProfile = await _profileService.GetProfileAsync(userId);
            if (existingProfile == null)
            {
                return NotFound("Profile not found.");
            }

            existingProfile.FirstName = profileUpdateDto.FirstName;
            existingProfile.LastName = profileUpdateDto.LastName;
            existingProfile.ContactInfo = profileUpdateDto.ContactInfo;

            if (profileUpdateDto.ProfilePicture != null)
            {
                // Save the file to the server
                var filePath = Path.Combine("wwwroot", "images", $"{userId}_{profileUpdateDto.ProfilePicture.FileName}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileUpdateDto.ProfilePicture.CopyToAsync(stream);
                }

                existingProfile.ProfilePicture = filePath;
            }

            await _profileService.UpdateProfileAsync(existingProfile);
            return NoContent();
        }
        
        [HttpGet("mybookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "nameid claim not found" });
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "Invalid nameid valuq" });
            }

            var bookings = await _profileService.GetBookingsForUserAsync(userId);
            return Ok(bookings);
        }
    }
    

    public class ProfileUpdateDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactInfo { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
