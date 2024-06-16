namespace HataProfileService.Models;

public class Profile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ContactInfo { get; set; }
    public string ProfilePicture { get; set; }
}