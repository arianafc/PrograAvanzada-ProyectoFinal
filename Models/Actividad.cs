using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Actividad
    {
        public String Nombre { get; set; }
        public int IdActividad { get; set; }

        public String Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public int TicketsDisponibles { get; set; }

        public int TicketsVendidos { get; set; }

        public int IdEstado { get; set; }

        public decimal PrecioBoleto { get; set; }

        public String Imagen { get; set; }

        public String Tipo { get; set; }


    }
}