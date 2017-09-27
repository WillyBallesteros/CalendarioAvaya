using System;
using System.Collections.Generic;
using Calendario.Datos;
using Calendario.Modelo;
using Newtonsoft.Json;

namespace Calendario.Negocio
{
    public static class ProgramsBl
    {
        public static string Get()
        {
            string resultado = "";
            List<Programs> result = ProgramsDal.Get();
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

        public static string Post(string programName)
        {
            var name = programName.Split(' ');
            string firstCh = "";
            string secondCh = "";
            if (name.Length >= 2)
            {
                firstCh = name[0].Substring(0, 1);
                secondCh = name[1].Substring(0, 1);
            }
            else
            {
                firstCh = name[0].Substring(0, 1);
            }


            var program = new Programs { programName = programName,programIcon = firstCh + secondCh };
            bool resp = ProgramsDal.Post(program);
            if (resp)
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";

        }
    }
}