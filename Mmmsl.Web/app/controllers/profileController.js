(function () {
    'use strict';

    var controllerId = 'profileController';

    angular.module('app').controller(controllerId,
        ['$scope', '$http', '$window', profileController]);

    function profileController($scope, $http, $window) {

        $scope.title = 'Profile';
        $scope.profile = {
            id: 0,
            userId: null,
            firstName: null,
            lastName: null,
            email: null,
            phone: null,
            mailingAddress: {
                address1: null,
                address2: null,
                city: null,
                state: null,
                zipCode: null
            }
        }
        $scope.managerTeams = [];
        $scope.playerTeams = [];

        $http.get($window.sessionStorage.apiUrl + '/api/account/current')
            .success(function (result) {
                $scope.profile = result.profile;
                $scope.managerTeams = result.managerTeams;
                $scope.playerTeams = result.playerTeams;
            });

        $scope.updateProfile = updateProfile;
        $scope.status = { submitted: false, submitting: false };

        function updateProfile(form, profile) {
            $scope.status.submitted = true;
            $scope.status.submitting = true;

            if (form.$invalid) {
                $scope.status.submitting = false;
                return;
            }

            $scope.status.submitting = false;
        }
    }
})();
