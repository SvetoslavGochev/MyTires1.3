namespace МоитеГуми.Infrastructure.Extensions
{

    using МоитеГуми.Services.Announcement;
    public static class ModelExtensions
    {
        public static string GetInformation(this IAnnouncementModel obqwa)
            => obqwa.Marka + "-" + obqwa.Size;
    }
}
