(function () {
    'use strict';

    var controllerId = 'divisionStandingsController';

    angular.module('app').controller(controllerId,
        ['$scope', '$routeParams', '$http', '$window', divisionStandingsController]);

    function divisionStandingsController($scope, $routeParams, $http, $window) {
        var division = $routeParams['division'];

        $scope.title = division + ' Division Standings';

        $scope.standings = [];
        $scope.isLoaded = false;

        $http.get($window.sessionStorage.apiUrl + '/api/divisions/' + division + '/standings')
            .success(function (result) {
                $scope.standings = result;
                $scope.isLoaded = true;
            });
    }
})();
