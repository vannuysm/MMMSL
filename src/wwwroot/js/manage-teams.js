(function() {
    window.InitiateDivisionalTeamSelect = function () {
        document.querySelector('.select-division-source').addEventListener('change', function (ev) {
            var divisionId = ev.target.value;
            fetch(`/manage/divisions/${divisionId}/teams/json`, { credentials: 'same-origin' })
                .then(function (response) { return response.json(); })
                .then(function (teams) {
                    document.querySelectorAll('.select-team-destination').forEach(function (element) {
                        element.disabled = true;
                        element.options.length = 0;

                        teams.forEach(function (team) {
                            element.options.add(new Option(team.name, team.id));
                        });

                        element.disabled = false;
                    });
                });
        });
    };
})();