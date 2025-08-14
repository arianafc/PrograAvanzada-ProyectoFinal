using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class TourViewModel
    {
        public int ID_ACTIVIDAD { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public DateTime FECHA { get; set; }
        public string IMAGEN { get; set; }
        public int? TICKETS_ADQUIRIDOS { get; set; }
        public decimal? TOTAL { get; set; }
    }

}