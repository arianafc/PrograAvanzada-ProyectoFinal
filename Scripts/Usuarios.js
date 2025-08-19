$(function () {


    $(document).on('click', '.BtnEditarUsuario', function () {
        
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        var apellido1 = $(this).data("apellido1");
        var apellido2 = $(this).data("apellido2");
        var correo = $(this).data("correo");
        var rol = $(this).data("rol");
        var estado = $(this).data("estado");

        $("#EditarIdUsuario").val(id);
        $("#EditarNombre").val(nombre);
        $("#EditarApellido1").val(apellido1);
        $("#EditarApellido2").val(apellido2);
        $("#EditarCorreo").val(correo);
        $("#EditarRol").val(rol);
        $("#EditarEstado").val(estado);
    });


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

    $('#TablaUsuarios').DataTable({
        responsive: true,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
       
        columnDefs: [
            { orderable: false, targets: [5] }
        ]
    });


    $("#formEditarUsuario").submit(function (e) {
        e.preventDefault(); 

        var formData = {
            IdUsuario: $("#EditarIdUsuario").val(),
            Nombre: $("#EditarNombre").val(),
            Apellido1: $("#EditarApellido1").val(),
            Apellido2: $("#EditarApellido2").val(),
            Correo: $("#EditarCorreo").val(),
            IdRol: $("#EditarRol").val(),
            IdEstado: $("#EditarEstado").val()
        };

        $.ajax({
            url: '/Usuarios/EditarUsuario',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: response.message,
                        timer: 2000,
                        confirmButtonColor: ' #062D3E'
                    }).then(() => {
                       
                        window.location.href = '/Usuarios/GestionUsuarios';
                    });

                    $("#modalEditarUsuario").modal('hide');
                    
                  
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message,
                        timer: 2000,
                        confirmButtonColor: ' #062D3E'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error inesperado.',
                    timer: 3000,
                    showConfirmButton: false
                });
            }
        });
    });


});