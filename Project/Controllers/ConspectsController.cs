using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ConspectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConspectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Conspects
        public async Task<IActionResult> Index()
        {
              return _context.Conspect != null ? 
                          View(await _context.Conspect.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Conspect'  is null.");
        }

        // GET: Conspects/ShowSearchBar
        public async Task<IActionResult> ShowSearchBar()
        {
            return View();
        }

        // PoST: Conspects/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            return View("Index", await _context.Conspect.Where(j => j.name.Contains(searchPhrase)).ToListAsync()); // Filters out search terms that do not match
        }

        // GET: Conspects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Conspect == null)
            {
                return NotFound();
            }

            var conspect = await _context.Conspect
                .FirstOrDefaultAsync(m => m.id == id);
            if (conspect == null)
            {
                return NotFound();
            }

            return View(conspect);
        }

        // GET: Conspects/Create
        [Authorize] // This makes it so that you need to be logged in to use this
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conspects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // This makes it so that you need to be logged in to use this
        public async Task<IActionResult> Create([Bind("id,name,text")] Conspect conspect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conspect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conspect);
        }

        // GET: Conspects/Edit/5
        [Authorize] // This makes it so that you need to be logged in to use this
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Conspect == null)
            {
                return NotFound();
            }

            var conspect = await _context.Conspect.FindAsync(id);
            if (conspect == null)
            {
                return NotFound();
            }
            return View(conspect);
        }

        // POST: Conspects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,text")] Conspect conspect)
        {
            if (id != conspect.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conspect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConspectExists(conspect.id))
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
            return View(conspect);
        }

        // GET: Conspects/Delete/5
        [Authorize] // This makes it so that you need to be logged in to use this
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Conspect == null)
            {
                return NotFound();
            }

            var conspect = await _context.Conspect
                .FirstOrDefaultAsync(m => m.id == id);
            if (conspect == null)
            {
                return NotFound();
            }

            return View(conspect);
        }

        // POST: Conspects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize] // This makes it so that you need to be logged in to use this
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Conspect == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Conspect'  is null.");
            }
            var conspect = await _context.Conspect.FindAsync(id);
            if (conspect != null)
            {
                _context.Conspect.Remove(conspect);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConspectExists(int id)
        {
          return (_context.Conspect?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
