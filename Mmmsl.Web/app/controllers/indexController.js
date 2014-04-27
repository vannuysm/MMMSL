(function () {
    'use strict';

    var controllerId = 'indexController';

    angular.module('app').controller(controllerId,
        ['$scope', homeController]);

    function homeController($scope) {
        $scope.title = 'Welcome to MMMSL';
    }
})();
