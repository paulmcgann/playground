using EPiServer.DataAbstraction;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Content_Cleaner.ViewModels
{
    public class ContentCleanerViewModel
    {
        public IEnumerable<SelectListItem> ContentItems { get; set; } = new List<SelectListItem>();

        public IEnumerable<ContentUsageBreadcrumb<ContentUsage>> ContentUsages { get; set; } = new List<ContentUsageBreadcrumb<ContentUsage>>();

        public int ContentTypeId { get; set; }
    }
}