var oTableTR;

calendarBackEndApp.controller('listaCampaignsController', function ($scope, calendarBackEndModel,getJson, $route, $routeParams, $location, $log, $q, $timeout) {
    
    $scope.loadingShow = loadingShow;
    $scope.loadingHide = loadingHide;
    

    $scope.mostrarListaEventos = function (result) {
        try {
            $scope.loadingShow();
            var i = 0;
            oTableTR = $('#listaEventosDt').DataTable({
                "aLengthMenu": [[15, 30, 60, 120, -1], [15, 30, 60, 120, "All"]],
                "bProcessing": true,
                responsive: false,
                //"autoWidth": false,
                "scrollX": true,
                "aaData": result,
                "aoColumns": [{ "mDataProp": "CampBlitzId" }, {
                        "render": function (data, type, row) {
                            var btns = "";
                            btns += '<button type="button" class="btn btn-blue" onclick="editEvent(\'' + row.CampBlitzId + '\'); return false;" style="margin-bottom:2px!important;width:38px!important;padding-top:2px;padding-bottom:1px;" data-toggle="tooltip" data-placement="bottom" title="Edit Campaign"><i class="fa fa-pencil-square-o"></i></button>&nbsp;&nbsp;';
                            btns += '<button type="button" class="btn btn-blue" onclick="cloneEvent(\'' + row.CampBlitzId + '\'); return false;" style="margin-bottom:2px!important;width:38px!important;padding-top:2px;padding-bottom:1px;" data-toggle="tooltip" data-placement="bottom" title="Clone Campaign"><i class="fa fa-clone"></i></button>&nbsp;&nbsp;';
                            btns += '<button type="button" class="btn btn-danger" onclick="deleteEvent(\'' + row.CampBlitzId + '\'); return false;" style="margin-bottom:2px!important;width:38px!important;padding-top:2px;padding-bottom:1px;" data-toggle="tooltip" data-placement="bottom" title="Delete Campaign"><i class="fa fa-times-circle"></i></button>&nbsp;&nbsp;';
                            return btns;
                        }
                    }, { "mDataProp": "CampBlitzName" }, { "mDataProp": "dateStartString" }, { "mDataProp": "CampBlitzPartnerName" }, { "mDataProp": "CampBlitzTheater" }, { "mDataProp": "CampBlitzStatus" },
                    { "mDataProp": "CampBlitzFmm" }, { "mDataProp": "CampBlitzState" }, { "mDataProp": "CampBlitzSFDCName" }, { "mDataProp": "CampBlitzSFDCLink" }
                    
                ],
                "aaSorting": [[0, "asc"]]
            });

            //$(".BSswitch").bootstrapSwitch();

            $('#listaEventosDt tbody').on('click', 'tr', function () {
                oTableTR.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');

            });

            $('#listaEventosDt tbody').on('click', 'button', function () {
                oTableTR.$('tr.selected').removeClass('selected');
                $(this).parents("tr").addClass("selected");
            });
           
            $scope.loadingHide();
            $scope.loadingHide();

        } catch (exception) {
            $scope.loadingHide();
            message("Error", "Attention", "error");
            
        }
        $scope.loadingHide();
        return false;
    };

    
    $scope.showAll = function () {
        var datos = $scope.listEvents;
        oTableTR = $('#listaEventosDt').DataTable();
        oTableTR.destroy();
        try {
            $scope.mostrarListaEventos(datos);
        } catch (exception) {
            message("Error", "Attention", "error");
        }
    }

    $scope.filter = function () {

        var datos = $scope.listEvents;
        var filtrados = searchIntoJson(datos, "CategoriaId", $scope.CategoriaIdSearch);
        oTableTR = $('#listaEventosDt').DataTable();
        oTableTR.destroy();
        try {
            $scope.mostrarListaEventos(filtrados);
        } catch (exception) {
            message("Error", "Attention", "error");
        }
    }

    $scope.getListaEventos = function () {
        $scope.loadingShow();
        calendarBackEndModel.getListaEventos().then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    oTableTR = $('#listaEventosDt').DataTable();
                    oTableTR.destroy();
                    try {
                        $scope.mostrarListaEventos(dataJson.data);
                        $scope.listEvents = dataJson.data;
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
                        message("Error", "Attention", "error");
                    }
                }
                
            } catch (e) {
                message("Error", "Attention", "error");
                $scope.loadingHide();
            }
        });
    };

    $scope.salir = function () {
        calendarBackEndModel.salir().then(function (result) {
            document.location.href = "index.html";
        });
    };

    $scope.validateUser = function () {
        calendarBackEndModel.validateUser().then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    $scope.nombre = dataJson.email;
                    $scope.admin = ($scope.nombre === "vparra@avaya.com") ? true : false;
                } else {
                    $scope.salir();
                }
            } catch (e) {
                $scope.salir();
            }
        });
    };
    
    
    $scope.getListaEventos();
    
   
    //Función eliminar campaña
    $scope.eventId = 0;
    $scope.deleteEvent = function (eventId) {
        $scope.eventId = eventId;
        $("#deleteModal").modal("show");
    };

    $scope.deleteEventSubmit = function () {
        $scope.loadingShow();
        calendarBackEndModel.deleteEvent($scope.eventId).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        $scope.loadingHide();
                        message("Blitz deleted", "Attention", "success");
                        $("#deleteModal").modal("hide");

                        oTableTR = $('#listaEventosDt').DataTable();
                        oTableTR.destroy();
                        try {
                            $scope.mostrarListaEventos(dataJson.data);
                            $scope.listEvents = dataJson.data;
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
        
    }

    /** Cambio de Password */
    $scope.changePassword = function () {
        var formulario = new Object();
        formulario.oldPassword = $scope.oldPassword;
        formulario.newPassword = $scope.newPassword;
        formulario.confirmNewPassword = $scope.confirmNewPassword;

        if (validarFormulario(formulario)) {
            if (formulario.newPassword === formulario.confirmNewPassword) {
                calendarBackEndModel.changePassword(formulario.newPassword).then(function (result) {
                    var data = result.d;
                    try {
                        $scope.loadingHide();
                        var dataJson = $.parseJSON(data);
                        if (dataJson.status === "ok") {
                            limpiarFormularioAdd(formulario);
                            $("#changePasswordModal").modal("hide");
                            message("", "Password Changed!", "success");
                        } else {
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
                $scope.loadingHide();
                message("New password and confirm password do not match", "Attention!", "warning");
            }
        } else {
            $scope.loadingHide();
            message("All fields are required", "Attention!", "warning");
        }
    }

    $scope.changePasswordModal = function () {
        $("#changePasswordModal").modal("show");
    };



    /* ------------------------------------- EVENTOS DEL LISTADO  --------------------------------------------*/
    $scope.getEventById = function (eventId) {
        var frm = searchFirstIntoJson($scope.listEvents, "CampBlitzId", eventId);
        $.each(frm, function (k, v) {
            if (k == "dateStartString") {
                $("#CampBlitzDateStart").val(v);
            } else if (k == "CampBlitzDateStart") {
                var m = 0;
            } else {
                $("#" + k).val(v);
            }
        });
    };

    $scope.clearForm = function (eventId) {
        var frm = searchFirstIntoJson($scope.listEvents, "CampBlitzId", eventId);
        limpiarFormularioV2(frm);
    };




    $scope.addCampaign = function () {
        $scope.clearForm(4);
        $scope.CampBlitzId = 0;
        $("#BlitzModal").modal("show");
        $("#createEvent").html("Add blitz");
        $("#formTitle").html("Add blitz");
    }

    $scope.editEvent = function (eventId) {
        $scope.getEventById(eventId);
        $scope.CampBlitzId = eventId;
        $("#BlitzModal").modal("show");
        $("#createEvent").html("Edit blitz");
        $("#formTitle").html("Edit blitz");
        
    }

    $scope.cloneEvent = function (eventId) {
        $scope.getEventById(eventId);
        $scope.CampBlitzId = 0;
        $("#BlitzModal").modal("show");
        $("#createEvent").html("Clone blitz");
        $("#formTitle").html("Clone blitz");
    }


    /* ----------------   Creacion del Blitz  --------------------------------------------------------------------------*/

    //Definicion de fechas
    $('#CampBlitzDateStartD').datetimepicker({
        format: 'MM/DD/YYYY',
    });

    function compareStrings(a, b) {
        // Assuming you want case-insensitive comparison
        a = a.toLowerCase();
        b = b.toLowerCase();
        return (a < b) ? -1 : (a > b) ? 1 : 0;
    }

    //Carga de datos en los selects y checkbox
    $scope.getAndLoadData = function (nameList) {
        var dataOrder = 0;
        getJson.get(nameList).success(function (data) {
            data.sort(function (a, b) {
                return compareStrings(a.name, b.name);
            });
            $scope[nameList] = data;
        });
    }

    
    $scope.getAndLoadData('theaterList');
    // fin de carga

    $scope.frm = {};
    
    $scope.submitBlitz = function() {
        $('#createEvent').prop('disabled', true);
        $scope.loadingShow();
        //var formulario = $scope.frm;
        var datosS = JSON.stringify($('#blitzForm').serializeObject());
        var formulario = $.parseJSON(datosS);



        var required = ["CampBlitzName", "CampBlitzDateStart", "CampBlitzPartnerName", "CampBlitzTheater", "CampBlitzStatus"];
        if (validarFormularioV2(formulario, required)) {
            formulario.CampBlitzId = $scope.CampBlitzId;
            calendarBackEndModel.submitBlitz(formulario).then(function (result) {
                var data = result.d;
                try {

                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        limpiarFormularioV2(formulario);

                        $scope.loadingHide();
                        $("#BlitzModal").modal("hide");
                        $scope.getListaEventos();
                        var content = ($scope.CampBlitzId == 0) ? "Your blitz has now been created and added to the calendar." : "Your blitz has now been updated.";
                        $scope.message = {
                            content: content,
                            title: "Thank you!"
                        }

                        $("#messageModal").modal("show");

                    } else {
                        $('#createEvent').prop('disabled', false);
                        $scope.loadingHide();
                        if (dataJson.data === "NoSession") {
                            document.location.href = "https://www4.avaya.com/mcs/site";
                        } else {
                            message("Error", "Attention", "danger");
                        }
                    }
                    $('#createEvent').prop('disabled', false);

                } catch (e) {
                    $('#createEvent').prop('disabled', false);
                    $scope.loadingHide();
                    message("Error", "Attention", "danger");
                }
            });
        } else {
            $('#createEvent').prop('disabled', false);
            $scope.loadingHide();
            message("Please, check the required fields", "Alert", "danger");
        }
    }
    

    $scope.doExport = function (params) {
        var options = {
            tableName: 'Campaigns',
            worksheetName: 'Campaigns'
        };
        $.extend(true, options, params);
        $('#listaEventosDt').tableExport(options);

    }

    $scope.toExcel = function () {
        $scope.doExport({ type: 'excel' });
    }

});

function changeStatus(eventId,oldStatus) {
    var scope = undefined;
    scope = angular.element(document.getElementById("body")).scope();
    scope.$apply(function () {
        scope.changeStatus(eventId, oldStatus);
    });
};

function editEvent(eventId) {
    var scope = undefined;
    scope = angular.element(document.getElementById("body")).scope();
    scope.$apply(function () {
        scope.editEvent(eventId);
    });
}

function cloneEvent(eventId) {
    var scope = undefined;
    scope = angular.element(document.getElementById("body")).scope();
    scope.$apply(function () {
        scope.cloneEvent(eventId);
    });
}

function deleteEvent(eventId) {
    var scope = undefined;
    scope = angular.element(document.getElementById("body")).scope();
    scope.$apply(function () {
        scope.deleteEvent(eventId);
    });
}


//Funciones externas para intermediar entre javascript puro y angularjs
function validate() {
    var scope = undefined;
    scope = angular.element(document.getElementById("body")).scope();
    scope.$apply(function () {
        scope.validateUser();
    });
}

$(document).ready(function () {
    validate();
});

function showObservations(observacion,tales) {
    message(observacion, "Content Text", "danger");
}
