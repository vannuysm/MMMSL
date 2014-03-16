(function () {
    'use strict';

    var id = 'app';

    var app = angular.module('app', [
        'ngAnimate',
        'ngRoute'
    ]);

    app.config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider
                .when('/', {
                    controller: 'indexController',
                    templateUrl: 'app/views/index.html'
                })
                .when('/player/register', {
                    controller: 'playerRegistrationController',
                    templateUrl: 'app/views/playerRegistration.html'
                });
        }
    ]);

    app.run(['$q', '$rootScope',
        function ($q, $rootScope) {

        }
    ]);
})();