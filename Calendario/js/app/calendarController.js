calendarBackEndApp.controller('calendarController', function ($scope, calendarBackEndModel, $route, $routeParams, $location, $log, $q, $timeout) {

    $scope.months = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
    $scope.dateSelected = "";

    $scope.getEventsActualDate = function () {
        loading.show();
        calendarBackEndModel.getEventsActualDate().then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    try {
                        $scope.semana1 = dataJson.semana1;
                        $scope.semana2 = dataJson.semana2;
                        $scope.semana3 = dataJson.semana3;
                        $scope.loadNav($scope.semana3);
                        $scope.semana4 = dataJson.semana4;
                        $scope.semana5 = dataJson.semana5;
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

    $scope.getEventsActualDate();

    $scope.showDetails = function (ev, close) {
        if (close) {
            $("#myModal").modal("hide");
        }
        $scope.actualEvent = ev;
        $("#modaltactic").modal("show");
    }

    $scope.showEvents = function (events) {
        $scope.dayEvents = events;
        $("#myModal").modal("show");
    }


    $scope.navMonth = {
        back: "",
        next: "",
        current: [],
        currentDate: ""
    };

    $scope.buildSelect = function (actualDate, first) {
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

    $scope.loadNav = function (semana3) {
        var actualDate = new Date(semana3[0].Date);
        var back = new Date(new Date(actualDate).setMonth(actualDate.getMonth() - 1));
        var next = new Date(new Date(actualDate).setMonth(actualDate.getMonth() + 1));
        var first = new Date(new Date(actualDate).setMonth(actualDate.getMonth() - 6));

        $scope.buildSelect(actualDate, first);
        $scope.navMonth.back = back;
        $scope.navMonth.next = next;
    };

    $scope.getEventsByMonthYear = function (date) {
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
        loading.show();
        calendarBackEndModel.getEventsByMonthYear(month, year).then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    try {
                        $scope.semana1 = dataJson.semana1;
                        $scope.semana2 = dataJson.semana2;
                        $scope.semana3 = dataJson.semana3;
                        $scope.loadNav($scope.semana3);
                        $scope.semana4 = dataJson.semana4;
                        $scope.semana5 = dataJson.semana5;
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

    $scope.addCampaign = function (fecha) {
        if (fecha) {
            $location.path("/addCampaign/" + fecha);
        } else {
            $location.path("/addCampaign");
        }
    }


});
