namespace МоитеГуми.Models.Announcement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using МоитеГуми.Models.Обява;
    using МоитеГуми.Services.Announcement;

    public class AnnouncementSearchingModel
    {
        public const int ObqwiPerPage = 3;
        public string Marka { get; set; }// za dropdown na markite
        //towa e selektiranata Marka
        //tuka sa vsi4ki marki

        [Display(Name = "Търсене по размер")]
        public string Size { get; set; }

        [Display(Name = "Търсене по марка")]
        public string SearchTerm { get; set; }

        public AnnouncementSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int CountObqwi { get; set; }

        public IEnumerable<string> Marki { get; set; }

        public IEnumerable<AnnouncementServicesModel> Obqwi { get; set; }
    }
}
