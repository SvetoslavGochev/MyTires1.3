namespace МоитеГуми.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using МоитеГуми.Models.Announcement;
    using Microsoft.AspNetCore.Authorization;
    using МоитеГуми.Infrastructure.Extensions;
    using МоитеГуми.Services.Announcement;
    using МоитеГуми.Services.Dealers;
    using AutoMapper;

    using static WebConstatnts;

    public class AnnouncementController : Controller
    {
        private readonly IDealerService dealers;
        private readonly IAnnouncementServices announcement;
        private readonly IMapper mapper;

        public AnnouncementController(
            IAnnouncementServices announcement,
            IDealerService dealers,
            IMapper mapper
            )
        {
            this.announcement = announcement;
            this.dealers = dealers;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AnnouncementSearchingModel query)
        {
            var queryResult = this.announcement.All(
                query.Marka,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AnnouncementSearchingModel.ObqwiPerPage);


            var obqwiMarki = this.announcement.AllMarki();

            query.CountObqwi = queryResult.CountObqwi;
            query.Marki = obqwiMarki;
            query.Obqwi = queryResult.Obqwa;

            return this.View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myObqwi = this.announcement.ByUser(this.User.Id());

            return View(myObqwi);
        }

        public IActionResult Details(int Id, string information)
        {
            var obqwa = this.announcement.Details(Id);
            if (information != obqwa.GetInformation())
            {
                return BadRequest();
            }


            return View(obqwa);
        }

        [Authorize]
        public IActionResult Delete(int Id)
        {
            this.announcement.DeleteAnoncment(Id);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!this.dealers.IsDealer(this.User.Id()))
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }
            //ot infrastrukture
            return View(new AnnouncementModel
            {
                Categories = this.announcement.AllCategories()//vieto shte ima info za kategoriite
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AnnouncementModel obqva)
        {
            var dealerId = this.dealers.IdByUser(this.User.Id());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }

            if (!this.announcement.CategoryExist(obqva.CategoryId))
            {
                this.ModelState.AddModelError(nameof(obqva.CategoryId), "Category is dont exist");
            }

            if (!ModelState.IsValid)
            {
                //IF NOT VALID PAK ПОКАЗЖА ЖИУТО ДА СЕ ПОПАЛНИ
                obqva.Categories = this.announcement.AllCategories();

                return View(obqva);
            }

           var obqwaId = this.announcement.Create(
                obqva.Marka,
                obqva.Description,
                obqva.Year,
                obqva.CategoryId,
                obqva.ImageUrl,
                obqva.Size,
                dealerId);

            TempData[GlobalMessageKey] = "Вашата обява е запазена и чака за одобрение";

            return RedirectToAction(nameof(Details), new { id = obqwaId, information = obqva.GetInformation()});// Always REDIREKT
        }

        [Authorize]
        public IActionResult Edit(int Id)
        {
            var userId = this.User.Id();

            if (!this.dealers.IsDealer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }

            var currentObqwa = this.announcement.Details(Id);

            if (currentObqwa.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var obqwaForm = this.mapper.Map<AnnouncementModel>(currentObqwa);

            obqwaForm.Categories = this.announcement.AllCategories();


            return View(obqwaForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int Id, AnnouncementModel obqva)
        {
            var dealerId = this.dealers.IdByUser(this.User.Id());

            if (dealerId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }

            if (!this.announcement.CategoryExist(obqva.CategoryId) && !User.IsAdmin())
            {
                this.ModelState.AddModelError(nameof(obqva.CategoryId), "Category is dont exist");
            }

            if (!ModelState.IsValid)
            {
                //IF NOT VALID PAK ПОКАЗЖА ЖИУТО ДА СЕ ПОПАЛНИ
                obqva.Categories = this.announcement.AllCategories();

                return View(obqva);
            }
            if (!this.announcement.IsByDealer(Id, dealerId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.announcement.Edit(
                    Id,
                    obqva.Marka,
                    obqva.Description,
                    obqva.Year,
                    obqva.CategoryId,
                    obqva.ImageUrl,
                    obqva.Size,
                    this.User.IsAdmin());

            TempData[GlobalMessageKey] = $"Вашата обява редактирана {(this.User.IsAdmin() ? string.Empty : "чака одобрение")}";

            return RedirectToAction(nameof(Details), new { Id , information = obqva.GetInformation() });
        }

        [Authorize]
        //[HttpPost]
        public IActionResult Info(int Id)
        {
            var currentObqwa = this.announcement.Info(Id);

            return View(currentObqwa);
        }



    }

}
