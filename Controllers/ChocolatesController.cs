using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6_nirmal.Data;
using Lab6_nirmal.Models;

//Add this import or the [Authorize] Wont work
using Microsoft.AspNetCore.Authorization;

namespace Lab6_nirmal.Controllers
{
    public class ChocolatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChocolatesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Chocolates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chocolates.ToListAsync());
        }
        [Authorize]
        // GET: Chocolates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chocolate = await _context.Chocolates
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chocolate == null)
            {
                return NotFound();
            }

            return View(chocolate);
        }

        // GET: Chocolates/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chocolates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,chocolateMake,chocolateName,expireYear,price")] Chocolate chocolate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chocolate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chocolate);
        }

        // GET: Chocolates/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chocolate = await _context.Chocolates.FindAsync(id);
            if (chocolate == null)
            {
                return NotFound();
            }
            return View(chocolate);
        }

        // POST: Chocolates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,chocolateMake,chocolateName,expireYear,price")] Chocolate chocolate)
        {
            if (id != chocolate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chocolate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChocolateExists(chocolate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chocolate);
        }

        // GET: Chocolates/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chocolate = await _context.Chocolates
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chocolate == null)
            {
                return NotFound();
            }

            return View(chocolate);
        }

        // POST: Chocolates/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chocolate = await _context.Chocolates.FindAsync(id);
            _context.Chocolates.Remove(chocolate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChocolateExists(int id)
        {
            return _context.Chocolates.Any(e => e.ID == id);
        }
        [Authorize]
        public async Task<ActionResult> Searching()
        {
            return View();
        }
        public async Task<IActionResult> ShowSearchResults(string SearchChocolate)
        {
            return View("Index", await _context.Chocolates.Where(s => s.chocolateMake.Contains(SearchChocolate)).ToListAsync());
        }
    }
}
