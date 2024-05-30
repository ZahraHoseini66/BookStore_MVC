using BookStoreRazor.Data;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreRazor.Pages.Categories
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Category category { get; set; }
        ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? categoryId)
        {
            if (categoryId != null || categoryId != 0)
            {

                category = _db.Categories.Find(categoryId);
            }
            

        }
        public IActionResult OnPost() {
       if(ModelState.IsValid &&  category != null)
            { 
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["Success"] = "Updated Category successfully!"; 

                return RedirectToPage("/Categories/Index");
            
            }
            TempData["Error"] = "Error!";
            return Page();
        }
    }
}
