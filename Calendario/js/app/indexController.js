indexApp.controller('indexController', function ($scope, indexModel, $route, $routeParams) {
   
    //Funciones Solicitar Acceso
    $scope.agregarUser = function () {
        $("#requestAccessModal").modal("show");
    };

    $scope.addNewUser = function () {
        var formularioAdd = new Object();
        formularioAdd.UsuarioNombreAdd = $scope.UsuarioAccesoAdd;
        formularioAdd.UsuarioEmailAdd = $scope.UsuarioEmailAdd;
        formularioAdd.UsuarioAccesoAdd = $scope.UsuarioAccesoAdd;
        formularioAdd.UsuarioPasswordAdd = $scope.UsuarioPasswordAdd;
        
        if (validarFormulario(formularioAdd)) {
            var formulario = new Object();
            formulario.UsuarioNombre = $scope.UsuarioAccesoAdd;
            formulario.UsuarioEmail = $scope.UsuarioEmailAdd;
            formulario.UsuarioAcceso = $scope.UsuarioAccesoAdd;
            formulario.UsuarioPassword = $scope.UsuarioPasswordAdd;
            formulario.UsuarioNivel = "Normal User";
            formulario.UsuarioEstado = "Inactive";

            indexModel.addNewUser(formulario).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        $("#requestAccessModal").modal("hide");
                        limpiarFormularioAdd(formularioAdd);
                        message("Request Access Sent!", "Attention", "success");
                        
                        
                    } else {
                        if (dataJson.data === "NoSession") {
                            document.location.href = "index.html?BackPageSession=NoSession";
                        } else {
                            message("Error, please check the email user", "Attention", "danger");
                        }
                    }
                } catch (e) {
                    message("Error", "Attention", "danger");
                }
            });
        } else {
            message("Please, check the format or required fields", "Alert", "danger");
        }
    }
    
    //functiones Olvido Password
    $scope.forgotPassword = function () {
        $("#forgotModal").modal("show");
    };

    $scope.recoveryPassword = function () {
        var formulario = new Object();
        formulario.UsuarioEmail = $scope.UsuarioEmail;

        if (validarFormulario(formulario)) {
            
            indexModel.recoveryPassword(formulario).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        message("please check your email mailbox", "Password Sent!", "success");
                        limpiarFormulario(formulario);
                        $("#forgotModal").modal("hide");
                    } else {
                        if (dataJson.data === "NoSession") {
                            document.location.href = "index.html?BackPageSession=NoSession";
                        } else {
                            message("Error", "Attention", "danger");
                        }
                    }
                } catch (e) {
                    message("Error", "Attention", "danger");
                }
            });
        } else {
            message("Please, check the format or required fields", "Alert", "danger");
        }
    }

    //login
    
    $scope.login = function () {
        var formulario = new Object();
        
        formulario.UsuarioAcceso = $scope.UsuarioAcceso;
        formulario.UsuarioPassword = $scope.UsuarioPassword;
        if (validarFormulario(formulario)) {
            formulario.UsuarioEmail = $scope.UsuarioAcceso;
            indexModel.login(formulario).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        document.location.href = dataJson.data;
                    } else {
                        if (dataJson.data === "NoSession") {
                            document.location.href = "index.html?BackPageSession=NoSession";
                        } else if (dataJson.data === "NoActive") {
                            message("Error: User not active.", "Attention", "danger");
                        } else if (dataJson.data === "NoPassword") {
                            message("Error: Incorrect Password.", "Attention", "danger");
                        } else if (dataJson.data === "NoExist") {
                            message("Error: No user created.", "Attention", "danger");
                        }

                        
                    }
                } catch (e) {
                    message("Error: Please contact your system administrator", "Attention", "danger");
                }
            });
        } else {
            message("Please, check the format or required fields", "Alert", "danger");
        }
    }

    $scope.QueryString = function () {
        var query_string = {};

        var query = window.location.search.substring(1);

        var vars = query.split("&");

        for (var i = 0; i < vars.length; i++) {

            var pair = vars[i].split("=");

            // If first entry with this name

            if (typeof query_string[pair[0]] === "undefined") {

                query_string[pair[0]] = pair[1];

                // If second entry with this name

            } else if (typeof query_string[pair[0]] === "string") {

                var arr = [query_string[pair[0]], pair[1]];

                query_string[pair[0]] = arr;

                // If third or later entry with this name

            } else {

                query_string[pair[0]].push(pair[1]);

            }

        }

        return query_string;

    }();

    $scope.login2 = function(tales) {
        indexModel.login2(tales).then(function(result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    document.location.href = dataJson.data;
                } else {
                    if (dataJson.data === "NoSession") {
                        document.location.href = "index.html?BackPageSession=NoSession";
                    } else if (dataJson.data === "NoActive") {
                        message("Error: User not active.", "Attention", "danger");
                    } else if (dataJson.data === "NoPassword") {
                        message("Error: Incorrect Password.", "Attention", "danger");
                    } else if (dataJson.data === "NoExist") {
                        message("Error: No user created.", "Attention", "danger");
                    }
                }
            } catch (e) {
                message("Error: Please contact your system administrator", "Attention", "danger");
            }
        });
    };

    var tales = $scope.QueryString.src;
    if (tales !== undefined) {
        var dec = window.atob(tales);
        $scope.login2(dec);
    }

    
});