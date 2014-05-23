(function () {
    'use strict';

    var controllerId = 'manageGamesController';

    angular.module('app').controller(controllerId,
        ['$scope', '$http', '$window', '$routeParams', '$modal', '$filter', manageGamesController]);

    function manageGamesController($scope, $http, $window, $routeParams, $modal, $filter) {
        $scope.title = 'Manage Games';

        $scope.isLoaded = false;
        $scope.isSubmitting = false;

        $scope.divisions = [];
        $scope.games = [];
        $scope.teams = [];
        $scope.locations = [];
        $scope.penaltyCards = [];

        $scope.selectedDivision = {};
        $scope.teamTabs = ['homeTeam', 'awayTeam'];

        $scope.newGame = new defaultGame();
        $scope.newGoal = new defaultGoal();
        $scope.newBooking = new defaultBooking();

        $scope.selectGame = function (game) {
            game.hasOpened = true;
        }

        $scope.selectTeam = function (team, game) {
            if (!team) {
                return;
            }

            if (team.initialized) {
                return;
            }

            $http.get($window.sessionStorage.apiUrl + '/api/teams/' + team.id + '/players')
                .success(function (result) {
                    team.players = result;

                    team.bookings = _.filter(game.bookings, function (booking) {
                        return _.contains(_.pluck(team.players, 'id'), booking.playerId);
                    });
                });
            
            $http.get($window.sessionStorage.apiUrl + '/api/goals/?gameId=' + game.id + '&teamId=' + team.id)
                .success(function (result) {
                    team.goals = result;
                });

            team.initialized = true;
        };

        $scope.addGame = function (game) {
            var newGame = $.extend(true, {}, game);
            newGame.divisionId = $scope.selectedDivision.id;
            newGame.date = moment.utc(game.date, 'YYYY-MM-DD hh:mm A').toISOString().substr(0, 19);
            
            $http.post($window.sessionStorage.apiUrl + '/api/games', newGame)
                .success(function (result) {
                    $scope.games.push(newGame);
                    $scope.newGame = new defaultGame();
                });
        };

        $scope.addGoal = function (team, goal, game) {
            if (!goal.player.id || !goal.count) {
                return;
            }

            var alreadyExists = team.goals.some(function (g) {
                return g.player.id == goal.player.id;
            });

            if (alreadyExists) {
                return;
            }

            $http.post($window.sessionStorage.apiUrl + '/api/goals', {
                gameId: game.id,
                playerId: goal.player.id,
                count: goal.count
            })
            .success(function (result) {
                goal.id = result.id;

                team.goals.push(goal);
                game.goals.push(goal);
            });

            $scope.newGoal = new defaultGoal();
        };

        $scope.addBooking = function (team, booking, game) {
            $http.post($window.sessionStorage.apiUrl + '/api/bookings', {
                gameId: game.id,
                playerId: booking.player.id,
                misconductCode: booking.penaltyCard.name
            })
            .success(function (result) {
                booking.id = result.id;
                booking.penaltyCard = result.penaltyCard;

                team.bookings.push(booking);
                game.bookings.push(booking);
            });

            $scope.newBooking = new defaultBooking();
        }

        $scope.saveResult = function (game) {
            $http.put($window.sessionStorage.apiUrl + '/api/games/' + game.id + '/result', game);
        }

        $scope.addPlayer = function (team) {
            var modalInstance = $modal.open({
                templateUrl: 'addPlayerModal.html',
                controller: addPlayerController,
                size: 'sm',
                resolve: {
                    team: function () {
                        return team;
                    }
                }
            });

            modalInstance.result.then(function (result) {
                team.players.push(result);
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
                var divisionAlias = $routeParams['divisionAlias'];
                if (divisionAlias) {
                    var division = _.findWhere(result, { alias: divisionAlias });
                    if (division) {
                        $scope.selectedDivision = division;
                    }
                }

                $scope.divisions = result;
                $scope.isLoaded = true;
            });

        $http.get($window.sessionStorage.apiUrl + '/api/penaltycards')
            .success(function (result) {
                $scope.penaltyCards = result;
            });

        function defaultGame() {
            return {
                date: null,
                homeTeam: {},
                awayTeam: {},
                location: {}
            };
        }

        function defaultGoal() {
            return {
                player: {},
                count: 0
            };
        }

        function defaultBooking() {
            return {
                player: {},
                penaltyCard: {}
            };
        }
    }

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
