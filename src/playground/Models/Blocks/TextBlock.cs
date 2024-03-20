using System.ComponentModel.DataAnnotations;

namespace playground.Models.Blocks
{
    /// <summary>
    /// Used to insert editorial content edited using a rich-text editor
    /// </summary>
    [SiteContentType(
        DisplayName ="Text Block",
        GUID = "3a1ac27c-ec93-487a-a4aa-40b4c0f8d805",
        GroupName = SystemTabNames.Content)]
    [SiteImageUrl]
    public class TextBlock : SiteBlockData
    {
        [Display(GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }
    }
}
