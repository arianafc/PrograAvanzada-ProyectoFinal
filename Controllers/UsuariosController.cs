using ProyectoFinal.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class UsuariosController : Controller
    {
        [HttpGet]
        public ActionResult GestionUsuarios()
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {

                    var usuarios = dbContext.ObtenerUsuariosSP().ToList();
                    return View(usuarios);

                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error al cargar los usuarios: " + e.Message;
                return RedirectToAction("Dashboard", "Dashboard");
            }
        }
    }
}