using ProyectoFinal.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class GestionActividadesModel
    {
        public Actividad NuevaActividad { get; set; }
        public List<VisualizarActividadesSP_Result> ListaActividades { get; set; }
    }
}