//*   CONTROLADOR LINKS   *//
var oTableTl;

calendarBackEndApp.controller('usersController', function ($scope, calendarBackEndModel, $route, $routeParams, $location) {
    $scope.contentId = $routeParams.contentId;
    $scope.initv = 0;
    $scope.datosUsers = "";
    $scope.UsuarioNombre = "";
    $scope.UsuarioEmail = "";
    $scope.UsuarioAcceso = "";
    $scope.UsuarioPassword = "";
    $scope.UsuarioNivel = "";
    $scope.UsuarioId = 0;

    $scope.mostrarListaUsers = function (result) {
        try {
            var i = 0;
            oTableTl = $('#listaUsersDt').DataTable({
                "aLengthMenu": [[15, 30, 60, 120, -1], [15, 30, 60, 120, "All"]],
                "bProcessing": true,
                responsive: true,
                "autoWidth": false,
                "aaData": result,
                "aoColumns": [{ "mDataProp": "UsuarioNombre" }, { "mDataProp": "UsuarioEmail" },
                     { "mDataProp": "UsuarioAcceso" }, { "mDataProp": "UsuarioPassword" }, { "mDataProp": "UsuarioNivel" }, { "mDataProp": "UsuarioEstado" },
                            {
                                "render": function (data, type, row) {
                                    var status = "";
                                    if (row.UsuarioEstado === "Inactive") {
                                        status = "Active";
                                        return '<button type="button" class="btn btn-success" style="margin-bottom:2px!important;padding-top:2px!important;padding-bottom:2px!important;" id="div' + i + '" onclick="activeUser(this.id,' + row.UsuarioId + ',\'' + status + '\'); return false;">Active</button>';
                                    } else {
                                        status = "Inactive";
                                        return '<button type="button" class="btn btn-danger" style="margin-bottom:2px!important;padding-top:2px!important;padding-bottom:2px!important;" id="div' + i + '" onclick="activeUser(this.id,' + row.UsuarioId + ',\'' + status + '\'); return false;">Inactive</button>';
                                    }

                                }
                            },
                            {
                                "render": function (data, type, row) {
                                    return '<button type="button" class="btn btn-success" style="margin-bottom:2px!important;padding-top:2px!important;padding-bottom:2px!important;" id="div' + i + '" onclick="editarUser(this.id,' + row.UsuarioId + '); return false;">Update</button>';

                                }
                            },
                     {
                         "render": function (data, type, row) {
                             return '<button type="button" class="btn btn-danger" style="margin-bottom:2px!important;padding-top:2px!important;padding-bottom:2px!important;" id="div' + i + '" onclick="eliminarUser(this.id,' + row.UsuarioId + '); return false;">Delete</button>';
                         }
                     }

                ],
                "aaSorting": [[0, "asc"]]
            });

            $('#listaUsersDt tbody').on('click', 'tr', function () {
                oTableTl.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');

            });

            $('#listaUsersDt tbody').on('click', 'button', function () {
                oTableTl.$('tr.selected').removeClass('selected');
                $(this).parents("tr").addClass("selected");
            });



        } catch (exception) {
            message("Error", "Attention", "error");
        }
        return false;
    };


    $scope.getUsers = function () {
        calendarBackEndModel.getUsers().then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    oTableTl = $('#listaUsersDt').DataTable();
                    oTableTl.destroy();
                    try {
                        $scope.mostrarListaUsers(dataJson.data);
                        $scope.datosUsers = dataJson.data;
                    } catch (exception) {

                        message("Error", "Attention", "error");
                    }

                } else {
                    if (dataJson.data === "NoSession") {
                        document.location.href = "index.html?BackPageSession=NoSession";
                    } else {
                        message("Error", "Attention", "error");
                    }
                }
            } catch (e) {
                message("Error", "Attention", "error");
            }
        });
    };

    function init() {
        $scope.getUsers();
    }

    init();

    $scope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase === '$apply' || phase === '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
    
    //funciones Editar Link
    $scope.editarUser = function (id, userId) {
        
        $scope.datosUser = searchFirstIntoJson($scope.datosUsers, "UsuarioId", userId);
        $scope.safeApply(function() {
            $scope.UsuarioNombre = $scope.datosUser.UsuarioNombre;
            $scope.UsuarioEmail = $scope.datosUser.UsuarioEmail;
            $scope.UsuarioAcceso = $scope.datosUser.UsuarioAcceso;
            $scope.UsuarioPassword = $scope.datosUser.UsuarioPassword;
            $scope.UsuarioNivel = $scope.datosUser.UsuarioNivel;
            $scope.UsuarioEstado = $scope.datosUser.UsuarioEstado;
            $scope.UsuarioId = userId;
        });
            // every changes goes here
           
        
        $("#updateUserModal").modal("show");
    };

    $scope.updateUser = function () {
        var formulario = new Object();
        formulario.UsuarioNombre = $scope.UsuarioNombre;
        formulario.UsuarioEmail = $scope.UsuarioEmail;
        formulario.UsuarioAcceso = $scope.UsuarioAcceso;
        formulario.UsuarioPassword = $scope.UsuarioPassword;
        formulario.UsuarioNivel = $scope.UsuarioNivel;
        formulario.UsuarioEstado= $scope.UsuarioEstado;

        if (validarFormulario(formulario)) {
            formulario.UsuarioId = $scope.UsuarioId;
            calendarBackEndModel.updateUser(formulario).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        message("User data updated", "Attention", "success");
                        //limpiarFormulario(formulario);
                        $("#updateUserModal").modal("hide");

                        oTableTl = $('#listaUsersDt').DataTable();
                        oTableTl.destroy();
                        try {
                            $scope.mostrarListaUsers(dataJson.data);
                            $scope.datosUsers = dataJson.data;
                        } catch (exception) {
                            message("Error", "Attention", "error");
                        }
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
            message("Please, check the required fields", "Alert", "danger");
        }
    }

    //Función eliminar contenido
    $scope.eliminarUser = function (id, userId) {
        $scope.userId = userId;
        $("#deleteUserModal").modal("show");
    };

    $scope.deleteUser = function () {
        calendarBackEndModel.deleteUser($scope.userId).then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    message("User data deleted", "Attention", "success");
                    $("#deleteUserModal").modal("hide");

                    oTableTl = $('#listaUsersDt').DataTable();
                    oTableTl.destroy();
                    try {
                        $scope.mostrarListaUsers(dataJson.data);
                        $scope.datosUsers = dataJson.data;
                    } catch (exception) {
                        message("Error", "Attention", "error");
                    }
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

    }

    //Funciones Agregar Contenido
    $scope.agregarUser = function () {
        //$scope.getContenidos("crear");
        $("#addUserModal").modal("show");
    };

    $scope.addNewUser = function () {
        var formularioAdd = new Object();
        formularioAdd.UsuarioNombreAdd = $scope.UsuarioNombreAdd;
        formularioAdd.UsuarioEmailAdd = $scope.UsuarioEmailAdd;
        formularioAdd.UsuarioAccesoAdd = $scope.UsuarioAccesoAdd;
        formularioAdd.UsuarioPasswordAdd = $scope.UsuarioPasswordAdd;
        formularioAdd.UsuarioNivelAdd = $scope.UsuarioNivelAdd;
        formularioAdd.UsuarioEstadoAdd = $scope.UsuarioEstadoAdd;
        
        if (validarFormulario(formularioAdd)) {
            var formulario = new Object();
            formulario.UsuarioNombre = $scope.UsuarioNombreAdd;
            formulario.UsuarioEmail = $scope.UsuarioEmailAdd;
            formulario.UsuarioAcceso = $scope.UsuarioAccesoAdd;
            formulario.UsuarioPassword = $scope.UsuarioPasswordAdd;
            formulario.UsuarioNivel = $scope.UsuarioNivelAdd;
            formulario.UsuarioEstado = $scope.UsuarioEstadoAdd;

            calendarBackEndModel.addNewUser(formulario).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        $scope.loadingHide();
                        message("User data saved", "Attention", "success");
                        limpiarFormularioAdd(formularioAdd);
                        $("#addUserModal").modal("hide");

                        oTableTl = $('#listaUsersDt').DataTable();
                        oTableTl.destroy();
                        try {
                            $scope.mostrarListaUsers(dataJson.data);
                            $scope.datosUsers = dataJson.data;
                            $scope.loadingHide();
                        } catch (exception) {
                            $scope.loadingHide();
                            message("Error", "Attention", "error");
                        }
                    } else {
                        $scope.loadingHide();
                        if (dataJson.data === "NoSession") {
                            document.location.href = "index.html?BackPageSession=NoSession";
                        } else {
                            message("Error", "Attention", "danger");
                        }
                    }
                } catch (e) {
                    $scope.loadingHide();
                    message("Error", "Attention", "danger");
                }
            });
        } else {
            message("Please, check the format or required fields", "Alert", "danger");
        }
    }

    $scope.activeUser = function(id, userId, status) {
        $scope.actualStatus = status;
        calendarBackEndModel.activeUser(userId, status).then(function(result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    if ($scope.actualStatus === "Active") {
                        message("User Activated", "Attention", "success");
                    } else {
                        message("User Inactive", "Attention", "danger");
                    }

                    oTableTl = $('#listaUsersDt').DataTable();
                    oTableTl.destroy();
                    try {
                        $scope.mostrarListaUsers(dataJson.data);
                        $scope.datosUsers = dataJson.data;
                    } catch (exception) {
                        message("Error", "Attention", "error");
                    }
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

    };

    $scope.doExport = function (params) {
        var options = {
            tableName: 'Users',
            worksheetName: 'Users'
        };
        $.extend(true, options, params);
        $('#listaUsersDt').tableExport(options);

    }

    $scope.toExcel = function () {
        $scope.doExport({ type: 'excel' });
    }
});

function eliminarUser(id, userId) {
    var scope = undefined;
    scope = angular.element(document.getElementById("usersControllerId")).scope();
    scope.$apply(function () {
        scope.eliminarUser(id, userId);
    });
}

function editarUser(id, userId) {
    var scope = undefined;
    scope = angular.element(document.getElementById("usersControllerId")).scope();
    scope.$apply(function () {
        scope.editarUser(id, userId);
    });
}

function activeUser(id, userId,status) {
    var scope = undefined;
    scope = angular.element(document.getElementById("usersControllerId")).scope();
    scope.$apply(function () {
        scope.activeUser(id, userId,status);
    });
}