using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dashboard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

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
                
                var result = dbContext.LOGIN_SP(usuario.Correo, usuario.Contrasenna).FirstOrDefault();

                if (result != null)
                {
                    Session["Nombre"] = result.NOMBRE;
                    Session["Apellido1"] = result.APELLIDO1;
                    Session["Cedula"] = result.IDENTIFICACION;
                    Session["idUsuario"] = result.ID_USUARIO;

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
                    var result = dbContext.REGISTRO_SP(
                        usuario.Nombre,
                        usuario.Apellido1,
                        usuario.Apellido2,
                        usuario.Correo,
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
                    return RedirectToAction("Registro");
                }
            }
        }


        public ActionResult Donaciones()
        {
            ViewBag.Message = "Your Donaciones page.";

            return View();
        }

        public ActionResult FormularioDonar()
        {
            ViewBag.Message = "Your Formulario page.";

            return View();
        }



        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("IniciarSesion", "Home");
        }
    }
}