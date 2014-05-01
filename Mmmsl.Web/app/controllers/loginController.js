(function () {
    'use strict';

    var controllerId = 'loginController';

    angular.module('app').controller(controllerId,
        ['$scope', '$http', '$window', '$location', loginController]);

    function loginController($scope, $http, $window, $location) {
        if ($window.sessionStorage.apiToken) {
            $location.path('/profile');
        }

        $scope.title = 'Login';

        $scope.login = login;
        $scope.errorMessage = null;
        $scope.errorClass = errorClass;
        $scope.status = { submitted: false, submitting: false };

        function login(form, user) {
            $scope.errorMessage = null;
            $scope.status.submitted = true;
            $scope.status.submitting = true;

            if (form.$invalid) {
                $scope.status.submitting = false;
                return;
            }

            user.grant_type = 'password';
            $http.post($window.sessionStorage.apiUrl + '/auth/token', $.param(user), {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                })
                .success(function (data) {
                    $window.sessionStorage.apiToken = data.access_token;

                    var returnUrl = $location.search().returnUrl;
                    if (returnUrl && returnUrl !== '/login') {
                        $location.search({});
                        $location.path(returnUrl)
                    }
                    else {
                        $location.path('/profile');
                    }
                })
                .error(function (response) {
                    $scope.errorMessage = response.error_description;
                })
                .finally(function() {
                    $scope.status.submitting = false;
                });
        }

        function errorClass(form, field) {
            if (typeof field === 'string') {
                return { 'has-error': hasError(field) };
            }

            return {
                'has-error': field.some(function (fieldName) {
                    return hasError(fieldName);
                })
            };

            function hasError(fieldName) {
                return (form[fieldName].$dirty || $scope.status.submitted) && form[fieldName].$invalid;
            }
        }
    }
})();
