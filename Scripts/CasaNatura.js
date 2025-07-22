document.addEventListener("DOMContentLoaded", function () {

    const swalSuccess = document.getElementById("swal-success");
    const swalError = document.getElementById("swal-error");

    if (swalSuccess) {
        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: swalSuccess.value
        });
    }

    if (swalError) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: swalError.value
        });
    }


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

    document.getElementById("formRegistro").addEventListener("submit", function (e) {
        let contrasena = document.getElementsByClassName("contrasenna").value;
        let confirmar = document.getElementsByClassName("confirmarContrasenna").value;

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