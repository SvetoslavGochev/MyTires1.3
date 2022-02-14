using System.Collections.Generic;

namespace МоитеГуми.Services.Announcement
{
    public class AnnouncementQueryServicesModel
    {
        public int CurrentPage { get; set; }

        public int CountObqwi { get; set; }

        public int ObqwiPerPage { get; set; }


        public IEnumerable<AnnouncementServicesModel> Obqwa { get; init; }
    }
}
