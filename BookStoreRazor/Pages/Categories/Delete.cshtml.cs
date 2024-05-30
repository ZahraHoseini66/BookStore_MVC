using BookStoreRazor.Data;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStoreRazor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Category category{ get; set; }
        ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void OnGet(int? categoryId)
        {
            if (categoryId != null || categoryId != 0) {

                category = _db.Categories.Find(categoryId);
                }
           

        }
        public IActionResult OnPost() { 
        _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["Success"] = "Dlete Category successfully!"; 

            return RedirectToPage("/Categories/Index");
        }
    }
}
