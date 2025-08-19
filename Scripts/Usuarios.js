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

    $('#TablaUsuarios').DataTable({
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