using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProyectoFinal.Controllers
{
    public class ApadrinamientoController : Controller
    {
        readonly Utilitarios service = new Utilitarios();

        [FiltroSesion]
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
                        return RedirectToAction("ConsultarAnimales", "Animal");
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
                int idMetodo = service.ObtenerIdMetodo(metodo);

                if (montoMensual <= 50)
                {
                    ViewBag.Error = "El monto debe ser mayor a 50 dólares.";

                    using (var dbcontext = new CASA_NATURAEntities())
                    {
                        var result = dbcontext.ObtenerAnimalPorIdSP(idAnimal).FirstOrDefault();
                        return View(result);
                    }
                }

                string referenciaFinal;

                if (idMetodo == 2)
                {
                    referenciaFinal = string.IsNullOrWhiteSpace(referencia) ? "N/A" : referencia;
                }
                else
                {
                    referenciaFinal = "N/A";
                }

                using (var dbcontext = new CASA_NATURAEntities())
                {
                    dbcontext.InsertarApadrinamientoSP(montoMensual, idUsuario, idMetodo, referenciaFinal, idAnimal);
                }

                ViewBag.Mensaje = "¡Apadrinamiento registrado exitosamente! Gracias por tu generosidad.";

                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.ObtenerAnimalPorIdSP(idAnimal).FirstOrDefault();
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al guardar el apadrinamiento: " +
                    (ex.InnerException?.InnerException?.Message ?? ex.Message);

                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.ObtenerAnimalPorIdSP(idAnimal).FirstOrDefault();
                    return View(result);
                }
            }
        }

        [HttpGet]
        [FiltroAdministrador]
        public ActionResult GestionApadrinamientos()
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var result = dbContext.VisualizarApadrinamientosSP().ToList();

                    var viewModel = new GestionApadrinamientosModel
                    {
                        NuevoApadrinamiento = new Apadrinamiento(),
                        ListaApadrinamientos = result,
                        ListaUsuarios = dbContext.USUARIOS_TB
                            .Where(a => a.ID_ESTADO == 1)
                            .Select(u => new SelectListItem
                            {
                                Value = u.ID_USUARIO.ToString(),
                                Text = u.NOMBRE + " " + u.APELLIDO1 + " " + u.APELLIDO2
                            })
                            .ToList(),
                        ListaAnimales = dbContext.ANIMAL_TB
                            .Where(a => a.ID_ESTADO == 1)
                            .Select(a => new SelectListItem
                            {
                                Value = a.ID_ANIMAL.ToString(),
                                Text = a.NOMBRE
                            })
                            .ToList(),
                        ListaMetodosPago = dbContext.METODO_PAGO_TB
                            .Where(a => a.ID_ESTADO == 1)
                            .Select(m => new SelectListItem
                            {
                                Value = m.ID_METODO.ToString(),
                                Text = m.METODO
                            })
                            .ToList()
                    };

                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al cargar los apadrinamientos: " + ex.Message;

                    return View(new GestionApadrinamientosModel
                    {
                        NuevoApadrinamiento = new Apadrinamiento(),
                        ListaApadrinamientos = new List<VisualizarApadrinamientosSP_Result>(),
                        ListaUsuarios = new List<SelectListItem>(),
                        ListaAnimales = new List<SelectListItem>(),
                        ListaMetodosPago = new List<SelectListItem>()
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult GestionApadrinamientos(GestionApadrinamientosModel apadrinamiento)
        {
            if (apadrinamiento.NuevoApadrinamiento.IdApadrinamiento > 0)
            {
                return EditarApadrinamiento(apadrinamiento);
            }

            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var apadrinamientos = new APADRINAMIENTOS_TB();

                    apadrinamientos.ID_APADRINAMIENTO = 0;
                    apadrinamientos.ID_ANIMAL = apadrinamiento.NuevoApadrinamiento.IdAnimal;
                    apadrinamientos.ID_USUARIO = apadrinamiento.NuevoApadrinamiento.IdUsuario;
                    apadrinamientos.FECHA = apadrinamiento.NuevoApadrinamiento.Fecha;
                    apadrinamientos.FECHA_BAJA = null;
                    apadrinamientos.MONTO_MENSUAL = apadrinamiento.NuevoApadrinamiento.MontoMensual;
                    apadrinamientos.REFERENCIA = apadrinamiento.NuevoApadrinamiento.Referencia ?? string.Empty;
                    apadrinamientos.ID_ESTADO = 1;
                    apadrinamientos.ID_METODO = apadrinamiento.NuevoApadrinamiento.IdMetodo;

                    dbContext.APADRINAMIENTOS_TB.Add(apadrinamientos);

                    var animal = dbContext.ANIMAL_TB.FirstOrDefault(a => a.ID_ANIMAL == apadrinamiento.NuevoApadrinamiento.IdAnimal);
                    if (animal != null)
                    {
                        animal.ID_ESTADO = 3;
                    }

                    var result = dbContext.SaveChanges();

                    if (result > 0)
                    {
                        TempData["Mensaje"] = "Apadrinamiento registrado con éxito";
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo registrar el apadrinamiento";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error al guardar el apadrinamiento: " +
                        (ex.InnerException?.InnerException?.Message ?? ex.Message);
                }

                return RedirectToAction("GestionApadrinamientos", "Apadrinamiento");
            }
        }

        [HttpPost]
        public ActionResult EditarApadrinamiento(GestionApadrinamientosModel model)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var apadrinamiento = dbContext.APADRINAMIENTOS_TB
                        .FirstOrDefault(a => a.ID_APADRINAMIENTO == model.NuevoApadrinamiento.IdApadrinamiento);

                    if (apadrinamiento == null)
                    {
                        TempData["Error"] = "El apadrinamiento no fue encontrado";
                        return RedirectToAction("GestionApadrinamientos", "Apadrinamiento");
                    }

                    int animalAnterior = apadrinamiento.ID_ANIMAL;

                    apadrinamiento.ID_ANIMAL = model.NuevoApadrinamiento.IdAnimal;
                    apadrinamiento.ID_USUARIO = model.NuevoApadrinamiento.IdUsuario;
                    apadrinamiento.FECHA = model.NuevoApadrinamiento.Fecha;
                    apadrinamiento.MONTO_MENSUAL = model.NuevoApadrinamiento.MontoMensual;
                    apadrinamiento.REFERENCIA = model.NuevoApadrinamiento.Referencia ?? string.Empty;
                    apadrinamiento.ID_METODO = model.NuevoApadrinamiento.IdMetodo;
                    apadrinamiento.FECHA_BAJA = model.NuevoApadrinamiento.FechaBaja;

                    if (model.NuevoApadrinamiento.FechaBaja.HasValue)
                    {
                        apadrinamiento.ID_ESTADO = 2;

                        var animalActual = dbContext.ANIMAL_TB.FirstOrDefault(a => a.ID_ANIMAL == model.NuevoApadrinamiento.IdAnimal);
                        if (animalActual != null)
                        {
                            animalActual.ID_ESTADO = 1;
                        }
                    }
                    else
                    {
                        apadrinamiento.ID_ESTADO = 1;

                        if (animalAnterior != model.NuevoApadrinamiento.IdAnimal)
                        {
                            var animalAnt = dbContext.ANIMAL_TB.FirstOrDefault(a => a.ID_ANIMAL == animalAnterior);
                            if (animalAnt != null)
                            {
                                animalAnt.ID_ESTADO = 1;
                            }

                            var animalNuevo = dbContext.ANIMAL_TB.FirstOrDefault(a => a.ID_ANIMAL == model.NuevoApadrinamiento.IdAnimal);
                            if (animalNuevo != null)
                            {
                                animalNuevo.ID_ESTADO = 2;
                            }
                        }
                    }

                    var result = dbContext.SaveChanges();

                    if (result > 0)
                    {
                        TempData["Mensaje"] = "Apadrinamiento actualizado con éxito";
                        return RedirectToAction("GestionApadrinamientos", "Apadrinamiento");
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo actualizar el apadrinamiento";
                        return RedirectToAction("GestionApadrinamientos", "Apadrinamiento");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException?.Message ?? ex.Message;
                    return RedirectToAction("GestionApadrinamientos", "Apadrinamiento");
                }
            }
        }

        [HttpPost]
        public ActionResult CambiarEstadoApadrinamiento(int IdApadrinamiento)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var result = dbContext.CambiarEstadoApadrinamientoSP(IdApadrinamiento);

                    if (result > 0)
                    {
                        TempData["Mensaje"] = "Estado cambiado exitosamente";
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo cambiar el estado";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("GestionApadrinamientos", "Apadrinamiento");
        }
    }
}