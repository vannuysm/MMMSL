(function ($) {
    if (window.location.hash) {
        var link = $(`a.nav-link[href="${window.location.hash}"]`);

        if (link.length > 0) {
            link.tab('show');
        }
    }

    $('a.nav-link').on('shown.bs.tab', function (e) {
        window.location.hash = e.target.hash;
    })
})(jQuery);