//using BookStore.DataAccess;
//using BookStore.m.Models;
using BookStore.DataAccess;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
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
            Category obj = _unitOfWork.Category.Get(u => u.Id == categoryId);
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
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
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
            Category obj = _unitOfWork.Category.Get(u => u.Id == categoryId);
            if (obj == null)
                return NotFound();
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["Success"] = "Updated Category successfully!";
            return RedirectToAction("Index");

        }
    }
}
