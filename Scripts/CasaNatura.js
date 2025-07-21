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