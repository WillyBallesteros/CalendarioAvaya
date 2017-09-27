using Calendario.Datos;
using Calendario.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace Calendario.Negocio
{
    public class UsuariosBl
    {
        public static string RecoveryPassword(Usuarios usuario)
        {
            usuario.UsuarioPassword = Membership.GeneratePassword(7, 0);
            var user = UsuariosDal.RecoveryPassword(usuario);
            if (user != null)
            {
                string plantilla = ControllerMails.GetContenidoMail("passwordPerdido", user);
                var correo = new ControllerMails
                {
                    FromMail = "creative@avaya.com",
                    Message = plantilla,
                    ToMail = usuario.UsuarioEmail,
                    Subject = "Password Generated"
                };
                if (ControllerMails.SendMail(correo))
                {
                    return "{\"status\":\"ok\",\"data\":\"ok\"}";
                }
                else
                {
                    return "{\"status\":\"error\",\"data\":\"error\"}";
                }
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string Login(Usuarios usuario)
        {

            var user = UsuariosDal.Login(usuario);
            if (user != null)
            {
                if (usuario.UsuarioPassword == user.password && user.status == "Active")
                {
                    if (user.accessCampaignCalendar == "YES")
                    {
                        return "{\"status\":\"ok\",\"data\":\"BackEnd.html\",\"email\":\"" + user.email + "\",\"name\":\"" + user.firstName + "\",\"usuarioId\":\"" + user.managerId + "\"}";
                    }
                    return "{\"status\":\"ok\",\"data\":\"Calendar.html\",\"email\":\"" + user.email + "\",\"name\":\"" + user.firstName + "\",\"usuarioId\":\"" + user.managerId + "\"}";
                }
                if (usuario.UsuarioPassword != user.password)
                {
                    return "{\"status\":\"error\",\"data\":\"NoPassword\",\"email\":\"error\",\"name\":\"error\"}";
                }
                if (user.status != "Active")
                {
                    return "{\"status\":\"error\",\"data\":\"NoActive\",\"email\":\"error\",\"name\":\"error\"}";
                }
            }
            return "{\"status\":\"error\",\"data\":\"NoExist\",\"email\":\"error\",\"name\":\"error\"}";
        }

        public static string Login(string correo)
        {
            var user = UsuariosDal.Login(correo);
            if (user != null)
            {
                if (user.UsuarioNivel == "Admin User")
                {
                    return "{\"status\":\"ok\",\"data\":\"BackEnd.html\",\"email\":\"" + user.UsuarioEmail + "\",\"name\":\"" + user.UsuarioNombre + "\",\"usuarioId\":\"" + user.UsuarioId + "\"}";
                }
                return "{\"status\":\"ok\",\"data\":\"Calendar.html\",\"email\":\"" + user.UsuarioEmail + "\",\"name\":\"" + user.UsuarioNombre + "\",\"usuarioId\":\"" + user.UsuarioId + "\"}";
            }
            return "{\"status\":\"error\",\"data\":\"NoExist\",\"email\":\"error\",\"name\":\"error\"}";
        }

        public static string ValidateUser(string email, string usuarioNivel, string name)
        {

            if (UsuariosDal.ValidateUser(email, usuarioNivel))
            {
                return "{\"status\":\"ok\",\"data\":\"ok\",\"email\":\"" + email + "\",\"name\":\"" + name + "\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\",\"email\":\"" + email + "\",\"name\":\"" + name + "\"}";
        }



        public static string ChangePassword(string email, string newPassword)
        {
            string resultado;
            if (UsuariosDal.ChangePassword(email, newPassword))
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        /* Users */
        public static string GetUsers()
        {
            var resultado = "";
            List<Usuarios> result = UsuariosDal.GetUsers();

            try
            {
                resultado = JsonConvert.SerializeObject(result);
                resultado = "{\"status\":\"ok\",\"data\":" + resultado + "}";
            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":" + ex.Message.ToString() + "}";
            }
            return resultado;
        }

        public static string UpdateUser(Usuarios usuarios)
        {
            if (UsuariosDal.UpdateUser(usuarios))
            {
                string resultado = GetUsers();
                return resultado;
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string DeleteUser(int userId)
        {
            if (UsuariosDal.DeleteUser(userId))
            {
                string resultado = GetUsers();
                return resultado;
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string ActiveUser(int userId, string status)
        {
            if (UsuariosDal.ActiveUser(userId, status))
            {
                string resultado = GetUsers();
                return resultado;
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }




        public static string AddNewUser(Usuarios usuarios)
        {
            if (UsuariosDal.AddNewUser(usuarios))
            {
                string resultado = GetUsers();
                return resultado;
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string AddNewUserClient(Usuarios usuarios)
        {
            if (UsuariosDal.AddNewUser(usuarios))
            {
                try
                {
                    string plantilla = ControllerMails.GetContenidoMail("solicitudUsuarioCliente", usuarios);
                    var correo = new ControllerMails
                    {
                        FromMail = "creative@avaya.com",
                        Message = plantilla,
                        ToMail = usuarios.UsuarioEmail,
                        Subject = "Request Access"
                    };
                    ControllerMails.SendMail(correo);
                    string plantilla2 = ControllerMails.GetContenidoMail("solicitudUsuarioAdmin", usuarios);
                    var correo2 = new ControllerMails
                    {
                        FromMail = "creative@avaya.com",
                        Message = plantilla2,
                        ToMail = "vparra@avaya.com",
                        Subject = "New Request Access"
                    };
                    ControllerMails.SendMail(correo2);
                    return "{\"status\":\"ok\",\"data\":\"ok\"}";
                }
                catch (Exception ex)
                {
                    return "{\"status\":\"error\",\"data\":\"error\"}";
                }
            }
            else
            {
                return "{\"status\":\"error\",\"data\":\"error\"}";
            }
        }
    }
}