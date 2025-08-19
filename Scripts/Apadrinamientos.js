
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

    $('#modalApadrinamientoLabel').text('EDITAR APADRINAMIENTO');
    $('#btnSubmit').text('Actualizar');

    $('#formApadrinamiento').attr('action', '/Apadrinamiento/EditarApadrinamiento');
}

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

function mostrarModalApadrinamiento() {
    const monto = document.getElementById('montoMensual').value;
    const metodo = document.getElementById('metodo').value;

    if (!monto || parseFloat(monto) < 50) {
        Swal.fire({
            icon: 'warning',
            title: 'Monto inválido',
            text: 'Por favor ingrese un monto mínimo de $50',
            confirmButtonColor: '#3085d6'
        });
        return;
    }

    if (!metodo) {
        Swal.fire({
            icon: 'warning',
            title: 'Método de pago requerido',
            text: 'Por favor seleccione un método de pago',
            confirmButtonColor: '#3085d6'
        });
        return;
    }

    switch (metodo) {
        case 'tarjeta':
            $('#modalTarjeta').modal('show');
            break;
        case 'sinpe':
            $('#modalSinpe').modal('show');
            break;
        case 'paypal':
            $('#modalPaypal').modal('show');
            break;
    }
}

function enviarFormularioApadrinamiento(tipoMetodo) {
    const monto = document.getElementById('montoMensual').value;
    const metodo = document.getElementById('metodo').value;
    let referencia = '';

    if (tipoMetodo === 1) { // Tarjeta
        const numeroTarjeta = document.getElementById('numeroTarjeta').value;
        const titular = document.getElementById('titularTarjeta').value;
        const codigo = document.getElementById('codigoSeguridad').value;

        if (!numeroTarjeta || !titular || !codigo) {
            Swal.fire({
                icon: 'warning',
                title: 'Campos incompletos',
                text: 'Por favor complete todos los campos de la tarjeta',
                confirmButtonColor: '#3085d6'
            });
            return;
        }
        referencia = numeroTarjeta.substring(numeroTarjeta.length - 4);
    }
    else if (tipoMetodo === 2) { // Sinpe
        referencia = document.getElementById('referenciaApadrinamiento').value;
        if (!referencia || referencia.length < 6) {
            Swal.fire({
                icon: 'warning',
                title: 'Referencia inválida',
                text: 'Por favor ingrese un número de referencia válido (mínimo 6 dígitos)',
                confirmButtonColor: '#3085d6'
            });
            return;
        }
    }
    else if (tipoMetodo === 3) { // PayPal
        const usuario = document.getElementById('usuarioPaypal').value;
        const contrasena = document.getElementById('contrasenaPaypal').value;

        if (!usuario || !contrasena) {
            Swal.fire({
                icon: 'warning',
                title: 'Campos incompletos',
                text: 'Por favor complete todos los campos de PayPal',
                confirmButtonColor: '#3085d6'
            });
            return;
        }
        referencia = usuario;
    }

    document.getElementById('inputMetodoPago').value = metodo;
    document.getElementById('inputMontoMensual').value = monto;
    document.getElementById('inputReferencia').value = referencia || "N/A";

    $('.modal').modal('hide');

    Swal.fire({
        title: 'Procesando apadrinamiento...',
        html: 'Por favor espere mientras procesamos su solicitud <br><div class="spinner-border text-primary mt-3" role="status"><span class="visually-hidden">Cargando...</span></div>',
        allowOutsideClick: false,
        showConfirmButton: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    setTimeout(() => {
        document.getElementById("formApadrinarAnimal").submit();
    }, 1000);
}

$('.modal').on('hidden.bs.modal', function () {
    $(this).find('input').val('');
});

$(document).ready(function () {
    initializeDataTable();
    setupModalEvents();

    // SweetAlerts de TempData
    if (typeof Swal !== 'undefined') {
        if ($('#SwalSuccess').length) {
            Swal.fire({
                icon: 'success',
                title: '¡Éxito!',
                text: $('#SwalSuccess').val(),
                confirmButtonColor: '#3085d6'
            });
        }
        if ($('#SwalError').length) {
            Swal.fire({
                icon: 'error',
                title: '¡Error!',
                text: $('#SwalError').val(),
                confirmButtonColor: '#d33'
            });
        }
    }
});

document.addEventListener("DOMContentLoaded", function () {
    setupStateChangeConfirmation();
});
