using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Animal
    {
            public int ID_ANIMAL { get; set; }        
            public string Nombre { get; set; }
            public int ID_raza { get; set; }
            public string Nombre_Raza { get; set; }     
            public DateTime Fecha_ingreso { get; set; }
            public DateTime Fecha_baja { get; set; }
            public DateTime Fecha_nacimiento { get; set; }
            public int ID_estado { get; set; }
            public string Estado { get; set; }         
            public int ID_salud { get; set; }
            public string Salud{ get; set; }           
            public string Imagen { get; set; }
            public string Historia { get; set; }
            public string Necesidad { get; set; }
    }
}