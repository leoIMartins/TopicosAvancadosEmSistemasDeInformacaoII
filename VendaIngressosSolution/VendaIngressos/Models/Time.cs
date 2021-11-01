using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjIngresso.Models
{
    public class Time
    {
        public int Id { get; set; }

        [DisplayName("Time")]
        public string NomeTime { get; set; }

        [DisplayName("Técnico")]
        public string Tecnico { get; set; }
    }
}
