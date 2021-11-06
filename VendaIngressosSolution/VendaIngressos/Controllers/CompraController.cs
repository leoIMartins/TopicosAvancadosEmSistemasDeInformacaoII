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
    public class CompraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compra
        public async Task<IActionResult> Index()
        {
            return View(await _context.Compra.Include(t => t.Torcedor).Include(i => i.Ingresso)
                .Include(j => j.Ingresso.Jogo).Include(a => a.Ingresso.Jogo.TimeA)
                .Include(b => b.Ingresso.Jogo.TimeB).ToListAsync());
        }

        // GET: Compra/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.Include(t => t.Torcedor)
                .Include(j => j.Ingresso.Jogo).Include(j => j.Ingresso.Jogo.TimeA)
                .Include(j => j.Ingresso.Jogo.TimeB).FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compra/Create
        public IActionResult Create()
        {
            var c = new Compra();
            var torcedores = _context.Torcedor.ToList();
            var ingressos = _context.Ingresso.Include(j => j.Jogo)
                .Include(j => j.Jogo.TimeA).Include(j => j.Jogo.TimeB).ToList();

            c.Torcedores = new List<SelectListItem>();
            c.Ingressos = new List<SelectListItem>();

            foreach (var t in torcedores)
            {
                if(t.Idade >= 18)
                    c.Torcedores.Add(new SelectListItem { Text = t.Nome, Value = t.Id.ToString() });
            }

            foreach (var i in ingressos)
            {
                if (i.Status == "Disponível")
                {
                    c.Ingressos.Add(new SelectListItem { Text = "R$" + i.Preco + " - " 
                    + i.Jogo.TimeA.NomeTime + " x " + i.Jogo.TimeB.NomeTime + " - "
                    + i.Jogo.NomeEstadio + " - " + i.Setor, Value = i.Id.ToString() });
                }
            }

            return View(c);
        }

        // POST: Compra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Compra compra)
        {
            int _torcedorId = int.Parse(Request.Form["Torcedor"].ToString());
            var torcedor = _context.Torcedor.FirstOrDefault(t => t.Id == _torcedorId);
            compra.Torcedor = torcedor;

            int _ingressoId = int.Parse(Request.Form["Ingresso"].ToString());
            var ingresso = _context.Ingresso.FirstOrDefault(i => i.Id == _ingressoId);
            //Regra de negócio alterar status do ingresso para "Vendido"
            ingresso.Status = "Vendido";
            //Fim da regra de negócio
            compra.Ingresso = ingresso;

            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compra);
        }

        // GET: Compra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = _context.Compra.Include(t => t.Torcedor).First(e => e.Id == id);
            var torcedores = _context.Torcedor.ToList();
            compra.Torcedores = new List<SelectListItem>();

            foreach (var t in torcedores)
            {
                compra.Torcedores.Add(new SelectListItem { Text = t.Nome, Value = t.Id.ToString() });
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////

            compra = _context.Compra.Include(i => i.Ingresso).First(e => e.Id == id);
            var ingressos = _context.Ingresso.Include(j => j.Jogo).Include(j => j.Jogo.TimeA)
                .Include(j => j.Jogo.TimeB).ToList();
            compra.Ingressos = new List<SelectListItem>();

            foreach (var i in ingressos)
            {
                compra.Ingressos.Add(new SelectListItem { Text = "R$" + i.Preco + " - "
                    + i.Jogo.TimeA.NomeTime + " x " + i.Jogo.TimeB.NomeTime + " - "
                    + i.Jogo.NomeEstadio + " - " + i.Setor, Value = i.Id.ToString() });
            }

            //var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            int _torcedorId = int.Parse(Request.Form["Torcedor"].ToString());
            var torcedor = _context.Torcedor.FirstOrDefault(t => t.Id == _torcedorId);
            compra.Torcedor = torcedor;

            int _ingressoId = int.Parse(Request.Form["Ingresso"].ToString());
            var ingresso = _context.Ingresso.FirstOrDefault(i => i.Id == _ingressoId);
            compra.Ingresso = ingresso;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.Id))
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
            return View(compra);
        }

        // GET: Compra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.Include(t => t.Torcedor)
                .Include(j => j.Ingresso.Jogo).Include(j => j.Ingresso.Jogo.TimeA)
                .Include(j => j.Ingresso.Jogo.TimeB)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.Id == id);
        }
    }
}
