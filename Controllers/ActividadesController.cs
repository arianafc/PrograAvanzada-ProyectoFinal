using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Ajax.Utilities;
using Microsoft.Win32.SafeHandles;
using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using Image = iText.Layout.Element.Image;
using Table = iText.Layout.Element.Table;

namespace ProyectoFinal.Controllers
{
    public class ActividadesController : Controller
    {

        Utilitarios service = new Utilitarios();

        #region GestionActividades
        [HttpGet]
        public ActionResult GestionActividades()
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {

                    var result = dbContext.VisualizarActividadesSP().ToList();

                    var viewModel = new GestionActividadesModel
                    {
                        NuevaActividad = new Actividad(),
                        ListaActividades = result
                    };

                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al cargar las actividades: " + ex.Message;

                return View(new GestionActividadesModel
                {
                    NuevaActividad = new Actividad(),
                    ListaActividades = new List<VisualizarActividadesSP_Result>()
                });
            }
        }
 


        [HttpPost]
        public ActionResult GestionActividades(GestionActividadesModel actividad, HttpPostedFileBase ImagenActividad, String Hora)

{
    try
    {
        using (var dbContext = new CASA_NATURAEntities())
        {

            var actividades = new ACTIVIDADES_TB();
            DateTime fechaCompleta;
            if (DateTime.TryParse(actividad.NuevaActividad.Fecha.ToString("yyyy-MM-dd") + " " + Hora, out fechaCompleta))
            {
                actividades.FECHA = fechaCompleta;
            }

            actividades.ID_ACTIVIDAD = 0;
            actividades.NOMBRE = actividad.NuevaActividad.Nombre;
            actividades.DESCRIPCION = actividad.NuevaActividad.Descripcion;
            actividades.PRECIO_BOLETO = actividad.NuevaActividad.PrecioBoleto;
            actividades.TICKETS_DISPONIBLES = actividad.NuevaActividad.TicketsDisponibles;
            actividades.ID_ESTADO = 1;
            actividades.IMAGEN = string.Empty;
            actividades.TIPO = actividad.NuevaActividad.Tipo;

            dbContext.ACTIVIDADES_TB.Add(actividades);
            var result = dbContext.SaveChanges();

            if (result > 0)
            {
                string extension = System.IO.Path.GetExtension(ImagenActividad.FileName);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\" + actividades.ID_ACTIVIDAD + extension;
                ImagenActividad.SaveAs(ruta);

                actividades.IMAGEN = "/Imagenes/" + actividades.ID_ACTIVIDAD + extension;
                dbContext.SaveChanges();

                return RedirectToAction("GestionActividades", "Actividades");


            }

            TempData["SwalSuccess"] = "Actividad registrado con éxito";
            return View();
        }
    }
    catch (Exception ex)
    {

        TempData["SwalError"] = ex.InnerException?.Message ?? ex.Message;
        return View();
    }
}


        
        #endregion

        #region CambiarEstadoActividad

        [HttpPost]
        public ActionResult CambioEstadoActividad(int IdEstado, int IdActividad)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var result = dbContext.CambioEstadoActividadSP(IdEstado, IdActividad);

                    if (result > 0)
                    {
                        TempData["SwalSuccess"] = "El estado de la actividad se actualizó correctamente.";
                        return RedirectToAction("GestionActividades", "Actividades");
                    }

                    TempData["SwalError"] = "No se pudo actualizar el estado de la actividad.";
                    return RedirectToAction("GestionActividades", "Actividades");
                }
            }
            catch (Exception ex)
            {
  
                TempData["SwalError"] = "Ocurrió un error al intentar cambiar el estado de la actividad."+ ex.Message;
                return RedirectToAction("GestionActividades", "Actividades");
            }
        }


        #endregion

        [HttpPost]
        public ActionResult EditarActividades(GestionActividadesModel actividad, HttpPostedFileBase ImagenActividad, string Hora)
        {
            try
            {
                var id = actividad.NuevaActividad.IdActividad;

                DateTime fechaCompleta;
                if (!DateTime.TryParse(actividad.NuevaActividad.Fecha.ToString("yyyy-MM-dd") + " " + Hora, out fechaCompleta))
                {
                    TempData["SwalError"] = "Fecha u hora inválida.";
                    return RedirectToAction("GestionActividades", "Actividades");
                }


                string rutaVirtual = null;

                if (ImagenActividad != null && ImagenActividad.ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(ImagenActividad.FileName);
                    string nombreArchivo = id + extension;
                    string rutaFisica = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", nombreArchivo);

                    ImagenActividad.SaveAs(rutaFisica);
                    rutaVirtual = "/Imagenes/" + nombreArchivo;
                }
                else
                {
                    rutaVirtual = actividad.NuevaActividad.Imagen;
                }

                using (var dbContext = new CASA_NATURAEntities())
                {

                    dbContext.EditarActividadSP(
                        id,
                        actividad.NuevaActividad.Descripcion,
                        fechaCompleta,
                        actividad.NuevaActividad.PrecioBoleto,
                        actividad.NuevaActividad.TicketsDisponibles,
                        rutaVirtual,
                        actividad.NuevaActividad.Tipo,
                        actividad.NuevaActividad.Nombre
                    );
                }

                TempData["SwalSuccess"] = "Actividad actualizada con éxito.";
                return RedirectToAction("GestionActividades", "Actividades");
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = ex.InnerException?.Message ?? ex.Message;
                return RedirectToAction("GestionActividades", "Actividades");
            }
        }

        [HttpGet]
        public ActionResult ActividadesDisponibles()
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    var result = dbcontext.VisualizarActividadesActivasSP().ToList();
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las actividades: " + ex.Message;
                return View(new List<VisualizarActividadesActivasSP_Result>());
            }
        }

        [HttpGet]

        public ActionResult DetalleActividad(int IdActividad)
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {

                    var result = dbcontext.DetalleActividadSP(IdActividad).FirstOrDefault();

                    if (result == null)
                    {
                        ViewBag.Error = "Actividad no encontrada";
                        return RedirectToAction("Index");
                    }

                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult ComprarBoletos(int IdActividad, int MetodoPago, int NumBoletos, string Referencia)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                    var usuario = dbContext.USUARIOS_TB.FirstOrDefault(u => u.ID_USUARIO == idUsuario);
                    var actividad = dbContext.ACTIVIDADES_TB.FirstOrDefault(a => a.ID_ACTIVIDAD == IdActividad);

                    if (usuario == null || actividad == null)
                    {
                        TempData["SwalError"] = "No se pudo encontrar la información del usuario o actividad.";
                        return RedirectToAction("ActividadesDisponibles", "Actividades");
                    }

                    if (string.IsNullOrWhiteSpace(Referencia))
                    {
                        Referencia = "N/A";
                    }

                    var result = dbContext.CompraActividadSP(idUsuario, MetodoPago, NumBoletos, IdActividad, Referencia);

                    if (result > 0)
                    {
                       
                        decimal total = actividad.PRECIO_BOLETO * NumBoletos;
                        string nombreUsuario = usuario.NOMBRE;
                        string correoDestino = usuario.CORREO;

                        StringBuilder mensaje = new StringBuilder();
                        mensaje.Append($"<p>Hola <strong>{nombreUsuario}</strong>,</p>");
                        mensaje.Append("<p>¡Gracias por tu compra en Casa Natura!</p>");
                        mensaje.Append("<h4>Detalles de la compra:</h4>");
                        mensaje.Append("<ul>");
                        mensaje.Append($"<li><strong>Actividad:</strong> {actividad.NOMBRE}</li>");
                        mensaje.Append($"<li><strong>Fecha:</strong> {actividad.FECHA.ToString("dd/MM/yyyy")} {actividad.FECHA.ToString("hh:mm tt")}</li>");
                        mensaje.Append($"<li><strong>Boletos comprados:</strong> {NumBoletos}</li>");
                        mensaje.Append($"<li><strong>Precio unitario:</strong> ₡{actividad.PRECIO_BOLETO:N2}</li>");
                        mensaje.Append($"<li><strong>Total pagado:</strong> ₡{total:N2}</li>");
                        mensaje.Append($"<li><strong>Método de pago:</strong> {MetodoPago}</li>");
                        mensaje.Append($"<li><strong>Referencia:</strong> {Referencia}</li>");
                        mensaje.Append("</ul>");
                        mensaje.Append("<p>Esperamos que disfrutes de la experiencia.<br>¡Nos vemos pronto!</p>");
                        mensaje.Append("<p><em>Casa Natura</em></p>");

                        bool enviado = service.EnviarCorreo(correoDestino, mensaje.ToString(), "Confirmación de compra - Casa Natura");

                        if (!enviado)
                        {
                            ViewBag.Mensaje = "La compra fue exitosa, pero no se pudo enviar el correo de confirmación.";
                            
                        }

                        TempData["SwalSuccess"] = "Compra realizada con éxito. Revisa tu correo para más detalles.";
                        return RedirectToAction("ActividadesDisponibles", "Actividades");
                    }

                    TempData["SwalError"] = "No se pudo completar la compra.";
                    return RedirectToAction("ActividadesDisponibles", "Actividades");
                }
            }
            catch (Exception ex)
            {
             
                TempData["SwalError"] = "Ocurrió un error al procesar la compra.";
                return RedirectToAction("ActividadesDisponibles", "Actividades");
            }
        }

        [HttpGet]

        public ActionResult GestionVentas()
        {
            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    
                    var result = dbcontext.VisualizacionVentasSP().ToList();
                    var SumaTotalVentas = result.Sum(x => x.TOTAL);
                    ViewBag.TotalVentas = SumaTotalVentas;
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar las actividades: " + ex.Message;
                return View(new List<VisualizacionVentasSP_Result>());
            }
            
        }


        [HttpGet]

        public ActionResult GenerarFactura(int NumeroFactura)
        {
           
        


            try
            {
                using (var dbcontext = new CASA_NATURAEntities())
                {
                    string logoPath = Server.MapPath("~/Imagenes/logo.png");
                    PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    var factura = dbcontext.VisualizacionVentasSP().FirstOrDefault(x => x.NUMERO_FACTURA == NumeroFactura);
                    if (factura == null)
                    {
                        TempData["SwalError"] = "ERROR: No pudimos generar la factura.";
                        return RedirectToAction("GestionVentas");
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        PdfWriter writer = new PdfWriter(ms);
                        PdfDocument pdf = new PdfDocument(writer);
                        Document doc = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                        doc.SetMargins(20, 20, 20, 20);

                        // --- Logo de fondo ---
                        ImageData imageData = ImageDataFactory.Create(logoPath);
                        Image logo = new Image(imageData)
                            .ScaleAbsolute(150, 150)
                            .SetFixedPosition(200, 500)
                            .SetOpacity(0.1f);
                        doc.Add(logo);

                        // --- Título ---
                        Paragraph titulo = new Paragraph("CASA NATURA - FACTURA")
                            .SetFontSize(24)
                            .SetFontColor(new DeviceRgb(6, 45, 62)) // #062D3E
                            .SetTextAlignment(TextAlignment.CENTER);
                            
                        doc.Add(titulo);

                        doc.Add(new Paragraph("\n")); // espacio

                        // --- Información del cliente ---
                        Table infoCliente = new Table(2, true);
                        infoCliente.SetWidth(UnitValue.CreatePercentValue(100));

                        infoCliente.AddCell(new Cell().Add(new Paragraph("Número de Factura:")).SetBorder(Border.NO_BORDER));
                        infoCliente.AddCell(new Cell().Add(new Paragraph(factura.NUMERO_FACTURA.ToString())).SetBorder(Border.NO_BORDER));

                        infoCliente.AddCell(new Cell().Add(new Paragraph("Fecha:")).SetBorder(Border.NO_BORDER));
                        infoCliente.AddCell(new Cell().Add(new Paragraph(factura.FECHA.ToString())).SetBorder(Border.NO_BORDER));

                        infoCliente.AddCell(new Cell().Add(new Paragraph("Cliente:")).SetBorder(Border.NO_BORDER));
                        infoCliente.AddCell(new Cell().Add(new Paragraph(factura.NOMBRE_COMPLETO)).SetBorder(Border.NO_BORDER));

                        infoCliente.AddCell(new Cell().Add(new Paragraph("Método de Pago:")).SetBorder(Border.NO_BORDER));
                        infoCliente.AddCell(new Cell().Add(new Paragraph(factura.METODO)).SetBorder(Border.NO_BORDER));

                        doc.Add(infoCliente);

                        doc.Add(new Paragraph("\n")); // espacio

                        // --- Tabla de actividades ---
                        Table tabla = new Table(4, true).UseAllAvailableWidth();
                        string[] headers = { "Actividad", "Tickets", "Precio Unitario", "Total" };
                        foreach (var header in headers)
                        {
                            tabla.AddHeaderCell(new Cell().Add(new Paragraph(header))
                                                         .SetBackgroundColor(new DeviceRgb(255, 193, 7)) // #FFC107
                                                         .SetFontColor(ColorConstants.BLACK)
                                                         .SetTextAlignment(TextAlignment.CENTER));
                        }

                        // Fila de datos
                        tabla.AddCell(new Cell().Add(new Paragraph(factura.NOMBRE_ACTIVIDAD)));
                        tabla.AddCell(new Cell().Add(new Paragraph(factura.TICKETS_ADQUIRIDOS.ToString())).SetTextAlignment(TextAlignment.CENTER));
                        tabla.AddCell(new Cell().Add(new Paragraph("₡" + (factura.TOTAL / factura.TICKETS_ADQUIRIDOS))).SetTextAlignment(TextAlignment.RIGHT));
                        tabla.AddCell(new Cell().Add(new Paragraph("₡" + factura.TOTAL)).SetTextAlignment(TextAlignment.RIGHT));

                        doc.Add(tabla);

                        doc.Add(new Paragraph("\n")); // espacio

                        // --- Total ---
                        Paragraph total = new Paragraph($"Total a Pagar: ₡{factura.TOTAL:N0}")
                            .SetFontSize(14)
                            .SetFontColor(ColorConstants.RED)
                            .SetTextAlignment(TextAlignment.RIGHT);
                           
                        doc.Add(total);

                        doc.Add(new Paragraph("\n")); // espacio

                        // --- Nota final ---
                        Paragraph nota = new Paragraph("¡Gracias por su compra! Esperamos verlo pronto.")
                            .SetFontSize(10)
                          
                            .SetTextAlignment(TextAlignment.CENTER);
                        doc.Add(nota);

                        doc.Close();

                        // Devolver PDF
                        return File(ms.ToArray(), "application/pdf", $"Factura_{factura.NUMERO_FACTURA}.pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                using (var ms = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(ms);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document doc = new Document(pdf);

                    doc.Add(new Paragraph("ERROR AL GENERAR FACTURA")
                        .SetFontSize(16)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                    doc.Add(new Paragraph("Detalle: " + ex.Message));

                    doc.Close();

                    return File(ms.ToArray(), "application/pdf", "ErrorFactura.pdf");
                }
            }
        }
    }

}
