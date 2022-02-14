namespace МоитеГуми.Models.Home
{
    using System.Collections.Generic;
    using МоитеГуми.Services.Announcement;

    public class ObqwaViewModel
    {
        public int CountAnnouncement { get; init; }
        public int CountUsers { get; init; }

        public IList<LatestAnnouncementServiseModel> obqwi { get; init; }
    }
}
