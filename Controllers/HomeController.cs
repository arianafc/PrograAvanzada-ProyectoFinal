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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult dashboard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Signup()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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

    }
}