using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjWebAPIDotNetCore.Model;

namespace ProjWebAPIDotNetCore.Data
{
    public class ProjWebAPIDotNetCoreContext : DbContext
    {
        public ProjWebAPIDotNetCoreContext (DbContextOptions<ProjWebAPIDotNetCoreContext> options)
            : base(options)
        {
        }

        public DbSet<ProjWebAPIDotNetCore.Model.Cliente> Cliente { get; set; }

        public DbSet<ProjWebAPIDotNetCore.Model.Padaria> Padaria { get; set; }

        public DbSet<ProjWebAPIDotNetCore.Model.Produto> Produto { get; set; }
    }
}
