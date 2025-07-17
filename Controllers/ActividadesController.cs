using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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

        public ActionResult GestionActividades(GestionActividadesModel actividad, HttpPostedFileBase ImagenActividad, String Hora)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var actividades = new ACTIVIDADES_TB();
                    DateTime fechaCompleta;
                    if (DateTime.TryParse(actividad.NuevaActividad.Fecha.ToString("yyyy-MM-dd") + " " + Hora, out fechaCompleta))
                    {
                        actividades.FECHA = fechaCompleta;
                    }
     
                    actividades.ID_ACTIVIDAD = 0;
                    actividades.NOMBRE = actividad.NuevaActividad.Nombre;
                    actividades.DESCRIPCION = actividad.NuevaActividad.Descripcion;
                    actividades.PRECIO_BOLETO = actividad.NuevaActividad.PrecioBoleto;
                    actividades.TICKETS_DISPONIBLES = actividad.NuevaActividad.TicketsDisponibles;
                    actividades.ID_ESTADO = 1;
                    actividades.IMAGEN = string.Empty;

                    dbContext.ACTIVIDADES_TB.Add(actividades);
                    var result = dbContext.SaveChanges();

                    if (result > 0)
                    {
                        string extension = Path.GetExtension(ImagenActividad.FileName);
                        string ruta = AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\" + actividades.ID_ACTIVIDAD + extension;
                        ImagenActividad.SaveAs(ruta);

                        actividades.IMAGEN = "/Imagenes/" + actividades.ID_ACTIVIDAD + extension;
                        dbContext.SaveChanges();

                        return RedirectToAction("GestionActividades", "Actividades");


                    }

                    TempData["SwalSuccess"] = "Actividad registrado con éxito";
                    return View();
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