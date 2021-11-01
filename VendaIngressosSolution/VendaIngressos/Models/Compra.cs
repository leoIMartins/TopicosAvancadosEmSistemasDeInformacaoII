using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjIngresso.Models
{
    public class Compra
    {
        public int Id { get; set; }

        public virtual Torcedor Torcedor { get; set; }

        [NotMapped]
        public virtual List<SelectListItem> Torcedores { get; set; }

        public virtual Ingresso Ingresso { get; set; }

        [NotMapped]
        public virtual List<SelectListItem> Ingressos { get; set; }

    }
}
