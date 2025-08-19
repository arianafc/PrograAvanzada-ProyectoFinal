using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class MiPerfilController : Controller
    {
        private bool EsUsuarioAutenticado()
        {
            return Session["idUsuario"] != null;
        }
        [HttpGet]
        [FiltroSesion]
        public ActionResult MiPerfil()
        {
            try
            {
                if (!EsUsuarioAutenticado())
                {
                    TempData["SwalError"] = "Debe iniciar sesión para acceder a su perfil.";
                    return RedirectToAction("IniciarSesion", "Home");
                }

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var direccion = db.DIRECCIONES_TB
                        .Where(d => d.ID_USUARIO == idUsuario)
                        .Select(d => new
                        {
                            DireccionExacta = d.DIRECCION_EXACTA,
                            Distrito = d.DISTRITOS_TB.NOMBRE,
                            Canton = d.DISTRITOS_TB.CANTONES_TB.NOMBRE,
                            Provincia = d.DISTRITOS_TB.CANTONES_TB.PROVINCIAS_TB.NOMBRE,
                            DistritoId = d.ID_DISTRITO,
                            CantonId = d.DISTRITOS_TB.CANTON,
                            ProvinciaId = d.DISTRITOS_TB.CANTONES_TB.PROVINCIA
                        })
                        .FirstOrDefault();

                    var provincias = db.PROVINCIAS_TB.ToList();
                    List<CANTONES_TB> cantones = new List<CANTONES_TB>();
                    List<DISTRITOS_TB> distritos = new List<DISTRITOS_TB>();

                    if (direccion != null)
                    {
                        cantones = db.CANTONES_TB.Where(c => c.PROVINCIA == direccion.ProvinciaId).ToList();
                        distritos = db.DISTRITOS_TB.Where(d => d.CANTON == direccion.CantonId).ToList();

                        ViewBag.Direccion = direccion.DireccionExacta;
                        ViewBag.Distrito = direccion.Distrito;
                        ViewBag.Canton = direccion.Canton;
                        ViewBag.Provincia = direccion.Provincia;
                        ViewBag.DistritoId = direccion.DistritoId;
                        ViewBag.CantonId = direccion.CantonId;
                        ViewBag.ProvinciaId = direccion.ProvinciaId;
                    }
                    else
                    {
                        ViewBag.Direccion = "No registrada";
                        ViewBag.Distrito = "-";
                        ViewBag.Canton = "-";
                        ViewBag.Provincia = "-";
                    }

                    ViewBag.Provincias = provincias;
                    ViewBag.Cantones = cantones;
                    ViewBag.Distritos = distritos;

                    return View();
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Ocurrió un error al cargar su perfil. Intente nuevamente.";
                return RedirectToAction("Home", "Index");
            }
        }

        [HttpGet]
        [FiltroSesion]
        public ActionResult Ajustes()
        {
            try
            {
                if (!EsUsuarioAutenticado())
                {
                    TempData["SwalError"] = "Debe iniciar sesión para acceder a su perfil.";
                    return RedirectToAction("IniciarSesion", "Home");
                }

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var direccion = db.DIRECCIONES_TB
                        .Where(d => d.ID_USUARIO == idUsuario)
                        .Select(d => new
                        {
                            DireccionExacta = d.DIRECCION_EXACTA,
                            Distrito = d.DISTRITOS_TB.NOMBRE,
                            Canton = d.DISTRITOS_TB.CANTONES_TB.NOMBRE,
                            Provincia = d.DISTRITOS_TB.CANTONES_TB.PROVINCIAS_TB.NOMBRE,
                            DistritoId = d.ID_DISTRITO,
                            CantonId = d.DISTRITOS_TB.CANTON,
                            ProvinciaId = d.DISTRITOS_TB.CANTONES_TB.PROVINCIA
                        })
                        .FirstOrDefault();

                    var provincias = db.PROVINCIAS_TB.ToList();
                    List<CANTONES_TB> cantones = new List<CANTONES_TB>();
                    List<DISTRITOS_TB> distritos = new List<DISTRITOS_TB>();

                    if (direccion != null)
                    {
                        cantones = db.CANTONES_TB.Where(c => c.PROVINCIA == direccion.ProvinciaId).ToList();
                        distritos = db.DISTRITOS_TB.Where(d => d.CANTON == direccion.CantonId).ToList();

                        ViewBag.Direccion = direccion.DireccionExacta;
                        ViewBag.Distrito = direccion.Distrito;
                        ViewBag.Canton = direccion.Canton;
                        ViewBag.Provincia = direccion.Provincia;
                        ViewBag.DistritoId = direccion.DistritoId;
                        ViewBag.CantonId = direccion.CantonId;
                        ViewBag.ProvinciaId = direccion.ProvinciaId;
                    }
                    else
                    {
                        ViewBag.Direccion = "No registrada";
                        ViewBag.Distrito = "-";
                        ViewBag.Canton = "-";
                        ViewBag.Provincia = "-";
                    }

                    ViewBag.Provincias = provincias;
                    ViewBag.Cantones = cantones;
                    ViewBag.Distritos = distritos;

                    return View();
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Ocurrió un error al cargar su perfil. Intente nuevamente.";
                return RedirectToAction("Home", "Index");
            }
        }

        [HttpPost]
        public ActionResult ActualizarPerfil(string nombre, string apellido1, string apellido2, string identificacion,
                                             string email, int provincia, int canton, int distrito, string exacta)
        {
            try
            {
                if (Session["idUsuario"] == null)
                    return RedirectToAction("IniciarSesion", "Home");

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var usuario = db.USUARIOS_TB.FirstOrDefault(u => u.ID_USUARIO == idUsuario);
                    if (usuario != null)
                    {
                        usuario.NOMBRE = nombre;
                        usuario.APELLIDO1 = apellido1;
                        usuario.APELLIDO2 = apellido2;
                        usuario.Identificacion = identificacion;
                        usuario.CORREO = email;
                    }

                    var direccion = db.DIRECCIONES_TB.FirstOrDefault(d => d.ID_USUARIO == idUsuario);
                    if (direccion != null)
                    {
                        direccion.ID_DISTRITO = distrito;
                        direccion.DIRECCION_EXACTA = exacta;
                    }
                    else
                    {
                        direccion = new DIRECCIONES_TB
                        {
                            ID_USUARIO = idUsuario,
                            ID_DISTRITO = distrito,
                            DIRECCION_EXACTA = exacta,
                            ID_ESTADO = 1
                        };
                        db.DIRECCIONES_TB.Add(direccion);
                    }

                    db.SaveChanges();

                    Session["Nombre"] = nombre;
                    Session["Apellido1"] = apellido1;
                    Session["Apellido2"] = apellido2;
                    Session["Cedula"] = identificacion;
                    Session["email"] = email;
                }

                if (Session["IdRol"] != null && Convert.ToInt32(Session["IdRol"]) == 1)
                {
                    return RedirectToAction("MiPerfil");
                }
                else if (Session["IdRol"] != null && Convert.ToInt32(Session["IdRol"]) == 2)
                {
                    return RedirectToAction("Ajustes"); 
                }

                return RedirectToAction("MiPerfil", "MiPerfil");
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Ocurrió un error al actualizar el perfil. Intente nuevamente.";
                return RedirectToAction("MiPerfil");
            }
        }

        public JsonResult ObtenerCantones(int provinciaId)
        {
            try
            {
                using (var db = new CASA_NATURAEntities())
                {
                    var cantones = db.CANTONES_TB
                        .Where(c => c.PROVINCIA == provinciaId)
                        .Select(c => new { ID_CANTON = c.ID_CANTON, NOMBRE = c.NOMBRE })
                        .ToList();
                    return Json(cantones, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                return Json(new { error = "Ocurrió un error al obtener los cantones." }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ObtenerDistritos(int cantonId)
        {
            try
            {
                using (var db = new CASA_NATURAEntities())
                {
                    var distritos = db.DISTRITOS_TB
                        .Where(d => d.CANTON == cantonId)
                        .Select(d => new { ID_DISTRITO = d.ID_DISTRITO, NOMBRE = d.NOMBRE })
                        .ToList();
                    return Json(distritos, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                return Json(new { error = "Ocurrió un error al obtener los distritos." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [FiltroSesion]
        public ActionResult MisDonaciones()
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var donaciones = db.DONACIONES_TB
                        .Where(d => d.ID_USUARIO == idUsuario)
                        .Select(d => new DonacionViewModel
                        {
                            DonacionId = d.ID_DONACION,
                            NombreCompleto = d.USUARIOS_TB.NOMBRE + " " + d.USUARIOS_TB.APELLIDO1 + " " + d.USUARIOS_TB.APELLIDO2,
                            Correo = d.USUARIOS_TB.CORREO,
                            Monto = d.MONTO ?? 0,
                            Fecha = d.FECHA ?? DateTime.Now
                        })
                        .OrderByDescending(d => d.Fecha)
                        .ToList();

                    return View(donaciones);
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Ocurrió un error al cargar sus donaciones.";
                return RedirectToAction("MiPerfil");
            }
        }


        [HttpGet]
        [FiltroSesion]
        public ActionResult MisTours()
        {
            try
            {
                if (!EsUsuarioAutenticado())
                    return RedirectToAction("IniciarSesion", "Home");

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var parametro = new SqlParameter("@IdUsuario", idUsuario);
                    var tours = db.Database
                                  .SqlQuery<MisToursViewModel>("EXEC SP_ObtenerMisTours @IdUsuario", parametro)
                                  .ToList();

                    return View(tours);
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Ocurrió un error al cargar sus tours.";
                return RedirectToAction("MiPerfil");
            }
        }

        [HttpGet]
        public ActionResult MisAnimales()
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var parametro = new SqlParameter("@IdUsuario", idUsuario);
                    var animales = db.Database
                                     .SqlQuery<AnimalViewModel>("EXEC SP_ObtenerMisAnimales @IdUsuario", parametro)
                                     .ToList();

                    return View(animales);
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Ocurrió un error al cargar sus animales.";
                return RedirectToAction("MiPerfil");
            }
        }

        [HttpPost]
        public ActionResult FinalizarApadrinamiento(int idAnimal)
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                using (var db = new CASA_NATURAEntities())
                {
                    var parametros = new[]
                    {
                        new SqlParameter("@IdUsuario", idUsuario),
                        new SqlParameter("@IdAnimal", idAnimal)
                    };

                    db.Database.ExecuteSqlCommand("EXEC SP_FinalizarApadrinamiento @IdUsuario, @IdAnimal", parametros);
                }

                TempData["Mensaje"] = "El apadrinamiento fue finalizado correctamente.";
                return RedirectToAction("MisAnimales");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en FinalizarApadrinamiento: {ex.Message}");
                TempData["Error"] = "No se pudo finalizar el apadrinamiento.";
                return RedirectToAction("MisAnimales");
            }
        }
    }
}
