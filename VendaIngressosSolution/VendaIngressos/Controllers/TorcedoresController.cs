using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjIngresso.Models;
using VendaIngressos.Data;

namespace VendaIngressos.Controllers
{
    [Authorize]
    public class TorcedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TorcedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Torcedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Torcedor.ToListAsync());
        }

        // GET: Torcedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torcedor = await _context.Torcedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (torcedor == null)
            {
                return NotFound();
            }

            return View(torcedor);
        }

        // GET: Torcedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Torcedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CPF,Nome,Idade")] Torcedor torcedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(torcedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(torcedor);
        }

        // GET: Torcedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torcedor = await _context.Torcedor.FindAsync(id);
            if (torcedor == null)
            {
                return NotFound();
            }
            return View(torcedor);
        }

        // POST: Torcedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CPF,Nome,Idade")] Torcedor torcedor)
        {
            if (id != torcedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(torcedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TorcedorExists(torcedor.Id))
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
            return View(torcedor);
        }

        // GET: Torcedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torcedor = await _context.Torcedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (torcedor == null)
            {
                return NotFound();
            }

            return View(torcedor);
        }

        // POST: Torcedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var torcedor = await _context.Torcedor.FindAsync(id);
            _context.Torcedor.Remove(torcedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TorcedorExists(int id)
        {
            return _context.Torcedor.Any(e => e.Id == id);
        }
    }
}
