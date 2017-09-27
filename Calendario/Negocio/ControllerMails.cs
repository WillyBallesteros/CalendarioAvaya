using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using Calendario.Datos;

namespace Calendario.Negocio
{
    public class ControllerMails
    {
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string CcEmail { get; set; }

        public static bool SendMail(ControllerMails mailData)
        {
            var message = new MailMessage();
            var mimeType = new ContentType("text/html");
            var body = HttpUtility.HtmlDecode(mailData.Message);
            var alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
            message.AlternateViews.Add(alternate);
            message.From = new MailAddress(mailData.FromMail, "Case Studies Team");
            message.To.Add(mailData.ToMail);
            message.Subject = mailData.Subject;
            if (mailData.CcEmail != null) message.CC.Add(mailData.CcEmail);
            var client = new SmtpClient("localhost");

            try
            {
                client.Send(message);
                return true;

            }
            catch (SmtpException e)
            {
                return false;
            }

        }

        public static string GetPlantilla(string tipoEmail)
        {
            var fullPath = HttpContext.Current.Server.MapPath("~");
            var html = "";
            switch (tipoEmail)
            {
                case "solicitudUsuarioCliente":
                    html = File.ReadAllText(fullPath + "\\MailsTemplates\\solicitudUsuarioCliente.html");
                    break;
                case "passwordPerdido":
                    html = File.ReadAllText(fullPath + "\\MailsTemplates\\passwordPerdido.html");
                    break;
                case "solicitudUsuarioAdmin":
                    html = File.ReadAllText(fullPath + "\\MailsTemplates\\solicitudUsuarioAdmin.html");
                    break;
            }
            return html;
        }
        public static string GetContenidoMail(string tipoMail, Usuarios usuario)
        {
            var plantilla = GetPlantilla(tipoMail);
            var dataIndex = new Dictionary<string, string>{
                    {"{UsuarioNombre}", usuario.UsuarioNombre},
                    {"{UsuarioPassword}", usuario.UsuarioPassword},
                    {"{UsuarioAcceso}", usuario.UsuarioAcceso},
                    {"{UsuarioEmail}", usuario.UsuarioEmail},
                    {"{UsuarioEstado}", usuario.UsuarioEstado},
                    {"{UsuarioNivel}", usuario.UsuarioNivel},
             };

            //Recorrer la plantilla del index para reemplazar el contenido
            foreach (var datos in dataIndex)
            {
                var buscar = datos.Key;
                var reemplazar = datos.Value;
                var index = plantilla.Replace(buscar, reemplazar);
                plantilla = index;
            }
            return plantilla;
        }
    }
}