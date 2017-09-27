//Definicion del modelo
calendarBlitzApp.service('calendarBlitzModel', function ($http, $q) {

    function handleError(response) {
        if (!angular.isObject(response.data) || !response.data.message) {
            return ($q.reject("Ha ocurrido un error."));
        }
        return ($q.reject(response.data.message));
    }

    function handleSuccess(response) {
        return (response.data);
    }

    function getEventsByMonthYear(month,year) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetEventsCampBlitzByMonthYear", contentType: 'application/json; charset=utf-8', data: { 'month': month,'year':year }
        });
        return (request.then(handleSuccess, handleError));
    }

    function searchServerV2(fecha, teatrosSeleccionados, campBlitzName,agencyInternal) {
        var request = $http({
            method: 'POST',
            url: "ServiciosWeb/CalendarService.asmx/SearchServerCampBlitzV2",
            contentType: 'application/json; charset=utf-8',
            data: { 'fecha': fecha, 'teatrosSeleccionados': teatrosSeleccionados, 'campBlitzName': campBlitzName, 'agencyInternal': agencyInternal }
        });
        return (request.then(handleSuccess, handleError));
    }

    function getEventsActualDate() {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetEventsCampBlitzActualDate", contentType: 'application/json; charset=utf-8', data: { 'vista': 'vista' }
        });
        return (request.then(handleSuccess, handleError));
    }

    function getEventById(id, type) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/GetEventById", contentType: 'application/json; charset=utf-8', data: { 'id': id, 'type': type }
        });
        return (request.then(handleSuccess, handleError));
    }

    
    //ACCESO
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
    
    //Feedback
    
    function saveFeedback(feebackNumber, feedbackObservations) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/CalendarService.asmx/SaveFeedback", contentType: 'application/json; charset=utf-8', data: { 'feebackNumber': feebackNumber, 'feedbackObservations': feedbackObservations }
        });
        return (request.then(handleSuccess, handleError));
    }

    return ({
        //Calendar
        getEventsActualDate:getEventsActualDate,
        getEventsByMonthYear: getEventsByMonthYear,
        getEventById:getEventById,
        //Acceso
        changePassword: changePassword,
        salir: salir,
        //Utilities
        loadSelect: loadSelect,
        //Feedback
        saveFeedback: saveFeedback,
        //searchServer: searchServer,
        searchServerV2:searchServerV2

});
    



});