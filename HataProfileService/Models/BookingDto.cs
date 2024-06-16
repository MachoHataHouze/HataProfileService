namespace HataProfileService.Models;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Contract { get; set; }
    public DateTime DateCreated { get; set; }
}