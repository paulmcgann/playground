using Content_Cleaner.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContentCleaner.Services
{
    public interface IContentTypeService
    {
        IEnumerable<SelectListItem> GetContentTypes();

        List<ContentUsageDataViewModel> GetContentTypeUsage(int selectedTypeID);
    }
}