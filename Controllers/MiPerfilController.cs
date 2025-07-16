using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult editarPerfil()
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

                // Si necesitas enviar la dirección como string:
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
            if (!EsUsuarioAutenticado()) return RedirectToAction("IniciarSesion", "Home");
            return View();
        }

        public ActionResult MisTours()
        {
            if (!EsUsuarioAutenticado()) return RedirectToAction("IniciarSesion", "Home");
            return View();
        }

        public ActionResult MisEventos()
        {
            if (!EsUsuarioAutenticado()) return RedirectToAction("IniciarSesion", "Home");
            return View();
        }

        public ActionResult Editar()
        {
            if (!EsUsuarioAutenticado()) return RedirectToAction("IniciarSesion", "Home");
            return View();
        }
    }
}