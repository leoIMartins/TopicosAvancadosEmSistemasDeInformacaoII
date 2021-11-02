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
    public class JogosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JogosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jogos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jogo.Include(a => a.TimeA).Include(b => b.TimeB).ToListAsync());
        }

        // GET: Jogos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogo = await _context.Jogo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jogo == null)
            {
                return NotFound();
            }

            return View(jogo);
        }

        // GET: Jogos/Create
        public IActionResult Create()
        {
            var j = new Jogo();
            var timesA = _context.Time.ToList();
            var timesB = _context.Time.ToList();

            j.TimesA = new List<SelectListItem>();
            j.TimesB = new List<SelectListItem>();

            foreach (var a in timesA)
            {
                j.TimesA.Add(new SelectListItem { Text = a.NomeTime, Value = a.Id.ToString() });
            }

            foreach (var b in timesB)
            {
                j.TimesB.Add(new SelectListItem { Text = b.NomeTime, Value = b.Id.ToString() });
            }

            return View(j);
        }

        // POST: Jogos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeEstadio,DataJogo")] Jogo jogo)
        {
            int _timeAId = int.Parse(Request.Form["TimeA"].ToString());
            var timeA = _context.Time.FirstOrDefault(t => t.Id == _timeAId);
            jogo.TimeA = timeA;

            int _timeBId = int.Parse(Request.Form["TimeB"].ToString());
            var timeB = _context.Time.FirstOrDefault(t => t.Id == _timeBId);
            jogo.TimeB = timeB;

            if (ModelState.IsValid)
            {
                _context.Add(jogo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jogo);
        }

        // GET: Jogos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogo = _context.Jogo.Include(t => t.TimeA).First(e => e.Id == id);
            var timesA = _context.Time.ToList();
            jogo.TimesA = new List<SelectListItem>();

            foreach (var a in timesA)
            {
                jogo.TimesA.Add(new SelectListItem { Text = a.NomeTime, Value = a.Id.ToString() });
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////

            jogo = _context.Jogo.Include(t => t.TimeB).First(e => e.Id == id);
            var timesB = _context.Time.ToList();
            jogo.TimesB = new List<SelectListItem>();

            foreach (var b in timesB)
            {
                jogo.TimesB.Add(new SelectListItem { Text = b.NomeTime, Value = b.Id.ToString() });
            }

            //var jogo = await _context.Jogo.FindAsync(id);
            if (jogo == null)
            {
                return NotFound();
            }
            return View(jogo);
        }

        // POST: Jogos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeEstadio,DataJogo")] Jogo jogo)
        {
            if (id != jogo.Id)
            {
                return NotFound();
            }

            int _timeAId = int.Parse(Request.Form["TimeA"].ToString());
            var timeA = _context.Time.FirstOrDefault(a => a.Id == _timeAId);
            jogo.TimeA = timeA;

            int _timeBId = int.Parse(Request.Form["TimeB"].ToString());
            var timeB = _context.Time.FirstOrDefault(b => b.Id == _timeBId);
            jogo.TimeB = timeB;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jogo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JogoExists(jogo.Id))
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
            return View(jogo);
        }

        // GET: Jogos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogo = await _context.Jogo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jogo == null)
            {
                return NotFound();
            }

            return View(jogo);
        }

        // POST: Jogos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jogo = await _context.Jogo.FindAsync(id);
            _context.Jogo.Remove(jogo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JogoExists(int id)
        {
            return _context.Jogo.Any(e => e.Id == id);
        }
    }
}
