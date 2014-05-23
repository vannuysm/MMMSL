(function () {
    'use strict';

    var controllerId = 'addPlayerController';

    angular.module('app').controller(controllerId,
        ['$scope', '$modalInstance', '$http', '$window', 'team', addPlayerController]);

    function addPlayerController($scope, $modalInstance, $http, $window, team) {
        $scope.player = {
            firstName: '',
            lastName: ''
        };

        $scope.team = team;

        $scope.save = function () {
            $http.post($window.sessionStorage.apiUrl + '/api/teams/' + team.id + '/players', $scope.player)
                .success(function (result) {
                    $modalInstance.close(result);
                });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
})();