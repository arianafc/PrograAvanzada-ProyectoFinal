using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProyectoFinal.Controllers
{
    [OutputCache(Duration = 0, Location = OutputCacheLocation.None, NoStore = true, VaryByParam = "*")]
    public class AnimalController : Controller
    {
        readonly Utilitarios service = new Utilitarios();

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
        [FiltroAdministrador]
        public ActionResult GestionAnimales()
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var result = dbContext.VisualizarAnimalesSP().ToList();

                    var viewModel = new GestionAnimalesModel
                    {
                        NuevoAnimal = new Animal(),
                        ListaAnimales = result,

                        ListaSalud = dbContext.ESTADOS_SALUD_TB
                    .Select(a => new SelectListItem
                    {
                        Value = a.ID_SALUD.ToString(),
                        Text = a.DESCRIPCION
                    })
                    .ToList(),

                        ListaRazas = dbContext.RAZAS_TB
                    .Where(a => a.ID_ESTADO == 1)
                    .Select(a => new SelectListItem
                    {
                        Value = a.ID_RAZA.ToString(),
                        Text = a.NOMBRE
                    })
                    .ToList()
                    };

                    return View(viewModel);
                }

                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al cargar los animales: " + ex.Message;

                    return View(new GestionAnimalesModel
                    {
                        NuevoAnimal = new Animal(),
                        ListaAnimales = new List<VisualizarAnimalesSP_Result>(),
                        ListaRazas = new List<SelectListItem>(),
                        ListaSalud = new List<SelectListItem>()
                    });
                }
            }
        }


        [HttpPost]
        public ActionResult GestionAnimales(GestionAnimalesModel animal, HttpPostedFileBase ImagenAnimal)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var animales = new ANIMAL_TB();

                    animales.ID_ANIMAL = 0;
                    animales.NOMBRE = animal.NuevoAnimal.Nombre;
                    animales.ID_RAZA = animal.NuevoAnimal.IdRaza;
                    animales.FECHA_NACIMIENTO = animal.NuevoAnimal.FechaNacimiento;
                    animales.FECHA_INGRESO = animal.NuevoAnimal.FechaIngreso = DateTime.Now;
                    animales.FECHA_BAJA = null;
                    animales.ID_ESTADO = 1;
                    animales.ID_SALUD = animal.NuevoAnimal.IdSalud;
                    animales.HISTORIA = animal.NuevoAnimal.Historia;
                    animales.NECESIDAD = animal.NuevoAnimal.Necesidad;
                    animales.IMAGEN = string.Empty;

                    dbContext.ANIMAL_TB.Add(animales);
                    var result = dbContext.SaveChanges();

                    if (result > 0)
                    {
                        string extension = Path.GetExtension(ImagenAnimal.FileName);
                        string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Animales", animales.ID_ANIMAL + extension);
                        ImagenAnimal.SaveAs(ruta);

                        animales.IMAGEN = "/Imagenes/Animales/" + animales.ID_ANIMAL + extension;
                        dbContext.SaveChanges();

                        TempData["Mensaje"] = "Animal registrado con éxito";
                        return RedirectToAction("GestionAnimales", "Animal");
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo registrar el animal";
                        return RedirectToAction("GestionAnimales", "Animal");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException?.Message ?? ex.Message;
                    return RedirectToAction("GestionAnimales", "Animal");
                }
            }
        }

        [HttpPost]
        public ActionResult EditarAnimal(GestionAnimalesModel animal, HttpPostedFileBase ImagenAnimal)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    if (string.IsNullOrEmpty(animal.NuevoAnimal.Nombre))
                    {
                        TempData["Error"] = "El nombre del animal es requerido";
                        return RedirectToAction("GestionAnimales", "Animal");
                    }
                    if (animal.NuevoAnimal.IdRaza == 0)
                    {
                        TempData["Error"] = "La raza del animal es requerida";
                        return RedirectToAction("GestionAnimales", "Animal");
                    }

                    string rutaVirtual = null;

                    if (ImagenAnimal != null && ImagenAnimal.ContentLength > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(animal.NuevoAnimal.Imagen))
                            {
                                string imagenAnterior = animal.NuevoAnimal.Imagen.Replace("/", "\\");
                                if (imagenAnterior.StartsWith("\\"))
                                    imagenAnterior = imagenAnterior.Substring(1);

                                string rutaFisicaAnterior = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagenAnterior);

                                if (System.IO.File.Exists(rutaFisicaAnterior))
                                {
                                    System.IO.File.Delete(rutaFisicaAnterior);
                                }
                            }

                            string extension = Path.GetExtension(ImagenAnimal.FileName);
                            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Animales", animal.NuevoAnimal.IdAnimal + extension);
                            ImagenAnimal.SaveAs(ruta);
                            rutaVirtual = "/Imagenes/Animales/" + animal.NuevoAnimal.IdAnimal + extension;
                        }
                        catch (Exception imgEx)
                        {
                            System.Diagnostics.Debug.WriteLine("Error al guardar imagen: " + imgEx.Message);
                        }
                    }

                    var result = dbContext.EditarAnimalSP(
                        animal.NuevoAnimal.IdAnimal,
                        animal.NuevoAnimal.Nombre,
                        animal.NuevoAnimal.IdRaza,
                        animal.NuevoAnimal.FechaNacimiento,
                        animal.NuevoAnimal.FechaIngreso,
                        animal.NuevoAnimal.Historia ?? string.Empty,
                        animal.NuevoAnimal.Necesidad ?? string.Empty,
                        rutaVirtual
                    );

                    if (result > 0)
                    {
                        TempData["Mensaje"] = "Animal actualizado con éxito";
                        return RedirectToAction("GestionAnimales", "Animal");
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo actualizar el animal";
                        return RedirectToAction("GestionAnimales", "Animal");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error: " + (ex.InnerException?.Message ?? ex.Message);
                    return RedirectToAction("GestionAnimales", "Animal");
                }
            }
        }

        [HttpPost]
        public ActionResult CambiarEstadoAnimal(int IdAnimal, int IdEstado)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var result = dbContext.CambiarEstadoAnimalSP(IdAnimal, IdEstado);

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

            return RedirectToAction("GestionAnimales", "Animal");
        }
    }
}