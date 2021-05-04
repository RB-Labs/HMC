using App.Data;
using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = UserRole.Customer)]
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
                .Where(x => x.Author.Email == User.Identity.Name)
                .ToListAsync();
            return View(tickets);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = _context.Tickets
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketHistory = _context.TicketHistories
                .Include(x => x.Ticket)
                .Include(x => x.User)
                .Where(x => x.Ticket.Id == id)
                .ToList();

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Text")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _context.Users
                    .Single(x => x.Email == User.Identity.Name);
                ticket.Author = currentUser;
                ticket.Date = DateTime.Now;
                _context.Add(ticket);
                TicketHistory initialRecord = new()
                {
                    Ticket = ticket,
                    User = currentUser,
                    Status = TicketStatus.New,
                    Date = DateTime.Now,
                    Description = "Ticket created"
                };
                _context.Add(initialRecord);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }
    }
}
