using System;
using System.Collections.Generic;
using System.Linq;
using Calendario.Datos;

namespace Calendario.Modelo
{
    public static class CampaignsDal
    {
        public static List<Campaigns> Get()
        {
            var bd = new calawebEntities();
            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                return bd.Campaigns.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static bool Post(Campaigns camp)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Campaigns.Add(camp);
                bd.SaveChanges();
                return  true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        } 
    }

    
         
    
    
}