using Microsoft.Ajax.Utilities;
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

namespace ProyectoFinal.Controllers
{
    public class ActividadesController : Controller
    {
        // GET: Actividades
        Utilitarios service = new Utilitarios();

        [HttpGet]
        public ActionResult GestionActividades()
        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
                {
                    var result = dbContext.VisualizarActividadesSP().ToList();

                    var viewModel = new GestionActividadesModel
                    {
                        NuevaActividad = new Actividad(),
                        ListaActividades = result
                    };

                    return View(viewModel);
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
        }


        [HttpPost]

        public ActionResult GestionActividades(GestionActividadesModel actividad, HttpPostedFileBase ImagenActividad, String Hora)

        {
            using (var dbContext = new CASA_NATURAEntities())
            {
                try
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

                    dbContext.ACTIVIDADES_TB.Add(actividades);
                    var result = dbContext.SaveChanges();

                    if (result > 0)
                    {
                        string extension = Path.GetExtension(ImagenActividad.FileName);
                        string ruta = AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\" + actividades.ID_ACTIVIDAD + extension;
                        ImagenActividad.SaveAs(ruta);

                        actividades.IMAGEN = "/Imagenes/" + actividades.ID_ACTIVIDAD + extension;
                        dbContext.SaveChanges();

                        return RedirectToAction("GestionActividades", "Actividades");


                    }

                    TempData["SwalSuccess"] = "Actividad registrado con éxito";
                    return View();
                }
                catch (Exception ex)
                {

                    TempData["SwalError"] = ex.InnerException?.Message ?? ex.Message;
                    return RedirectToAction("Registro");
                }
            }

        }


        [HttpPost]

        public ActionResult CambioEstadoActividad(int IdEstado, int IdActividad) {

            using (var dbContext = new CASA_NATURAEntities())
            {
                var result = dbContext.CambioEstadoActividadSP(IdEstado, IdActividad);
                if (result > 0)
                {
                    return RedirectToAction("GestionActividades", "Actividades");
                }
                return View();
            }

        }

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
                    string extension = Path.GetExtension(ImagenActividad.FileName);
                    string nombreArchivo = id + extension;
                    string rutaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", nombreArchivo);

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
                        // Armar mensaje
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


    }

}
