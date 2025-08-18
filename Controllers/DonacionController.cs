using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProyectoFinal.Controllers
{
    public class DonacionController : Controller
    {
        readonly Utilitarios service = new Utilitarios();
        [HttpGet]
        public ActionResult Donaciones()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminDonaciones()
        {
            using (var db = new CASA_NATURAEntities())
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
                int idMetodo = service.ObtenerIdMetodo(metodo);

                if (idMetodo == 0)
                {
                    ViewBag.Error = "Método de pago no válido.";
                    return View();
                }

                // Solo usamos la referencia si el método es SINPE
                string referenciaFinal = (idMetodo == 2) ? referencia : null;

                using (var db = new CASA_NATURAEntities())
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

        [HttpPost]
        public ActionResult ExportarPDF()
        {
            using (var db = new CASA_NATURAEntities())
            {
                var donaciones = (from d in db.DONACIONES_TB
                                  join u in db.USUARIOS_TB on d.ID_USUARIO equals u.ID_USUARIO
                                  select new
                                  {
                                      d.ID_DONACION,
                                      Nombre = u.NOMBRE + " " + u.APELLIDO1 + " " + u.APELLIDO2,
                                      u.CORREO,
                                      d.MONTO,
                                      d.FECHA
                                  }).ToList();

                var ms = new MemoryStream();

                var document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(document, ms).CloseStream = false;

                document.Open();

                var titulo = new Paragraph("Reporte de Donaciones\n\n",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                titulo.Alignment = Element.ALIGN_CENTER;
                document.Add(titulo);

                var table = new PdfPTable(5) { WidthPercentage = 100 };
                table.AddCell("ID");
                table.AddCell("Usuario");
                table.AddCell("Correo");
                table.AddCell("Monto");
                table.AddCell("Fecha");

                foreach (var d in donaciones)
                {
                    table.AddCell(d.ID_DONACION.ToString());
                    table.AddCell(d.Nombre);
                    table.AddCell(d.CORREO);
                    table.AddCell(d.MONTO.Value.ToString("C", new System.Globalization.CultureInfo("es-CR")));
                    table.AddCell(d.FECHA.Value.ToString("dd/MM/yyyy"));
                }

                document.Add(table);
                document.Close();

                ms.Position = 0;
                return File(ms, "application/pdf", "Donaciones.pdf");
            }
        }

        [HttpPost]
        public ActionResult ExportarTXT()
        {
            using (var db = new CASA_NATURAEntities())
            {
                var donaciones = (from d in db.DONACIONES_TB
                                  join u in db.USUARIOS_TB on d.ID_USUARIO equals u.ID_USUARIO
                                  select new
                                  {
                                      d.ID_DONACION,
                                      Nombre = u.NOMBRE + " " + u.APELLIDO1 + " " + u.APELLIDO2,
                                      u.CORREO,
                                      d.MONTO,
                                      d.FECHA
                                  }).ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ID | Usuario | Correo | Monto | Fecha");
                sb.AppendLine("----------------------------------------------------------");

                foreach (var d in donaciones)
                {
                    sb.AppendLine($"{d.ID_DONACION} | {d.Nombre} | {d.CORREO} | {d.MONTO.Value.ToString("C", new System.Globalization.CultureInfo("es-CR"))} | {d.FECHA.Value:dd/MM/yyyy}");
                }

                byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(fileBytes, "text/plain", "Donaciones.txt");
            }
        }
    }
}
