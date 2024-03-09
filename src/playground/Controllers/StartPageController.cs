using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using playground.Features.Email;
using playground.Models.Pages;
using playground.Models.ViewModels;

namespace playground.Controllers
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        private readonly IEmailService _emailService;

        public StartPageController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(StartPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            // Check if it is the StartPage or just a page of the StartPage type.
            if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentPage.ContentLink))
            {
                // Connect the view models logotype property to the start page's to make it editable
                var editHints = ViewData.GetEditHints<PageViewModel<StartPage>, StartPage>();
                editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
                editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
                editHints.AddConnection(m => m.Layout.CompanyInformationPages, p => p.CompanyInformationPageLinks);
                editHints.AddConnection(m => m.Layout.NewsPages, p => p.NewsPageLinks);
                editHints.AddConnection(m => m.Layout.CustomerZonePages, p => p.CustomerZonePageLinks);
            }

            await _emailService.SendEmailAsync("toEmailAddress", "Hello World", "Sending an email using code");

            await _emailService.SendEmailFluentAsync("toEmailAddress", "Hello World");

            return View(model);
        }
    }
}
