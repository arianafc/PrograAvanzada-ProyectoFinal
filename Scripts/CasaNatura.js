$(document).ready(function () {

    $(document).on('click', '.VerFacturaBtn', function () {
        var NumeroFactura = $(this).data("factura");
        window.location.href = '/Actividades/GenerarFactura?NumeroFactura=' + NumeroFactura;
    });
   
    const subtotalSpan = document.getElementById("subtotal");

    $('#cantidadBoletos').on('input', function () {
        let cantidad = parseInt(this.value);
        if (cantidad > maxDisponibles) {
            Swal.fire({
                icon: 'error',
                title: 'Cantidad no válida',
                text: 'La cantidad no puede ser mayor a los boletos disponibles.',
                confirmButtonColor: '#dc3545'
            });
            this.value = maxDisponibles;
            cantidad = maxDisponibles;
        }
        const subtotal = cantidad * precioUnitario;
        subtotalSpan.textContent = subtotal.toLocaleString('es-CR');
    });




    $('#TablaVentas').DataTable({
        responsive: true,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        order: [[0, "desc"]],
        columnDefs: [
            { orderable: false, targets: [8] }
        ]
    });

});

//Funciones para metodos de pago
function enviarFormularioCompra(metodoPagoId) {
    const cantidad = parseInt(document.getElementById("cantidadBoletos").value);

    if (!cantidad || cantidad <= 0) {
        Swal.fire({
            icon: 'error',
            title: 'Cantidad inválida',
            text: 'La cantidad debe ser mayor a cero.'
        });
        return;
    }


    if (metodoPagoId === 1) {
        const numero = document.getElementById("numeroTarjeta").value;
        const titular = document.getElementById("titularTarjeta").value;
        const codigo = document.getElementById("codigoSeguridad").value;
        if (!numero || !titular || !codigo) {
            alert("Completa todos los campos de tarjeta.");
            return;
        }
        bootstrap.Modal.getInstance(document.getElementById("modalTarjeta")).hide();
    }

    if (metodoPagoId === 2) {
        const ref = document.getElementById("referenciaActividad").value;

        if (!/^\d{6,30}$/.test(ref)) {
            Swal.fire({
                icon: 'error',
                title: 'Referencia SINPE inválida',
                text: 'Debe ser numérica y tener al menos 6 dígitos.',
            });
            return;
        }

        document.getElementById("inputReferencia").value = ref;
        bootstrap.Modal.getInstance(document.getElementById("modalSinpe")).hide();
    }

    if (metodoPagoId === 3) {
        const u = document.getElementById("usuarioPaypal").value;
        const p = document.getElementById("contrasenaPaypal").value;
        if (!u || !p) {
            alert("Completa los datos de PayPal.");
            return;
        }
        bootstrap.Modal.getInstance(document.getElementById("modalPaypal")).hide();
    }

    document.getElementById("inputMetodoPago").value = metodoPagoId;
    document.getElementById("inputNumBoletos").value = cantidad;

    document.forms[0].submit();
}

function mostrarModal() {
    const metodo = document.getElementById('metodo').value;
    const campoReferencia = document.getElementById('campoReferencia');

    
    if (campoReferencia) {
        campoReferencia.classList.add('d-none');
    }

    switch (metodo) {
        case 'tarjeta':
            const modalTarjeta = new bootstrap.Modal(document.getElementById('modalTarjeta'));
            modalTarjeta.show();
            break;
        case 'sinpe':
            const modalSinpe = new bootstrap.Modal(document.getElementById('modalSinpe'));
            modalSinpe.show();
            if (campoReferencia) {
                campoReferencia.classList.remove('d-none');
            }
            break;
        case 'paypal':
            const modalPaypal = new bootstrap.Modal(document.getElementById('modalPaypal'));
            modalPaypal.show();
            break;
    }
}

function validarTarjeta() {
    const numeroTarjeta = document.getElementById('numeroTarjeta').value;
    const titularTarjeta = document.getElementById('titularTarjeta').value;
    const codigoSeguridad = document.getElementById('codigoSeguridad').value;

    if (numeroTarjeta && titularTarjeta && codigoSeguridad) {
        const modalTarjeta = bootstrap.Modal.getInstance(document.getElementById('modalTarjeta'));
        if (modalTarjeta) {
            modalTarjeta.hide();
        }

        const submitButton = document.getElementById('submit-button');
        if (submitButton) {
            submitButton.disabled = false;
        }

        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: 'Datos de tarjeta validados correctamente'
        });
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Campos incompletos',
            text: 'Por favor complete todos los campos'
        });
    }
}

function validarSinpe() {
    const referencia = document.getElementById('referencia').value;

    if (referencia && referencia.length >= 6) {
        const modalSinpe = bootstrap.Modal.getInstance(document.getElementById('modalSinpe'));
        if (modalSinpe) {
            modalSinpe.hide();
        }

        const submitButton = document.getElementById('submit-button');
        if (submitButton) {
            submitButton.disabled = false;
        }

        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: 'Transferencia Sinpe confirmada'
        });
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Referencia inválida',
            text: 'Por favor ingrese un número de referencia válido (mínimo 6 dígitos)'
        });
    }
}

function validarPaypal() {
    const usuarioPaypal = document.getElementById('usuarioPaypal').value;
    const contrasenaPaypal = document.getElementById('contrasenaPaypal').value;

    if (usuarioPaypal && contrasenaPaypal) {
        const modalPaypal = bootstrap.Modal.getInstance(document.getElementById('modalPaypal'));
        if (modalPaypal) {
            modalPaypal.hide();
        }

        const submitButton = document.getElementById('submit-button');
        if (submitButton) {
            submitButton.disabled = false;
        }

        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: 'Pago con PayPal procesado correctamente'
        });
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Campos incompletos',
            text: 'Por favor complete todos los campos de PayPal'
        });
    }
}

