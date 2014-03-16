(function () {
    'use strict';

    var controllerId = 'playerRegistrationController';

    angular.module('app').controller(controllerId,
        ['$scope', playerRegistrationController]
    );

    function playerRegistrationController($scope) {
        $scope.activate = activate;
        $scope.title = 'Player Registration';

        function activate() { }
    }
})();
