using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class DonacionViewModel
    {
        public int DonacionId { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public decimal Monto { get; set; }
        public System.DateTime Fecha { get; set; }
    }
}