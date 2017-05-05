(function() {
    $('[data-toggle="tooltip"]').tooltip();

    document.body.addEventListener('click', function (ev) {
        var link = ev.target;
        
        if (link.dataset.toggle == null && isValidHashLink(link.hash)) {
            document.querySelector(link.hash).scrollIntoView({
                behavior: 'smooth'
            });
        }
    });

    var slideout = new Slideout({
        panel: document.getElementById('main'),
        menu: document.getElementById('main-nav'),
        padding: 256,
        tolerance: 70,
        side: 'right',
        touch: false
    });

    document.getElementById('main-nav-toggle').addEventListener('click', function () {
        slideout.toggle();
    });

    function isValidHashLink(hash) {
        if (hash == null || !hash.startsWith('#') || hash === '' || hash === '#') {
            return false;
        }
        
        return document.querySelector(hash) != null;
    }
})();