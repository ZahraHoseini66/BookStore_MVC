using BookStore.DataAccess;
using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.Models;
using BookStore.Models.ViewModels;
using BookStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult RoleManagement(string userId) {
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;
          RoleManagmentVM RoleVM = new RoleManagmentVM()
          {
              ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault( u=> u.Id == userId),
              RoleList = _db.Roles.Select(i => new SelectListItem {
              Text  = i.Name,
              Value = i.Name}),
              CompanyList = _db.Companies.Select(i => new SelectListItem{
              Text = i.Name,
              Value = i.Id.ToString()
              }),
          };
            RoleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u=>u.Id == RoleID).Name;
            return View(RoleVM);
        }
        [HttpPost]
        public IActionResult RoleManagement(RoleManagmentVM roleManagmentVM)
        {
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagmentVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
            if(RoleID != oldRole)
            {

                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagmentVM.ApplicationUser.Id);
                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }

            return RedirectToAction("Index");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult Getall()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u=> u.Company).ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Company == null)
                    user.Company = new Company() { Name = "" };

            }
            return Json(new { data = objUserList });

        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] String id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });

            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                
            }
            _db.SaveChanges();

            return Json(new { success = true, message = "Operation successful." });

        }
        #endregion
    }


}
