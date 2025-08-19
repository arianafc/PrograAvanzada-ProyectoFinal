using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoFinal.Controllers
{
    public class UsuariosController : Controller
    {

        #region GestionUsuarios
        [HttpGet]
        public ActionResult GestionUsuarios()
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {

                    var usuarios = dbContext.ObtenerUsuariosSP().ToList();
                    var roles = dbContext.ROLES_TB.ToList();

                    var DatosModelo = new GestionUsuariosModel
                    {
                        Usuarios = usuarios,
                        Roles = roles,
                    };

                    return View(DatosModelo);

                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error al cargar los usuarios: " + e.Message;
                return RedirectToAction("Dashboard", "Dashboard");
            }
        }
        #endregion

        #region CambiarEstado
        [HttpPost]
        public ActionResult CambioEstadoUsuario(int IdEstado, int IdUsuario)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var usuario = dbContext.USUARIOS_TB.FirstOrDefault(u => u.ID_USUARIO == IdUsuario);

                    if (usuario == null)
                    {
                      
                        TempData["SwalError"] = "El usuario no existe.";
                        return RedirectToAction("GestionUsuarios", "Usuarios");
                    }

                    usuario.ID_ESTADO = IdEstado;
                    dbContext.SaveChanges();

                    TempData["SwalSuccess"] = "Estado actualizado correctamente.";
                    return RedirectToAction("GestionUsuarios", "Usuarios");
                }
            }
            catch (Exception ex)
            {
                
                TempData["ErrorMessage"] = "Ocurrió un error al cambiar el estado del usuario." + ex.Message;
                return RedirectToAction("GestionUsuarios", "Usuarios");
            }
        }

        #endregion

        #region EditarUsuario
        [HttpPost]
        public ActionResult EditarUsuario(int IdUsuario, string Nombre, string Apellido1, string Apellido2, string Correo, int IdRol, int IdEstado)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var usuario = dbContext.USUARIOS_TB.FirstOrDefault(u => u.ID_USUARIO == IdUsuario);
                    if (usuario == null)
                    {
                        return Json(new { success = false, message = "El usuario no existe." });
                    }

                    bool correoExiste = dbContext.USUARIOS_TB
                        .Any(u => u.CORREO.ToLower() == Correo.ToLower() && u.ID_USUARIO != IdUsuario);

                    if (correoExiste)
                    {
                        return Json(new { success = false, message = "El correo ingresado ya está registrado en otro usuario." });
                    }

                    usuario.NOMBRE = Nombre;
                    usuario.APELLIDO1 = Apellido1;
                    usuario.APELLIDO2 = Apellido2;
                    usuario.CORREO = Correo.ToLower();
                    usuario.ID_ROL = IdRol;
                    usuario.ID_ESTADO = IdEstado;

                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Usuario actualizado correctamente." });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Ocurrió un error al actualizar el usuario: " + e.Message });
            }
        }

        #endregion
    }

}