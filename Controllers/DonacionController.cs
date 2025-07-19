using System;
using System.Linq;
using System.Web.Mvc;
using ProyectoFinal.EF;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    public class DonacionController : Controller
    {
        [HttpGet]
        public ActionResult Donaciones()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminDonaciones()
        {
            using (var db = new CASA_NATURAEntities1())
            {
                var donaciones = (from d in db.DONACIONES_TB
                                  join u in db.USUARIOS_TB on d.ID_USUARIO equals u.ID_USUARIO
                                  select new DonacionViewModel
                                  {
                                      DonacionId = d.ID_DONACION,
                                      NombreCompleto = u.NOMBRE + " " + u.APELLIDO1 + " " + u.APELLIDO2,
                                      Correo = u.CORREO,
                                      Monto = d.MONTO.Value,
                                      Fecha = d.FECHA.Value
                                  }).ToList();

                return View("AdminDonaciones", donaciones);
            }
        }

        [HttpGet]
        public ActionResult FormularioDonar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormularioDonar(decimal cantidad, string metodo, string referencia)
        {
            try
            {
                if (Session["IdUsuario"] == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
                int idMetodo = ObtenerIdMetodo(metodo);

                if (idMetodo == 0)
                {
                    ViewBag.Error = "Método de pago no válido.";
                    return View();
                }

                // Solo usamos la referencia si el método es SINPE
                string referenciaFinal = (idMetodo == 2) ? referencia : null;

                using (var db = new CASA_NATURAEntities1())
                {
                    db.InsertarDonacionSP(cantidad, idUsuario, idMetodo, referenciaFinal);
                }

                TempData["Mensaje"] = "Donación registrada exitosamente.";
                return RedirectToAction("FormularioDonar");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al guardar la donación: " +
                    (ex.InnerException?.InnerException?.Message ?? ex.Message);
                return View();
            }
        }

        [HttpGet]
        public JsonResult VerificarLogin()
        {
            bool logueado = Session["IdUsuario"] != null;
            return Json(new { logueado = logueado }, JsonRequestBehavior.AllowGet);
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
