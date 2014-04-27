(function () {
    'use strict';

    var controllerId = 'divisionScheduleController';

    angular.module('app').controller(controllerId,
        ['$scope', '$routeParams', '$http', '$window', divisionScheduleController]);

    function divisionScheduleController($scope, $routeParams, $http, $window) {
        var division = $routeParams['division'];

        $scope.title = division + ' Division Games';

        $scope.schedule = [];
        $scope.isLoaded = false;

        $http.get($window.sessionStorage.apiUrl + '/api/divisions/' + division + '/schedule')
            .success(function (result) {
                $scope.schedule = result;
                $scope.isLoaded = true;
            });
    }
})();
