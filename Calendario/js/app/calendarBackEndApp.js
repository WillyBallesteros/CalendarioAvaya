var calendarBackEndApp = angular.module('calendarBackEndApp', ['ngRoute', 'ngSanitize']);

function loadingShow(texto) {
    var pleaseWaitDiv = "";
    if (texto !== undefined) {
        pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:500px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">' + texto + '</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
    } else {
        pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:250px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">Loading...</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
    }
    pleaseWaitDiv.modal("show");
};


function loadingHide() {
    $('#pleaseWaitDialog').each(function(index, value) {
        $(this).remove();
    });

    $('.modal-backdrop').each(function(index, value) {
        $(this).remove();
    });

    $('body').removeClass('modal-open');


};



calendarBackEndApp.factory('getJson', function ($http) {
    return {
        get: function (nameList) {
            var foo = $http.get('Data/' + nameList + '.json');
                return foo;
        }
    };
});

calendarBackEndApp.filter('cmdate', [
        '$filter', function ($filter) {
            return function (input, format) {
                return $filter('date')(new Date(input), format).toUpperCase();
            };
        }
]);

calendarBackEndApp.filter('cmQ', [
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

calendarBackEndApp.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function(scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function(val) {
                return parseInt(val, 10);
            });
            ngModel.$formatters.push(function(val) {
                return '' + val;
            });
        }
    };
});

// Configuración de las rutas
calendarBackEndApp.config(function ($routeProvider) {


    $routeProvider
        .when('/', {
            templateUrl: 'views/backEnd/listaCampaigns.html?ver=1.2',
            controller: 'listaCampaignsController'
        })
        .when('/listCampaigns', {
            templateUrl: 'views/backEnd/listaCampaigns.html?ver=1.2',
            controller: 'listaCampaignsController'
        })
        .when('/addCampaign', {
            templateUrl: 'views/backEnd/addCampaign.html?ver=1.2',
            controller: 'addCampaignController'
        })
         .when('/addCampaign/:fecha', {
             templateUrl: 'views/backEnd/addCampaign.html?ver=1.2',
             controller: 'addCampaignController'
         })
        .when('/updateCampaign/:eventId', {
            templateUrl: 'views/backEnd/updateCampaign.html?ver=1.2',
            controller: 'updateCampaignController'
        })
        .when('/cloneCampaign/:eventId', {
            templateUrl: 'views/backEnd/cloneCampaign.html?ver=1.2',
            controller: 'cloneCampaignController'
        })
        .when('/calendar', {
            templateUrl: 'views/backEnd/calendar.html?ver=1.2',
            controller: 'calendarController'
        })
        .when('/listaUsuarios', {
            templateUrl: 'views/backEnd/users.html?ver=1.2',
            controller: 'usersController'
        })
        .when('/listaFeedbacks', {
            templateUrl: 'views/backEnd/feedback.html?ver=1.2',
            controller: 'feedbackController'
        })
        .otherwise({
            redirectTo: '/'
        });

    calendarBackEndApp.directive('pleaseWaitDialog', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                scope.dismiss = function () {
                    element.modal('hide');
                };
            }
        }
    });
});
