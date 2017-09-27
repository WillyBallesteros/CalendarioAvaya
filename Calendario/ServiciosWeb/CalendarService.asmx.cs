using Calendario.Datos;
using Calendario.Negocio;
using System;
using System.Web;
using System.Web.Services;

namespace Calendario.ServiciosWeb
{
    /// <summary>
    /// Descripción breve de CalendarService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class CalendarService : System.Web.Services.WebService
    {

        #region Campaign Events

        /// <summary>
        /// Create new CampaingEvent
        /// </summary>
        /// <remarks>
        /// POST -- AddNewEvent
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string AddNewEvent(CampaingEvents evento)
        {
            if (!UsuariosService.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            var sessionUsuario = HttpContext.Current.Session;
            try
            {
                int usuarioId = Convert.ToInt32(sessionUsuario["UsuarioId"]);
                evento.usuarioId = usuarioId;
            }
            catch (Exception ex)
            {
                evento.usuarioId = 12;
            }

            string datos = CampaingEventsBl.Post(evento);
            return datos;
        }

        /// <summary>
        /// Get events actual date
        /// </summary>
        /// <remarks>
        /// GET -- Use the actual server date
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventsActualDate(string vista)
        {
            string datos = CampaingEventsBl.GetEventsActualDate();
            return datos;
        }


        [WebMethod(EnableSession = true)]
        public string SearchServer(DateTime fecha, string vert, string[] teatrosSeleccionados, string[] buSeleccionados, string[] bsSeleccionados, string programName, string langs, string campaignName, string tacticStage)
        {
            string datos = CampaingEventsBl.SearchServer(fecha, vert, teatrosSeleccionados, buSeleccionados, bsSeleccionados, programName, langs, campaignName, tacticStage);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string SearchServerV2(DateTime fecha, string vert, string teatrosSeleccionados, string buSeleccionados, string bsSeleccionados, string programName, string langs, string campaignName, string tacticStage, string agency)
        {
            string datos = CampaingEventsBl.SearchServerV3(fecha, vert, teatrosSeleccionados, buSeleccionados, bsSeleccionados, programName, langs, campaignName, tacticStage, agency);
            return datos;
        }

        /// <summary>
        /// Get events by month and yeaf form client
        /// </summary>
        /// <remarks>
        /// GET -- Events 
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventsByMonthYear(int month, int year)
        {
            string datos = CampaingEventsBl.GetEventsByMonthYear(month, year);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string GetListaEventos(string data)
        {
            var sessionUsuario = HttpContext.Current.Session;
            int usuarioId = Convert.ToInt32(sessionUsuario["UsuarioId"]);
            string datos = CampaingEventsBl.GetListaEventos(usuarioId);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteEvent(int eventId)
        {
            if (!UsuariosService.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            var sessionUsuario = HttpContext.Current.Session;
            int usuarioId = Convert.ToInt32(sessionUsuario["UsuarioId"]);
            string datos = CampaingEventsBl.DeleteEvent(eventId, usuarioId);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string GetEventById(int eventId)
        {
            string datos = CampaingEventsBl.GetEventById(eventId);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string GetEventByIdV2(int eventId)
        {
            string datos = CampaingEventsBl.GetEventById(eventId);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateEvent(CampaingEvents evento)
        {
            if (!UsuariosService.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }

            //var sessionUsuario = HttpContext.Current.Session;
            //int usuarioId = Convert.ToInt32(sessionUsuario["UsuarioId"]);
            //int[] usuarios = {1, 2, 10, 11, 12};
            //if (usuarios.Contains(usuarioId))
            //{

            //}
            //evento.usuarioId = usuarioId;
            string datos = CampaingEventsBl.UpdateEvent(evento);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateStatusEvent(int eventId, string status)
        {
            if (!UsuariosService.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            string datos = CampaingEventsBl.UpdateStatusEvent(eventId, status);
            return datos;
        }




        #endregion


        #region Campaigns

        [WebMethod(EnableSession = true)]
        public string GetCampaigns(string vista)
        {
            string datos = CampaignsBl.Get();
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string AddNewCampaign(string campaignName)
        {
            string datos = CampaignsBl.Post(campaignName);
            return datos;
        }

        #endregion


        #region Programs
        /// <summary>
        /// Get all Programs
        /// </summary>
        /// <remarks>
        /// GET -- GetPrograms
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetPrograms(string vista)
        {
            string datos = ProgramsBl.Get();
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string AddNewProgram(string programName)
        {
            string datos = ProgramsBl.Post(programName);
            return datos;
        }


        #endregion



        #region Feedback
        /// <summary>
        /// Save Feedback
        /// </summary>
        /// <remarks>
        /// GET -- SaveFeedback
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string SaveFeedback(int feebackNumber, string feedbackObservations)
        {
            string datos = FeedbackBl.Post(feebackNumber, feedbackObservations);
            return datos;
        }


        [WebMethod(EnableSession = true)]
        public string GetFeedbacks(string data)
        {
            string datos = FeedbackBl.Get();
            return datos;
        }



        #endregion


        #region Blitz Calendar
        /// <summary>
        /// Filter Blitz Calendar
        /// </summary>
        /// <remarks>
        /// GET -- Events Blitz
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string SearchServerBlitzV2(DateTime fecha, string teatrosSeleccionados, string campBlitzPartnerName, string campBlitzStatus, string campBlitzName,string agency)
        {
            string datos = CampaingEventsBl.SearchServerBlitzV2(fecha, teatrosSeleccionados, campBlitzPartnerName, campBlitzStatus, campBlitzName,agency);
            return datos;
        }

        /// <summary>
        /// Get events by month and yeaf form client
        /// </summary>
        /// <remarks>
        /// GET -- Events 
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventsBlitzByMonthYear(int month, int year)
        {
            string datos = CampaingEventsBl.GetEventsBlitzByMonthYear(month, year);
            return datos;
        }

        /// <summary>
        /// Get events actual date
        /// </summary>
        /// <remarks>
        /// GET -- Use the actual server date
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventsBlitzActualDate(string vista)
        {
            string datos = CampaingEventsBl.GetEventsBlitzActualDate();
            return datos;
        }

        #endregion

        #region Blitz Calendar BackEnd
        /// <summary>
        /// Traer la lista de eventos del blitz
        /// </summary>
        /// <remarks>
        /// GET -- Events Blitz
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetListaEventosBlitz(string data)
        {
            //var sessionUsuario = HttpContext.Current.Session;
            //int usuarioId = Convert.ToInt32(sessionUsuario["UsuarioId"]);
            string datos = CampaingEventsBl.GetListaEventosBlitz();
            return datos;
        }



        [WebMethod(EnableSession = true)]
        public string SubmitBlitz(CalendarBlitz blitz)
        {
            string datos = "";
            datos = blitz.CampBlitzId == 0 ? CampaingEventsBl.CreateBlitz(blitz) : CampaingEventsBl.UpdateBlitz(blitz);
            return datos;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteEventBlitz(int eventId)
        {
            //if (!UsuariosService.SessionExists()) { return "{\"status\":\"error\",\"data\":\"NoSession\"}"; }
            //var sessionUsuario = HttpContext.Current.Session;
            //int usuarioId = Convert.ToInt32(sessionUsuario["UsuarioId"]);
            string datos = CampaingEventsBl.DeleteEventBlitz(eventId);
            return datos;
        }

        #endregion



        #region Blitz & Campaigns Calendar
        /// <summary>
        /// Filter Blitz Calendar
        /// </summary>
        /// <remarks>
        /// GET -- Events Blitz
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string SearchServerCampBlitzV2(DateTime fecha, string teatrosSeleccionados, string campBlitzName, string agencyInternal)
        {
            string datos = CampaingEventsBl.SearchServerCampBlitzV2(fecha, teatrosSeleccionados, campBlitzName, agencyInternal);
            return datos;
        }

        /// <summary>
        /// Get events by month and yeaf form client
        /// </summary>
        /// <remarks>
        /// GET -- Events 
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventsCampBlitzByMonthYear(int month, int year)
        {
            string datos = CampaingEventsBl.GetEventsCampBlitzByMonthYear(month, year);
            return datos;
        }

        /// <summary>
        /// Get events actual date
        /// </summary>
        /// <remarks>
        /// GET -- Use the actual server date
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventsCampBlitzActualDate(string vista)
        {
            string datos = CampaingEventsBl.GetEventsCampBlitzActualDate();
            return datos;
        }

        /// <summary>
        /// Get event by Id and type of calendar (blitz or campaigns)
        /// </summary>
        /// <remarks>
        /// GET -- Events 
        /// </remarks> 
        [WebMethod(EnableSession = true)]
        public string GetEventById(int id, string type)
        {
            string datos = CampaingEventsBl.GetEventById(id, type);
            return datos;
        }

        #endregion
    }
}
