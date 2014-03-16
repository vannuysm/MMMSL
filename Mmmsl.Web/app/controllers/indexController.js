(function () {
    'use strict';

    var controllerId = 'indexController';

    angular.module('app').controller(controllerId,
        ['$scope', homeController]);

    function homeController($scope) {
        $scope.activate = activate;
        $scope.title = 'Welcome to MMMSL';

        function activate() { }
    }
})();
