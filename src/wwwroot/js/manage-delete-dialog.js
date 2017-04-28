(function ($) {
    var confirmationTarget;

    $('#delete-confirmation').on('shown.bs.modal', function (ev) {
        confirmationTarget = ev.relatedTarget;
    });

    document.getElementById('delete-confirmed-button').addEventListener('click', function (ev) {
        var form = document.querySelector(confirmationTarget.dataset.targetForm);

        if (form != null) {
            form.submit();
        }
    });
})(jQuery);