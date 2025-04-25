namespace MovieAPI.Core.Models.ShowTime;

public class Reservation
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ShowTimeId { get; set; }
    public ShowTime ShowTime { get; set; } = null!;
    public int SeatsReserved { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
}

public enum ReservationStatus
{
    Confirmed,
    Cancelled,
    Pending,
    //Expired,
    //Rejected,
    //Failed,
    //Unknown
}