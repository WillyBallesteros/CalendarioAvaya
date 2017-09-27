

    function message(texto, titulo, tipo) {
        BootstrapDialog.show({
            title: titulo,
            message: texto,
            cssClass: 'type-' + tipo
        });
        return false;
    }

    var calendarBlitzApp = angular.module('calendarBlitzApp', ['ngRoute', 'ngSanitize', 'AngularPrint']);

    function loadingShow(texto) {
        var pleaseWaitDiv = "";
        if (texto !== undefined) {
            pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:500px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">' + texto + '</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
        } else {
            pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:250px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">Loading...</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
        }
        pleaseWaitDiv.modal("show");
    };
    
    calendarBlitzApp.factory('getJson', function ($http) {
        return {
            get: function (nameList) {
                var foo = $http.get('Data/' + nameList + '.json');
                return foo;
            }
        };
    });

    calendarBlitzApp.directive('convertToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (val) {
                    return parseInt(val, 10);
                });
                ngModel.$formatters.push(function (val) {
                    return '' + val;
                });
            }
        };
    });

    function loadingHide() {
        $('#pleaseWaitDialog').each(function (index, value) {
            $(this).remove();
        });

        $('.modal-backdrop').each(function (index, value) {
            $(this).remove();
        });

        $('body').removeClass('modal-open');


    };

    calendarBlitzApp.filter('searchFor', function () {
        return function (arr, searchString) {
            if (!searchString) {
                return arr;
            }
            var result = [];
            searchString = searchString.toLowerCase();
            angular.forEach(arr, function (item) {
                if (item.title.toLowerCase().indexOf(searchString) !== -1) {
                    result.push(item);
                }
            });
            return result;
        };
    });

    calendarBlitzApp.filter("commaBreak", function () {
        return function (value) {
            try {
                if (!value.length) return;
                return value.split(',');
            } catch (e) {
                return "";
            }
        }
    });
    
    calendarBlitzApp.directive('pleaseWaitDialog', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                scope.dismiss = function () {
                    element.modal('hide');
                };
            }
        }
    });

    calendarBlitzApp.filter('cmdate', [
        '$filter', function ($filter) {
            return function (input, format) {
                return $filter('date')(new Date(input), format).toUpperCase();
            };
        }
    ]);

    calendarBlitzApp.filter('cmQ', [
        '$filter', function ($filter) {
            return function (input) {
                var fecha = new Date(input);
                var year = fecha.getFullYear().toString().substr(2, 2);
                var Q = "";
                var month = fecha.getMonth();
                if (month >= 0 && month <= 2) {
                    Q = "Q2";
                }
                if (month >= 3 && month <= 5) {
                    Q = "Q3";
                }
                if (month >= 6 && month <= 8) {
                    Q = "Q4";
                }
                if (month >= 9 && month <= 11) {
                    Q = "Q1";
                    year = (fecha.getFullYear() + 1).toString().substr(2, 2);
                }
                //return $filter('date')(new Date(input), format);
                return "FY" + year + Q;
            };
        }
    ]);

    calendarBlitzApp.config(function ($routeProvider) {


        $routeProvider
            .when('/', {
                templateUrl: 'views/frontEnd/calendarBlitz.html',
                controller: 'calendarBlitzController'
            })
            .when('/detailsCampaign', {
                templateUrl: 'views/frontEnd/detailsCampaign.html',
                controller: 'detailsCampaignController'
            })
            .otherwise({
                redirectTo: '/'
            });

        
    });

   
    

