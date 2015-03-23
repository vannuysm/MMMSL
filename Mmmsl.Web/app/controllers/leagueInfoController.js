(function () {
    'use strict';

    var controllerId = 'leagueInfoController';

    angular.module('app').controller(controllerId,
        ['$scope', leagueInfoController]);

    function leagueInfoController($scope) {
        $scope.title = 'League Information :: MMMSL';
    }
})();