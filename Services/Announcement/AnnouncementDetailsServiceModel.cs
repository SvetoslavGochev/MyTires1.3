namespace МоитеГуми.Services.Announcement
{
    public class AnnouncementDetailsServiceModel : AnnouncementServicesModel
    {
        public int DealerId { get; init; }

        public int CategoryId { get; init; }

        //public string CategoryName { get; set; }

        public string DealerName { get; init; }

        public string UserId { get; init; }
    }
}
