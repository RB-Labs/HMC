using App.Data;
using App.Models;
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
                .FirstOrDefault(x => x.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
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
                ticket.Author = _context.Users
                    .Single(x => x.Email == User.Identity.Name);
                ticket.Date = DateTime.Now;
                _context.Add(ticket);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }
    }
}
