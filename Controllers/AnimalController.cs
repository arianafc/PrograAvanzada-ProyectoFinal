using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.EF;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    public class AnimalController : Controller
    {
        // Vista principal - mostrar todos los animales activos
        [HttpGet]
        public ActionResult ConsultarAnimales()
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.ObtenerAnimalesActivos_SP().ToList();
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar los animales: " + ex.Message;
                return View(new List<ObtenerAnimalesActivos_SP_Result>());
            }
        }

        // Vista de detalles de un animal específico
        [HttpGet]
        public ActionResult DetalleAnimal(int id)
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    // Usar SP para obtener un animal específico
                    var result = dbcontext.ObtenerAnimalPorId_SP(id).FirstOrDefault();

                    if (result == null)
                    {
                        ViewBag.Error = "Animal no encontrado";
                        return RedirectToAction("Index");
                    }

                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Vista para apadrinar un animal
        [HttpGet]
        public ActionResult ApadrinarAnimal(int id)
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.ObtenerAnimalPorId_SP(id).FirstOrDefault();

                    if (result == null)
                    {
                        ViewBag.Error = "Animal no encontrado";
                        return RedirectToAction("Index");
                    }

                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}