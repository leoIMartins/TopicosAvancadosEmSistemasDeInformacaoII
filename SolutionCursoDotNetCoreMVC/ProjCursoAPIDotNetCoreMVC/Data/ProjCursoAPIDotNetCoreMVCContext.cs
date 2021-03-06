using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CursoWebCoreMVC.Models;

namespace ProjCursoAPIDotNetCoreMVC.Data
{
    public class ProjCursoAPIDotNetCoreMVCContext : DbContext
    {
        public ProjCursoAPIDotNetCoreMVCContext (DbContextOptions<ProjCursoAPIDotNetCoreMVCContext> options)
            : base(options)
        {
        }

        public DbSet<CursoWebCoreMVC.Models.Aluno> Aluno { get; set; }

        public DbSet<CursoWebCoreMVC.Models.Disciplina> Disciplina { get; set; }

        public DbSet<CursoWebCoreMVC.Models.Curso> Curso { get; set; }
    }
}
