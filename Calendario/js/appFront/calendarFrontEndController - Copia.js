calendarFrontEndApp.controller('calendarInitController', function($scope, $route,calendarFrontEndModel, $routeParams, $location, $log, $q, $timeout) {
    $scope.loadingShow = loadingShow;
    $scope.loadingHide = loadingHide;
    $scope.allWeeks = [];
    $scope.success = false;
    $scope.showFeedback = function() {
        $("#feedbackModal").modal("show");
    };

    $scope.saveFeedback = function () {
        $scope.success = "";
        $scope.success = false;
        $scope.loadingShow();
        var feebackNumber = $('#mRadio input[name=optradio]:checked').val();
        var feedbackObservations = $scope.observations;
        calendarFrontEndModel.saveFeedback(feebackNumber, feedbackObservations).then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    try {
                        //$("#feedbackModal").modal("hide");
                        $('#mRadio input[name=optradio]').each(function (index, value) {
                            $(this).prop('checked', false);
                        });
                        $scope.observations = "";
                        $scope.success = true;
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
});

calendarFrontEndApp.controller('calendarController', function ($scope, calendarFrontEndModel,getJson, $route, $routeParams, $location, $log, $q, $timeout) {
        $scope.loadingShow = loadingShow;
        $scope.loadingHide = loadingHide;
        $scope.allWeeks = [];
        $scope.months = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
        $scope.dateSelected = "";
        $scope.semana1= [];
        $scope.semana2= [];
        $scope.semana3= [];
        $scope.semana4= [];
        $scope.semana5 = [];

        $scope.collect = function() {
            var ret = {};
            var len = arguments.length;
            for (var i = 0; i < len; i++) {
                for (p in arguments[i]) {
                    if (arguments[i].hasOwnProperty(p)) {
                        ret[p] = arguments[i][p];
                    }
                }
            }
            return ret;
        }

        //TODO revisar funcionamiento
        $scope.containsObject = function (list, obj) {
            if (list.length === 0) return true;
            
            for (var i = 0; i < list.length; i++) {
                if (list[i] === obj) {
                    return false;
                }
            }
            return true;
        }
        
        $scope.containsObject2 = function (obj, list) {
            if (list.length === 0) return true;

            for (var i = 0; i < list.length; i++) {
                if (list[i].eventId === obj.eventId) {
                    return false;
                }
            }
            return true;
        }

        $scope.getEvents = function () {
            var evReport = [];
            var len = arguments.length;
            var cond = false;
            for (var i = 0; i < len; i++) {
                angular.forEach(arguments[i], function (item) {
                    angular.forEach(item.Events, function (field) {
                        cond = $scope.containsObject2(field, evReport);
                        if (cond) {
                            evReport.push(field);
                        }
                        //evReport.push(field);
                    });
                });
            }
            return evReport;
        }

        $scope.getEventsActualDate = function() {
            $scope.loadingShow();
            calendarFrontEndModel.getEventsActualDate().then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        try {
                            $scope.semana1Ori = dataJson.semana1;
                            $scope.semana2Ori = dataJson.semana2;
                            $scope.semana3Ori = dataJson.semana3;
                            $scope.semana4Ori = dataJson.semana4;
                            $scope.semana5Ori = dataJson.semana5;

                            $scope.semana1 = dataJson.semana1;
                            $scope.semana2 = dataJson.semana2;
                            $scope.semana3 = dataJson.semana3;
                            $scope.loadNav($scope.semana3);
                            $scope.semana4 = dataJson.semana4;
                            $scope.semana5 = dataJson.semana5;
                            $scope.allWeeks = $scope.getEvents($scope.semana1, $scope.semana2, $scope.semana3, $scope.semana4, $scope.semana5);
                            $scope.tales = "stop";
                            $scope.loadingHide();
                            $scope.loadingHide();
                        } catch (exception) {
                            $scope.loadingHide();
                            $scope.loadingHide();
                            message("Error", "Attention", "error");
                        }
                    } else {
                        $scope.loadingHide();
                        $scope.loadingHide();
                        message("Error", "Attention", "error");
                    }
                    
                } catch (e) {
                    $scope.loadingHide();
                    $scope.loadingHide();
                    message("Error", "Attention", "error");
                    
                }
                
                return false;
            });
        }

        $scope.getEventsActualDate();

        $scope.showDetails = function(ev,close)
        {
            if (close) {
                $("#myModal").modal("hide");
            }
            $scope.actualEvent = ev;
            $("#modaltactic").modal("show");
        }

        $scope.showEvents = function(events) {
            $scope.dayEvents = events;
            $("#myModal").modal("show");
        }
        
        $scope.navMonth = {
            back: "",
            next: "",
            current: [],
            currentDate : ""
        };

        $scope.buildSelect = function(actualDate, first) {
            $scope.navMonth.current = [];
            for (var i = 0; i <= 12; i++) {
                $scope.select = {
                    date: "",
                    selected: ""
                };
                $scope.select.date = new Date(new Date(first).setMonth(first.getMonth() + i));
                var act = $scope.select.date.toDateString();
                var cur = actualDate.toDateString();
                if (act === cur) {
                    $scope.select.selected = true;
                    $scope.navMonth.currentDate = actualDate;
                    $scope.navMonth.current.push($scope.select);
                } else {
                    $scope.select.selected = false;
                    $scope.navMonth.current.push($scope.select);
                }
            }
        };

        $scope.actualDateNav = "";

        //Inicializar multiselect
        
        $scope.loadMultiselects = function() {
            var tales = $scope.languagesList;
            $.each($scope.languagesList, function (i, valor) {
                $("#languages").append('<option value="' + valor.name + '">' + valor.name + '</option>');
            });

            $('#languages').multiselect({
                includeSelectAllOption: true,
                //enableCaseInsensitiveFiltering: true,
                numberDisplayed: 0,
                maxHeight: 500,
                onChange: function (option, checked, select) {
                    //alert('Changed option ' + $(option).val() + '.');
                    console.log("test");
                }
            });

            $("#languages").multiselect('deselectAll', false);
            $("#languages").multiselect('updateButtonText');


            $.each($scope.verticalsList, function (i, valor) {
                $("#vertical").append('<option value="' + valor.name + '">' + valor.name + '</option>');
            });

            $('#vertical').multiselect({
                includeSelectAllOption: true,
                //enableCaseInsensitiveFiltering: true,
                numberDisplayed: 0,
                maxHeight: 500,
                onChange: function(option, checked, select) {
                    //alert('Changed option ' + $(option).val() + '.');
                    console.log("test");
                }
            });

            $("#vertical").multiselect('deselectAll', false);
            $("#vertical").multiselect('updateButtonText');
        };
    

        $("#languages").empty();
        $('#vertical').empty();
        setTimeout(function () {
            $scope.loadMultiselects();
        }, 4000);
        

        $scope.clearFilters = function() {
            $("#languages").empty();
            $('#vertical').empty();
            $scope.loadMultiselects();
            $("#programName").val("");
            $("#tacticStage").val("");
            $("#campaignName").val("");
            $scope.teatrosSeleccionados = [];
            $scope.tss = {};
            $scope.buSeleccionados = [];
            $scope.buss = {};
            $scope.bsSeleccionados = [];
            $scope.busss = {};
            
        }

    $scope.loadNav = function (semana3) {
            var actualDate = new Date(semana3[0].Date);
            $scope.actualDateNav = actualDate;
            var back = new Date(new Date(actualDate).setMonth(actualDate.getMonth() - 1));
            var next = new Date(new Date(actualDate).setMonth(actualDate.getMonth() + 1));
            var first = new Date(new Date(actualDate).setMonth(actualDate.getMonth() - 6));
            
            $scope.buildSelect(actualDate, first);
            $scope.navMonth.back = back;
            $scope.navMonth.next = next;
            //$scope.clearFilters();

    };


    $scope.getEventsByMonthYear = function(date) {
        var year = 0;
        var month = 0;
        if (date) {
            year = new Date(date).getFullYear();
            month = new Date(date).getMonth() + 1;
        } else {
            date = $scope.dateSelected;
            year = new Date(date).getFullYear();
            month = new Date(date).getMonth() + 1;
        }

        if ($scope.existeBusqueda()) {
            var campaignName = ($scope.campaignName !== undefined) ? $scope.campaignName : "";
            var programName = ($scope.programName !== undefined) ? $scope.programName : "";
            var tacticStage = ($scope.tacticStage !== undefined) ? $scope.tacticStage : "";
            var langs = $scope.getLangs();
            var vert = $scope.getVerticals();

            $scope.searchServer(date, vert, $scope.teatrosSeleccionados, $scope.buSeleccionados, $scope.bsSeleccionados, programName, langs, campaignName, tacticStage);
            $scope.getEventsByMonthYear2(date);
        } else {
            $scope.loadingShow();
            calendarFrontEndModel.getEventsByMonthYear(month, year).then(function(result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        try {

                            $scope.semana1Ori = dataJson.semana1;
                            $scope.semana2Ori = dataJson.semana2;
                            $scope.semana3Ori = dataJson.semana3;
                            $scope.semana4Ori = dataJson.semana4;
                            $scope.semana5Ori = dataJson.semana5;

                            $scope.semana1 = dataJson.semana1;
                            $scope.semana2 = dataJson.semana2;
                            $scope.semana3 = dataJson.semana3;
                            $scope.loadNav($scope.semana3);
                            $scope.semana4 = dataJson.semana4;

                            $scope.semana5 = dataJson.semana5;
                            $scope.allWeeks = $scope.getEvents($scope.semana1, $scope.semana2, $scope.semana3, $scope.semana4, $scope.semana5);
                            //trae los datos //validar si existen filtros

                            $scope.loadingHide();
                            $scope.loadingHide();
                        } catch (exception) {
                            $scope.loadingHide();
                            $scope.loadingHide();
                            message("Error", "Attention", "error");
                        }
                    } else {
                        $scope.loadingHide();
                        $scope.loadingHide();
                        message("Error", "Attention", "error");
                    }

                } catch (e) {
                    $scope.loadingHide();
                    $scope.loadingHide();
                    message("Error", "Attention", "error");

                }

                return false;
            });
        }
    };

        $scope.getEventsByMonthYear2 = function (date) {
            var year = 0;
            var month = 0;
            if (date) {
                year = new Date(date).getFullYear();
                month = new Date(date).getMonth() + 1;
            } else {
                date = $scope.dateSelected;
                year = new Date(date).getFullYear();
                month = new Date(date).getMonth() + 1;
            }
            $scope.loadingShow();
            calendarFrontEndModel.getEventsByMonthYear(month, year).then(function (result) {
                var data = result.d;
                try {
                    var dataJson = $.parseJSON(data);
                    if (dataJson.status === "ok") {
                        try {
                            $scope.semana1Ori = dataJson.semana1;
                            $scope.semana2Ori = dataJson.semana2;
                            $scope.semana3Ori = dataJson.semana3;
                            $scope.semana4Ori = dataJson.semana4;
                            $scope.semana5Ori = dataJson.semana5;
                            $scope.loadingHide();
                            $scope.loadingHide();
                        } catch (exception) {
                            $scope.loadingHide();
                            $scope.loadingHide();
                            message("Error", "Attention", "error");
                        }
                    } else {
                        $scope.loadingHide();
                        $scope.loadingHide();
                        message("Error", "Attention", "error");
                    }
                    
                } catch (e) {
                    $scope.loadingHide();
                    $scope.loadingHide();
                    message("Error", "Attention", "error");
                    
                }
                
                return false;
            });
        }

        
        //Filtrado del calendario
        //Carga de datos en los selects y checkbox
        $scope.getAndLoadData = function (nameList) {
            getJson.get(nameList).success(function (data) {
                $scope[nameList] = data;
            });
        }

        $scope.getAndLoadData('businessSegmentList');
        $scope.getAndLoadData('businessUnitList');
        $scope.getAndLoadData('languagesList');
        //$scope.getAndLoadData('subAreasList');
        $scope.getAndLoadData('targetAudienceList');
        $scope.getAndLoadData('theaterList');
        $scope.getAndLoadData('verticalsList');
        // fin de carga

        //Funciones para Evaluar los valores de los Checkbox
        $scope.lenguajesSeleccionados = [];
        $scope.pss = {};

        $scope.evaluarListaLenguajes = function (valor) {
            var a = $scope.lenguajesSeleccionados.indexOf(valor);
            if (a === -1) {
                $scope.lenguajesSeleccionados.push(valor);

            } else {
                $scope.lenguajesSeleccionados.splice(a, 1);
                $scope.pss[valor] = false;
            }
        }

        $scope.changeLanguages = function (valor) {
            $scope.evaluarListaLenguajes(valor);
        };

        $scope.verticalesSeleccionados = [];
        $scope.vss = {};
        $scope.teatrosSeleccionados = [];
        $scope.tss = {};
        $scope.buSeleccionados = [];
        $scope.buss = {};
        $scope.bsSeleccionados = [];
        $scope.busss = {};

        $scope.evaluarListaVerticales = function (valor) {
            var a = $scope.verticalesSeleccionados.indexOf(valor);
            if (a === -1) {
                $scope.verticalesSeleccionados.push(valor);
                //$scope.ps[valor] = true;
            } else {
                $scope.verticalesSeleccionados.splice(a, 1);
                $scope.vss[valor] = false;
            }
        }

        $scope.evaluarListas = function (lista,valor,model) {
            var a = $scope[lista].indexOf(valor);
            if (a === -1) {
                $scope[lista].push(valor);
                //$scope.ps[valor] = true;
            } else {
                $scope[lista].splice(a, 1);
                $scope[model][valor] = false;
            }
        }

        $scope.changeVerticals = function (valor) {
            $scope.evaluarListas('verticalesSeleccionados', valor, 'vss');
        };
        //$scope.changeTactic = function (valor) {
        //    $scope.evaluarListas('tacticasSeleccionadas', valor, 'tass');
        //};programId
        $scope.changeTheaters = function (valor) {
            $scope.evaluarListas('teatrosSeleccionados', valor, 'tss');
        };
        $scope.changeBusinessU = function (valor) {
            $scope.evaluarListas('buSeleccionados', valor, 'buss');
        };
        $scope.changeBusinessS = function (valor) {
            $scope.evaluarListas('bsSeleccionados', valor, 'busss');
        };
    $scope.getLangs = function() {
        var langValues2 = "";
        var langs2 = "";
        $("#languages option:selected").each(function() {
            var lang2 = $(this).text();
            var langValue2 = $(this).val();
            langs2 = langs2 + lang2 + ",";
            langValues2 = langValues2 + langValue2 + ",";
        });
        if ((langValues2.substring(langValues2.length - 1, langValues2.length)) === ',') {
            langValues2 = langValues2.substring(0, langValues2.length - 1);
        }
        return langValues2;
    };
    $scope.getVerticals = function() {
        var langValues2 = "";
        var langs2 = "";
        $("#vertical option:selected").each(function() {
            var lang2 = $(this).text();
            var langValue2 = $(this).val();
            langs2 = langs2 + lang2 + ",";
            langValues2 = langValues2 + langValue2 + ",";
        });
        if ((langValues2.substring(langValues2.length - 1, langValues2.length)) === ',') {
            langValues2 = langValues2.substring(0, langValues2.length - 1);
        }
        return langValues2;
    };

    //changeTactic   changeTheaters    changeBusinessU    changeBusinessS    changeVerticals

    //FIN -- Funciones para Evaluar los valores de los Checkbox

    //FUNCIONES PARA CARGA DE SELECTS DESDE BASE DE DATOS -- SERVICIO WEB

    $scope.loadSelect = function(service, idToLoad, param) {
        $scope.loadingShow();
        var vistaActual = (param !== undefined) ? param : "fake";
        var datae = { 'vista': vistaActual };
        calendarFrontEndModel.loadSelect(service, datae).then(function(result) {
            var data = result.d;
            try {
                $scope.loadingHide();
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
    };

        //$scope.loadSelect("Serviciosweb/CalendarService.asmx/GetCampaigns", "campaignsList");
        //setTimeout(function () {
        //    $scope.loadSelect("Serviciosweb/CalendarService.asmx/GetPrograms", "programsList");
        //}, 2000);



    //FIN -- FUNCIONES PARA CARGA DE SELECTS DESDE BASE DE DATOS -- SERVICIO WEB



    /**  INICIO DEL FILTRO  **/

   

    $scope.searchServer = function (actualDate, vert, teatrosSeleccionados, buSeleccionados, bsSeleccionados, programName, langs, campaignName, tacticStage) {
        $scope.loadingShow();
        calendarFrontEndModel.searchServer(actualDate, vert, teatrosSeleccionados, buSeleccionados, bsSeleccionados, programName, langs, campaignName, tacticStage).then(function (result) {
            var data = result.d;
            try {
                
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    try {

                        $scope.semana1Ori = dataJson.semana1;
                        $scope.semana2Ori = dataJson.semana2;
                        $scope.semana3Ori = dataJson.semana3;
                        $scope.semana4Ori = dataJson.semana4;
                        $scope.semana5Ori = dataJson.semana5;

                        $scope.semana1 = dataJson.semana1;
                        $scope.semana2 = dataJson.semana2;
                        $scope.semana3 = dataJson.semana3;
                        $scope.loadNav($scope.semana3);
                        $scope.semana4 = dataJson.semana4;
                        $scope.semana5 = dataJson.semana5;
                        $scope.allWeeks = $scope.getEvents($scope.semana1, $scope.semana2, $scope.semana3, $scope.semana4, $scope.semana5);
                        $scope.loadingHide();
                        $scope.loadingHide();
                    } catch (exception) {
                        $scope.loadingHide();
                        $scope.loadingHide();
                        message("Error", "Attention", "error");
                    }
                } else {
                    $scope.loadingHide();
                    $scope.loadingHide();
                    message("Error", "Attention", "error");
                }
                
            } catch (e) {
                $scope.loadingHide();
                $scope.loadingHide();
                message("Error", "Attention", "error");
                
            }
            return false;
        });

        
    };

    $scope.existeBusqueda = function() {
        var campaignName = ($scope.campaignName !== undefined) ? $scope.campaignName : "";
        var programName = ($scope.programName !== undefined) ? $scope.programName : "";
        var tacticStage = ($scope.tacticStage !== undefined) ? $scope.tacticStage : "";
        var langs = $scope.getLangs();
        var vert = $scope.getVerticals();
        var teatro = ($scope.teatrosSeleccionados.length === 0) ? "" : $scope.teatrosSeleccionados;
        var bu = ($scope.buSeleccionados.length === 0) ? "" : $scope.buSeleccionados;
        var bs = ($scope.bsSeleccionados.length === 0) ? "" : $scope.bsSeleccionados; 

        if (campaignName || programName || tacticStage || langs || vert || teatro || bu || bs) {
            return true;
        }
        return false;
    };


    $scope.search = function () {
        var campaignName = ($scope.campaignName !== undefined) ? $scope.campaignName : "";
        var programName = ($scope.programName !== undefined) ? $scope.programName : "";
        var tacticStage = ($scope.tacticStage !== undefined) ? $scope.tacticStage : "";
        var langs = $scope.getLangs();
        var vert = $scope.getVerticals();
          
        var semana3 = $scope.semana3;
        var actualDate = new Date(semana3[0].Date);

        $scope.searchServer(actualDate, vert, $scope.teatrosSeleccionados, $scope.buSeleccionados, $scope.bsSeleccionados, programName, langs, campaignName, tacticStage);
        $scope.getEventsByMonthYear2(actualDate);
            
    };

    $scope.showAll = function() {
        $scope.loadingShow();
        var semana3 = $scope.semana3;
        var actualDate = new Date(semana3[0].Date);
        $scope.getEventsByMonthYear(actualDate);
        $scope.loadingHide();
    };

    

    //$scope.searchIntoJson = function (obj, value) {

    //    var searchString = value;
    //    var arr = obj;
    //    if (!searchString) {
    //        return [];
    //    } else {
    //        var result = [];
    //        searchString = searchString.toLowerCase();

    //        angular.forEach(arr, function (item) {
    //            var cond = false;
    //            angular.forEach(item, function (field) {
    //                try {
    //                    if (field.toLowerCase().indexOf(searchString) !== -1) {
    //                        cond = $scope.containsObject(item, result);
    //                        if (cond) {
    //                            result.push(item);
    //                        }
    //                    }
    //                } catch (e) {
    //                    if (field) {
    //                        field = field.toString();
    //                        if (field.toLowerCase().indexOf(searchString) !== -1) {
    //                            cond = $scope.containsObject(item, result);
    //                            if (cond) {
    //                                result.push(item);
    //                            }
    //                        }
    //                    }
    //                }
    //            });
    //        });

    //        if (result !== []) {
    //            try {
    //                $scope.result = [];
    //                $scope.result = result;
    //                return $scope.result;
    //            } catch (e) {
    //                return $scope.result;
    //            }
    //        } else {
    //            return [];
    //        }

    //    }
    //};

    //**  FIN DEL FILTRO  **///

   

    ///***  COMIENZA EL EXPORT ***///


    $scope.doExport = function(params) {
        var options = {
            tableName: 'FeedBacks',
            worksheetName: 'FeedBacks'
        };
        $.extend(true, options, params);
        $('#reporte').tableExport(options);
    };

    $scope.toExcel = function() {
        $scope.doExport({ type: 'excel' });
    };
	
	$scope.changeToMontly = function () {
        $scope.montly = true;

        $("#weeklytable").removeClass("display");
        $("#weeklytable").addClass("hide");
        $("#weeklytabletabs").removeClass("display");
        $("#weeklytabletabs").addClass("hide");

        $("#monthlytable").removeClass("hide");
        $("#monthlytable").addClass("display");
        $("#monthlytablehdr").removeClass("hide");
        $("#monthlytablehdr").addClass("display");
    };

    $scope.changeToWeekly = function () {
        $scope.montly = false;

        $("#weeklytable").removeClass("hide");
        $("#weeklytable").addClass("display");
        $("#weeklytabletabs").removeClass("hide");
        $("#weeklytabletabs").addClass("display");

        $("#monthlytable").removeClass("display");
        $("#monthlytable").addClass("hide");
        $("#monthlytablehdr").removeClass("display");
        $("#monthlytablehdr").addClass("hide");
    };

    $scope.changeToWeekly();


});


$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip(); 
});
