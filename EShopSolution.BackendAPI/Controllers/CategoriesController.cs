﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopSolution.Application.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPaging(string languageId)
        {
            var categories = await _categoryService.GetAll(languageId);
            return Ok(categories);
        }

    }
}