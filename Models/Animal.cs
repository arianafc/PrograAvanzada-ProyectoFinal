using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Animal
    {
        public int IdAnimal { get; set; }
        public string Nombre { get; set; }
        public int IdRaza { get; set; }
        public string NombreRaza { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaBaja { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdSalud { get; set; }
        public string Salud { get; set; }
        public string Imagen { get; set; }
        public string Historia { get; set; }
        public string Necesidad { get; set; }

    }
}