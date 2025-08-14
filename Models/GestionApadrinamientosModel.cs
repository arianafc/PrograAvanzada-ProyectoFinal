using System.Collections.Generic;
using System.Web.Mvc;
using ProyectoFinal.EF;

namespace ProyectoFinal.Models
{
    public class GestionApadrinamientosModel
    {
        public Apadrinamiento NuevoApadrinamiento { get; set; }
        public List<VisualizarApadrinamientosSP_Result> ListaApadrinamientos { get; set; }
        public List<SelectListItem> ListaUsuarios { get; set; }
        public List<SelectListItem> ListaAnimales { get; set; }
        public List<SelectListItem> ListaMetodosPago { get; set; }
    }
}
