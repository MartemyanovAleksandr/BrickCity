using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrickCity.Models.DB;

namespace BrickCity.Controllers
{
    public class HouseConsumptionController : Controller
    {
        private readonly BrickDB _context;

        public HouseConsumptionController(BrickDB context)
        {
            _context = context;
        }

        // GET: HouseConsumption
        public async Task<IActionResult> Index()
        {
            var brickDB = _context.HouseConsumption.Include(h => h.House);
            return View(await brickDB.ToListAsync());
        }

        // GET: HouseConsumption/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseConsumption = await _context.HouseConsumption
                .Include(h => h.House)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (houseConsumption == null)
            {
                return NotFound();
            }

            return View(houseConsumption);
        }

        // GET: HouseConsumption/Create
        public IActionResult Create()
        {
            ViewData["HouseId"] = new SelectList(_context.House, "Id", "Id");
            return View();
        }

        // POST: HouseConsumption/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HouseId,Date,Value,Weather")] HouseConsumption houseConsumption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(houseConsumption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HouseId"] = new SelectList(_context.House, "Id", "Id", houseConsumption.HouseId);
            return View(houseConsumption);
        }

        // GET: HouseConsumption/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseConsumption = await _context.HouseConsumption.FindAsync(id);
            if (houseConsumption == null)
            {
                return NotFound();
            }
            ViewData["HouseId"] = new SelectList(_context.House, "Id", "Id", houseConsumption.HouseId);
            return View(houseConsumption);
        }

        // POST: HouseConsumption/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HouseId,Date,Value,Weather")] HouseConsumption houseConsumption)
        {
            if (id != houseConsumption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(houseConsumption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseConsumptionExists(houseConsumption.Id))
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
            ViewData["HouseId"] = new SelectList(_context.House, "Id", "Id", houseConsumption.HouseId);
            return View(houseConsumption);
        }

        // GET: HouseConsumption/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseConsumption = await _context.HouseConsumption
                .Include(h => h.House)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (houseConsumption == null)
            {
                return NotFound();
            }

            return View(houseConsumption);
        }

        // POST: HouseConsumption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var houseConsumption = await _context.HouseConsumption.FindAsync(id);
            _context.HouseConsumption.Remove(houseConsumption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HouseConsumptionExists(int id)
        {
            return _context.HouseConsumption.Any(e => e.Id == id);
        }
    }
}
