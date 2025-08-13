$(document).ready(function () {


    $('#TablaVentas').DataTable({
        responsive: true,
        autoWidth: false,
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json",

            "search": "Buscar:",
            "lengthMenu": "Número de Registros _MENU_",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ ventas",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "zeroRecords": "No se encontraron resultados",
            "infoEmpty": "No hay actividades para mostrar",
            "infoFiltered": "(filtrado de _MAX_ actividades totales)"
        }
    });
   
    function MostrarAlertaLogin() {
        Swal.fire({
            title: 'Debes iniciar sesión',
            text: 'Para acceder a esta opción, primero inicia sesión en tu cuenta.',
            icon: 'warning',
            confirmButtonText: 'Iniciar sesión',
            confirmButtonColor: ' #0a3c52'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = LoginUrl;
            }
        });
    }

    // Función para capitalizar cada palabra
    function capitalizeWords(str) {
        return str
            ? str.toLowerCase().split(' ').map(word =>
                word.charAt(0).toUpperCase() + word.slice(1)
            ).join(' ')
            : "";
    }

    // Función para consultar la API
    function ConsultarPersonaApi() {
        $("#nombre").val("");
        $("#apellido1").val("");
        $("#apellido2").val("");

        let identificacion = $("#cedula").val();

        if (identificacion.length >= 9) {
            $.ajax({
                type: 'GET',
                url: 'https://apis.gometa.org/cedulas/' + identificacion,
                dataType: 'json',
                success: function (data) {
                    if (data.resultcount > 0) {
                        let persona = data.results[0];
                        $("#nombre").val(capitalizeWords(persona.firstname1));
                        $("#apellido1").val(capitalizeWords(persona.lastname1));
                        $("#apellido2").val(capitalizeWords(persona.lastname2));
                    } else {
                        alert("No se encontró información para esa cédula");
                    }
                },
                error: function () {
                    alert("Error al consultar la API");
                }
            });
        }
    }

    // Asociar evento keyup al input
    $("#cedula").on("keyup", ConsultarPersonaApi);
    $(".ValidacionLoginBtn").on("click", MostrarAlertaLogin)

    $(document).ready(function () {
        $("#formRegistro").on("submit", function (e) {
            let contrasena = $(".contrasenna").val();
            let confirmar = $(".confirmarContrasenna").val();

            if (contrasena.length < 8) {
                e.preventDefault();
                Swal.fire({
                    icon: 'error',
                    title: 'Contraseña inválida',
                    text: 'La contraseña debe tener al menos 8 caracteres.'
                });
                return;
            }

            if (contrasena !== confirmar) {
                e.preventDefault();
                Swal.fire({
                    icon: 'error',
                    title: 'Contraseñas no coinciden',
                    text: 'Por favor, asegúrate de que ambas contraseñas coincidan.'
                });
                return;
            }
        });
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

});


function mostrarModal(idActividad) {
    const metodo = document.getElementById("metodo").value;
    if (!metodo) {
        Swal.fire({
            icon: 'warning',
            title: 'Selecciona un método de pago',
            confirmButtonColor: '#3085d6'
        });
        return;
    }

    switch (metodo) {
        case "tarjeta":
            new bootstrap.Modal(document.getElementById("modalTarjeta")).show();
            break;
        case "sinpe":
            new bootstrap.Modal(document.getElementById("modalSinpe")).show();
            break;
        case "paypal":
            new bootstrap.Modal(document.getElementById("modalPaypal")).show();
            break;
    }
}

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

    // Ocultar campo de referencia por defecto
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