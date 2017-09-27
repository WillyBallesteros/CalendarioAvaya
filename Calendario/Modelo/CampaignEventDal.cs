using Calendario.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendario.Modelo
{
    public static class CampaignEventDal
    {
        public static bool Post(CampaingEvents evento)
        {
            using (var bd = new calawebEntities())
            {

                try
                {
                    bd.CampaingEvents.Add(evento);
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


        public static List<ViewCampaingEvents> GetEventsByMonthYear(int mes, int year)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                return bd.ViewCampaingEvents.Where(ce => ce.dateStart.Year == year && (ce.dateStart.Month == mes || ce.dateStart.Month == mes - 1) && ce.status == "publish").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<ViewCampaingEvents> GetEventsByMonthYearFilter(int mes, int year, string[] vertString, string[] teatrosString, string[] buString, string[] bsString, string[] lString, string programName, string campaignName, string tacticStage)
        {
            var bd = new calawebEntities();
            //(intval == 0) ? dm.ID == 0 : dm.ID != 0

            bool teatrosB = teatrosString.Length > 0;
            bool buB = buString.Length > 0;
            bool bsB = bsString.Length > 0;
            //Differentes
            string lang = "";
            string vert = "";
            if (lString.Length > 0)
            {
                lang = lString[0].ToString();
            }
            if (vertString.Length > 0)
            {
                vert = vertString[0].ToString();
            }

            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                var tales = bd.ViewCampaingEvents.Where(ce => ce.dateStart.Year == year && (ce.dateStart.Month == mes || ce.dateStart.Month == mes - 1) && ce.status == "publish"
                    && (teatrosB) ? teatrosString.Contains(ce.theater) : ce.theater != null
                    && (buB) ? buString.Contains(ce.businessUnit) : ce.businessUnit != null
                    && (bsB) ? bsString.Contains(ce.businessSegment) : ce.businessSegment != null
                    && (programName != "") ? ce.programName.Contains(programName) : ce.programName != null
                    && (campaignName != "") ? ce.campaignName.Contains(campaignName) : ce.campaignName != null
                    && (lang != "") ? ce.languages.Contains(lang) : ce.languages != null
                    && (vert != "") ? ce.vertical.Contains(vert) : ce.vertical != null
                    && (tacticStage != "") ? ce.tacticStage.Contains(tacticStage) : ce.tacticStage != null).ToList();
                return tales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<ViewCampaingEvents> GetEventsByMonthYearFilterV2(int mes, int year, string vertString, string teatrosString, string buString, string bsString, string lString, string programName, string campaignName, string tacticStage)
        {
            var bd = new calawebEntities();

            try
            {
                //bd.Configuration.ProxyCreationEnabled = false;
                //var tales = bd.ViewCampaingEvents.Where(ce => ce.dateStart.Year == year && (ce.dateStart.Month == mes || ce.dateStart.Month == mes - 1) && ce.status == "publish" && (!teatrosString.Equals("")) ? ce.theater.Equals(teatrosString) : !ce.theater.Equals("") && (!buString.Equals("")) ? ce.businessUnit.Equals(buString) : !ce.businessUnit.Equals("")
                //    && (!bsString.Equals("")) ? ce.businessSegment.Equals(bsString) : !ce.businessSegment.Equals("") && (!programName.Equals("")) ? ce.programName.Contains(programName) : !ce.programName.Equals("") && (!campaignName.Equals("")) ? ce.campaignName.Contains(campaignName) : !ce.campaignName.Equals("")
                //    && (!lString.Equals("")) ? ce.languages.Contains(lString) : !ce.languages.Equals("") && (!vertString.Equals("")) ? ce.vertical.Contains(vertString) : !ce.vertical.Equals("") && (!tacticStage.Equals("")) ? ce.tacticStage.Equals(tacticStage) : !ce.tacticStage.Equals("")).ToList();

                var tales = bd.ViewCampaingEvents.Where(ce => ce.dateStart.Year == year && (ce.dateStart.Month == mes || ce.dateStart.Month == mes - 1) && ce.status == "publish").ToList();


                return tales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<ViewCampaingEvents> GetEventsByMonthYearFilter(int mes, int year)
        {
            var bd = new calawebEntities();

            try
            {
                var tales = bd.ViewCampaingEvents.Where(ce => ce.dateStart.Year == year && (ce.dateStart.Month == mes || ce.dateStart.Month == mes - 1) && ce.status == "publish").ToList();
                return tales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<ViewCampaingEvents> GetListaEventos(int usuarioId)
        {

            int[] adminList = { 2, 10, 12, 1, 11 };
            var bd = new calawebEntities();
            try
            {
                bd.Database.CommandTimeout = 380;
                if (adminList.Contains(usuarioId))
                {
                    return bd.ViewCampaingEvents.ToList();
                }
                return bd.ViewCampaingEvents.Where(ce => ce.usuarioId == usuarioId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        public static bool DeleteEvent(int eventId)
        {
            bool transact;
            CampaingEvents campaignToDelete;

            using (var ctx = new calawebEntities())
            {
                campaignToDelete = ctx.CampaingEvents.FirstOrDefault<CampaingEvents>(s => s.eventId == eventId);
            }
            using (var newContext = new calawebEntities())
            {
                try
                {
                    newContext.Entry(campaignToDelete).State = System.Data.Entity.EntityState.Deleted;
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


        public static ViewCampaingEvents GetEventById(int eventId)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Database.CommandTimeout = 380;
                return bd.ViewCampaingEvents.FirstOrDefault(c => c.eventId == eventId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static int GetUserByEvent(int eventId)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Database.CommandTimeout = 380;
                var evento = bd.ViewCampaingEvents.FirstOrDefault(c => c.eventId == eventId);
                return Convert.ToInt32(evento.usuarioId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }


        public static bool UpdateEvent(CampaingEvents evento)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.CampaingEvents.FirstOrDefault(td => td.eventId == evento.eventId);

                    if (query != null)
                    {
                        bd.Entry(query).CurrentValues.SetValues(evento);
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

        public static bool UpdateStatusEvent(int eventId, string status)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.CampaingEvents.FirstOrDefault(td => td.eventId == eventId);
                    if (query != null)
                    {
                        query.status = status;
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

        public static View_CalendarBlitz GetEventsBlitzById(int id)
        {
            var bd = new calawebEntities();
            try
            {
                var tales = bd.View_CalendarBlitz.FirstOrDefault(ce => ce.CampBlitzId == id);
                return tales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<View_CalendarBlitz> GetEventsByMonthYearFilterV2(int mes, int year)
        {
            var bd = new calawebEntities();
            try
            {
                var tales = bd.View_CalendarBlitz.Where(ce => ce.CampBlitzDateStart.Value.Year == year && (ce.CampBlitzDateStart.Value.Month == mes || ce.CampBlitzDateStart.Value.Month == mes - 1)).ToList();
                return tales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<View_CalendarBlitz> GetEventsBlitzByMonthYear(int mes, int year)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                return bd.View_CalendarBlitz.Where(ce => ce.CampBlitzDateStart.Value.Year == year && (ce.CampBlitzDateStart.Value.Month == mes || ce.CampBlitzDateStart.Value.Month == mes - 1)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<View_CalendarAll> GetEventsCampBlitzByMonthYear(int mes, int year)
        {
            var bd = new calawebEntities();
            try
            {
                bd.Configuration.ProxyCreationEnabled = false;
                return bd.View_CalendarAll.Where(ce => ce.DateStart.Value.Year == year && (ce.DateStart.Value.Month == mes || ce.DateStart.Value.Month == mes - 1)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<View_CalendarBlitz> GetListaEventosBlitz()
        {
            var bd = new calawebEntities();
            try
            {
                bd.Database.CommandTimeout = 380;
                return bd.View_CalendarBlitz.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static bool CreateBlitz(CalendarBlitz blitz)
        {
            using (var bd = new calawebEntities())
            {

                try
                {
                    bd.CalendarBlitz.Add(blitz);
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

        public static bool UpdateBlitz(CalendarBlitz blitz)
        {
            using (var bd = new calawebEntities())
            {
                try
                {
                    var query = bd.CalendarBlitz.FirstOrDefault(td => td.CampBlitzId == blitz.CampBlitzId);
                    if (query != null)
                    {
                        bd.Entry(query).CurrentValues.SetValues(blitz);
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

        public static bool DeleteEventBlitz(int eventId)
        {
            bool transact;
            CalendarBlitz campaignToDelete;

            using (var ctx = new calawebEntities())
            {
                campaignToDelete = ctx.CalendarBlitz.FirstOrDefault<CalendarBlitz>(s => s.CampBlitzId == eventId);
            }
            using (var newContext = new calawebEntities())
            {
                try
                {
                    newContext.Entry(campaignToDelete).State = System.Data.Entity.EntityState.Deleted;
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

        public static List<View_CalendarAll> GetAllEventsByMonthYearFilterV2(int mes, int year)
        {
            var bd = new calawebEntities();
            try
            {
                var tales = bd.View_CalendarAll.Where(ce => ce.DateStart.Value.Year == year && (ce.DateStart.Value.Month == mes || ce.DateStart.Value.Month == mes - 1)).ToList();
                return tales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



    }
}