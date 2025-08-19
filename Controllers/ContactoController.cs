using ProyectoFinal.EF;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ContactoController : Controller
    {
        // GET: /Contacto
        [HttpGet]
        public ActionResult Contacto()
        {
            return View();
        }

        // POST: /Contacto/EnviarConsulta
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public ActionResult EnviarConsulta(ConsultaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SwalError"] = "Hubo un problema al enviar la consulta.";
                return View("Contacto", model);
            }

            using (var db = new CASA_NATURAEntities())
            {
                try
                {
                    var estadoPendienteId = db.ESTADOS_TB
                                              .Where(e => e.DESCRIPCION == "Pendiente")
                                              .Select(e => e.ID_ESTADO)
                                              .FirstOrDefault();

                    if (estadoPendienteId == 0)
                    {
                        TempData["SwalError"] = "No se encontró el estado 'Pendiente' en ESTADOS_TB.";
                        return View("Contacto", model);
                    }

                    int? idUsuarioActual = null;
                    if (User?.Identity?.IsAuthenticated == true)
                    {
                        var emailLogin = User.Identity.Name;
                        var u = db.USUARIOS_TB.Where(x => x.CORREO == emailLogin)
                                              .Select(x => new { x.ID_USUARIO })
                                              .FirstOrDefault();
                        if (u != null) idUsuarioActual = u.ID_USUARIO;
                    }

                    var nueva = new CONSULTAS_TB
                    {
                        NOMBRE = model.Nombre,
                        APELLIDO = model.Apellido,
                        CORREO = model.Correo,
                        MENSAJE = model.Mensaje,
                        FECHA = DateTime.Now,
                        ID_USUARIO = idUsuarioActual,
                        ID_ESTADO = estadoPendienteId
                    };

                    db.CONSULTAS_TB.Add(nueva);
                    db.SaveChanges();

                    try
                    {
                        EnviarCorreoCasaNatura(nueva);
                    }
                    catch (Exception exCorreo)
                    {
                        TempData["SwalError"] = "Error al enviar correo: " + exCorreo.Message;
                        Utilitarios.RegistrarError(exCorreo, idUsuarioActual);
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException exDb)
                {
                    var msg = exDb.InnerException?.InnerException?.Message
                              ?? exDb.InnerException?.Message
                              ?? exDb.Message;

                    TempData["SwalError"] = "Error al guardar la consulta: " + msg;
                    Utilitarios.RegistrarError(exDb, null); 

                    return View("Contacto", model);
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "Ocurrió un error inesperado al procesar la consulta.";
                    Utilitarios.RegistrarError(ex, null);
                    return View("Contacto", model);
                }
            }

            TempData["Mensaje"] = "Consulta enviada con éxito.";
            ModelState.Clear();
            return RedirectToAction("Contacto");
        }


        [HttpGet]
        [FiltroAdministrador]
        public ActionResult GestionDudas()
        {
            try
            {
                using (var db = new CASA_NATURAEntities())
                {
                    var estadoPendienteId = db.ESTADOS_TB
                        .Where(e => e.DESCRIPCION == "Pendiente")
                        .Select(e => e.ID_ESTADO)
                        .FirstOrDefault();

                    var estadoResueltoId = db.ESTADOS_TB
                        .Where(e => e.DESCRIPCION == "Resuelto")
                        .Select(e => e.ID_ESTADO)
                        .FirstOrDefault();

                    if (estadoPendienteId == 0 || estadoResueltoId == 0)
                    {
                        TempData["Error"] = "No se encontraron los estados 'Pendiente' o 'Resuelto' en ESTADOS_TB.";
                        return View("GestionDudas", Tuple.Create(
                            new List<ConsultaViewModel>(),
                            new List<ConsultaViewModel>()
                        ));
                    }

                    var resueltas = (
                        from c in db.CONSULTAS_TB
                        join e in db.ESTADOS_TB on c.ID_ESTADO equals e.ID_ESTADO
                        where c.ID_ESTADO == estadoResueltoId
                        orderby c.FECHA descending
                        select new ConsultaViewModel
                        {
                            IdConsulta = c.ID_CONSULTA,
                            Nombre = c.NOMBRE,
                            Apellido = c.APELLIDO,
                            Correo = c.CORREO,
                            Mensaje = c.MENSAJE,
                            Fecha = c.FECHA,
                            FechaResuelta = c.FECHA_RESUELTA,
                            Estado = e.DESCRIPCION
                        }
                    ).ToList();

                    var pendientes = (
                        from c in db.CONSULTAS_TB
                        join e in db.ESTADOS_TB on c.ID_ESTADO equals e.ID_ESTADO
                        where c.ID_ESTADO == estadoPendienteId
                        orderby c.FECHA descending
                        select new ConsultaViewModel
                        {
                            IdConsulta = c.ID_CONSULTA,
                            Nombre = c.NOMBRE,
                            Apellido = c.APELLIDO,
                            Correo = c.CORREO,
                            Mensaje = c.MENSAJE,
                            Fecha = c.FECHA,
                            Estado = e.DESCRIPCION
                        }
                    ).ToList();

                    return View("GestionDudas", Tuple.Create(resueltas, pendientes));
                }
            }
            catch (Exception ex)
            {
               
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);

                TempData["SwalError"] = "Ocurrió un error al cargar las dudas.";

                
                return View("GestionDudas", Tuple.Create(
                    new List<ConsultaViewModel>(),
                    new List<ConsultaViewModel>()
                ));
            }
        }

        [HttpPost]
        public ActionResult MarcarComoResuelta(int id)
        {
            try
            {
                using (var db = new CASA_NATURAEntities())
                {
                    var consulta = db.CONSULTAS_TB.Find(id);
                    if (consulta != null)
                    {
                        var estadoResueltoId = db.ESTADOS_TB
                            .Where(e => e.DESCRIPCION == "Resuelto")
                            .Select(e => e.ID_ESTADO)
                            .FirstOrDefault();

                        if (estadoResueltoId == 0)
                        {
                            TempData["SwalError"] = "No se encontró el estado 'Resuelto' en la tabla ESTADOS_TB.";
                            return RedirectToAction("GestionDudas");
                        }

                        consulta.ID_ESTADO = estadoResueltoId;
                        consulta.FECHA_RESUELTA = DateTime.Now;

                        db.SaveChanges();
                    }
                }

                TempData["SwalSuccess"] = "La consulta fue marcada como resuelta correctamente.";
            }
            catch (Exception ex)
            {
                
                Utilitarios.RegistrarError(ex, (int?)Session["idUsuario"]);

                TempData["SwalError"] = "Ocurrió un error al intentar marcar la consulta como resuelta.";
            }

            return RedirectToAction("GestionDudas");
        }

        // ---------- Privados ----------
        private void EnviarCorreoCasaNatura(CONSULTAS_TB c)
        {
            var remitente = System.Configuration.ConfigurationManager.AppSettings["CorreoRemitente"];
            var claveApp = System.Configuration.ConfigurationManager.AppSettings["CorreoPassword"];
            var fromName = "Casa Natura";

            var destino = remitente;

            var asunto = $"[Consulta #{c.ID_CONSULTA}] Nueva consulta de {c.NOMBRE} {c.APELLIDO}";

            // Sanitizar para HTML
            var nombre = HttpUtility.HtmlEncode(c.NOMBRE ?? "");
            var apellido = HttpUtility.HtmlEncode(c.APELLIDO ?? "");
            var correoUsuario = HttpUtility.HtmlEncode(c.CORREO ?? "");
            var correoUsuarioAttr = HttpUtility.HtmlAttributeEncode(c.CORREO ?? "");
            var mensajeHtml = HttpUtility.HtmlEncode(c.MENSAJE ?? "").Replace("\r\n", "<br/>").Replace("\n", "<br/>");

            // Mailto
            var asuntoReply = Uri.EscapeDataString($"Re: Consulta Casa Natura #{c.ID_CONSULTA}");
            var bodyReply = Uri.EscapeDataString($"Hola {c.NOMBRE},\r\n\r\n");

            var cuerpoHtml = $@"
<h2>Nueva consulta recibida</h2>
<p><strong>ID:</strong> {c.ID_CONSULTA}</p>
<p><strong>Nombre:</strong> {nombre} {apellido}</p>
<p><strong>Correo:</strong> <a href=""mailto:{correoUsuarioAttr}?subject={asuntoReply}&body={bodyReply}"">{correoUsuario}</a></p>
<p><strong>Fecha:</strong> {c.FECHA:dd/MM/yyyy HH:mm}</p>
<p><strong>Mensaje:</strong><br/>{mensajeHtml}</p>";

            var mail = new MailMessage
            {
                From = new MailAddress(remitente, fromName),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            mail.To.Add(destino);

            if (!string.IsNullOrWhiteSpace(c.CORREO))
            {
                mail.ReplyToList.Clear();
                mail.ReplyToList.Add(new MailAddress(c.CORREO, $"{c.NOMBRE} {c.APELLIDO}".Trim()));
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = false;

            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(remitente, claveApp);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 20000;

                    smtp.Send(mail);
                    return;
                }
            }
            catch
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 465))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(remitente, claveApp);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 20000;

                    smtp.Send(mail);
                }
            }
        }
    }
}