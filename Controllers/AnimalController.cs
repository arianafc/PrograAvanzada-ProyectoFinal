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
        
        [HttpGet]
        public ActionResult ConsultarAnimales()
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.ObtenerAnimalesActivosSP().ToList();
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar los animales: " + ex.Message;
                return View(new List<ObtenerAnimalesActivosSP_Result>());
            }
        }

        
        [HttpGet]
        public ActionResult DetalleAnimal(int id)
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    
                    var result = dbcontext.ObtenerAnimalPorIdSP(id).FirstOrDefault();

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

        
        [HttpGet]
        public ActionResult ApadrinarAnimal(int id)
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.ObtenerAnimalPorIdSP(id).FirstOrDefault();

                    if (result == null)
                    {
                        ViewBag.Error = "Animal no encontrado";
                        return RedirectToAction("ConsultarAnimales");
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

        [HttpPost]
        public ActionResult ApadrinarAnimal(decimal montoMensual, string metodo, string referencia, int idAnimal)
        {
            try
            {
                if (Session["IdUsuario"] == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }
                int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
                int idMetodo = ObtenerIdMetodo(metodo);

                if (montoMensual <= 50)
                {
                    ViewBag.Error = "El monto debe ser mayor a 50 doláres.";
                    return RedirectToAction("ApadrinarAnimal", new { id = idAnimal });
                }

                string referenciaFinal = (idMetodo == 2) ? referencia : null;

                using (var dbcontext = new CASA_NATURAEntities())
                {
                    dbcontext.InsertarApadrinamientoSP(montoMensual, idUsuario, idMetodo, referenciaFinal, idAnimal);
                }
               // TempData["Mensaje"] = "Apadrinamiento registrado exitosamente.";
                return RedirectToAction("ConsultarAnimales");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al apadrinar el animal: " + ex.Message;
                return RedirectToAction("ConsultarAnimales");
            }
        }

        private int ObtenerIdMetodo(string metodo)
        {
            switch (metodo?.ToLower())
            {
                case "tarjeta": return 1;
                case "sinpe": return 2;
                case "paypal": return 3;
                default: return 0;
            }
        }
    }
}