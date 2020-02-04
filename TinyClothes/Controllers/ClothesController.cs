using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class ClothesController : Controller
    {

        private readonly StoreContext _context;

        public ClothesController(StoreContext context)
        {
            _context = context;      // the "this" keyword is optional since its a different name
        }

        [HttpGet]
        public async Task<IActionResult> ShowAll(int? page)
        {
            const int PageSize = 2;
            // if page is not null, use its value... otherwise use 1
            // int pageNumber = page.HasValue ? page.Value : 1;    
            // NUll 
            int pageNumber = page ?? 1; // same as above
            ViewData["CurrentPage"] = pageNumber;
            
            int maxPage = await GetMaxPage(PageSize);

            ViewData["MaxPage"] = maxPage;

            List<Clothing> clothes = await ClothingDb.GetClothingByPage(_context, pageNum: pageNumber, pageSize: PageSize);
            return View(clothes);
        }

        private async Task<int> GetMaxPage(int PageSize)
        {
            int numProducts = await ClothingDb.GetNumClothing(_context);

            int maxPage = Convert.ToInt32(Math.Ceiling((double)numProducts / PageSize));
            return maxPage;
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Clothing c)
        {
            if(ModelState.IsValid)
            {
                await ClothingDb.Add(c, _context);

                // TempData lasts for one redirect
                TempData["Message"] = $"{c.Title} added successfully";
                return RedirectToAction("ShowAll");
            }

            // Return same view with validation messages
            return View(c);
        }
    }
}