namespace МоитеГуми.Services.Announcement
{
    public class AnnouncementServicesModel : IAnnouncementModel
    {
        public int Id { get; set; }

        public string Marka { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Year { get; set; }

        public string CategoryName { get; set; }

        public bool IsPublic { get; init; }
    }
}
