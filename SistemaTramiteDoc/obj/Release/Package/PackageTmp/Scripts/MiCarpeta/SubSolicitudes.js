var tabladata;
$(document).ready(function () {

    ////validamos el formulario
    $("#form").validate({
        rules: {
            Comentario: "required",
        },
        messages: {
            Comentario: "(*)",
        },
        errorElement: 'span'
    });
})


function AgregarSubSolicitudes() {

    if ($("#form").valid()) {
        console.log("Validando...");

        // Arreglo para almacenar los objetos SubSolicitud
        var subSolicitudes = [];

        // Selecciona todos los checkboxes que están seleccionados
        $("input[type=checkbox]:checked").each(function () {
            var checkboxId = $(this).attr('id');
            var label = $('label[for="' + checkboxId + '"]').text();
            var selectId = checkboxId + 'Persona';
            var selectedPerson = $('#' + selectId + ' option:selected').text();

            // Crea un objeto SubSolicitud y agrégalo al arreglo
            var subSolicitud = {
                NombreAreaExterna: label,
                NombreResponsable: selectedPerson,
                IdSolicitud: idSolicitud,
                Comentario: $("#comentario").val(),
                TituloSolicitud: titulo,
                Titulo: '',
                IdCreadoPor: null
            };
            subSolicitudes.push(subSolicitud);
        });

        var request = {
            objetos: subSolicitudes
        };

        console.log(request);

        jQuery.ajax({
            url: $.MisUrls.url._ObtenerSubSolicitudes,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.OperacionExitosa) {
                    tabladata.ajax.reload();
                    $('#agregarSolicitudModal').modal('hide');
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
}

