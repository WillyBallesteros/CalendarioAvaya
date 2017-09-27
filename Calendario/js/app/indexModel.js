//Definicion del modelo
indexApp.service('indexModel', function ($http, $q) {

    function handleError(response) {
        if (!angular.isObject(response.data) || !response.data.message) {
            return ($q.reject("Ha ocurrido un error."));
        }
        return ($q.reject(response.data.message));
    }

    function handleSuccess(response) {
        return (response.data);
    }

    /* Users */
    function addNewUser(datae) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/AddNewUserClient", contentType: 'application/json; charset=utf-8', data: { 'usuarios': datae }
        });
        return (request.then(handleSuccess, handleError));
    }

    function recoveryPassword(datae) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/RecoveryPassword", contentType: 'application/json; charset=utf-8', data: { 'usuarios': datae }
        });
        return (request.then(handleSuccess, handleError));
    }

    function login(datae) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/Login", contentType: 'application/json; charset=utf-8', data: { 'usuarios': datae }
        });
        return (request.then(handleSuccess, handleError));
    }

    function login2(tales) {
        var request = $http({
            method: 'POST', url: "ServiciosWeb/UsuariosService.asmx/Login2", contentType: 'application/json; charset=utf-8', data: { 'tales': tales }
        });
        return (request.then(handleSuccess, handleError));
    }
    return ({
        //Users
        addNewUser: addNewUser,
        //Login
        recoveryPassword: recoveryPassword,
        login: login,
        login2: login2
    });




});