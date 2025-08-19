using System.Web.Mvc;

namespace ProyectoFinal.Models
{
    public class FiltroSesion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var contexto = filterContext.HttpContext;

            if (contexto.Session.Count == 0)
            {
                filterContext.Controller.TempData["SwalError"] = "Debes iniciar sesión para acceder a esta página.";
                filterContext.Result = new RedirectResult("~/Home/IniciarSesion");
            }
            base.OnActionExecuting(filterContext);
        }

    }
    public class FiltroAdministrador : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var contexto = filterContext.HttpContext;

            if (contexto.Session.Count == 0 || contexto.Session["IdRol"].ToString() != "2")
            {
                filterContext.Controller.TempData["SwalError"] = "No tienes permiso para acceder a esta página.";
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}