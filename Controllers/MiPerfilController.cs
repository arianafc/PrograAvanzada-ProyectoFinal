using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class MiPerfilController : Controller
    {
        // Verifica si hay sesión iniciada
        private bool EsUsuarioAutenticado()
        {
            return Session["idUsuario"] != null;
        }

        public ActionResult MiPerfil()
        {
            if (Session["idUsuario"] == null)
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
                        Direccion = d.DIRECCION_EXACTA
                    })
                    .FirstOrDefault();

                if (direccion != null)
                {
                    ViewBag.Direccion = direccion.Direccion;
                }
                else
                {
                    ViewBag.Direccion = "No registrada";
                }

                return View();
            }
        }

        public ActionResult EditarInfoPerfil()
        {
            if (Session["idUsuario"] == null)
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
                        Direccion = d.DIRECCION_EXACTA
                    })
                    .FirstOrDefault();

                if (direccion != null)
                {
                    ViewBag.Direccion = direccion.Direccion;
                }
                else
                {
                    ViewBag.Direccion = "No registrada";
                }

                return View();
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
            if (!EsUsuarioAutenticado()) return RedirectToAction("IniciarSesion", "Home");
            return View();
        }


    }
}