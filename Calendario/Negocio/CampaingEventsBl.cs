using Calendario.Datos;
using Calendario.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendario.Negocio
{
    public class CampaingEventsBl
    {

        public static int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
        //Agregar un nuevo evento de campaña

        public static string CrearIniciales(string programName)
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
            return firstCh + secondCh;
        }

        public static string Post(CampaingEvents evento)
        {
            //evento.programInitials = CrearIniciales(evento.programName);
            bool answ = CampaignEventDal.Post(evento);
            if (answ)
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        //Obtener los eventos del mes actual segun la fecha del servidor
        public static string GetEventsActualDate()
        {
            DateTime ahora = DateTime.Now;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";
            //Solo se muestran en el calendario las publicadas status = publish
            List<ViewCampaingEvents> result = CampaignEventDal.GetEventsByMonthYear(mes, year);
            resultado = BuildCalendar(result, mes, year);
            return resultado;
        }

        public static string SearchServer(DateTime fechaActual, string vert, string[] teatrosSeleccionados, string[] buSeleccionados, string[] bsSeleccionados, string programName, string langs, string campaignName, string tacticStage)
        {
            DateTime ahora = fechaActual;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";
            string[] vertString = { };
            string[] lString = { };
            //Solo se muestran en el calendario las publicadas status = publish
            if (vert != "")
            {
                vertString = vert.Split(',');
            }

            string[] teatrosString = teatrosSeleccionados;
            string[] buString = buSeleccionados;
            string[] bsString = bsSeleccionados;
            if (langs != "")
            {
                lString = langs.Split(',');
            }
            List<ViewCampaingEvents> result = CampaignEventDal.GetEventsByMonthYearFilter(mes, year, vertString, teatrosString, buString, bsString, lString, programName, campaignName, tacticStage);

            resultado = BuildCalendar(result, mes, year);
            return resultado;
        }

        public static string SearchServerV2(DateTime fechaActual, string vert, string teatrosSeleccionados, string buSeleccionados, string bsSeleccionados, string programName, string langs, string campaignName, string tacticStage, string agency)
        {
            DateTime ahora = fechaActual;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";

            List<ViewCampaingEvents> result = CampaignEventDal.GetEventsByMonthYearFilterV2(mes, year, vert, teatrosSeleccionados, buSeleccionados, bsSeleccionados, langs, programName, campaignName, tacticStage);


            if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                tacticStage.Equals(""))
            {
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit == buSeleccionados).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment == bsSeleccionados).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName == programName).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.businessUnit == buSeleccionados).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.businessSegment == bsSeleccionados).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.programName == programName).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") && campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") && !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater == teatrosSeleccionados && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit == buSeleccionados && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit == buSeleccionados && r.programName == programName).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit == buSeleccionados && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit == buSeleccionados && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit == buSeleccionados && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment == bsSeleccionados && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment == bsSeleccionados && r.programName == programName).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment == bsSeleccionados && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment == bsSeleccionados && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment == bsSeleccionados && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.programName == programName).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName == programName && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName == programName && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName == programName && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.vertical.Contains(vert) && r.tacticStage == tacticStage).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.programName.Equals(programName) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.programName.Equals(programName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.vertical.Contains(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.programName.Equals(programName) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.programName.Equals(programName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.vertical.Contains(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment.Equals(bsSeleccionados) && r.programName.Equals(programName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment.Equals(bsSeleccionados) && r.vertical.Contains(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment.Equals(bsSeleccionados) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.vertical.Equals(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.vertical.Equals(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName.Equals(programName) && r.languages.Contains(langs) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName.Equals(programName) && r.tacticStage.Equals(tacticStage) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.tacticStage.Equals(tacticStage) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.programName.Equals(programName) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.programName.Equals(programName) && r.languages.Contains(langs) && r.campaignName.Contains(campaignName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.programName.Equals(programName) && r.languages.Contains(langs) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.tacticStage.Equals(tacticStage) && r.languages.Contains(langs) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.languages.Contains(langs) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.languages.Contains(langs) && r.vertical.Contains(vert) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.languages.Contains(langs) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment.Equals(bsSeleccionados) && r.vertical.Contains(vert) && r.programName.Equals(programName) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessSegment.Equals(bsSeleccionados) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.vertical.Contains(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.campaignName.Contains(campaignName) && r.tacticStage.Equals(tacticStage) && r.vertical.Contains(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.programName.Equals(programName) && r.tacticStage.Equals(tacticStage) && r.vertical.Contains(vert) && r.languages.Contains(langs)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.languages.Contains(langs) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.languages.Contains(langs) && r.vertical.Contains(vert) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.languages.Contains(langs) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.languages.Contains(langs) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.vertical.Contains(vert) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.tacticStage.Equals(tacticStage) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.vertical.Contains(vert) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.tacticStage.Equals(tacticStage) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.vertical.Contains(vert) && r.campaignName.Contains(campaignName) && r.tacticStage.Equals(tacticStage) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.theater.Equals(teatrosSeleccionados) && r.campaignName.Contains(campaignName) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.vertical.Contains(vert) && r.theater.Equals(teatrosSeleccionados) && r.campaignName.Contains(campaignName) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.tacticStage.Equals(tacticStage) && r.theater.Equals(teatrosSeleccionados) && r.campaignName.Contains(campaignName) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.vertical.Equals(vert) && r.campaignName.Contains(campaignName) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.languages.Contains(langs) && r.vertical.Equals(vert) && r.campaignName.Contains(campaignName) && r.tacticStage.Equals(tacticStage) && r.businessSegment.Equals(bsSeleccionados) && r.programName.Equals(programName)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") && !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") && tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.languages.Contains(langs) && r.vertical.Contains(vert)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.languages.Contains(langs) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.languages.Contains(langs) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !buSeleccionados.Equals("") && !bsSeleccionados.Equals("") &&
                     !campaignName.Equals("") && !programName.Equals("") && !langs.Equals("") && !vert.Equals("") &&
                     !tacticStage.Equals(""))
            {
                result = result.Where(r => r.theater.Equals(teatrosSeleccionados) && r.businessUnit.Equals(buSeleccionados) && r.businessSegment.Equals(bsSeleccionados) && r.campaignName.Contains(campaignName) && r.programName.Equals(programName) && r.languages.Contains(langs) && r.vertical.Contains(vert) && r.tacticStage.Equals(tacticStage)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }
            return resultado;
        }

        public static string SearchServerV3(DateTime fechaActual, string vertical, string theater,
            string businessUnit, string businessSegment, string programName, string languages, string campaignName,
            string tacticStage, string agencyInternal)
        {
            DateTime ahora = fechaActual;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";

            List<ViewCampaingEvents> result = CampaignEventDal.GetEventsByMonthYearFilter(mes, year);

            if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") &&
                programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") &&
                agencyInternal.Equals(""))
            {
                resultado = BuildCalendar(result, mes, year);
            }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && tacticStage.Equals("") && !agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.agencyInternal.Contains(agencyInternal)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") && !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") && !campaignName.Equals("") && !tacticStage.Equals("") && agencyInternal.Equals("")) { result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage)).ToList(); resultado = BuildCalendar(result, mes, year); }
            else if (!vertical.Equals("") && !theater.Equals("") && !businessUnit.Equals("") &&
                     !businessSegment.Equals("") && !programName.Equals("") && !languages.Equals("") &&
                     !campaignName.Equals("") && !tacticStage.Equals("") && !agencyInternal.Equals(""))
            {
                result = result.Where(r => r.vertical.Contains(vertical) && r.theater.Contains(theater) && r.businessUnit.Contains(businessUnit) && r.businessSegment.Contains(businessSegment) && r.programName.Contains(programName) && r.languages.Contains(languages) && r.campaignName.Contains(campaignName) && r.tacticStage.Contains(tacticStage) && r.agencyInternal.Contains(agencyInternal)).ToList();
                resultado = BuildCalendar(result, mes, year);
            }


            return resultado;
        }


        //obtener los eventos del mes segun los datos enviados por el cliente
        public static string GetEventsByMonthYear(int mes, int year)
        {
            string resultado = "";
            //Solo se muestran en el calendario las publicadas status = publish
            List<ViewCampaingEvents> result = CampaignEventDal.GetEventsByMonthYear(mes, year);
            resultado = BuildCalendar(result, mes, year);
            return resultado;
        }

        //obtener los eventos del mes segun los datos enviados por el cliente
        public static string GetListaEventos(int usuarioId)
        {
            string resultado = "";
            List<ViewCampaingEvents> result = CampaignEventDal.GetListaEventos(usuarioId);
            var orderResult = result.OrderByDescending(r => r.dateStart);
            try
            {
                var data = JsonConvert.SerializeObject(orderResult);
                resultado = "{\"status\":\"ok\",\"data\":" + data + "}";
            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }

        public static string GetEventById(int eventId)
        {
            string resultado = "";
            var result = CampaignEventDal.GetEventById(eventId);
            try
            {
                var data = JsonConvert.SerializeObject(result);
                resultado = "{\"status\":\"ok\",\"data\":" + data + "}";
            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }



        public static string DeleteEvent(int eventId, int usuarioId)
        {
            var foo = CampaignEventDal.DeleteEvent(eventId);
            return GetListaEventos(usuarioId);
        }

        public static string UpdateEvent(CampaingEvents evento)
        {
            //evento.programInitials = CrearIniciales(evento.programName);
            evento.usuarioId = CampaignEventDal.GetUserByEvent(evento.eventId);
            if (CampaignEventDal.UpdateEvent(evento))
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string UpdateStatusEvent(int eventId, string status)
        {
            if (CampaignEventDal.UpdateStatusEvent(eventId, status))
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public class Event
        {
            public int EventId { get; set; }
            public string CampaignName { get; set; }
            public string Tactic { get; set; }

        }

        public class MyDay
        {
            public DateTime Date { get; set; }
            public List<ViewCampaingEvents> Events { get; set; }
            public string ActualMonth { get; set; }
            public string Weekend { get; set; }
            public int Day { get; set; }

            public string AddCampaign { get; set; }
        }

        public class MyDayBlitz
        {
            public DateTime Date { get; set; }
            public List<View_CalendarBlitz> Events { get; set; }
            public string ActualMonth { get; set; }
            public string Weekend { get; set; }
            public int Day { get; set; }

            public string AddCampaign { get; set; }
        }

        public class MyDayCampBlitz
        {
            public DateTime Date { get; set; }
            public List<View_CalendarAll> Events { get; set; }
            public string ActualMonth { get; set; }
            public string Weekend { get; set; }
            public int Day { get; set; }

            public string AddCampaign { get; set; }
        }

        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var result = date.AddDays(-((date.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
            return result;
        }

        public static DateTime LastDayOfWeek(DateTime dt)
        {
            return dt.AddDays(6);
        }


        public static List<MyDay> BuildWeek(List<ViewCampaingEvents> events, int mes, int year, DateTime firstDay, DateTime lastDay, DateTime firstDayMonth, DateTime lastDayMonth)
        {
            List<MyDay> datosSemana = new List<MyDay>();
            var hoy = DateTime.Now;

            hoy = hoy.Date.Add(new TimeSpan(0, 0, 0));

            for (int i = 0; i <= 6; i++)
            {
                var actualDate = firstDay.AddDays(i);

                var day = new MyDay()
                {
                    Date = actualDate,
                    //Events = events.Where(e => e.dateStart.Value.Day == actualDate.Day && e.dateStart.Value.Month == actualDate.Month && e.dateStart.Value.Year == actualDate.Year).ToList(),
                    Events = events.Where(e => e.dateStart.Date <= actualDate.Date && e.dateEnd.Date >= actualDate.Date).ToList(),
                    ActualMonth = (actualDate.Month == mes) ? "YES" : "NO",
                    Weekend = (actualDate.DayOfWeek == DayOfWeek.Sunday || actualDate.DayOfWeek == DayOfWeek.Saturday) ? "YES" : "NO",
                    Day = actualDate.Day,
                    AddCampaign = (actualDate >= hoy) ? "YES" : "NO"
                };
                datosSemana.Add(day);

            }
            return datosSemana;
        }

        public static string BuildCalendar(List<ViewCampaingEvents> events, int mes, int year)
        {
            string resultado = "";
            DateTime date = new DateTime(year, mes, 1);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstDayOfWeek = FirstDayOfWeek(firstDayOfMonth);
            var lastDayOfWeek = LastDayOfWeek(firstDayOfWeek);
            var semana1 = BuildWeek(events, mes, year, firstDayOfWeek, lastDayOfWeek, firstDayOfMonth, lastDayOfMonth);
            var semana2 = BuildWeek(events, mes, year, firstDayOfWeek.AddDays(7), lastDayOfWeek.AddDays(7), firstDayOfMonth, lastDayOfMonth);
            var semana3 = BuildWeek(events, mes, year, firstDayOfWeek.AddDays(14), lastDayOfWeek.AddDays(14), firstDayOfMonth, lastDayOfMonth);
            var semana4 = BuildWeek(events, mes, year, firstDayOfWeek.AddDays(21), lastDayOfWeek.AddDays(21), firstDayOfMonth, lastDayOfMonth);
            var semana5 = BuildWeek(events, mes, year, firstDayOfWeek.AddDays(28), lastDayOfWeek.AddDays(28), firstDayOfMonth, lastDayOfMonth);
            try
            {
                var s1 = JsonConvert.SerializeObject(semana1);
                var s2 = JsonConvert.SerializeObject(semana2);
                var s3 = JsonConvert.SerializeObject(semana3);
                var s4 = JsonConvert.SerializeObject(semana4);
                var s5 = JsonConvert.SerializeObject(semana5);
                resultado = "{\"status\":\"ok\",\"semana1\":" + s1 + ",\"semana2\":" + s2 + ",\"semana3\":" + s3 + ",\"semana4\":" + s4 + ",\"semana5\":" + s5 + "}";

            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }

        public static List<MyDayBlitz> BuildWeekBlitz(List<View_CalendarBlitz> events, int mes, int year, DateTime firstDay, DateTime lastDay, DateTime firstDayMonth, DateTime lastDayMonth)
        {
            List<MyDayBlitz> datosSemana = new List<MyDayBlitz>();
            var hoy = DateTime.Now;

            hoy = hoy.Date.Add(new TimeSpan(0, 0, 0));

            for (int i = 0; i <= 6; i++)
            {
                var actualDate = firstDay.AddDays(i);

                var day = new MyDayBlitz();

                day.Date = actualDate;
                try
                {
                    day.Events =
                        events.Where(
                            e =>
                                e.CampBlitzDateStart.Value.Day == actualDate.Day &&
                                e.CampBlitzDateStart.Value.Month == actualDate.Month &&
                                e.CampBlitzDateStart.Value.Year == actualDate.Year).ToList();
                }
                catch (ArgumentNullException ex)
                {
                    day.Events = null;
                }

                day.ActualMonth = (actualDate.Month == mes) ? "YES" : "NO";
                day.Weekend = (actualDate.DayOfWeek == DayOfWeek.Sunday || actualDate.DayOfWeek == DayOfWeek.Saturday)
                    ? "YES"
                    : "NO";
                day.Day = actualDate.Day;
                day.AddCampaign = (actualDate >= hoy) ? "YES" : "NO";

                datosSemana.Add(day);

            }
            return datosSemana;
        }

        public static string BuildCalendarBlitz(List<View_CalendarBlitz> events, int mes, int year)
        {
            string resultado = "";
            DateTime date = new DateTime(year, mes, 1);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstDayOfWeek = FirstDayOfWeek(firstDayOfMonth);
            var lastDayOfWeek = LastDayOfWeek(firstDayOfWeek);
            var semana1 = BuildWeekBlitz(events, mes, year, firstDayOfWeek, lastDayOfWeek, firstDayOfMonth, lastDayOfMonth);
            var semana2 = BuildWeekBlitz(events, mes, year, firstDayOfWeek.AddDays(7), lastDayOfWeek.AddDays(7), firstDayOfMonth, lastDayOfMonth);
            var semana3 = BuildWeekBlitz(events, mes, year, firstDayOfWeek.AddDays(14), lastDayOfWeek.AddDays(14), firstDayOfMonth, lastDayOfMonth);
            var semana4 = BuildWeekBlitz(events, mes, year, firstDayOfWeek.AddDays(21), lastDayOfWeek.AddDays(21), firstDayOfMonth, lastDayOfMonth);
            var semana5 = BuildWeekBlitz(events, mes, year, firstDayOfWeek.AddDays(28), lastDayOfWeek.AddDays(28), firstDayOfMonth, lastDayOfMonth);
            try
            {
                var s1 = JsonConvert.SerializeObject(semana1);
                var s2 = JsonConvert.SerializeObject(semana2);
                var s3 = JsonConvert.SerializeObject(semana3);
                var s4 = JsonConvert.SerializeObject(semana4);
                var s5 = JsonConvert.SerializeObject(semana5);
                resultado = "{\"status\":\"ok\",\"semana1\":" + s1 + ",\"semana2\":" + s2 + ",\"semana3\":" + s3 + ",\"semana4\":" + s4 + ",\"semana5\":" + s5 + "}";

            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }


        public static string SearchServerBlitzV2(DateTime fechaActual, string teatrosSeleccionados, string campBlitzPartnerName, string campBlitzStatus, string campBlitzName,string agency)
        {
            DateTime ahora = fechaActual;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";
            List<View_CalendarBlitz> result = CampaignEventDal.GetEventsByMonthYearFilterV2(mes, year);
            if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzName == campBlitzName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            //2
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzStatus == campBlitzStatus).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzName == campBlitzName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }

            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzPartnerName == campBlitzPartnerName && r.CampBlitzName == campBlitzName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzPartnerName == campBlitzPartnerName && r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus && r.CampBlitzName == campBlitzName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus && r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzName == campBlitzName && r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            //3
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzStatus == campBlitzStatus && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzName == campBlitzName && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzAgency == agency && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzStatus == campBlitzStatus && r.CampBlitzName == campBlitzName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzName == campBlitzName && r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus && r.CampBlitzName == campBlitzName && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus && r.CampBlitzAgency == agency && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzName == campBlitzName && r.CampBlitzAgency == agency && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzStatus == campBlitzStatus && r.CampBlitzAgency == agency && r.CampBlitzName == campBlitzName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            //4
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzStatus == campBlitzStatus && r.CampBlitzName == campBlitzName && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzStatus == campBlitzStatus && r.CampBlitzAgency == agency && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzName == campBlitzName && r.CampBlitzAgency == agency && r.CampBlitzPartnerName == campBlitzPartnerName).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzName == campBlitzName && r.CampBlitzAgency == agency && r.CampBlitzStatus == campBlitzStatus).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzPartnerName == campBlitzPartnerName && r.CampBlitzName == campBlitzName && r.CampBlitzAgency == agency && r.CampBlitzStatus == campBlitzStatus).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            //5
            else if (!teatrosSeleccionados.Equals("") && !campBlitzPartnerName.Equals("") && !campBlitzStatus.Equals("") && !campBlitzName.Equals("") && !agency.Equals(""))
            {
                result = result.Where(r => r.CampBlitzTheater == teatrosSeleccionados && r.CampBlitzStatus == campBlitzStatus && r.CampBlitzName == campBlitzName && r.CampBlitzPartnerName == campBlitzPartnerName && r.CampBlitzAgency == agency).ToList();
                resultado = BuildCalendarBlitz(result, mes, year);
            }
            return resultado;
        }

        //obtener los eventos del mes segun los datos enviados por el cliente
        public static string GetEventsBlitzByMonthYear(int mes, int year)
        {
            string resultado = "";
            //Solo se muestran en el calendario las publicadas status = publish
            List<View_CalendarBlitz> result = CampaignEventDal.GetEventsBlitzByMonthYear(mes, year);
            resultado = BuildCalendarBlitz(result, mes, year);
            return resultado;
        }

        //Obtener los eventos del mes actual segun la fecha del servidor
        public static string GetEventsBlitzActualDate()
        {
            DateTime ahora = DateTime.Now;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";
            //Solo se muestran en el calendario las publicadas status = publish
            List<View_CalendarBlitz> result = CampaignEventDal.GetEventsBlitzByMonthYear(mes, year);
            resultado = BuildCalendarBlitz(result, mes, year);
            return resultado;
        }

        public static string GetListaEventosBlitz()
        {
            string resultado = "";
            List<View_CalendarBlitz> result = CampaignEventDal.GetListaEventosBlitz();
            var orderResult = result.OrderByDescending(r => r.CampBlitzDateStart);
            try
            {
                var data = JsonConvert.SerializeObject(orderResult);
                resultado = "{\"status\":\"ok\",\"data\":" + data + "}";
            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }

        public static string CreateBlitz(CalendarBlitz blitz)
        {
            //evento.programInitials = CrearIniciales(evento.programName);
            bool answ = CampaignEventDal.CreateBlitz(blitz);
            if (answ)
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string UpdateBlitz(CalendarBlitz blitz)
        {
            //evento.programInitials = CrearIniciales(evento.programName);
            bool answ = CampaignEventDal.UpdateBlitz(blitz);
            if (answ)
            {
                return "{\"status\":\"ok\",\"data\":\"ok\"}";
            }
            return "{\"status\":\"error\",\"data\":\"error\"}";
        }

        public static string DeleteEventBlitz(int eventId)
        {
            var foo = CampaignEventDal.DeleteEventBlitz(eventId);
            return GetListaEventosBlitz();
        }



        /*  METODOS CALENDAR ALL */
        //obtener los eventos del mes segun los datos enviados por el cliente
        public static string GetEventsCampBlitzByMonthYear(int mes, int year)
        {
            string resultado = "";
            //Solo se muestran en el calendario las publicadas status = publish
            List<View_CalendarAll> result = CampaignEventDal.GetEventsCampBlitzByMonthYear(mes, year);
            resultado = BuildCalendarCampBlitz(result, mes, year);
            return resultado;
        }

        //Obtener los eventos del mes actual segun la fecha del servidor
        public static string GetEventsCampBlitzActualDate()
        {
            DateTime ahora = DateTime.Now;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";
            //Solo se muestran en el calendario las publicadas status = publish
            List<View_CalendarAll> result = CampaignEventDal.GetEventsCampBlitzByMonthYear(mes, year);
            resultado = BuildCalendarCampBlitz(result, mes, year);
            return resultado;
        }

        public static string BuildCalendarCampBlitz(List<View_CalendarAll> events, int mes, int year)
        {
            string resultado = "";
            DateTime date = new DateTime(year, mes, 1);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstDayOfWeek = FirstDayOfWeek(firstDayOfMonth);
            var lastDayOfWeek = LastDayOfWeek(firstDayOfWeek);
            var semana1 = BuildWeekCampBlitz(events, mes, year, firstDayOfWeek, lastDayOfWeek, firstDayOfMonth, lastDayOfMonth);
            var semana2 = BuildWeekCampBlitz(events, mes, year, firstDayOfWeek.AddDays(7), lastDayOfWeek.AddDays(7), firstDayOfMonth, lastDayOfMonth);
            var semana3 = BuildWeekCampBlitz(events, mes, year, firstDayOfWeek.AddDays(14), lastDayOfWeek.AddDays(14), firstDayOfMonth, lastDayOfMonth);
            var semana4 = BuildWeekCampBlitz(events, mes, year, firstDayOfWeek.AddDays(21), lastDayOfWeek.AddDays(21), firstDayOfMonth, lastDayOfMonth);
            var semana5 = BuildWeekCampBlitz(events, mes, year, firstDayOfWeek.AddDays(28), lastDayOfWeek.AddDays(28), firstDayOfMonth, lastDayOfMonth);
            try
            {
                var s1 = JsonConvert.SerializeObject(semana1);
                var s2 = JsonConvert.SerializeObject(semana2);
                var s3 = JsonConvert.SerializeObject(semana3);
                var s4 = JsonConvert.SerializeObject(semana4);
                var s5 = JsonConvert.SerializeObject(semana5);
                resultado = "{\"status\":\"ok\",\"semana1\":" + s1 + ",\"semana2\":" + s2 + ",\"semana3\":" + s3 + ",\"semana4\":" + s4 + ",\"semana5\":" + s5 + "}";

            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }

        public static List<MyDayCampBlitz> BuildWeekCampBlitz(List<View_CalendarAll> events, int mes, int year, DateTime firstDay, DateTime lastDay, DateTime firstDayMonth, DateTime lastDayMonth)
        {
            List<MyDayCampBlitz> datosSemana = new List<MyDayCampBlitz>();
            var hoy = DateTime.Now;

            hoy = hoy.Date.Add(new TimeSpan(0, 0, 0));

            for (int i = 0; i <= 6; i++)
            {
                var actualDate = firstDay.AddDays(i);

                var day = new MyDayCampBlitz();

                day.Date = actualDate;
                try
                {
                    day.Events =
                        events.Where(
                            e =>
                                e.DateStart.Value.Day == actualDate.Day &&
                                e.DateStart.Value.Month == actualDate.Month &&
                                e.DateStart.Value.Year == actualDate.Year).ToList();
                }
                catch (ArgumentNullException ex)
                {
                    day.Events = null;
                }

                day.ActualMonth = (actualDate.Month == mes) ? "YES" : "NO";
                day.Weekend = (actualDate.DayOfWeek == DayOfWeek.Sunday || actualDate.DayOfWeek == DayOfWeek.Saturday)
                    ? "YES"
                    : "NO";
                day.Day = actualDate.Day;
                day.AddCampaign = (actualDate >= hoy) ? "YES" : "NO";

                datosSemana.Add(day);

            }
            return datosSemana;
        }

        public static string SearchServerCampBlitzV2(DateTime fechaActual, string teatrosSeleccionados, string campBlitzName, string agencyInternal)
        {
            DateTime ahora = fechaActual;
            int mes = ahora.Month;
            int year = ahora.Year;
            string resultado = "";
            List<View_CalendarAll> result = CampaignEventDal.GetAllEventsByMonthYearFilterV2(mes, year);
            if (teatrosSeleccionados.Equals("") && campBlitzName.Equals("") && agencyInternal.Equals(""))
            {
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzName.Equals("") && agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Theater == teatrosSeleccionados).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }


            else if (teatrosSeleccionados.Equals("") && !campBlitzName.Equals("") && agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Name.Contains(campBlitzName)).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }

            else if (teatrosSeleccionados.Equals("") && campBlitzName.Equals("") && !agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Agency == agencyInternal).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }
            //2

            else if (!teatrosSeleccionados.Equals("") && !campBlitzName.Equals("") && agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Name.Contains(campBlitzName) && r.Theater == teatrosSeleccionados).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }
            else if (!teatrosSeleccionados.Equals("") && campBlitzName.Equals("") && !agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Theater == teatrosSeleccionados && r.Agency == agencyInternal).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }
            else if (teatrosSeleccionados.Equals("") && !campBlitzName.Equals("") && !agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Name.Contains(campBlitzName) && r.Agency == agencyInternal).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }
            //3
            else if (!teatrosSeleccionados.Equals("") && !campBlitzName.Equals("") && !agencyInternal.Equals(""))
            {
                result = result.Where(r => r.Name.Contains(campBlitzName) && r.Agency == agencyInternal && r.Theater == teatrosSeleccionados).ToList();
                resultado = BuildCalendarCampBlitz(result, mes, year);
            }

            return resultado;
        }

        public static string GetEventById(int eventId, string type)
        {
            string resultado = "";
            try
            {
                if (type == "Campaigns")
                {
                    var result = CampaignEventDal.GetEventById(eventId);
                    var data = JsonConvert.SerializeObject(result);
                    resultado = "{\"status\":\"ok\",\"data\":" + data + "}";
                }
                else
                {
                    var result1 = CampaignEventDal.GetEventsBlitzById(eventId);
                    var data = JsonConvert.SerializeObject(result1);
                    resultado = "{\"status\":\"ok\",\"data\":" + data + "}";
                }
            }
            catch (Exception ex)
            {
                resultado = "{\"status\":\"error\",\"data\":\"error\"}";
            }
            return resultado;
        }

    }



}