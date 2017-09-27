using Calendario.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendario.Modelo
{
    public class UsuariosDal
    {
        public static List<Usuarios> GetUsers()
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    bd.Database.CommandTimeout = 380;
                    return bd.Usuarios.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public static bool UpdateUser(Usuarios usuarios)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => td.UsuarioId == usuarios.UsuarioId);

                    if (query != null)
                    {
                        bd.Entry(query).CurrentValues.SetValues(usuarios);
                        bd.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public static bool DeleteUser(int userId)
        {
            bool transact;
            Usuarios userToDelete;
            //1. Get student from DB
            using (var ctx = new calawebEntities())
            {
                userToDelete = ctx.Usuarios.FirstOrDefault(s => s.UsuarioId == userId);
            }

            //Create new context for disconnected scenario
            using (var newContext = new calawebEntities())
            {
                try
                {
                    newContext.Entry(userToDelete).State = System.Data.Entity.EntityState.Deleted;
                    newContext.SaveChanges();
                    transact = true;
                }
                catch (Exception ex)
                {
                    transact = false;
                }
            }
            return transact;
        }

        public static bool ActiveUser(int userId, string status)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => td.UsuarioId == userId);

                    if (query != null)
                    {
                        query.UsuarioEstado = status;
                        bd.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public static bool ChangePassword(string email, string newPassword)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => td.UsuarioEmail == email);
                    if (query != null)
                    {
                        query.UsuarioPassword = newPassword;
                        bd.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public static bool AddNewUser(Usuarios usuarios)
        {
            var transact = false;
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => td.UsuarioEmail == usuarios.UsuarioEmail);
                    if (query == null)
                    {
                        bd.Usuarios.Add(usuarios);
                        bd.SaveChanges();
                        transact = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transact = false;
                }
            }
            return transact;
        }

        public static Usuarios RecoveryPassword(Usuarios usuarios)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => td.UsuarioEmail == usuarios.UsuarioEmail);

                    if (query != null)
                    {
                        query.UsuarioPassword = usuarios.UsuarioPassword;
                        bd.SaveChanges();
                        return query;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        public static RA_managers Login(Usuarios usuarios)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.RA_managers.FirstOrDefault(td => (td.email == usuarios.UsuarioEmail || td.usuario == usuarios.UsuarioEmail));
                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        public static Usuarios Login(string correo)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => (td.UsuarioEmail == correo));
                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        public static bool ValidateUser(string email, string usuarioNivel)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.Usuarios.FirstOrDefault(td => td.UsuarioEmail == email && td.UsuarioNivel == usuarioNivel && td.UsuarioEstado == "Active");
                    if (query != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }
    }
}