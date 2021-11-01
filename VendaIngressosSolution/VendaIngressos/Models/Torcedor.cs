using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjIngresso.Models
{
    public class Torcedor
    {
        public int Id { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
    }
}
