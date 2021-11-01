using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjIngresso.Models
{
    public class Ingresso
    {
        public int Id { get; set; }

        [DisplayName("Preço")]
        public decimal Preco { get; set; }

        public string Setor { get; set; }

        public virtual Jogo Jogo { get; set; }

        [NotMapped]
        public virtual List<SelectListItem> Jogos { get; set; }

        [DisplayName("Nome da Imagem")]
        public string Imagem { get; set; }

        [NotMapped]
        [DisplayName("Imagem do Ingresso")]
        public IFormFile ImagemIngresso { get; set; }
    }
}