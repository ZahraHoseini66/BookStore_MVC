using BookStoreWeb.Data;
using BookStoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreWeb.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {

            if (category.Name.ToLower() == "test")
            {
                ModelState.AddModelError("Name", "Name cannot be test!");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["Success"] = "Created Category successfully!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Error!";

            return View();

        }
        public IActionResult Edit(int? categoryId)
        {
            if (categoryId == 0 || categoryId == null)
                return NotFound();
            Category obj = _db.Categories.Find(categoryId);
            if (obj == null)    
                return NotFound();
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {

            if (category.Name.ToLower() == "test")
            {
                ModelState.AddModelError("Name", "Name cannot be test!");
            }
            if (ModelState.IsValid)
            {
                 _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["Success"] = "Updated Category successfully!";

                return RedirectToAction("Index");
            }
            TempData["Error"] = "Error!";
            return View();

        }
        public IActionResult Delete(int? categoryId)
        {
            if (categoryId == 0 || categoryId == null)
                return NotFound();
            Category obj = _db.Categories.Find(categoryId);
            if (obj == null)    
                return NotFound();
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
              _db.Categories.Remove(category);
                _db.SaveChanges();
            TempData["Success"] = "Updated Category successfully!";
            return RedirectToAction("Index");
          
        }
    }
}
