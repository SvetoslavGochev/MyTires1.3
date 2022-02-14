namespace МоитеГуми.Services.Announcement
{
    using System.Collections.Generic;
    using МоитеГуми.Data.Models;
    using МоитеГуми.Models.Обява;
    public interface IAnnouncementServices
    {
        Announcement Info(int Id);

        void DeleteAnoncment(int Id);

        AnnouncementQueryServicesModel All(
            string marka = null,
            string searchTerm = null,
            AnnouncementSorting obqwiSorting = AnnouncementSorting.DateCreated,
            int currentPage = 1,
            int obqwiPerPage = int.MaxValue,
            bool publicOnly = true);

        IEnumerable<LatestAnnouncementServiseModel> Latest();


        AnnouncementDetailsServiceModel Details(int obqwaId);


        int Create(
                string marka,
                string description,
                int year,
                int categoryId,
                string imageUrl,
                string size,
                int dealerId);
        bool Edit(
                int obqwaId,
                string marka,
                string description,
                int year,
                int categoryId,
                string imageUrl,
                string size,
                bool isPublic);

        IEnumerable<AnnouncementServicesModel> ByUser(string userId);

        bool IsByDealer(int obqwaId, int dealerId);

        void ChangeVisibility(int obqwaId);

        IEnumerable<string> AllMarki();

        IEnumerable<Models.Announcement.AnnouncementCategoryServiceModel> AllCategories();

        bool CategoryExist(int categoryId);



    }
}
