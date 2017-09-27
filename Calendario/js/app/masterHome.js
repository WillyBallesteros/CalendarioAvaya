// Creación del módulo
var angularRoutingApp = angular.module('scriptingToolApp', ['ngRoute', 'ngSanitize']);

angularRoutingApp.directive('myEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.myEnter);
                });

                event.preventDefault();
            }
        });
    };
});
// Configuración de las rutas
angularRoutingApp.config(function ($routeProvider) {
   

    $routeProvider
        .when('/', {
            templateUrl: 'views/home.html',
            controller: 'homeController'
            
        })
        .when('/focusMessaging', {
            templateUrl: 'views/focusMessaging.html',
            controller: 'focusMessagingController',
            activeTab: 'focusMessaging'
        })
        .when('/handleObjections', {
            templateUrl: 'views/handleObjections.html',
            controller: 'handleObjectionsController',
            activeTab: 'handleObjections'
        })
        .when('/voiceMails', {
            templateUrl: 'views/voiceMails.html',
            controller: 'voiceMailsController',
            activeTab: 'voiceMails'
        })
        .when('/propositionMessages', {
            templateUrl: 'views/propositionMessages.html',
            controller: 'propositionMessagesController',
            activeTab: 'propositionMessages'
        })
        .when('/campaignsDirectory', {
            templateUrl: 'views/campaignsDirectory.html',
            controller: 'campaignsDirectoryController',
            activeTab: 'campaignsDirectory'
        })
        .when('/usefulLinks', {
            templateUrl: 'views/usefulLinks.html',
            controller: 'usefulLinksController',
            activeTab: 'usefulLinks'
        })
        .when('/library', {
            templateUrl: 'views/library.html',
            controller: 'libraryController',
            activeTab: 'library'
        })
        .when('/training', {
            templateUrl: 'views/training.html',
            controller: 'trainingController',
            activeTab: 'training'
        })
        .when('/stacattoSalesTechnique', {
            templateUrl: 'views/stacattoSalesTechnique.html',
            controller: 'stacattoSalesTechniqueController',
            activeTab: 'stacattoSalesTechnique'
        })
        .when('/responders', {
            templateUrl: 'views/responders.html',
            controller: 'respondersController',
            activeTab: 'responders'
        })
        .when('/list', {
            templateUrl: 'views/list.html',
            controller: 'listController',
            activeTab: 'list'
        })
        .when('/search/:searchParam', {
            templateUrl: 'views/search.html',
            controller: 'searchController'
        })
        .otherwise({
            redirectTo: '/'
        });
    
});




