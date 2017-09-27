using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calendario.Datos;

namespace Calendario.Modelo
{
    public static class ProgramsDal
    {
        public static List<Programs> Get()
        {
            var bd = new calawebEntities();
            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                return bd.Programs.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static bool Post(Programs program)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Programs.Add(program);
                bd.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        } 
    }
}