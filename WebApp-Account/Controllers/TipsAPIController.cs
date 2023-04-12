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
    [ApiController]
    [Route("api/Tips")]
    public class TipsAPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET Tips
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return _context.Tip != null ?
                        Ok(await _context.Tip.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Tip'  is null.");
        }

        // GET Tips/SearchText
        [HttpGet("Search/{SearchText}")]
        public async Task<IActionResult> SearchResult(String SearchText)
        {
            var TipsList = await _context.Tip.Where(t => t.Name.Contains(SearchText)).ToListAsync();
            return Ok(TipsList);
        }

        // GET: Tips/Details
        [Authorize]
        [HttpGet("Details/{id}",Name = "TipDetails")]
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

            return Ok(tip);
        }

        //GET Take/PARAMITER Crate
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Id,Name,Detail")] Tip tip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tip);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("TipDetails", new { Id = tip.Id }, tip);
                //return RedirectToAction(nameof(Index)); ถ้าใช้อันเดิมจะส่งกลับ 200
            }
            return BadRequest("Invalid model object");
        }
        

        // GET: Tips/Edit
        [HttpPut("Edit/{id}")] //return 204,200,201 (if created)
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
                        //log error here
                        return StatusCode(500);
                    }
                }
                return NoContent();//204
            }
            return BadRequest("Invaild model object");
        }

        //GET ID/DELETE
        [HttpDelete("Delete/{id}")]
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
            return Ok();
        }

        // Tip/HELP
        private bool TipExists(int id)
        {
            return (_context.Tip?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
