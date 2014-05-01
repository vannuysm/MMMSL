(function () {
    'use strict';

    var controllerId = 'manageGamesController';

    angular.module('app').controller(controllerId,
        ['$scope', '$http', '$window', manageGamesController]);

    function manageGamesController($scope, $http, $window) {
        $scope.title = 'Manage Games';

        $scope.isLoaded = false;
        $scope.isSubmitting = false;

        $scope.divisions = [];
        $scope.games = [];
        $scope.teams = [];
        $scope.locations = [];

        $scope.selectedDivision = {};

        $scope.newGame = new emptyGame();

        $scope.addGame = function (game) {
            var newGame = $.extend(true, {}, game);
            newGame.divisionId = $scope.selectedDivision.id;
            newGame.date = moment.utc(game.date, 'YYYY-MM-DD hh:mm A').toISOString().substr(0, 19);
            
            $http.post($window.sessionStorage.apiUrl + '/api/games', newGame)
                .success(function (result) {
                    $scope.games.push(newGame);
                    $scope.newGame = new emptyGame();
                });
        };

        $scope.$watch('selectedDivision', function (division) {
            if (!division.id) {
                $scope.teams = [];
                return;
            }

            $http.get($window.sessionStorage.apiUrl + '/api/divisions/' + division.alias + '/teams')
                .success(function (result) {
                    $scope.teams = result;
                });

            $http.get($window.sessionStorage.apiUrl + '/api/divisions/' + division.alias + '/games')
                .success(function (result) {
                    $scope.games = result;
                });

            $http.get($window.sessionStorage.apiUrl + '/api/locations/')
                .success(function (result) {
                    $scope.locations = result;
                });
        });

        $http.get($window.sessionStorage.apiUrl + '/api/leagues/mmmsl/divisions')
            .success(function (result) {
                $scope.divisions = result;
                $scope.isLoaded = true;
            });

        function emptyGame() {
            return {
                date: null,
                homeTeam: {},
                awayTeam: {},
                location: {}
            };
        }
    }
})();
