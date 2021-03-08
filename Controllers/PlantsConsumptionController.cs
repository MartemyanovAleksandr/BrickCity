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
    public class PlantsConsumptionController : Controller
    {
        private readonly BrickDB _context;

        public PlantsConsumptionController(BrickDB context)
        {
            _context = context;
        }

        // GET: PlantsConsumption
        public async Task<IActionResult> Index()
        {
            var brickDB = _context.PlantsConsumption.Include(p => p.Plant);
            return View(await brickDB.ToListAsync());
        }

        // GET: PlantsConsumption/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantsConsumption = await _context.PlantsConsumption
                .Include(p => p.Plant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plantsConsumption == null)
            {
                return NotFound();
            }

            return View(plantsConsumption);
        }

        // GET: PlantsConsumption/Create
        public IActionResult Create()
        {
            ViewData["PlantId"] = new SelectList(_context.Plant, "Id", "Id");
            return View();
        }

        // POST: PlantsConsumption/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlantId,Date,Value,Price")] PlantsConsumption plantsConsumption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantsConsumption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlantId"] = new SelectList(_context.Plant, "Id", "Id", plantsConsumption.PlantId);
            return View(plantsConsumption);
        }

        // GET: PlantsConsumption/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantsConsumption = await _context.PlantsConsumption.FindAsync(id);
            if (plantsConsumption == null)
            {
                return NotFound();
            }
            ViewData["PlantId"] = new SelectList(_context.Plant, "Id", "Id", plantsConsumption.PlantId);
            return View(plantsConsumption);
        }

        // POST: PlantsConsumption/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlantId,Date,Value,Price")] PlantsConsumption plantsConsumption)
        {
            if (id != plantsConsumption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantsConsumption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantsConsumptionExists(plantsConsumption.Id))
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
            ViewData["PlantId"] = new SelectList(_context.Plant, "Id", "Id", plantsConsumption.PlantId);
            return View(plantsConsumption);
        }

        // GET: PlantsConsumption/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantsConsumption = await _context.PlantsConsumption
                .Include(p => p.Plant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plantsConsumption == null)
            {
                return NotFound();
            }

            return View(plantsConsumption);
        }

        // POST: PlantsConsumption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantsConsumption = await _context.PlantsConsumption.FindAsync(id);
            _context.PlantsConsumption.Remove(plantsConsumption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantsConsumptionExists(int id)
        {
            return _context.PlantsConsumption.Any(e => e.Id == id);
        }
    }
}
