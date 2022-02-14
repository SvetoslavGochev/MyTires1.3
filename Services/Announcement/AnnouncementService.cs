namespace МоитеГуми.Services.Announcement
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System.Collections.Generic;
    using System.Linq;
    using МоитеГуми.Data;
    using МоитеГуми.Data.Models;
    using МоитеГуми.Models.Announcement;
    using МоитеГуми.Models.Обява;

    public class AnnouncementService : IAnnouncementServices
    {
        private readonly ApplicationDbContext data;
        private readonly IMapper mapper;

        public AnnouncementService(ApplicationDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public AnnouncementQueryServicesModel All(
             string marka = null,
            string searchTerm = null,
            AnnouncementSorting obqwiSorting = AnnouncementSorting.DateCreated,
            int currentPage = 1,
            int obqwiPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var obqwaQuery = this.data.Announcements
                .Where(o => !publicOnly || o.IsPublic);

            if (!string.IsNullOrWhiteSpace(marka))
            {
                obqwaQuery = obqwaQuery.Where(o => o.Marka == marka);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                obqwaQuery = obqwaQuery
                    .Where(o =>
                    o.Marka.ToLower().Contains(searchTerm.ToLower()) ||
                    o.Size.ToLower().Contains(searchTerm.ToLower()) ||
                    o.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            obqwaQuery = obqwiSorting switch
            {
                AnnouncementSorting.DateCreated => obqwaQuery.OrderByDescending(o => o.Id),
               AnnouncementSorting.Year => obqwaQuery.OrderByDescending(o => o.Year),
               AnnouncementSorting.Marka => obqwaQuery.OrderBy(o => o.Marka).ThenBy(o => o.Size),
                _ => obqwaQuery.OrderByDescending(o => o.Id)

            };

            var CountObqwi = obqwaQuery.Count();

            var obqwi = GetObqwi(obqwaQuery
                .Skip((currentPage - 1) * obqwiPerPage)
                .Take(obqwiPerPage)
               );

            return new AnnouncementQueryServicesModel
            {
                CountObqwi = CountObqwi,
                CurrentPage = currentPage,
                ObqwiPerPage = obqwiPerPage,
                Obqwa = obqwi
            };
        }

        public IEnumerable<string> AllMarki()
         => this.data.Announcements
               .Select(o => o.Marka)
                .Distinct()
                .OrderBy(x => x)
                .ToList();// samo unikalnite vzima

        public IEnumerable<AnnouncementServicesModel> ByUser(string userId)
          => this.GetObqwi(this.data
              .Announcements
              .Where(o => o.Dealer.UserId == userId));


        private IEnumerable<AnnouncementServicesModel> GetObqwi(IQueryable<Announcement> obqwaQuery)
        => obqwaQuery
            .ProjectTo<AnnouncementServicesModel>(this.mapper.ConfigurationProvider)
               .ToList();

        public IEnumerable<Models.Announcement.AnnouncementCategoryServiceModel> AllCategories()
            => (IEnumerable<Models.Announcement.AnnouncementCategoryServiceModel>)this.data
            .Categories
            .ProjectTo<AnnouncementCategoryServiceModel>(this.mapper.ConfigurationProvider)
            .ToList();
        public AnnouncementDetailsServiceModel Details(int obqwaId)
             => this.data
                .Announcements
                .Where(o => o.Id == obqwaId)
                .ProjectTo<AnnouncementDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

        public bool CategoryExist(int categoryId)
            => this.data.Categories.Any(c => c.Id == categoryId);

        public int Create(string marka, string description, int year, int categoryId, string imageUrl, string size, int dealerId)
        {
            var obqvaData = new Announcement
            {
                Marka = marka,
                Description = description,
                Year = year,
                CategoryId = categoryId,
                ImageUrl = imageUrl,
                Size = size,
                DealerId = dealerId,
                IsPublic = false
            };

            this.data.Announcements.Add(obqvaData);
            this.data.SaveChanges();

            return obqvaData.Id;
        }

        public bool Edit(
            int obqwaId,
            string marka,
            string description,
            int year,
            int categoryId,
            string imageUrl,
            string size,
            bool isPublic)
        {
            var obqvaData = this.data.Announcements.Find(obqwaId);

            if (obqvaData == null)
            {
                return false;
            }

            obqvaData.Marka = marka;
            obqvaData.Description = description;
            obqvaData.Year = year;
            obqvaData.CategoryId = categoryId;
            obqvaData.ImageUrl = imageUrl;
            obqvaData.Size = size;
            obqvaData.IsPublic = isPublic;

            this.data.SaveChanges();

            return true;
        }

        public bool IsByDealer(int obqwaId, int dealerId)
        => this.data
            .Announcements
            .Any(o => o.Id == obqwaId && o.DealerId == dealerId);

        public void DeleteAnoncment(int Id)
        {
            var deleteObqwa = this.data
                  .Announcements
                  .Where(o => o.Id == Id)
                  .FirstOrDefault();

            this.data.Remove(deleteObqwa);
            this.data.SaveChanges();
        }

        public Announcement Info(int Id)
        {
            var obqvaData = this.data.Announcements.Find(Id);


            return obqvaData;
        }

        public IEnumerable<LatestAnnouncementServiseModel> Latest()
        => this.data
              .Announcements
              .Where(o => o.IsPublic)
              .OrderByDescending(c => c.Id)
              .ProjectTo<LatestAnnouncementServiseModel>(this.mapper.ConfigurationProvider)
              .Take(3)
              .ToList();

        public void ChangeVisibility(int obqwaId)
        {
            var obqwa = this.data.Announcements.Find(obqwaId);

            obqwa.IsPublic = !obqwa.IsPublic;

            this.data.SaveChanges();
        }
    }
}
