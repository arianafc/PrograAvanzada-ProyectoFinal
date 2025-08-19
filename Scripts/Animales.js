$(function () {

    if (swalError) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: swalError,
            confirmButtonColor: ' #062D3E;'
        });
    }

    if (swalSuccess) {
        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: swalSuccess,
            confirmButtonColor: ' #062D3E'
        });
    }
});

// Función para inicializar DataTable
function initializeDataTable() {
    $('#TablaAnimales').DataTable({
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

$(document).on('click', '.AgregarBtn', function () {
    abrirModalAgregar();
});

// Función para abrir modal en modo AGREGAR
function abrirModalAgregar() {
    $('#FechaIngreso').hide();
    $('#modalAnimal input[type="number"]').val('');
    $('#modalAnimal input[type="date"]').val('');
    $('#modalAnimal input[type="text"]').val('');
    $('#modalAnimal input[type="file"]').val('');
    $('#modalAnimal input[type="hidden"]').val('0');
    $('#modalAnimal textarea').val('');

    $('#modalAnimal select').each(function () {
        $(this).prop('selectedIndex', 0);
    });

    var today = new Date().toISOString().split('T')[0];
    $('input[name="NuevoAnimal.FechaIngreso"]').val(today);
    

    $('#modalAnimalLabel').text('REGISTRAR ANIMAL');
    $('#btnSubmit').text('Guardar');

    $('#formAnimal').attr('action', '/Animal/GestionAnimales');

    
}

// Función para abrir modal en modo EDITAR
function abrirModalEditar(data) {

    $('#FechaIngreso').show();
    $('input[name="NuevoAnimal.IdAnimal"]').val(data.id);
    $('input[name="NuevoAnimal.Nombre"]').val(data.nombre);
    $('input[name="NuevoAnimal.FechaNacimiento"]').val(data.fechanacimiento);
    $('input[name="NuevoAnimal.FechaIngreso"]').val(data.fechaingreso);
    $('textarea[name="NuevoAnimal.Historia"]').val(data.historia);
    $('textarea[name="NuevoAnimal.Necesidad"]').val(data.necesidad);
    $('select[name="NuevoAnimal.IdRaza"]').val(data.idraza);
    $('select[name="NuevoAnimal.IdSalud"]').val(data.idsalud);

    if (data.imagen && data.imagen !== '') {
        $('.current-image-info').remove();
        $('input[name="ImagenAnimal"]').after(
            '<div class="current-image-info text-muted mt-1">Imagen actual: ' +
            data.imagen.split('/').pop() + '</div>'
        );
    }
    $('#modalAnimalLabel').text('EDITAR ANIMAL');
    $('#btnSubmit').text('Actualizar');
    $('#formAnimal').attr('action', '/Animal/EditarAnimal');

    
}

$(document).on('click', '.btn-editar-Animal', function () {
    var button = $(this);
    var data = {
        id: button.data('id'),
        nombre: button.data('nombre'),
        idraza: button.data('idraza'),
        idsalud: button.data('idsalud'), 
        fechanacimiento: button.data('fechanacimiento'),
        fechaingreso: button.data('fechaingreso'),
        historia: button.data('historia'),
        necesidad: button.data('necesidad'),
        imagen: button.data('imagen')
    };

    abrirModalEditar(data);
});

function setupStateChangeConfirmation() {
    document.querySelectorAll(".FormCambiarEstado").forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            var row = form.closest('tr');
            var estadoAnimal = row.cells[8].textContent.trim();

            if (estadoAnimal === 'Apadrinado' || estadoAnimal === 'APADRINADO') {
                Swal.fire({
                    title: 'No se puede cambiar el estado',
                    html: 'Este animal tiene un <strong>apadrinamiento activo</strong>.<br><br>' +
                        'Para cambiar su estado, primero debe:<br>' +
                        '• Finalizar el apadrinamiento activo<br>' +
                        '• O cambiar el estado del apadrinamiento a inactivo',
                    icon: 'warning',
                    confirmButtonText: 'Entendido',
                    confirmButtonColor: '#3085d6'
                });
                return;
            }

            Swal.fire({
                title: '¿Estás seguro?',
                text: "Esta acción cambiará el estado del Animal.",
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
});

// Inicialización cuando el DOM esté completamente cargado
document.addEventListener("DOMContentLoaded", function () {
    setupStateChangeConfirmation();
});