using EPiServer.Framework.Web.Resources;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using playground.Models.Blocks;
using playground.Models.ViewModels;

namespace playground.Features.Models.Blocks
{
    public class StarRatingBlockViewComponent : BlockComponent<StarRatingBlock>
    {
        private readonly IRequiredClientResourceList _requiredClientResourceList;

        public StarRatingBlockViewComponent(IRequiredClientResourceList requiredClientResourceList)
        {
            _requiredClientResourceList = requiredClientResourceList;
        }

        protected override IViewComponentResult InvokeComponent(StarRatingBlock currentContent)
        {
            _requiredClientResourceList.RequireStyle("/css/star-rating.css").AtHeader();

            _requiredClientResourceList.RequireScript("/js/star-rating.js").AtFooter();

            var model = new StarRatingViewModel
            {
                Stars = 3
            };

            return View(model);
        }
    }
}
