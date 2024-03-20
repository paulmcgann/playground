using EPiServer.Authorization;
using EPiServer.Shell.Navigation;

namespace Content_Cleaner.Presentation
{
    [MenuProvider]
    public class ContentCleanerMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var contentCleaner = new UrlMenuItem("Content Cleaner", MenuPaths.Global + "/cms/admin/contentcleaner", "/contentcleaner/index")
            {
                IsAvailable = _ => true,
                SortIndex = 70,
                AuthorizationPolicy = CmsPolicyNames.CmsAdmin
            };

            return new List<MenuItem>(1)
        {
            contentCleaner
        };
        }
    }
}