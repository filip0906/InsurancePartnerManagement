// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#partnerModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var partnerId = button.data('id');

        $.get('/Partner/Details/' + partnerId, function (data) {
            $('#partnerModal .modal-body').html(data);
        });
    });
});
