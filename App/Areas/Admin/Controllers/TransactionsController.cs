using App.Data;
using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRole.Internal)]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(x => x.Customer)
                .ToListAsync();
            return View(transactions);
        }

        public async Task<IActionResult> Create()
        {
            var model = new TransactionViewModel(await GetCustomers());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Value,Description,CustomerId")] TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ToTransaction(model));
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private async Task<IList<User>> GetCustomers()
        {
            return await _userManager.GetUsersInRoleAsync(UserRole.Customer);
        }

        private Transaction ToTransaction(TransactionViewModel model)
        {
            var customer = _context.Users.Single(x => x.Id == model.CustomerId);
            return new Transaction
            {
                Customer = customer,
                Value = Convert.ToDouble(model.Value),
                Description = model.Description,
                Date = DateTime.Now
            };
        }
    }
}
