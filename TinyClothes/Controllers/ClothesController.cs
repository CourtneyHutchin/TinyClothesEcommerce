using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);

            if (c == null) // Clothing item is not found in the DB
            {
                return NotFound(); // returns a HTTP 404 - Not Found
                // return RedirectToAction("ShowAll"); // Returns the user to the ShowAll Page
            }

            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Clothing c)
        {
            if (ModelState.IsValid)
            {
                await ClothingDb.Edit(c, _context);
                ViewData["Message"] = c.Title + " updated successfully";
                return View(c);
            }

            return View(c);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            
            // Check if Clothing does not exist
            if(c == null)
            {
                return NotFound();
            }

            return View(c);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            await ClothingDb.Delete(c, _context);
            TempData["Message"] = $"{c.Title} deleted successfully";
            return RedirectToAction(nameof(ShowAll));
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchCriteria search)
        {
            if (ModelState.IsValid)
            {
                if(search.IsBeingSearched())
                {
                    await ClothingDb.BuildSearchQuery(search, _context);
                    return View(search);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You must search by at least one criteria");
                    return View(search);
                }
            }
            return View();
        }
    }
}