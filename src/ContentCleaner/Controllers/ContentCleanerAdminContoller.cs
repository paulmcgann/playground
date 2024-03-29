﻿using Content_Cleaner.ViewModels;
using ContentCleaner.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContentCleaner.Controllers
{
    public sealed class ContentCleanerAdminContoller : Controller
    {
        private readonly IContentTypeService _contentTypeService;

        public ContentCleanerAdminContoller(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        [HttpGet]
        [Route("/content.cleaner/admin/")]
        public IActionResult Index()
        {
            var model = new ContentCleanerViewModel()
            {
                ContentItems = _contentTypeService.GetContentTypes()
            };

            return View("~/Views/ContentCleaner/Index.cshtml", model);
        }
    }
}