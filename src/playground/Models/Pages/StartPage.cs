using EPiServer.SpecializedProperties;
using playground.Models.Blocks;
using System.ComponentModel.DataAnnotations;
using static playground.Globals;

namespace playground.Models.Pages
{
    /// <summary>
    /// Used for the site's start page and also acts as a container for site settings
    /// </summary>
    [ContentType(
        GUID = "19671657-B684-4D95-A61F-8DD4FE60D559",
        GroupName = Globals.GroupNames.Specialized)]
    [SiteImageUrl]
    [AvailableContentTypes(
        Availability.Specific,
        Include =
        [
            typeof(ContainerPage),
            typeof(ProductPage),
            typeof(StandardPage),
            typeof(ISearchPage),
            typeof(LandingPage),
            typeof(ContentFolder)
        ], // Pages we can create under the start page...
        ExcludeOn =
        [
            typeof(ContainerPage),
            typeof(ProductPage),
            typeof(StandardPage),
            typeof(ISearchPage),
            typeof(LandingPage)
        ])] // ...and underneath those we can't create additional start pages
    public class StartPage : SitePageData
    {

        [Display(Name = "Icon Picker", GroupName = SystemTabNames.Content, Order = 10)]
        [UIHint(CmsUiHints.IconPicker)]
        public virtual string Icon { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 320)]
        [CultureSpecific]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 300)]
        public virtual LinkItemCollection ProductPageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 350)]
        public virtual LinkItemCollection CompanyInformationPageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 400)]
        public virtual LinkItemCollection NewsPageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 450)]
        public virtual LinkItemCollection CustomerZonePageLinks { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual PageReference GlobalNewsPageLink { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual PageReference ContactsPageLink { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual PageReference SearchPageLink { get; set; }

        [Display(GroupName = Globals.GroupNames.SiteSettings)]
        public virtual SiteLogotypeBlock SiteLogotype { get; set; }
    }
}
