using ProyectoFinal.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ProyectoFinal.Services
{
    public class Utilitarios
    {
        public string GenerarPassword(int longitud = 8)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var sb = new StringBuilder(longitud);

            for (int i = 0; i < longitud; i++)
            {
                int index = random.Next(caracteres.Length);
                sb.Append(caracteres[index]);
            }

            return sb.ToString();
        }

        public bool EnviarCorreo(string destinatario, string mensaje, string asunto)
        {
            try
            {
                var remitente = ConfigurationManager.AppSettings["CorreoRemitente"];
                var contrasena = ConfigurationManager.AppSettings["CorreoPassword"];

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(remitente),
                    To = { destinatario },
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true
                };

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(remitente, contrasena),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;

                //El servidor SMTP requiere una conexión segura o el cliente no se autenticó. La respuesta del servidor fue: 5.7.0 Authentication Required. For more information, go to
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int ObtenerIdMetodo(string metodo)
        {
            switch (metodo?.ToLower())
            {
                case "tarjeta": return 1;
                case "sinpe": return 2;
                case "paypal": return 3;
                default: return 0;
            }
        }

        public static void RegistrarError(Exception ex, int? idUsuario = null)
        {
            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var error = new REGISTRO_ERRORES_TB
                    {
                        ID_USUARIO = idUsuario,
                        FECHA_ACTUAL = DateTime.Now,
                        MENSAJE = ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "")
                    };

                    dbContext.REGISTRO_ERRORES_TB.Add(error);
                    dbContext.SaveChanges();
                }
            }
            catch
            {
               
            }
        }

    }
}