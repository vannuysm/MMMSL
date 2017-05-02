(function ($) {
    if (window.location.hash) {
        var link = $('a.nav-link[href="' + window.location.hash + '"]');

        if (link.length > 0) {
            link.tab('show');
        }
    }

    $('.date').datetimepicker({
        stepping: 15,
        sideBySide: true,
        ignoreReadonly: true,
        allowInputToggle: true,
        icons: {
            time: 'icon icon-clock',
            date: 'icon icon-calendar',
            up: 'icon icon-chevron-up',
            down: 'icon icon-chevron-down',
            previous: 'icon icon-chevron-left',
            next: 'icon icon-chevron-right',
            today: 'icon icon-hair-cross',
            clear: 'icon icon-erase',
            close: 'icon icon-cross'
        }
    });

    $('a.nav-link').on('shown.bs.tab', function (e) {
        window.location.hash = e.target.hash;
    });

    var gameStatusElement = document.getElementById('Game_Status');
    var gameResultForm = $('#game-result-form');

    if (gameStatusElement != null && gameResultForm != null) {
        gameStatusElement.addEventListener('input', function (ev) {
            if (ev.target.value == 3) {
                gameResultForm.collapse('show');
            }
            else {
                gameResultForm.collapse('hide');
            }
        });
    }

    window.InitiateDivisionalTeamSelect = function () {
        document.querySelector('.select-division-source').addEventListener('input', function (ev) {
            var divisionId = ev.target.value;
            var url = '/manage/divisions/' + divisionId + '/teams/json';

            fetch(url, { credentials: 'same-origin' })
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
})(jQuery);