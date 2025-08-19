using ProyectoFinal.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class GestionUsuariosModel
    {
        public IEnumerable<ObtenerUsuariosSP_Result> Usuarios { get; set; }
        public List<ROLES_TB> Roles { get; set; }
    }
}