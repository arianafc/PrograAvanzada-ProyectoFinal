using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class DashboardModel
    {

        public Decimal TotalVentas { get; set; }

        public int NumeroUsuariosActivos { get; set; }
        public int NumeroAnimalesTotales { get; set; }
        public int NumeroAnimalesApadrinados { get; set; }

        public int NumeroUsuariosInactivos { get; set; }

        public List<VentasMes> VentasPorMes { get; set; }
        public List<string> NombresMeses { get; set; }

        public List<Usuario> TopUsuarios { get; set; } 

        public int NumeroActividadesActivadas { get; set; }
        
    }
}