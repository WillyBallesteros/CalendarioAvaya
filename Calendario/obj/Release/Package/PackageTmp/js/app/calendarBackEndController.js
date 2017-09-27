var oTableTR;

calendarBackEndApp.controller('listaCampaignsController', function ($scope, calendarBackEndModel, $route, $routeParams, $location, $log, $q, $timeout) {
    
    $scope.loadingShow = loadingShow;
    $scope.loadingHide = loadingHide;
    

    $scope.mostrarListaEventos = function (result) {
        try {
            $scope.loadingShow();
            var i = 0;
            oTableTR = $('#listaEventosDt').DataTable({
                "aLengthMenu": [[15, 30, 60, 120, -1], [15, 30, 60, 120, "All"]],
                "bProcessing": true,
                responsive: true,
                "autoWidth": false,
                "aaData": result,
                "aoColumns": [
                    {
                        "render": function(data, type, row) {
                            return '<div class="checkbox" style="float:right;display:inline-block;"><input type="checkbox" value="' + row.eventId + '"></div>';
                        }
                    }, { "mDataProp": "eventId" }, { "mDataProp": "campaignName" }, { "mDataProp": "dateStartString" }, { "mDataProp": "dateEndString" }, { "mDataProp": "programName" }, { "mDataProp": "theater" }, { "mDataProp": "businessUnit" },
                    {
                        "render": function (data, type, row) {
                            if (row.status === "publish")
                            {
                                return '<input id="TheCheckBox' + row.eventId + '" data-size="mini" type="checkbox" onchange="changeStatus(' + row.eventId + ',\'' + row.status + '\')" data-off-text="unpublished" data-on-text="published" checked class="BSswitch">';
                            }
                            return '<input id="TheCheckBox' + row.eventId + '" data-size="mini" type="checkbox" onchange="changeStatus(' + row.eventId + ',\'' + row.status + '\')" data-off-text="unpublished" data-on-text="published"  class="BSswitch">';
                         }
                    }, {
                        "render": function (data, type, row) {
                            var btns = ""; 
                            btns += '<button type="button" class="btn btn-blue" onclick="editEvent(\'' + row.eventId + '\'); return false;" style="margin-bottom:2px!important;width:38px!important;padding-top:2px;padding-bottom:1px;" data-toggle="tooltip" data-placement="bottom" title="Edit Campaign"><i class="fa fa-pencil-square-o"></i></button>&nbsp;&nbsp;';
                            btns += '<button type="button" class="btn btn-blue" onclick="cloneEvent(\'' + row.eventId + '\'); return false;" style="margin-bottom:2px!important;width:38px!important;padding-top:2px;padding-bottom:1px;" data-toggle="tooltip" data-placement="bottom" title="Clone Campaign"><i class="fa fa-clone"></i></button>&nbsp;&nbsp;';
                            btns += '<button type="button" class="btn btn-danger" onclick="deleteEvent(\'' + row.eventId + '\'); return false;" style="margin-bottom:2px!important;width:38px!important;padding-top:2px;padding-bottom:1px;" data-toggle="tooltip" data-placement="bottom" title="Delete Campaign"><i class="fa fa-times-circle"></i></button>&nbsp;&nbsp;';
                            return btns;
                        }
                    }
                ],
                "aaSorting": [[0, "asc"]]
            });

            $(".BSswitch").bootstrapSwitch();

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
                    $scope.admin = ($scope.nombre === "jtamayo@avaya.com") ? true : false;
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
                        message("Campaign Calendar deleted", "Attention", "success");
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

    $scope.addCampaign = function() {
        $location.path("/addCampaign");
    }

    $scope.editEvent = function (eventId) {
        $location.path("/updateCampaign/" + eventId);
    }

    $scope.cloneEvent = function (eventId) {
        $location.path("/cloneCampaign/" + eventId);
    }

    $scope.changeStatus = function (eventId, oldStatus) {
        $scope.loadingShow();
        var statusAct = $("#TheCheckBox" + eventId).prop('checked');
        var actualStatus = (statusAct === false) ? "unpublish" : "publish";
        calendarBackEndModel.updateStatusEvent(eventId, actualStatus).then(function (result) {
            var data = result.d;
            try {
                $scope.loadingHide();
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    //message("Status changed", "Attention", "success");
                    
                } else {
                    if (dataJson.data === "NoSession") {
                        document.location.href = "index.html?BackPageSession=NoSession";
                    } else {
                        message("Error", "Attention", "danger");
                    }
                }
                $scope.loadingHide();
            } catch (e) {
                $scope.loadingHide();
                message("Error", "Attention", "danger");
                
            }
        });
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
