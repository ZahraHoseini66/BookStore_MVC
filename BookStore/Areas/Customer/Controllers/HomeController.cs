using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.Models;
using BookStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookStoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IUnitOfWork _db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
           
            IEnumerable<Product> products = _db.Product.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Details(int productId)
        {

            ShoppingCart cart = new()
            {
                Product = _db.Product.Get(u => u.Id == productId),
                Count = 0,
                ProductId = productId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            ShoppingCart shoppingCartDb=_db.ShoppingCart.Get(u=>u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if(shoppingCartDb != null)
            { 
                shoppingCartDb.Count += shoppingCart.Count;
                _db.ShoppingCart.Update(shoppingCartDb);
             
            }
            
            else
            { 
                _db.ShoppingCart.Add(shoppingCart);
              
            }
            _db.Save();
            HttpContext.Session.SetInt32(SD.SessionCart,
                   _db.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            TempData["success"] = "Cart Updated successfully";
          
           
          return RedirectToAction(nameof(Index));

        }
    }
}
