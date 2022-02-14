namespace МоитеГуми.Infrastructure
{
    using AutoMapper;
    using МоитеГуми.Data.Models;
    using МоитеГуми.Models.Announcement;
    using МоитеГуми.Services.Announcement;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Announcement, AnnouncementServicesModel>()
            .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));

            this.CreateMap<Category, Models.Announcement.AnnouncementCategoryServiceModel>();

            this.CreateMap<AnnouncementDetailsServiceModel, AnnouncementModel>();
            this.CreateMap<Announcement, LatestAnnouncementServiseModel>();


            this.CreateMap<Announcement, AnnouncementDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId))
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
    
}
