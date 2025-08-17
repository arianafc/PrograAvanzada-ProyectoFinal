using System.Collections.Generic;
using System.Web.Mvc;
using ProyectoFinal.EF;

namespace ProyectoFinal.Models
{
    public class GestionAnimalesModel
    {
        public Animal NuevoAnimal { get; set; }
        public List<VisualizarAnimalesSP_Result> ListaAnimales { get; set; }

        public List<SelectListItem> ListaRazas { get; set; }
        public List<SelectListItem> ListaSalud { get; set; }

    }
}
