namespace МоитеГуми.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using МоитеГуми.Models.Api;
    using МоитеГуми.Services.Announcement;

    [ApiController]
    [Route("api/obqwa")]
    public class ObqwaApiController : ControllerBase
    {
        private readonly IAnnouncementServices obqwa;

        public ObqwaApiController(IAnnouncementServices obqwa)
        {
            this.obqwa = obqwa;
        }

        [HttpGet]
        public AnnouncementQueryServicesModel All([FromQuery] AllObqwiApiRequestModel query)
        {
            return this.obqwa.All(
                query.Marka,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.ObqwiPerPage);
        }
    }
}
