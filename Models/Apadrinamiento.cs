using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Models
{
    public class Apadrinamiento
    {
        public int IdApadrinamiento { get; set; }
        public decimal MontoMensual { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstado { get; set; }
        public int IdAnimal { get; set; }
        public int IdMetodo { get; set; }
        public string Referencia { get; set; }
    }
}