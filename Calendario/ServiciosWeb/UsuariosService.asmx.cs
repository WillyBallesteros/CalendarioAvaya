using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Calendario.Datos;
using Calendario.Negocio;
using Newtonsoft.Json;

namespace Calendario.ServiciosWeb
{
    /// <summary>
    /// Descripción breve de UsuariosService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class UsuariosService : System.Web.Services.WebService
    {

        /*  Login  */
        [WebMethod(EnableSession = true)]
        public string RecoveryPassword(Usuarios usuarios)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.RecoveryPassword(usuarios);
            return datos;
        }

        public class Respuesta
        {
            public string data { get; set; }
            public string status { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public string usuarioId { get; set; }

        }

        [WebMethod(EnableSession = true)]
        public string Login(Usuarios usuarios)
        {
            
            string datos = UsuariosBl.Login(usuarios);
            var sessionUsuario = HttpContext.Current.Session;
            var data = JsonConvert.DeserializeObject<Respuesta>(datos);
            sessionUsuario["miusuario"] = usuarios.UsuarioAcceso;
            sessionUsuario["UsuarioId"] = data.usuarioId;
            string[] nombres = data.name.Split(' ');
            sessionUsuario["name"] = nombres[0];
            sessionUsuario["email"] = data.email;
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string Login2(string tales)
        {
            
            string datos = UsuariosBl.Login(tales);
            var sessionUsuario = HttpContext.Current.Session;
            var data = JsonConvert.DeserializeObject<Respuesta>(datos);
            sessionUsuario["miusuario"] = tales;
            sessionUsuario["UsuarioId"] = data.usuarioId;
            string[] nombres = data.name.Split(' ');
            sessionUsuario["name"] = nombres[0];
            sessionUsuario["email"] = data.email;
            return datos;
        }

        

        [WebMethod(EnableSession = true)]
        public string ValidateUser(string usuarioNivel)
        {
            var sessionUsuario = HttpContext.Current.Session;
            var email = sessionUsuario["email"];
            var name = sessionUsuario["name"];
            if (email != null && name != null)
            {
                string datos = UsuariosBl.ValidateUser(email.ToString(), usuarioNivel, name.ToString());
                return datos;
            }
            return "{\"status\":\"error\",\"data\":\"error\",\"email\":\"error\"}";
        }

        [WebMethod(EnableSession = true)]
        public string Exit(string vista)
        {
            var sessionUsuario = HttpContext.Current.Session;
            var resultado = "";
            try
            {
                sessionUsuario.Clear();
                sessionUsuario.Abandon();
                resultado = "ok";
            }
            catch (Exception ex)
            {
                resultado = "error";
                //utilidades.WriteError(ex.Message, "Utilities.asmx", "cerrarSession");
            }
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public string ChangePassword(string newPassword)
        {
            var sessionUsuario = HttpContext.Current.Session;
            var email = sessionUsuario["email"];
            string datos = UsuariosBl.ChangePassword(email.ToString(), newPassword);
            return datos;
        }

        /** Users **/

        [WebMethod(EnableSession = true)]
        public string GetUsers(string data)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.GetUsers();
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateUser(Usuarios usuarios)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.UpdateUser(usuarios);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteUser(int userId)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.DeleteUser(userId);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string AddNewUser(Usuarios usuarios)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.AddNewUser(usuarios);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string AddNewUserClient(Usuarios usuarios)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.AddNewUserClient(usuarios);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string ActiveUser(int userId, string status)
        {
            //if (!Utilities.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = UsuariosBl.ActiveUser(userId, status);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public static bool SessionExists()
        {
            var sessionUsuario = HttpContext.Current.Session;
            return (sessionUsuario["UsuarioId"] != null);
        }

        
    }
}
