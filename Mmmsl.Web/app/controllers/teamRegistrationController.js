(function () {
    'use strict';

    var controllerId = 'teamRegistrationController';

    angular.module('app').controller(controllerId,
        ['$scope', teamRegistrationController]
    );

    function teamRegistrationController($scope) {
        $scope.title = 'Team Registration';

        $scope.register = register;
        $scope.errorClass = errorClass;
        $scope.status = { submitted: false, submitting: false };

        function register(form, team) {
            $scope.status.submitted = true;
            $scope.status.submitting = true;

            if (form.$invalid) {
                $scope.status.submitting = false;
                return;
            }
            console.log(team);

            $scope.status.submitting = false;
        }

        function errorClass(form, field) {
            if (typeof field === 'string') {
                return { 'has-error': hasError(field) };
            }

            return {
                'has-error': field.some(function(fieldName) {
                    return hasError(fieldName);
                })
            };

            function hasError(fieldName) {
                return (form[fieldName].$dirty || $scope.status.submitted) && form[fieldName].$invalid;
            }
        }
    }
})();
