using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using ProjIngresso.Models;
using VendaIngressos.Data;

namespace VendaIngressos.Controllers
{
    [Authorize]
    public class IngressosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IngressosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Ingressos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ingresso.Include(j => j.Jogo).Include(j => j.Jogo.TimeA).Include(j => j.Jogo.TimeB).ToListAsync());
        }

        // GET: Ingressos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingresso = await _context.Ingresso.Include(j => j.Jogo).Include(j => j.Jogo.TimeA).Include(j => j.Jogo.TimeB)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingresso == null)
            {
                return NotFound();
            }

            return View(ingresso);
        }

        // GET: Ingressos/Create
        public IActionResult Create()
        {
            var i = new Ingresso();
            var jogos = _context.Jogo.Include(a => a.TimeA).Include(b => b.TimeB).ToList();

            i.Jogos = new List<SelectListItem>();

            foreach (var j in jogos)
            {
                i.Jogos.Add(new SelectListItem { Text = j.TimeA.NomeTime + " x " + j.TimeB.NomeTime, Value = j.Id.ToString() });
            }

            return View(i);
        }

        // POST: Ingressos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Preco,Setor,ImagemIngresso")] Ingresso ingresso)
        {
            int _jogoId = int.Parse(Request.Form["Jogo"].ToString());
            var jogo = _context.Jogo.FirstOrDefault(j => j.Id == _jogoId);
            ingresso.Jogo = jogo;
            //Regra de negócio do status do In
            ingresso.Status = "Disponível";
            //Fim da regra de negócio

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(ingresso.ImagemIngresso.FileName);
                string extension = Path.GetExtension(ingresso.ImagemIngresso.FileName);
                ingresso.Imagem = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ingresso.ImagemIngresso.CopyToAsync(fileStream);
                }

                _context.Add(ingresso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingresso);
        }

        // GET: Ingressos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingresso = _context.Ingresso.Include(j => j.Jogo).First(i => i.Id == id);
            var jogos = _context.Jogo.Include(a => a.TimeA).Include(b => b.TimeB).ToList();
            ingresso.Jogos = new List<SelectListItem>();

            foreach (var j in jogos)
            {
                ingresso.Jogos.Add(new SelectListItem { Text = j.TimeA.NomeTime + " x " + j.TimeB.NomeTime, Value = j.Id.ToString() });
            }

            //var ingresso = await _context.Ingresso.FindAsync(id);
            if (ingresso == null)
            {
                return NotFound();
            }
            return View(ingresso);
        }

        // POST: Ingressos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Preco,Setor,Imagem")] Ingresso ingresso)
        {
            if (id != ingresso.Id)
            {
                return NotFound();
            }

            int _jogoId = int.Parse(Request.Form["Jogo"].ToString());
            var jogo = _context.Jogo.FirstOrDefault(j => j.Id == _jogoId);
            ingresso.Jogo = jogo;

            if (ModelState.IsValid)
            {
                try
                {
                    //Verifiingresso se o nome da imagem mudou:
                    var ingressoCompare = _context.Ingresso.Find(ingresso.Id);

                    ingresso.Imagem = (ingresso.ImagemIngresso == null) ? "" : ingresso.ImagemIngresso.FileName;

                    if (!CompareFileName(ingressoCompare.Imagem, ingresso.Imagem))
                    {
                        //Remover Imagem anterior
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", ingressoCompare.Imagem);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        //Incluir nova
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(ingresso.ImagemIngresso.FileName);
                        string extension = Path.GetExtension(ingresso.ImagemIngresso.FileName);
                        ingresso.Imagem = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/image", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await ingresso.ImagemIngresso.CopyToAsync(fileStream);
                        }
                    }

                    ingressoCompare.Preco = ingresso.Preco;
                    ingressoCompare.Setor = ingresso.Setor;
                    ingressoCompare.Jogo = ingresso.Jogo;
                    ingressoCompare.Imagem = string.IsNullOrEmpty(ingresso.Imagem) ? ingressoCompare.Imagem : ingresso.Imagem;

                    _context.Update(ingressoCompare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngressoExists(ingresso.Id))
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
            return View(ingresso);
        }

        private bool CompareFileName(string name, string newName)
        {
            //Se não foi selecionada uma imagem nova fica a antiga. 
            if (string.IsNullOrEmpty(newName))
                return true;

            if (string.IsNullOrEmpty(name))
                return false;

            //extensão do arquivo
            var validateName = name.Split('.');
            var validateNewName = newName.Split('.');

            if (validateName[1] != validateNewName[1])
                return false;

            //Remover a data gerada
            string nameOld = validateName[0].Replace(validateName[0]
                                            .Substring(validateName[0].Length - 9, 9), "");

            if (newName == nameOld)
                return true;

            return false;
        }

        // GET: Ingressos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingresso = await _context.Ingresso.Include(a => a.Jogo.TimeA).Include(b => b.Jogo.TimeB)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingresso == null)
            {
                return NotFound();
            }

            return View(ingresso);
        }

        // POST: Ingressos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingresso = await _context.Ingresso.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", ingresso.Imagem);

            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            _context.Ingresso.Remove(ingresso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngressoExists(int id)
        {
            return _context.Ingresso.Any(e => e.Id == id);
        }
    }
}
