using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class ConsultaViewModel
    {
        public int IdConsulta { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaResuelta { get; set; }

    }
}