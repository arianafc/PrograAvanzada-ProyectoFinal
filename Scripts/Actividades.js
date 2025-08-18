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

    const today = new Date().toISOString().split('T')[0];


    $('#FechaCrear').attr('min', today);


    $('#formCrearActividad').on('submit', function (e) {
        const formId = $(this).attr('id');
        const fechaInput = formId === 'formCrearActividad' ? '#FechaCrear' : '#Fecha';
        const fechaSeleccionada = $(fechaInput).val();

        if (fechaSeleccionada < today) {
            e.preventDefault();
            Swal.fire({
                icon: 'error',
                title: 'Fecha no válida',
                text: 'La fecha seleccionada no puede ser anterior a hoy.',
                confirmButtonColor: '#dc3545'
            });
        }
    });

    $('#TablaActividades').DataTable({
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


    $('#modalEditarActividad').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var modal = $(this);

        modal.find('input[name="NuevaActividad.IdActividad"]').val(button.data('id'));
        modal.find('input[name="NuevaActividad.Imagen"]').val(button.data('imagen'));
        modal.find('input[name="NuevaActividad.IdEstado"]').val(button.data('estado'));
        modal.find('input[name="NuevaActividad.Nombre"]').val(button.data('nombre'));
        modal.find('input[name="NuevaActividad.Fecha"]').val(button.data('fecha'));
        modal.find('input[name="Hora"]').val(button.data('hora'));
        modal.find('textarea[name="NuevaActividad.Descripcion"]').val(button.data('descripcion'));
        modal.find('input[name="NuevaActividad.PrecioBoleto"]').val(button.data('precio'));
        modal.find('input[name="NuevaActividad.TicketsDisponibles"]').val(button.data('tickets'));
        modal.find('select[name="NuevaActividad.Tipo"]').val(button.data('tipo'));
        modal.find('#ImagenExistente').attr('src', button.data('imagen')).show();

    });

    $(".FormCambiarEstado").submit(function (e) {
        e.preventDefault();

        const form = this;

        Swal.fire({
            title: '¿Está seguro?',
            text: "¿Desea cambiar el estado de esta actividad?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, cambiar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit(); 
            }
        });
    });

});