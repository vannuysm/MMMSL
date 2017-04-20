(function() {
    window.InitiateDivisionalTeamSelect = function() {
        document.querySelector('.select-division').addEventListener('change', function(ev) {
            var divisionId = ev.target.value;
            fetch(`/manage/teams/${divisionId}/json`, { credentials: 'same-origin' })
                .then(function(response) { return response.json(); })
                .then(function(teams) {
                    document.querySelectorAll('.select-team').forEach(function(element) {
                        element.disabled = true;
                        element.options.length = 0;

                        teams.forEach(function(team) {
                            element.options.add(new Option(team.text, team.Value));
                        });
                        
                        element.disabled = false;
                    });
                });
        });
    }
})();