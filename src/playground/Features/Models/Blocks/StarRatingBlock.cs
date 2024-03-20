using playground.Models;
using playground.Models.Blocks;

namespace playground.Features.Models.Blocks
{
    /// <summary>
    /// Used to insert editorial content edited using a rich-text editor
    /// </summary>
    [SiteContentType(
        DisplayName = "Star Rating Block",
        GUID = "18bafd9d-bb86-4ffa-b0c0-e32b91afc14e",
        GroupName = SystemTabNames.Content)]
    [SiteImageUrl]
    public class StarRatingBlock : SiteBlockData
    {
    }
}
