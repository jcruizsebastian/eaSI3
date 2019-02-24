namespace eaSI3Web.Models
{
    public class WorkTracking
    {
        public int WorkTrackingId { get; set; }
        public User User { get; set; }
        public int TotalHours { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public int TrackingDate { get; set; }
        public int TrackResult { get; set; }
        public string TrackResultAddtionalInfo { get; set; }
    }
}
