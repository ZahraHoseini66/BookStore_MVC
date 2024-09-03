using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS
       // public IActionResult GetAll() {
       // List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties:"OrderDetail").ToList();
       //retutn
        }
        #endregion
    }
}
