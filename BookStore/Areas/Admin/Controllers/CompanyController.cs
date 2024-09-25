using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using BookStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id != null && id != 0)
            {
                company = _unitOfWork.Company.Get(c => c.Id == id);

            }
            return View(company);
        }
            [HttpPost]
            public IActionResult Upsert(Company company)
            {
                if (company.Id != 0)
                {
                    _unitOfWork.Company.Update(company);
                }
                else
                {
                    _unitOfWork.Company.Add(company);
                }
            _unitOfWork.Save();
            TempData["Success"] = "Company Created Successfully!";
            return RedirectToAction("Index");
             }
            
        
        #region API CALLS
        [HttpGet]
        public IActionResult Getall()
        {
            List<Company> companyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companyList });

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            Company company = _unitOfWork.Company.Get(c => c.Id == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Company successfully." });

        }
        #endregion
    }


}
