(function () {
    'use strict';

    var controllerId = 'manageGameResultsController';

    angular.module('app').controller(controllerId,
        ['$scope', '$routeParams', '$http', '$window', manageGameResultsController]);

    function manageGameResultsController($scope, $routeParams, $http, $window) {
        var gameId = $routeParams['gameId'];


    }
})();
