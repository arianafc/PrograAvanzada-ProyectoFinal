using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.EF;
using ProyectoFinal.Models;

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
                TempData["Error"] = "Hubo un problema al enviar la consulta.";
                return View("Contacto", model);
            }

            using (var db = new CASA_NATURAEntities())
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

                try
                {
                    EnviarCorreoCasaNatura(nueva);
                }
                catch (Exception ex)
                {
                    TempData["ErrorCorreo"] = "SMTP: " + ex.Message +
                        (ex.InnerException != null ? " | INNER: " + ex.InnerException.Message : "");
                }
            }

            TempData["Mensaje"] = "Consulta enviada con éxito.";
            return RedirectToAction("Contacto");
        }

        [HttpGet]
        public ActionResult GestionDudas()
        {
            using (var db = new CASA_NATURAEntities())
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
            using (var db = new CASA_NATURAEntities())
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