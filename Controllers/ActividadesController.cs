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
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var result = dbContext.VisualizarActividadesSP().ToList();

                    var viewModel = new GestionActividadesModel
                    {
                        NuevaActividad = new Actividad(),
                        ListaActividades = result
                    };

                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al cargar las actividades: " + ex.Message;

                    return View(new GestionActividadesModel
                    {
                        NuevaActividad = new Actividad(),
                        ListaActividades = new List<VisualizarActividadesSP_Result>()
                    });
                }
            }
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