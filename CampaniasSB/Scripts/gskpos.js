$(document).ready(function () {
     $("#CiudadId").change(function () {
        $("#MunicipioDelegacionId").empty();
        $("#MunicipioDelegacionId").append('<option value="0">[Seleccionar un Municipio o Delegación...]</option>');
        $.ajax({
            type: 'POST',
            url: UrlM,
            dataType: 'json',
            data: { ciudadId: $("#CiudadId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#MunicipioDelegacionId").append('<option value="'
                        + data.MunicipioDelegacionId + '">'
                        + data.Nombre + '</option>');
                });
            },
            error: function (ex) {
                alert('Falló al obtener las Delegaciones.' + ex);
            }
        });
        return false;
    })
});


$(document).ready(function () {
    $("#RegionId").change(function () {
        $("#CiudadId").empty();
        $("#CiudadId").append('<option value="0">[Seleccionar...]</option>');
        $.ajax({
            type: 'POST',
            url: UrlR,
            dataType: 'json',
            data: { regionId: $("#RegionId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#CiudadId").append('<option value="'
                        + data.CiudadId + '">'
                        + data.Nombre + '</option>');
                });
            },
            error: function (ex) {
                alert('Falló al obtener las Ciudades.' + ex);
            }
        });
        return false;
    })
});



$(document).ready(function () {
    $("#MunicipioDelegacionId").change(function () {
        $("#ColoniaId").empty();
        $("#ColoniaId").append('<option value="0">[Seleccionar una Colonia...]</option>');
        $.ajax({
            type: 'POST',
            url: UrlC,
            dataType: 'json',
            data: { delegacionId: $("#MunicipioDelegacionId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#ColoniaId").append('<option value="'
                        + data.ColoniaId + '">'
                        + data.Nombre + '</option>');
                });
            },
            error: function (ex) {
                alert('Falló al obtener las colonias.' + ex);
            }
        });
        return false;
    })
});
