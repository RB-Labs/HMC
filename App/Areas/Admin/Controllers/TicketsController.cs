using App.Data;
using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRole.Internal)]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .Include(x => x.Author)
                .ToListAsync();
            return View(tickets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories
                .Include(x => x.Ticket)
                .Include(x => x.User)
                .Where(x => x.Ticket.Id == id)
                .ToListAsync();

            if (ticketHistory.Count == 1)
            {
                var currentUser = _context.Users
                    .Single(x => x.Email == User.Identity.Name);
                TicketHistory initialRecord = new()
                {
                    Ticket = ticket,
                    User = currentUser,
                    Status = TicketStatus.Pending,
                    Date = DateTime.Now,
                    Description = "Ticket viewed"
                };
                _context.Add(initialRecord);
                _context.SaveChanges();
            }

            TicketDetailsViewModel model = new()
            {
                Id = ticket.Id,
                Name = ticket.Name,
                Text = ticket.Text,
                Author = ticket.Author.UserName,
                Date = ticket.Date,
                TicketHistory = ticketHistory
                    .Select(x => new TicketDetailsViewModel.History
                    {
                        User = x.User.UserName,
                        Description = x.Description,
                        Status = x.Status.ToString(),
                        Date = x.Date
                    })
                    .ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            TicketEditViewModel model = new()
            {
                Id = ticket.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NewStatus,Description")] TicketEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                var currentUser = _context.Users.Single(x => x.Email == User.Identity.Name);
                var ticketRecord = new TicketHistory()
                {
                    Ticket = ticket,
                    User = currentUser,
                    Status = (TicketStatus)Enum.Parse(typeof(TicketStatus), model.NewStatus),
                    Description = model.Description,
                    Date = DateTime.Now
                };
                _context.Update(ticketRecord);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id });
            }
            return View(model);
        }
    }
}
