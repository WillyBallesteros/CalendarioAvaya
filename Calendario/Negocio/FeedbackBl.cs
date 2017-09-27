using System;
using System.Collections.Generic;
using Calendario.Datos;
using Calendario.Modelo;
using Newtonsoft.Json;

namespace Calendario.Negocio
{
    public static class FeedbackBl
    {
        public static string Post(int feebackNumber, string feedbackObservations)
        {
            var feedback = new feedback
            {
                feebackNumber = feebackNumber,
                feedbackDate = DateTime.Now,
                feedbackObservations = feedbackObservations
            };
            bool resp = FeedbackDal.Post(feedback);
            if (resp)
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";

        }

        public static string Get()
        {
            string resultado = "";
            List<feedback> result = FeedbackDal.Get();
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
    }
}