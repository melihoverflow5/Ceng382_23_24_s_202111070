using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ReservationSystem.Models;
using ReservationSystem.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReservationSystem.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(AppDbContext context, ILogger<CreateModel> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public SelectList RoomList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RoomList = new SelectList(await _context.Rooms.ToListAsync(), "Id", "RoomName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Reservation.UserId");
            if (!ModelState.IsValid)
            {
                RoomList = new SelectList(await _context.Rooms.ToListAsync(), "Id", "RoomName");
                return Page();
            }

            if (Reservation.ReservationDate <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Reservation date cannot be in the past.");
                RoomList = new SelectList(await _context.Rooms.ToListAsync(), "Id", "RoomName");
                return Page();
            }

            var conflictingReservation = await _context.Reservations
                .Where(r => r.RoomId == Reservation.RoomId && r.ReservationDate == Reservation.ReservationDate)
                .FirstOrDefaultAsync();

            if (conflictingReservation != null)
            {
                ModelState.AddModelError(string.Empty, "There is already a reservation for this room at the selected time.");
                RoomList = new SelectList(await _context.Rooms.ToListAsync(), "Id", "RoomName");
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var newReservation = new Reservation
            {
                ReservationDate = Reservation.ReservationDate,
                RoomId = Reservation.RoomId,
                UserId = user.Id
            };

            _logger.LogInformation($"User {user.UserName} created a reservation on {DateTime.Now}");

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
