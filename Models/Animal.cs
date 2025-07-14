using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Animal
    {
            public int ID_ANIMAL { get; set; }          // nuevo
            public string Nombre { get; set; }
            public int ID_raza { get; set; }
            public string NOMBRE_RAZA { get; set; }     // nuevo
            public DateTime Fecha_ingreso { get; set; }
            public DateTime Fecha_baja { get; set; }
            public DateTime Fecha_nacimiento { get; set; }
            public int ID_estado { get; set; }
            public string ESTADO { get; set; }          // nuevo
            public int ID_salud { get; set; }
            public string SALUD { get; set; }           // nuevo
            public string Imagen { get; set; }
            public string Historia { get; set; }
            public string Necesidad { get; set; }
    }
}