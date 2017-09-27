using System;
using System.Collections.Generic;
using Calendario.Datos;
using Calendario.Modelo;
using Newtonsoft.Json;

namespace Calendario.Negocio
{
    public static class CampaignsBl
    {
        public static string Get()
        {
            string resultado = "";
            List<Campaigns> result = CampaignsDal.Get();
            try
            {
                resultado = JsonConvert.SerializeObject(result);
                resultado = "{\"status\":\"ok\",\"data\":" + resultado + "}";
            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }

        public static string Post(string campaignName)
        {
            var campaign = new Campaigns { campaignName = campaignName };
            bool resp = CampaignsDal.Post(campaign);
            if (resp)
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
            
        }
    }
}