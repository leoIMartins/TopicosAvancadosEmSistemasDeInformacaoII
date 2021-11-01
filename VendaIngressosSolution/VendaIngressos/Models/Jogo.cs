using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjIngresso.Models
{
    public class Jogo
    {
        public int Id { get; set; }

        [DisplayName("Time A")]
        public virtual Time TimeA { get; set; }
        
        [NotMapped]
        public virtual List<SelectListItem> TimesA { get; set; }

        [DisplayName("Time B")]
        public virtual Time TimeB { get; set; }
        
        [NotMapped]
        public virtual List<SelectListItem> TimesB { get; set; }

        [DisplayName("Estádio")]
        public string NomeEstadio { get; set; }

        [DisplayName("Data")]
        public DateTime DataJogo { get; set; }
    }
}
