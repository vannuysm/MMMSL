(function() {
    $('[data-toggle="tooltip"]').tooltip();

    window.InitializeLock = function(clientId, domain, callbackUrl, state, nonce) {
        var lock = new Auth0Lock(clientId, domain, {
            theme: {
                logo: `${window.location.origin}/images/logo.svg`
            },
            auth: {
                redirectUrl: callbackUrl,
                responseType: 'code',
                params: {
                    scope: 'openid',
                    state: state,
                    nonce: nonce
                }
            }
        });

        var login = document.getElementById('login');
        if (login != null) {
            login.addEventListener('click', (e) => {
                lock.show();
                e.preventDefault();
            });
        }
    }
})();