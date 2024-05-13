using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Models;

namespace ReservationSystem.Pages.Rooms{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IList<Room> Rooms { get;set; }
        
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }


        public async Task OnGetAsync()
        {
            Rooms = await _context.Rooms.OrderBy(r => r.RoomName).ToListAsync();
        }
    }
}