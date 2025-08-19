$(document).ready(function () {
    $('#ProvinciaAdmin').change(function () {
        var provinciaId = $(this).val();

        $('#CantonAdmin').empty().append('<option value="">Seleccione Cantón</option>');
        $('#DistritoAdmin').empty().append('<option value="">Seleccione Distrito</option>');

        if (provinciaId) {
            $.getJSON('/MiPerfil/ObtenerCantones', { provinciaId: provinciaId }, function (cantones) {
                $.each(cantones, function (index, canton) {
                    $('#CantonAdmin').append('<option value="' + canton.ID_CANTON + '">' + canton.NOMBRE + '</option>');
                });
            });
        }
    });

    $('#CantonAdmin').change(function () {
        var cantonId = $(this).val();

        $('#DistritoAdmin').empty().append('<option value="">Seleccione Distrito</option>');

        if (cantonId) {
            $.getJSON('/MiPerfil/ObtenerDistritos', { cantonId: cantonId }, function (distritos) {
                $.each(distritos, function (index, distrito) {
                    $('#DistritoAdmin').append('<option value="' + distrito.ID_DISTRITO + '">' + distrito.NOMBRE + '</option>');
                });
            });
        }
    });
});