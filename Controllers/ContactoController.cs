using System;
using System.Linq;
using System.Web.Mvc;
using ProyectoFinal.EF;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    public class ContactoController : Controller
    {
        public ActionResult Contacto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarConsulta(ConsultaViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new CASA_NATURAEntities1())
                {
                    var nueva = new CONSULTAS_TB
                    {
                        NOMBRE = model.Nombre,
                        APELLIDO = model.Apellido,
                        CORREO = model.Correo,
                        MENSAJE = model.Mensaje,
                        ESTADO = "Pendiente",
                        FECHA = DateTime.Now
                    };

                    db.CONSULTAS_TB.Add(nueva);
                    db.SaveChanges();
                }

                TempData["Mensaje"] = "Consulta enviada con éxito.";
                return RedirectToAction("Contacto");
            }

            TempData["Error"] = "Hubo un problema al enviar la consulta.";
            return View("Contacto", model);
        }

        [HttpGet]
        public ActionResult GestionDudas()
        {
            using (var db = new CASA_NATURAEntities1())
            {
                var resueltas = db.CONSULTAS_TB
                    .Where(c => c.ESTADO == "Resuelto")
                    .Select(c => new ConsultaViewModel
                    {
                        IdConsulta = c.ID_CONSULTA,
                        Nombre = c.NOMBRE,
                        Apellido = c.APELLIDO,
                        Correo = c.CORREO,
                        Mensaje = c.MENSAJE,
                        Fecha = c.FECHA,
                        Estado = c.ESTADO,
                        FechaResuelta = c.FECHA_RESUELTA
                    }).ToList();

                var pendientes = db.CONSULTAS_TB
                    .Where(c => c.ESTADO == "Pendiente")
                    .Select(c => new ConsultaViewModel
                    {
                        IdConsulta = c.ID_CONSULTA,
                        Nombre = c.NOMBRE,
                        Apellido = c.APELLIDO,
                        Correo = c.CORREO,
                        Mensaje = c.MENSAJE,
                        Fecha = c.FECHA,
                        Estado = c.ESTADO
                    }).ToList();

                return View("GestionDudas", Tuple.Create(resueltas, pendientes));
            }
        }

        [HttpPost]
        public ActionResult MarcarComoResuelta(int id)
        {
            using (var db = new CASA_NATURAEntities1())
            {
                var consulta = db.CONSULTAS_TB.Find(id);
                if (consulta != null)
                {
                    consulta.ESTADO = "Resuelto";
                    consulta.FECHA_RESUELTA = DateTime.Now;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("GestionDudas");
        }
    }
}
