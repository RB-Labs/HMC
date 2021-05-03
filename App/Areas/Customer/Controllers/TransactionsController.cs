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
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(x => x.Customer)
                .Where(x => x.Customer.Email == User.Identity.Name)
                .ToListAsync();
            ViewBag.Balance = transactions.Sum(x => x.Value);
            return View(transactions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Value,Description")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Customer = _context.Users
                    .Single(x => x.Email == User.Identity.Name);
                transaction.Date = DateTime.Now;
                _context.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }
    }
}
