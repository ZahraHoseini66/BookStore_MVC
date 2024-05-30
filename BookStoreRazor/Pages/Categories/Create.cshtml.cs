using BookStoreRazor.Data;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace BookStoreRazor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Category category { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            { 
            _db.Categories.Add(category);
            _db.SaveChanges();
                TempData["Success"] = "Created Category successfully!"; 
                return RedirectToPage("/Categories/Index");
            }
            TempData["Error"] = "Error!";

            return Page();
        }
       }
    }
