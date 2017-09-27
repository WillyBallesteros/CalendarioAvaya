calendarBackEndApp.controller('cloneCampaignController', function ($scope, calendarBackEndModel, getJson,$route, $routeParams, $location, $log, $q, $timeout) {

    $scope.loadingShow = loadingShow;
    $scope.loadingHide = loadingHide;

    //Definicion de fechas
    $('#dateStartD').datetimepicker({
        format: 'MM/DD/YYYY hh:mm'
        //minDate: new Date()
    });
    $('#dateEndD').datetimepicker({
        format: 'MM/DD/YYYY hh:mm'
    });

    //$('#campaignDateStartD').datetimepicker({
    //    format: 'MM/DD/YYYY hh:mm',
    //    //minDate: new Date()
    //});
    //$('#campaignDateEndD').datetimepicker({
    //    format: 'MM/DD/YYYY hh:mm'
    //});
    $("#dateStartD").on("dp.change", function (e) {
        $("#dateEndD").data("DateTimePicker").minDate(e.date);
    });

    $("#dateStart").focus(function () {
        $("#dateStartD").data("DateTimePicker").show();
    });
    $("#dateEnd").focus(function () {
        $("#dateEndD").data("DateTimePicker").show();
    });

    //$("#campaignDateStartD").on("dp.change", function (e) {
    //    $("#campaignDateEndD").data("DateTimePicker").minDate(e.date);
    //});

    //$("#campaignDateStart").focus(function () {
    //    $("#campaignDateStartD").data("DateTimePicker").show();
    //});
    //$("#campaignDateEnd").focus(function () {
    //    $("#campaignDateEndD").data("DateTimePicker").show();
    //});


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

   

    $scope.getAndLoadData('businessSegmentList');
    $scope.getAndLoadData('businessUnitList');
    $scope.getAndLoadData('languagesList');
    $scope.getAndLoadData('targetAudienceList');
    $scope.getAndLoadData('theaterList');
    $scope.getAndLoadData('verticalsList');
    $scope.getAndLoadData('tacticList');
    $scope.getAndLoadData('ctaOfferList');
    $scope.getAndLoadData('subAreasList');
    // fin de carga

    //Funciones para Evaluar los valores de los Checkbox
    $scope.lenguagesSeleccionados = [];
    $scope.pss = {};
    $scope.verticalsSeleccionados = [];
    $scope.vss = {};

    $scope.evaluarListaLenguajes = function (valor) {
        var a = $scope.lenguagesSeleccionados.indexOf(valor);
        if (a === -1) {
            $scope.lenguagesSeleccionados.push(valor);

        } else {
            $scope.lenguagesSeleccionados.splice(a, 1);
            $scope.pss[valor] = false;
        }
    }

    $scope.changeLanguages = function (valor) {
        $scope.evaluarListaLenguajes(valor);

    };

    
    

    $scope.evaluarListaVerticales = function (valor) {
        var a = $scope.verticalsSeleccionados.indexOf(valor);
        if (a === -1) {
            $scope.verticalsSeleccionados.push(valor);
            //$scope.ps[valor] = true;
        } else {
            $scope.verticalsSeleccionados.splice(a, 1);
            $scope.vss[valor] = false;
        }
    }

    $scope.changeVerticals = function (valor) {
        $scope.evaluarListaVerticales(valor);

    };
    //FIN -- Funciones para Evaluar los valores de los Checkbox

    //FUNCIONES PARA CARGA DE SELECTS DESDE BASE DE DATOS -- SERVICIO WEB

    $scope.loadSelect = function (service, idToLoad, param) {
        var vistaActual = (param !== undefined) ? param : "fake";
        var datae = { 'vista': vistaActual };
        calendarBackEndModel.loadSelect(service, datae).then(function (result) {
            var data = result.d;
            try {
                
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    try {
                        $scope[idToLoad] = "";
                        $scope[idToLoad] = dataJson.data;
                        $scope.loadingHide();
                    } catch (exception) {
                        $scope.loadingHide();
                        message("Error", "Attention", "error");
                    }
                } else {
                    $scope.loadingHide();
                    message("Error", "Attention", "error");
                }
            } catch (e) {
                $scope.loadingHide();
                message("Error", "Attention", "error");
            }
            return false;
        });
    }

    //$scope.loadSelect("Serviciosweb/CalendarService.asmx/GetCampaigns", "campaignsList");
    //setTimeout(function () {
    //    $scope.loadSelect("Serviciosweb/CalendarService.asmx/GetPrograms", "programsList");
    //}, 2000);

    //FIN -- FUNCIONES PARA CARGA DE SELECTS DESDE BASE DE DATOS -- SERVICIO WEB

    $scope.eventId = $routeParams.eventId;

    $scope.getEventById = function () {
        $scope.loadingShow();
        calendarBackEndModel.getEventById($scope.eventId).then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    var datos = dataJson.data;
                    var foo = datos.languages;
                    var array = foo.split(",");
                    $scope.lenguagesSeleccionados = array;
                    
                    $.each(array, function (i) {
                        $('input[value="' + array[i] + '"]:checkbox').prop('checked', true);
                    });

                    var foo2 = datos.vertical;
                    var array2 = foo2.split(",");
                    $scope.verticalsSeleccionados = array2;

                    $.each(array2, function (i) {
                        $('input[value="' + array2[i] + '"]:checkbox').prop('checked', true);
                    });
                    $scope.eventActual = datos;
                    //setTimeout(function () {
                    //    setSelectByValueDiffType('programId', datos.programId);
                    //    setSelectByValueDiffType('campaignId', datos.campaignId);
                    //}, 2000);
                    $scope.loadingHide();
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
                $scope.loadingHide();
                message("Error", "Attention", "danger");
            }
        });
    };
    
    $scope.getEventById();

    $scope.updateNewEvent = function () {
		$('#createEvent').prop('disabled', true);
        $scope.loadingShow();
        var formulario = new Object();
        formulario.campaignName = $scope.eventActual.campaignName;
        formulario.programName = $scope.eventActual.programName;
        formulario.tacticName = $scope.eventActual.tacticName;
        formulario.tacticStage = $scope.eventActual.tacticStage;
        formulario.ctaOffer = $scope.eventActual.ctaOffer;
        formulario.dateStart = $("#dateStart").val();
        formulario.dateEnd = $("#dateEnd").val();
        formulario.sfdcCampaignName = $scope.eventActual.sfdcCampaignName;
        formulario.sfdcDashboardLink = $scope.eventActual.sfdcDashboardLink;
        formulario.theater = $scope.eventActual.theater;
        formulario.subArea = $scope.eventActual.subArea;
        formulario.languages = $scope.lenguagesSeleccionados.join();
        formulario.businessUnit = $scope.eventActual.businessUnit;
        formulario.businessSegment = $scope.eventActual.businessSegment;
        formulario.vertical = $scope.verticalsSeleccionados.join();
        formulario.promocode = $scope.eventActual.promocode;
        formulario.targetAudience = $scope.eventActual.targetAudience;
        formulario.agencyInternal = $scope.eventActual.agencyInternal;
        formulario.comments = $scope.eventActual.comments;
        formulario.linkLibrary = $scope.eventActual.linkLibrary;

        if (validarFormulario(formulario)) {
            
            formulario.eventId = $scope.eventId;
            formulario.status = $scope.eventActual.status;
            formulario.programInitials = $("#tacticName option:selected").data('icon');
            
            calendarBackEndModel.addNewEvent(formulario).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        $scope.loadingHide();
                        
                        limpiarFormulario(formulario);
                        $('#languagesCh :checkbox').each(function () {
                            $(this).prop('checked', false);
                        });
                        $('#verticalCh :checkbox').each(function () {
                            $(this).prop('checked', false);
                        });
                        $('#statusCh :checkbox').each(function () {
                            $(this).prop('checked', false);
                        });
                        $scope.loadingHide();
                        $scope.message = {
                            content: "Campaign Calendar Cloned...",
                            title: "Attention"
                        }

                        $("#messageModal").modal("show");
                        
                    } else {
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
                    $scope.loadingHide();
                    message("Error", "Attention", "danger");
                    
                }
            });
        } else {
			$('#createEvent').prop('disabled', false);
            $scope.loadingHide();
            $scope.loadingHide();
            message("Please, check the required fields", "Alert", "danger");
        }
    }


    //Adicionar nueva campaña
    $scope.addCampaign = function () {
        $("#addCampaignModal").modal("show");
    };

    $scope.addCampaignSubmit = function () {
        $scope.loadingShow();
        calendarBackEndModel.addNewCampaign($scope.campaignName).then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    //message("Content data saved", "Attention", "success");
                    $scope.loadingHide();
                    $("#addCampaignModal").modal("hide");
                    $scope.campaignName = "";
                    $scope.loadSelect("Serviciosweb/CalendarService.asmx/GetCampaigns", "campaignsList");
                } else {
                    $scope.loadingHide();
                    if (dataJson.data === "NoSession") {
                        document.location.href = "https://www4.avaya.com/mcs/site";
                    } else {
                        message("Error", "Attention", "danger");
                    }
                }
                
            } catch (e) {
                $scope.loadingHide();
                message("Error", "Attention", "danger");
            }
        });
    };
    //FIN - Adicionar nueva campaña

    //Adicionar nueva tactica/program
    $scope.addProgram = function () {
        $("#addProgramModal").modal("show");

    };

    $scope.addProgramSubmit = function () {
        $scope.loadingShow();
        calendarBackEndModel.addNewProgram($scope.programName).then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    $("#addProgramModal").modal("hide");
                    $scope.programName = "";
                    $scope.loadSelect("Serviciosweb/CalendarService.asmx/GetPrograms", "programsList");
                    $scope.loadingHide();
                } else {
                    $scope.loadingHide();
                    if (dataJson.data === "NoSession") {
                        document.location.href = "https://www4.avaya.com/mcs/site";
                    } else {
                        message("Error", "Attention", "danger");
                    }
                }
                
            } catch (e) {
                $scope.loadingHide();
                message("Error", "Attention", "danger");
                
            }
        });

    };
    //FIN - Adicionar nueva tactica/program

});