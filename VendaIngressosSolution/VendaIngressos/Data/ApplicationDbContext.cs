using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ProjIngresso.Models;

namespace VendaIngressos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjIngresso.Models.Torcedor> Torcedor { get; set; }
        public DbSet<ProjIngresso.Models.Time> Time { get; set; }
        public DbSet<ProjIngresso.Models.Jogo> Jogo { get; set; }
        public DbSet<ProjIngresso.Models.Ingresso> Ingresso { get; set; }
        public DbSet<ProjIngresso.Models.Compra> Compra { get; set; }
    }
}
