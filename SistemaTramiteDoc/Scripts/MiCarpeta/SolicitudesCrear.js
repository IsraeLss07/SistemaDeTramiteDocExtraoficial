var tabladata;
$(document).ready(function () {

    ////validamos el formulario
    $("#form").validate({
        rules: {
            NumeroDoc: "required",
            Asunto: "required",
            Referencia: "required",
            NombreEmpresa: "required",
            TipoDoc: "required",
            NombreFormato: "required",
            NombreInstitucion: "required",
            NombreProyecto: "required",
            PlazoAtc: "required",
            Penalidad: "required",
            FechaEmision: "required",
            FechaRecepcion: "required",
            FechaVencInterno: "required",
            FechaEventoExterno: "required",
            Piso: "required",
            Estante: "required",
            File: "required",
            AnalistaNombre: "required"
        },
        messages: {
            NumeroDoc: "(*)",
            Asunto: "(*)",
            Referencia: "(*)",
            NombreEmpresa: "(*)",
            TipoDoc: "(*)",
            NombreFormato: "(*)",
            NombreInstitucion: "(*)",
            NombreProyecto: "(*)",
            PlazoAtc: "(*)",
            Penalidad: "(*)",
            FechaEmision: "(*)",
            FechaRecepcion: "(*)",
            FechaVencInterno: "(*)",
            FechaEventoExterno: "(*)",
            Piso: "(*)",
            Estante: "(*)",
            File: "(*)",
            AnalistaNombre: "(*)"
        },
        errorElement: 'span'
    });

    /*
    tabladata = $('#tbdata').DataTable({
        "ajax": {
            "url": $.MisUrls.url._ObtenerSolicitudes,
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data" // Asegúrate de que DataTables busca los datos en la propiedad "data"
        },
        "columns": [
            { "data": "Titulo", "title": "Nro. Exp" },
            { "data": "NumeroDoc", "title": "Nro. Documento" },
            { "data": "Asunto", "title": "Asunto" },
            {
                "data": "FechaEventoExterno",
                "title": "F. Vencimiento Externo",
                "render": function (data) {
                    return moment(data).format('DD/MM/YYYY'); // Formato de fecha opcional
                }
            },
            { "data": "AnalistaNombre", "title": "Analista Legal" },
            { "data": "AreaExternaNombre", "title": "Áreas Externas" },
            { "data": "NombreFase", "title": "Fase" },
            { "data": "NombreEstado", "title": "Estado" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return "<i class='fas fa-eye' style='cursor:pointer;' onclick='verDetalles(" + JSON.stringify(row) + ", " + roleId + ")'></i>";
                },
                "orderable": false,
                "searchable": false,
                "width": "50px"
            }

        ],
        "language": {
            "url": $.MisUrls.url.Url_datatable_spanish
        },
        responsive: true
    });

    $(document).ready(function() {
        $("#btnBuscar").click(function() {
            var nroExpediente = $("#nroExpediente").val();
    
            $.ajax({
                url:  $.MisUrls.url._ObtenerSolicitudesPorParametros,
                type: 'GET',
                data: { nroExpediente: nroExpediente },
                success: function(data) {
                    // Limpiar la tabla
                    console.log(data); // Depura la respuesta
                    $("#tablaSolicitudes tbody").empty();
    
                    // Llenar la tabla con los nuevos datos
                    data.forEach(function(solicitud) {
                        var row = "<tr>" +
                            "<td>" + solicitud.Titulo + "</td>" +
                            "<td>" + solicitud.NumeroDoc + "</td>" +
                            "<td>" + solicitud.Asunto + "</td>" +
                            "<td>" + solicitud.FechaEventoExterno + "</td>" +
                            "<td>" + solicitud.AnalistaNombre + "</td>" +
                            "<td>" + solicitud.AreaExternaNombre + "</td>" +
                            "<td>" + solicitud.NombreFase + "</td>" +
                            "<td>" + solicitud.NombreEstado + "</td>" +
                            "</tr>";
                        $("#tablaSolicitudes tbody").append(row);
                    });
                },
                error: function(error) {
                    console.log(error);
                }
            });
        });S
    });

})
*/
/*
function abrirPopUpForm(json) {

    //$("#txtid").val(0);

    if (json != null) {

        $("#nroDocumento").val(json.NumeroDoc);
        $("#asunto").val(json.Asunto);
        $("#referencia").val(json.Referencia);
        $("#empresa").val(json.NombreEmpresa);
        $("#tipoDoc").val(json.TipoDoc);
        $("#formato").val(json.NombreFormato);
        $("#institucion").val(json.NombreInstitucion);
        $("#proyecto").val(json.NombreProyecto);
        $("#plazo").val(json.PlazoAtc);
        $("#penalidad").val(json.Penalidad);
        $("#fechaEmision").val(json.FechaEmision);
        $("#fechaRecepcion").val(json.FechaRecepcion);
        $("#fVenInterno").val(json.FechaVencInterno);
        $("#fVenExterno").val(json.FechaEventoExterno);
        $("#piso").val(json.Piso);
        $("#estante").val(json.Estante);
        $("#file").val(json.File);
        $("#analista").val(json.AnalistaNombre);


    } else {
        // Restablecer valores por defecto
        $("#nroDocumento").val("");
        $("#asunto").val("");
        $("#referencia").val("");
        $("#empresa").val($("#empresa option:first").val());
        $("#tipoDoc").val($("#tipoDoc option:first").val());
        $("#formato").val($("#formato option:first").val());
        $("#institucion").val($("#institucion option:first").val());
        $("#proyecto").val($("#proyecto option:first").val());
        $("#plazo").val("");
        $("#penalidad").val("");
        $("#fechaEmision").val("");
        $("#fechaRecepcion").val("");
        $("#fVenInterno").val("");
        $("#fVenExterno").val("");
        $("#piso").val("");
        $("#estante").val("");
        $("#file").val("");
        $("#analista").val($("#analista option:first").val());
    }

    $('#FormModal').modal('show');

}
*/

function Guardar() {
    if ($("#form").valid()) {
        console.log("Validando...");

        var request = {
            objeto: {
                NumeroDoc: $("#nroDocumento").val(),
                Asunto: $("#asunto").val(),
                Referencia: $("#referencia").val(),
                NombreEmpresa: $("#empresa option:selected").text(), // Obtener el texto seleccionado
                NombreFormato: $("#formato option:selected").text(), // Obtener el texto seleccionado
                NombreProyecto:  $("#proyecto option:selected").text(), // Obtener el texto seleccionado
                Penalidad: $("#penalidad").val(),
                NombreInstitucion: $("#institucion option:selected").text(), // Obtener el texto seleccionado
                TipoDoc: $("#tipoDoc option:selected").text(), // Obtener el texto seleccionado
                PlazoAtc: $("#plazo").val(),
                FechaVencInterno: $("#fVenInterno").val(),
                FechaEventoExterno: $("#fVenExterno").val(),
                FechaEmision: $("#fechaEmision").val(),
                FechaRecepcion: $("#fechaRecepcion").val(),
                Piso: $("#piso").val(),
                Estante: $("#estante").val(),
                File: $("#file").val(),
                AnalistaNombre: $("#analista option:selected").text()
            }
        };
        console.log(request);

        jQuery.ajax({
            url: $.MisUrls.url._GuardarSolicitud,
            type: "POST",
            data: JSON.stringify(request),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.OperacionExitosa) {
                    tabladata.ajax.reload();
                    $('#FormModal').modal('hide');
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


function verDetalles(solicitud,roleId) {
    let url;
    if(roleId==3){
        url = '/VistaColaborador/DetalleRespuesta?numeroDoc=' + encodeURIComponent(solicitud.NumeroDoc); 
    }
    else{
        if (solicitud.NombreFase == 'Registrado') {
            url = '/DetalleSolicitud/VerDetalles?numeroDoc=' + encodeURIComponent(solicitud.NumeroDoc);
        } else if (solicitud.NombreFase == 'Rechazado') {
            url = '/Solicitudes/Inicio';
        } else {
            url = '/DetalleSolicitud/VerDetalles2?numeroDoc=' + encodeURIComponent(solicitud.NumeroDoc);
        }
    }
    window.location.href = url;
}


