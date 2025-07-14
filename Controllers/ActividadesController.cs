using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ActividadesController : Controller
    {
        // GET: Actividades

        [HttpGet]
        public ActionResult GestionActividades()
        {
            return View();
        }

        [HttpPost]

        public ActionResult GestionActividades( Actividad actividad)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var result = dbContext.AgregarActividadSP(
                        actividad.Descripcion, actividad.Fecha, actividad.PrecioBoleto, actividad.TicketsDisponibles,
                        actividad.Imagen, actividad.Tipo, actividad.Nombre
                        );

                    if (result == -1)
                    {
                        TempData["SwalError"] = "Lo sentimos. No se pudo registrar su información.";
                        return RedirectToAction("Registro");

                    }

                    TempData["SwalSuccess"] = "Actividad registrado con éxito";
                    return RedirectToAction("GestionActividades", "Actividades");
                }
                catch (Exception ex)
                {

                    TempData["SwalError"] = ex.InnerException?.Message ?? ex.Message;
                    return RedirectToAction("Registro");
                }
            }

        }

    }

}