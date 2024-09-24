using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.Models;
using BookStore.Models.ViewModels;
using BookStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Template;
using NuGet.Protocol.Plugins;
using System.Reflection.Metadata.Ecma335;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        IUnitOfWork _unitOfWork;
        IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment; 
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            return View(products);
        }
        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
                 .GetAll().Select(c => new SelectListItem
                 {
                     Text = c.Name,
                     Value = c.Id.ToString()
                 }),
                Product = new Product()
            };
            return View(productVM);
        }
        [HttpPost]
        public ActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath =Path.Combine(wwwRootPath , @"images\product");
                    if (!String.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        // delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName;

                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);

                }
                _unitOfWork.Save();
                TempData["Success"] = "Create Product successfully!";
                return RedirectToAction("Index");
            }
            else {
                ProductVM productVM = new()
                {
                    CategoryList = _unitOfWork.Category
                    .GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),
                    Product = new Product()
                };
                return View(productVM);
            }
            //TempData["Error"] = "Error";

        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
                {
                    CategoryList = _unitOfWork.Category.GetAll().Select(p => new SelectListItem
                    {
                        Text = p.Name,
                        Value = p.Id.ToString()
                    }),
                    Product = new Product()
                };
                if (id == 0 || id == null)
                {//create
                    return View(productVM);
                }
                else
                {//update
                    productVM.Product = _unitOfWork.Product.Get(p => p.Id == id);
                    return View(productVM);
                }




        }
        [HttpPost]
        public ActionResult Edit(Product obj) 
        {
           if(ModelState.IsValid)
            { 
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
                TempData["Success"] = "Updated Category Successfully!";
            return RedirectToAction("Index");
        }
            TempData["Error"]="Error";
            return View();
        }
        //public IActionResult Delete(int productId)
        //{
        //   if(productId== 0 || productId==null) 
        //        return NotFound();
        //    Product obj = _unitOfWork.Product.Get(p => p.Id == productId);
        //   if (obj == null) 
        //        return NotFound();
        //    return View(obj);
        //}
        //[HttpPost]
        //public ActionResult Delete(Product product)
        //{
        //    _unitOfWork.Product.Remove(product);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Update Product Successfully!";
        //    return RedirectToAction("Index");
        //}
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });

        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u=>u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

           }
            if (!String.IsNullOrEmpty(productToBeDeleted.ImageUrl))
            {
                // delete the old image
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _unitOfWork.Product.Remove(productToBeDeleted); 
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });

        }
        #endregion
    }
}
