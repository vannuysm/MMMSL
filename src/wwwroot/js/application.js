(function() {
    $('[data-toggle="tooltip"]').tooltip();

    document.body.addEventListener('click', function (ev) {
        var link = ev.target;
        
        if (isValidHashLink(link.hash)) {
            document.querySelector(link.hash).scrollIntoView({
                behavior: 'smooth'
            });
        }
    });

    function isValidHashLink(hash) {
        if (hash == null || !hash.startsWith('#') || hash === '' || hash === '#') {
            return false;
        }

        return document.querySelector(hash) != null;
    }
})();