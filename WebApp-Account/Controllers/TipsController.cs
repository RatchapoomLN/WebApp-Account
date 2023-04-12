using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp_Account.Data;
using WebApp_Account.Models;

namespace WebApp_Account.Controllers
{
    [Authorize]
    public class TipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tips
        public async Task<IActionResult> Index()
        {
              return _context.Tip != null ? 
                          View(await _context.Tip.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tip'  is null.");
        }

        // GET: Tips
        public async Task<IActionResult> SearchForm()
        {
            return View();
        }

        // GET: Tips
        public async Task<IActionResult> SearchResult(String SearchText)
        {
              return _context.Tip != null ? 
                          View("Index",await _context.Tip.Where(t=>t.Name.Contains(SearchText)).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tip'  is null.");
        }

        // GET: Tips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tip == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // GET: Tips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Detail")] Tip tip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tip);
        }

        // GET: Tips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tip == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip.FindAsync(id);
            if (tip == null)
            {
                return NotFound();
            }
            return View(tip);
        }

        // POST: Tips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Detail")] Tip tip)
        {
            if (id != tip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipExists(tip.Id))
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
            return View(tip);
        }

        // GET: Tips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tip == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // POST: Tips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tip == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tip'  is null.");
            }
            var tip = await _context.Tip.FindAsync(id);
            if (tip != null)
            {
                _context.Tip.Remove(tip);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipExists(int id)
        {
          return (_context.Tip?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
