using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Data;
using ReservationSystem.Models;

namespace ReservationSystem.Pages.Rooms{
    public class AddModel : PageModel
    {
        private readonly AppDbContext _context;
        
        public AddModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index"); // Redirect to the index page after adding
        }
    }
}