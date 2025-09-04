function confirmarRechazar(id) {
    Swal.fire({
        title: '¿ Desea rechazar el expendiente ?',
        icon: 'warning',
        input: 'textarea',
        inputPlaceholder: 'Ingrese el motivo del rechazo...',
        showCancelButton: true,
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar',
        preConfirm: (motivo) => {
            if (!motivo) {
                Swal.showValidationMessage('Debe ingresar un motivo para el rechazo');
            }
            return motivo;
        }
    }).then((result) => {
        if (result.isConfirmed) {
            Modificar(id);
        }
    });
}
/*
function confirmarRechazar() {    
    $('#RechazarModal').modal('show'); // Abre el modal de confirmación
}

$('#aceptarButton').click(function () {
    $('#RechazarModal').modal('hide'); // Cierra el modal
    Modificar(8); // Llama a la función Modificar
});


function confirmarModificar() {    
    $('#ModificarModal').modal('show'); // Abre el modal de confirmación
}

$('#modificarButton').click(function () {
    $('#ModificarModal').modal('hide'); // Cierra el modal
    Modificar(2); // Llama a la función Modificar
});

*/


function confirmarModificar(id) {
    Swal.fire({
        title: '¿Desea admitir el expediente ? , se le asignará a usted como responsable de la atención.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar',
        customClass: {
            popup: 'sweetalert2-custom'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            Modificar(id);
        }
    });
}


function Modificar(modo) {

    var request = {
        NumeroDoc: numeroDoc,
        parametro: modo // Incluir la variable modo en el objeto request
    };
    console.log(request);

    jQuery.ajax({
        url: modificarSolicitudesUrl, // Asegúrate de que esta URL sea correcta
        type: "POST",
        data: JSON.stringify(request),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.OperacionExitosa) {
                //tabladata.ajax.reload
                // Redirigir después de guardar
                window.location.href = '/Solicitudes/Inicio';
            } else {
                swal("Mensaje", "No se pudo guardar los cambios", "warning");
            }
        },
        error: function (error) {
            console.log(error);
        },
        beforeSend: function () {
            // agregar lógica aquí si necesitas hacer algo antes de enviar la solicitud
        }
        
    });
}