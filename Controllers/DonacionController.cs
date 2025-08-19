using iText.Kernel.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class DonacionController : Controller
    {
        readonly Utilitarios service = new Utilitarios();
        [HttpGet]
        public ActionResult Donaciones()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Error al cargar las donaciones." + ex.Message;
                return View();
            }
        }

        [HttpGet]
        [FiltroAdministrador]
        public ActionResult AdminDonaciones()
        {
            try
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
                                          Monto = d.MONTO ?? 0,  
                                          Fecha = d.FECHA ?? DateTime.MinValue
                                      }).ToList();

                    return View("AdminDonaciones", donaciones);
                }
            }
            catch (Exception ex)
            {
                
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);

                TempData["SwalError"] = "Ocurrió un error al cargar las donaciones.";
                return View("AdminDonaciones", new List<DonacionViewModel>());
            }
        }


        [HttpGet]
        [FiltroSesion]
        public ActionResult FormularioDonar()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Error al cargar el formulario." + ex.Message;
                return View();
            }
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
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalSuccess"] = "Error al guardar la donación: " +
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
            try
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

                    if (!donaciones.Any())
                    {
                        TempData["SwalError"] = "No hay donaciones registradas para exportar.";
                        return RedirectToAction("AdminDonaciones");
                    }

                   
                    var ms = new MemoryStream();

                    var document = new Document(PageSize.A4, 25, 25, 30, 30);
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);
                    writer.CloseStream = false;

                    document.Open();

                  
                    var titulo = new Paragraph("Reporte de Donaciones\n\n",
                        FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK))
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(titulo);

                    var table = new PdfPTable(5) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 10f, 25f, 25f, 15f, 25f });

                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                    var cellHeaderBackground = new BaseColor(6, 45, 62); // #062D3E

                    void AddHeaderCell(string text)
                    {
                        var cell = new PdfPCell(new Phrase(text, headerFont))
                        {
                            BackgroundColor = cellHeaderBackground,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 5
                        };
                        table.AddCell(cell);
                    }

                    AddHeaderCell("ID");
                    AddHeaderCell("Usuario");
                    AddHeaderCell("Correo");
                    AddHeaderCell("Monto");
                    AddHeaderCell("Fecha");

                    var rowFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

                    foreach (var d in donaciones)
                    {
                        table.AddCell(new Phrase(d.ID_DONACION.ToString(), rowFont));
                        table.AddCell(new Phrase(d.Nombre, rowFont));
                        table.AddCell(new Phrase(d.CORREO, rowFont));
                        table.AddCell(new Phrase(d.MONTO.HasValue
                            ? d.MONTO.Value.ToString("C", new System.Globalization.CultureInfo("es-CR"))
                            : "N/A", rowFont));
                        table.AddCell(new Phrase(d.FECHA.HasValue
                            ? d.FECHA.Value.ToString("dd/MM/yyyy")
                            : "N/A", rowFont));
                    }

                    document.Add(table);
                    document.Add(new Paragraph($"\nGenerado el {DateTime.Now:dd/MM/yyyy HH:mm}",
                        FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, BaseColor.GRAY)));

                    document.Close();
                    ms.Position = 0;

                    return File(ms, "application/pdf", "Donaciones.pdf");
                }
            }
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);
                TempData["SwalError"] = "Error al generar el PDF: " + ex.Message;
                return RedirectToAction("AdminDonaciones");
            }
        }



        [HttpPost]
        public ActionResult ExportarTXT()
        {
            try
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
            catch (Exception ex)
            {
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);

                TempData["SwalError"] = "Ocurrió un error al exportar el TXT.";
                return RedirectToAction("AdminDonaciones");
            }
        }

    }
}
