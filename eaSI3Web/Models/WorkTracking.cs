using System;

namespace eaSI3Web.Models
{
    public class WorkTracking
    {
        public int WorkTrackingId { get; set; }
        public User User { get; set; }
        public int TotalHours { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime TrackingDate { get; set; }
        public TrackResult TrackResult { get; set; }
        public string TrackResultAddtionalInfo { get; set; }
    }
}
