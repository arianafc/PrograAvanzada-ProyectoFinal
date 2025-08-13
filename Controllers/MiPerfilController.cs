using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult MiPerfil()
        {
            if (!EsUsuarioAutenticado())
            {
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

        [HttpPost]
        public ActionResult ActualizarPerfil(
            string nombre,
            string apellido1,
            string apellido2,
            string identificacion,
            string email,
            int provincia,
            int canton,
            int distrito,
            string exacta)
        {
            if (Session["idUsuario"] == null)
                return RedirectToAction("IniciarSesion", "Home");

            int idUsuario = Convert.ToInt32(Session["idUsuario"]);

            using (var db = new CASA_NATURAEntities())
            {
                // Actualizar datos del usuario
                var usuario = db.USUARIOS_TB.FirstOrDefault(u => u.ID_USUARIO == idUsuario);
                if (usuario != null)
                {
                    usuario.NOMBRE = nombre;
                    usuario.APELLIDO1 = apellido1;
                    usuario.APELLIDO2 = apellido2;
                    usuario.Identificacion = identificacion;
                    usuario.CORREO = email;
                }

                // Actualizar o insertar dirección
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
                        ID_ESTADO = 1 // o el estado por defecto que manejes
                    };
                    db.DIRECCIONES_TB.Add(direccion);
                }

                db.SaveChanges();

                // Actualizar valores en sesión para que se reflejen sin cerrar sesión
                Session["Nombre"] = nombre;
                Session["Apellido1"] = apellido1;
                Session["Apellido2"] = apellido2;
                Session["Cedula"] = identificacion;
                Session["email"] = email;
            }

            return RedirectToAction("MiPerfil");
        }


        public JsonResult ObtenerCantones(int provinciaId)
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

        public JsonResult ObtenerDistritos(int cantonId)
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







        public ActionResult MisDonaciones()
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
                        Monto = d.MONTO.Value,
                        Fecha = d.FECHA.Value
                    })
                    .OrderByDescending(d => d.Fecha)
                    .ToList();

                return View(donaciones);
            }
        }

        public ActionResult MisAnimales()
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            int ID_USUARIO = Convert.ToInt32(Session["idUsuario"]);

            using (var db = new CASA_NATURAEntities())
            {
                // Llamada al SP a través del método generado por EF
                var animales = db.ObtenerMisAnimalesSP(ID_USUARIO).ToList();

                return View(animales);
            }
        }



        public ActionResult MisTours()
        {
            if (!EsUsuarioAutenticado())
                return RedirectToAction("IniciarSesion", "Home");

            int idUsuario = Convert.ToInt32(Session["idUsuario"]);
            DateTime fechaLimite = DateTime.Today.AddDays(-1); // ayer a las 00:00

            using (var db = new CASA_NATURAEntities())
            {
                var tours = (from ua in db.USUARIO_ACTIVIDAD_TB
                             join a in db.ACTIVIDADES_TB
                                 on ua.ID_ACTIVIDAD equals a.ID_ACTIVIDAD
                             where ua.ID_USUARIO == idUsuario
                             select new
                             {
                                 a.ID_ACTIVIDAD,
                                 a.NOMBRE,
                                 a.DESCRIPCION,
                                 a.FECHA,
                                 a.IMAGEN,
                                 ua.TICKETS_ADQUIRIDOS,
                                 ua.TOTAL
                             }).ToList();

                ViewBag.Tours = tours;
            }

            return View();
        }







    }
}