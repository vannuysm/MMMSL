(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngRoute'
    ]);

    app.factory('authInterceptor', ['$rootScope', '$q', '$window', '$location',
        function ($rootScope, $q, $window, $location) {
            return {
                request: function (config) {
                    config.headers = config.headers || {};
                    if ($window.sessionStorage.apiToken) {
                        config.headers.Authorization = 'Bearer ' + $window.sessionStorage.apiToken;
                    }
                    return config;
                },
                responseError: function (response) {
                    if (response.status === 401) {
                        var returnUrl = $location.$$path;
                        $location.path('/login').search({ returnUrl: returnUrl });
                    }
                    return $q.reject(response);
                }
            };
        }
    ]);

    app.config(['$routeProvider', '$httpProvider',
        function ($routeProvider, $httpProvider) {
            $httpProvider.interceptors.push('authInterceptor');

            $routeProvider
                .when('/', {
                    controller: 'indexController',
                    templateUrl: 'app/views/index.html'
                })
                .when('/login', {
                    controller: 'loginController',
                    templateUrl: 'app/views/login.html'
                })
                .when('/profile', {
                    controller: 'profileController',
                    templateUrl: 'app/views/profile.html'
                })
                .when('/team/register', {
                    controller: 'teamRegistrationController',
                    templateUrl: 'app/views/teamRegistration.html'
                })
                .when('/:division/schedule', {
                    controller: 'divisionScheduleController',
                    templateUrl: 'app/views/divisionSchedule.html'
                })
                .when('/manage/games', {
                    controller: 'manageGamesController',
                    templateUrl: 'app/views/manageGames.html'
                });
        }
    ]);

    app.run(['$rootScope', '$window', '$location',
        function ($rootScope, $window, $location) {
            $rootScope.$on('$routeChangeStart', function (event, next, current) {
                var isRestricted = next.$$route.originalPath.substring(0, 7) === '/manage';

                if (isRestricted && !$window.sessionStorage.apiToken) {
                    var returnUrl = $location.$$path;
                    $location.path('/login').search({ returnUrl: returnUrl });
                }
            });
        }
    ]);
})();