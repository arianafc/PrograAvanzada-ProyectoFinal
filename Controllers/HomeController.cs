using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class HomeController : Controller
    {

        Utilitarios service = new Utilitarios();

        #region HomePage
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Dashboard

        [HttpGet]
        public ActionResult Dashboard()
        {

            return View();
        }

        #endregion

        #region IniciarSesion
        [HttpGet]
        public ActionResult IniciarSesion()
        {

            return View();
        }


        [HttpPost]
        public ActionResult IniciarSesion(Usuario usuario)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {

                var result = dbContext.LoginSP(usuario.Correo, usuario.Contrasenna).FirstOrDefault();

                if (result != null)
                {
                    Session["Nombre"] = result.NOMBRE;
                    Session["Apellido1"] = result.APELLIDO1;
                    Session["Apellido2"] = result.APELLIDO2;
                    Session["Cedula"] = result.IDENTIFICACION;
                    Session["idUsuario"] = result.ID_USUARIO;
                    Session["ID_USUARIO"] = result.ID_USUARIO;
                    Session["email"] = result.CORREO;
                    Session["IdRol"] = result.ID_ROL;
                    if (result.ID_ROL == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("dashboard", "Home");
                    }
                }

                TempData["SwalError"] = "Lo sentimos. Usuario o contraseña incorrectos";
                return View();
            }

        }
        #endregion

        #region Registro

        [HttpGet]
        public ActionResult Registro()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario usuario)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var result = dbContext.RegistroSP(
                        usuario.Nombre,
                        usuario.Apellido1,
                        usuario.Apellido2,
                        usuario.Correo.ToLower(),
                        usuario.Contrasenna,
                        usuario.Identificacion
                    );

                    if (result == -1)
                    {
                        TempData["SwalError"] = "Lo sentimos. No se pudo registrar su información.";
                        return RedirectToAction("Registro");

                    }

                    TempData["SwalSuccess"] = "Usuario registrado con éxito";
                    return RedirectToAction("IniciarSesion", "Home");
                }
                catch (Exception ex)
                {

                    TempData["SwalError"] = ex.InnerException?.Message ?? ex.Message;
                    return View();
                }
            }
        }

        #endregion

        #region CerrarSesion

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("IniciarSesion", "Home");
        }
        #endregion

        #region RecuperarAcceso
        [HttpGet]

        public ActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]

        public ActionResult RecuperarAcceso(Usuario user)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {

                var result = dbContext.USUARIOS_TB.FirstOrDefault(u => u.CORREO.ToLower() == user.Correo.ToLower()
                                                             && u.Identificacion == user.Identificacion);

                if (result != null)
                {
                    string link = Url.Action(
                       "CambioContrasenna",
                           "Home",
                        new { correo = user.Correo.ToLower() },
                       protocol: Request.Url.Scheme
                       );
                    StringBuilder mensaje = new StringBuilder();

                    mensaje.Append("<p>Estimado <strong>" + result.NOMBRE + "</strong>,</p>");
                    mensaje.Append("<p>Recibimos una solicitud para recuperar su acceso.</p>");
                    mensaje.Append("<p>Haga clic en el siguiente enlace para cambiar su contraseña:</p>");
                    mensaje.Append("<p><a href='" + link + "' style='color:#007bff;'>" + link + "</a></p>");
                    mensaje.Append("<p>Este enlace es válido por un tiempo limitado.</p>");
                    mensaje.Append("<p>Gracias,<br/>El equipo de soporte</p>");


                    if (service.EnviarCorreo(result.CORREO, mensaje.ToString(), "Solicitud de acceso"))
                    {
                        TempData["SwalSuccess"] = "Hemos enviado un link de recuperación de acceso al correo" +
                            "electrónico registrado.";
                        return RedirectToAction("IniciarSesion", "Home");
                    }


                    TempData["SwalError"] = "No se pudo realizar la notificación de su acceso al sistema";
                    return View();
                }

                TempData["SwalError"] = "Lo sentimos, la cédula o correo indicados no se encuentran registrados o son incorrectos.";
                return View();

            }
        }

        #endregion

        #region CambioContrasenna

        [HttpGet]
        public ActionResult CambioContrasenna(string correo)
        {
            ViewBag.Correo = correo;
            return View();
        }

        [HttpPost]
        public ActionResult CambioContrasenna(Usuario user, String correo)
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                var result = dbContext.CambiarContrasennaSP(correo, user.Contrasenna);

                if (result > 0)
                {
                    TempData["SwalSuccess"] = "Contraseña actualizada con éxito.";
                    return RedirectToAction("IniciarSesion", "Home");
                }

                TempData["SwalError"] = "No se pudo actualizar la contraseña.";
                return View();
            }

        }

        #endregion

        #region Nosotros
        [HttpGet]

        public ActionResult Nosotros()
        {
            return View();
        }
    }
}
    #endregion

    