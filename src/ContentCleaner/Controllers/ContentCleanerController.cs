using Content_Cleaner.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Security;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text;

namespace Content_Cleaner.Controllers
{
    [Authorize(Roles = "CmsAdmins")]
    [Route("[controller]")]
    public class ContentCleanerController : Controller
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentModelUsage _contentModelUsage;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IUrlResolver _urlResolver;

        public ContentCleanerController(IUrlResolver urlResolver, IContentTypeRepository contentTypeRepository, IContentModelUsage contentModelUsage, IContentRepository contentRepository)
        {
            _urlResolver = urlResolver;
            _contentTypeRepository = contentTypeRepository;
            _contentModelUsage = contentModelUsage;
            _contentRepository = contentRepository;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ContentCleanerViewModel()
            {
                ContentItems = GetContentTypes()
            };

            return View("/Views/ContentCleaner/Index.cshtml", model);
        }

        [Route("[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContentCleanerViewModel model)
        {
            model.ContentItems = GetContentTypes();

            model.ContentUsages = GetContentTypeUsages(model.ContentTypeId);

            return View("/Views/ContentCleaner/Index.cshtml", model);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Delete(string contentRef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DeleteItem(contentRef);
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ContentType = "application/json",
                    Content = $"Error - {ex.Message}"
                };
            }
        }

        private void DeleteItem(string contentRef)
        {
            _contentRepository.Delete(ContentReference.Parse(contentRef), forceDelete: false, access: AccessLevel.NoAccess);
        }

        private List<ContentUsageBreadcrumb<ContentUsage>> GetContentTypeUsages(int selectedTypeID)
        {
            var contentType = _contentTypeRepository.Load(selectedTypeID);
            return _contentModelUsage.ListContentOfContentType(contentType)
                .Select(x => new ContentUsageBreadcrumb<ContentUsage>(x)
                {
                    Breadcrumb = BreadCrumb(x.ContentLink),
                    ContentUrl = _urlResolver.GetUrl(x.ContentLink)
                })
                .OrderBy(x => x.Name)
                .ToList();
        }

        private IEnumerable<SelectListItem> GetContentTypes()
        {
            var contentTypes = _contentTypeRepository.List();

            var exclusions = new List<int>() { 1, 2, 3, 4 };

            var result = contentTypes.Where(p => !exclusions.Exists(p2 => p2 == p.ID)).OrderBy(x => x.Name);

            return result.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Name
            }).ToList();
        }

        public string BreadCrumb(ContentReference cReference)
        {
            if (cReference == null)
                return string.Empty;

            var ancestor = _contentRepository.GetAncestors(cReference).ToList();
            var content = _contentRepository.Get<IContent>(cReference);

            var path = new StringBuilder();

            for (var x = ancestor.Count - 1; x >= 0; x--)
            {
                path.Append($"\\{ancestor[x].Name}");
            }

            return $"{path}\\{content.Name}".Trim("\\".ToCharArray());
        }
    }
}