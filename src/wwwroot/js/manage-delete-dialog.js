(function ($) {
    var confirmationTarget;

    $('#delete-confirmation').on('shown.bs.modal', function (ev) {
        confirmationTarget = ev.relatedTarget;
    });

    document.getElementById('delete-confirmation').addEventListener('keypress', function (ev) {
        if (ev.which === 13) {
            document.getElementById('delete-confirmed-button').click();
        }
    });

    document.getElementById('delete-confirmed-button').addEventListener('click', function (ev) {
        var form = document.querySelector(confirmationTarget.dataset.targetForm);

        if (form != null) {
            form.submit();
        }
    });
})(jQuery);