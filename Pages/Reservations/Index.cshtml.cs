using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationSystem.Models;
using ReservationSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Reservation> Reservations { get; set; }
        public IDictionary<int, string> RoomNames { get; set; }
        public IDictionary<string, string> UserNames { get; set; }
        public SelectList RoomList { get; set; }
        public SelectList CapacityList { get; set; }
        public string CurrentFilterRoom { get; set; }
        public string CurrentFilterCapacity { get; set; }
        public DateTime? CurrentFilterStartDate { get; set; }
        public DateTime CurrentFilterEndDate => CurrentFilterStartDate?.AddDays(6) ?? DateTime.Now.AddDays(6);

        public async Task OnGetAsync(string room, string capacity, DateTime? startDate)
        {
            CurrentFilterRoom = room;
            CurrentFilterCapacity = capacity;
            CurrentFilterStartDate = startDate ?? DateTime.Now;

            var endDate = CurrentFilterEndDate;

            var query = _context.Reservations.AsQueryable();

            if (CurrentFilterStartDate.HasValue && endDate != null)
            {
                query = query.Where(r => r.ReservationDate >= CurrentFilterStartDate && r.ReservationDate <= endDate);
            }

            if (!string.IsNullOrEmpty(room))
            {
                var roomEntity = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomName == room);
                if (roomEntity != null)
                {
                    query = query.Where(r => r.RoomId == roomEntity.Id);
                }
            }

            if (!string.IsNullOrEmpty(capacity))
            {
                int cap = int.Parse(capacity);
                var roomIds = await _context.Rooms.Where(r => r.Capacity == cap).Select(r => r.Id).ToListAsync();
                query = query.Where(r => roomIds.Contains(r.RoomId));
            }

            Reservations = await query.ToListAsync();

            RoomNames = await _context.Rooms.ToDictionaryAsync(r => r.Id, r => r.RoomName);
            UserNames = await _context.Users.ToDictionaryAsync(u => u.Id, u => u.Email);
            RoomList = new SelectList(await _context.Rooms.Select(r => r.RoomName).Distinct().ToListAsync());
            CapacityList = new SelectList(await _context.Rooms.Select(r => r.Capacity.ToString()).Distinct().ToListAsync());
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reservation with ID {id} has been deleted.");

            return RedirectToPage();
        }
    }
}
