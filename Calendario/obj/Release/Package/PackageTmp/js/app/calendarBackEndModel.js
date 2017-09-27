//Definicion del modelo
calendarBackEndApp.service('calendarBackEndModel', function ($http, $q) {

    function handleError(response) {
        if (!angular.isObject(response.data) || !response.data.message) {
            return ($q.reject("Ha ocurrido un error."));
        }
        return ($q.reject(response.data.message));
    }

    function handleSuccess(response) {
        return (response.data);
    }

    function getListaEventos() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetListaEventos", contentType: 'application/json; charset=utf-8', data: { data: 'key' }
        });
        return (request.then(handleSuccess, handleError));
    }

    function addNewEvent(datae) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/AddNewEvent", contentType: 'application/json; charset=utf-8', data: { 'evento': datae }
        });
        return (request.then(handleSuccess, handleError));
    }

    function updateEvent(evento) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/UpdateEvent", contentType: 'application/json; charset=utf-8', data: { 'evento': evento }
        });
        return (request.then(handleSuccess, handleError));
    }

    function updateStatusEvent(eventId,status) {
        var request = $http({method: 'POST',url: "ServiciosWeb/CalendarService.asmx/UpdateStatusEvent",contentType: 'application/json; charset=utf-8',data: {'eventId': eventId, 'status':status}});
        return (request.then(handleSuccess, handleError));
    }
    

    function deleteEvent(eventId) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/DeleteEvent", contentType: 'application/json; charset=utf-8', data: { 'eventId': eventId }
        });
        return (request.then(handleSuccess, handleError));
    }

    function getEventById(eventId) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetEventByIdV2", contentType: 'application/json; charset=utf-8', data: { 'eventId': eventId }
        });
        return (request.then(handleSuccess, handleError));
    }

    
    function loadLogos() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CasosService.asmx/LoadLogos", contentType: 'application/json; charset=utf-8', data: { data: 'key' }
        });
        return (request.then(handleSuccess, handleError));
    }

    /* Users */
    function getUsers() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/GetUsers", contentType: 'application/json; charset=utf-8', data: { data: 'key' }
        });
        return (request.then(handleSuccess, handleError));
    }

    function updateUser(datae) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/UpdateUser", contentType: 'application/json; charset=utf-8', data: { 'usuarios': datae }
        });
        return (request.then(handleSuccess, handleError));
    }

    function deleteUser(userId) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/DeleteUser", contentType: 'application/json; charset=utf-8', data: { 'userId': userId }
        });
        return (request.then(handleSuccess, handleError));
    }

    function addNewUser(datae) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/AddNewUser", contentType: 'application/json; charset=utf-8', data: { 'usuarios': datae }
        });
        return (request.then(handleSuccess, handleError));
    }

    function activeUser(userId,status) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/ActiveUser", contentType: 'application/json; charset=utf-8', data: { 'userId': userId, 'status': status }
        });
        return (request.then(handleSuccess, handleError));
    }

    function validateUser() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/ValidateUser", contentType: 'application/json; charset=utf-8', data: { 'usuarioNivel': "Admin User" }
        });
        return (request.then(handleSuccess, handleError));
    }

    function salir() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/Exit", contentType: 'application/json; charset=utf-8', data: { 'vista': "vista" }
        });
        return (request.then(handleSuccess, handleError));
    }

    function changePassword(newPassword) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/ChangePassword", contentType: 'application/json; charset=utf-8', data: { 'newPassword': newPassword }
        });
        return (request.then(handleSuccess, handleError));
    }
    

    //UTILITIES
    function loadSelect(url, data) {
        var request = $http({
            method: 'POST', url: url, contentType: 'application/json; charset=utf-8', data: data
        });
        return (request.then(handleSuccess, handleError));
    }
    
    //CALENDAR
    function getEventsByMonthYear(month, year) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetEventsByMonthYear", contentType: 'application/json; charset=utf-8', data: { 'month': month, 'year': year }
        });
        return (request.then(handleSuccess, handleError));
    }

    function getEventsActualDate() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetEventsActualDate", contentType: 'application/json; charset=utf-8', data: { 'vista': 'vista' }
        });
        return (request.then(handleSuccess, handleError));
    }
    
    //PROGRAMS
    
    function addNewProgram(programName) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/AddNewProgram", contentType: 'application/json; charset=utf-8', data: { 'programName': programName }
        });
        return (request.then(handleSuccess, handleError));
    }

    //CAMPAIGNS
    function addNewCampaign(campaignName) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/AddNewCampaign", contentType: 'application/json; charset=utf-8', data: { 'campaignName': campaignName }
        });
        return (request.then(handleSuccess, handleError));
    }

    //FEEDBACKS
    function getFeedbacks() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetFeedbacks", contentType: 'application/json; charset=utf-8', data: { 'data': "data" }
        });
        return (request.then(handleSuccess, handleError));
    }
    

    return ({
        //Listado
        getListaEventos: getListaEventos,
        
        addNewEvent: addNewEvent,
        updateEvent: updateEvent,
        deleteEvent: deleteEvent,
        getEventById: getEventById,
        updateStatusEvent:updateStatusEvent,
        //Users
        getUsers: getUsers,
        updateUser: updateUser,
        deleteUser: deleteUser,
        addNewUser: addNewUser,
        activeUser: activeUser,
        //Acceso
        changePassword: changePassword,
        validateUser: validateUser,
        salir: salir,
        //
        loadLogos: loadLogos,
        loadSelect: loadSelect,
        //Calendar
        getEventsByMonthYear: getEventsByMonthYear,
        getEventsActualDate: getEventsActualDate,
        //programs
        addNewProgram: addNewProgram,
        //campaigns
        addNewCampaign: addNewCampaign,
        //feedbacks
        getFeedbacks: getFeedbacks

});
    



});