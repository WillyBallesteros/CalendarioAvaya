using System;
using System.Collections.Generic;
using System.Linq;
using Calendario.Datos;


namespace Calendario.Modelo
{
    public static class FeedbackDal
    {
        public static bool Post(feedback feed)
        {
            var bd = new calawebEntities();
            try
            {
                bd.feedback.Add(feed);
                bd.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public static List<feedback> Get()
        {
            var bd = new calawebEntities();
            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                return bd.feedback.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }

    
}