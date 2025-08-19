using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class AnimalViewModel
    {
        public int ID_ANIMAL { get; set; }
        public string NOMBRE_ANIMAL { get; set; }
        public string RAZA { get; set; }
        public string ESPECIE { get; set; }
        public string IMAGEN { get; set; }
        public decimal MONTO_MENSUAL { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public string REFERENCIA { get; set; }
    }



}