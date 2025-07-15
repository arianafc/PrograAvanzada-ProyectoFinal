using System.Collections.Generic;
using ProyectoFinal.EF;

namespace ProyectoFinal.Models
{
    public class GestionActividadesModel
    {
        public Actividad NuevaActividad { get; set; }
        public List<VisualizarActividadesSP_Result> ListaActividades { get; set; }
    }
}
