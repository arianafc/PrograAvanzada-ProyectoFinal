// Función para inicializar DataTable
function initializeDataTable() {
    $('#TablaApadrinamientos').DataTable({
        responsive: true,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        columnDefs: [
            { orderable: false, targets: [9] }
        ]
    });
}

// Función para abrir modal en modo AGREGAR
function abrirModalAgregar() {
    $('#modalApadrinamiento input[type="number"]').val('');
    $('#modalApadrinamiento input[type="date"]').val('');
    $('#modalApadrinamiento input[type="text"]').val('');
    $('#modalApadrinamiento input[type="hidden"]').val('0');

    $('#modalApadrinamiento select').each(function () {
        $(this).prop('selectedIndex', 0);
    });

    $('#fechaBajaContainer').hide();

    $('#modalApadrinamientoLabel').text('REGISTRAR APADRINAMIENTO');
    $('#btnSubmit').text('Guardar');

    $('#formApadrinamiento').attr('action', '/Apadrinamiento/GestionApadrinamientos');
}

// Función para abrir modal en modo EDITAR
function abrirModalEditar(data) {
    $('input[name="NuevoApadrinamiento.IdApadrinamiento"]').val(data.id);

    var montoString = data.monto.toString().replace(',', '.');
    var montoNumerico = parseFloat(montoString);
    if (!isNaN(montoNumerico)) {
        var $montoField = $('input[name="NuevoApadrinamiento.MontoMensual"]');

        $montoField.val('');
        $montoField.val(montoNumerico);
        $montoField.focus().blur();
    } else {
        $('input[name="NuevoApadrinamiento.MontoMensual"]').val('');
    }

    $('input[name="NuevoApadrinamiento.Fecha"]').val(data.fechainicio);
    $('input[name="NuevoApadrinamiento.Referencia"]').val(data.referencia);

    if (data.fechabaja && data.fechabaja !== '') {
        $('input[name="NuevoApadrinamiento.FechaBaja"]').val(data.fechabaja);
    } else {
        $('input[name="NuevoApadrinamiento.FechaBaja"]').val('');
    }

    $('select[name="NuevoApadrinamiento.IdUsuario"]').val(data.usuarioid);
    $('select[name="NuevoApadrinamiento.IdAnimal"]').val(data.animalid);
    $('select[name="NuevoApadrinamiento.IdMetodo"]').val(data.metodoid);

    $('input[name="NuevoApadrinamiento.Fecha"]').prop('readonly', true);
    $('select[name="NuevoApadrinamiento.IdUsuario"]').prop('disabled', true);
    $('select[name="NuevoApadrinamiento.IdMetodo"]').prop('disabled', true);

    $('#modalApadrinamientoLabel').text('EDITAR APADRINAMIENTO');
    $('#btnSubmit').text('Actualizar');

    $('#formApadrinamiento').attr('action', '/Apadrinamiento/EditarApadrinamiento');
}

// Función para configurar eventos de los botones
function setupModalEvents() {
    // Evento para botón "Nuevo Apadrinamiento"
    $(document).on('click', 'button[data-bs-target="#modalApadrinamiento"]:not(.btn-editar-apadrinamiento)', function () {
        abrirModalAgregar();
    });

    // Evento para botones de editar
    $(document).on('click', '.btn-editar-apadrinamiento', function () {
        var button = $(this);
        var data = {
            id: button.data('id'),
            usuarioid: button.data('usuarioid'),
            animalid: button.data('animalid'),
            metodoid: button.data('metodoid'),
            monto: button.data('monto'),
            fechainicio: button.data('fechainicio'),
            fechabaja: button.data('fechabaja'),
            referencia: button.data('referencia'),
            estado: button.data('estado')
        };

        abrirModalEditar(data);
    });
}

// Función para configurar confirmaciones de cambio de estado
function setupStateChangeConfirmation() {
    document.querySelectorAll(".FormCambiarEstado").forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            Swal.fire({
                title: '¿Estás seguro?',
                text: "Esta acción cambiará el estado del apadrinamiento.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, confirmar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
        });
    });
}

// Función para mostrar notificaciones SweetAlert
function showSwalNotifications(successMessage, errorMessage) {
    if (successMessage) {
        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: successMessage,
            timer: 2000,
            showConfirmButton: false
        });
    }

    if (errorMessage) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: errorMessage
        });
    }
}

// Inicialización cuando el documento esté listo
$(document).ready(function () {
    initializeDataTable();
    setupModalEvents();
});

// Inicialización cuando el DOM esté completamente cargado
document.addEventListener("DOMContentLoaded", function () {
    setupStateChangeConfirmation();
});