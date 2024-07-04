using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using System.Reflection.Metadata.Ecma335;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        IUnitOfWork _unitOfWork;


       
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                TempData["Success"] = "Create Product successfully!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Error";
            return View();
        }
        public IActionResult Edit(int productId)
        {
            if (productId == 0 || productId == null)
                return NotFound();
             Product obj = _unitOfWork.Product.Get(p=>p.Id==productId);
            if (obj == null)
                return NotFound();
            return View(obj);
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
        public IActionResult Delete(int productId)
        {
           if(productId== 0 || productId==null) 
                return NotFound();
            Product obj = _unitOfWork.Product.Get(p => p.Id == productId);
           if (obj == null) 
                return NotFound();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(Product product)
        {
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["Success"] = "Update Product Successfully!";
            return RedirectToAction("Index");
        }
    }
}
