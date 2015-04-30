function borrarCategoria(categoria) {
    var categorias = $("#divCategorias").html();
    categorias = categorias.replace("&nbsp;&nbsp;<button id=\"" + categoria + "\" class=\"btn btn-primary\" onclick=\"borrarCategoria('" + categoria + "')\">" + categoria + "</button>", " ");
    $("#divCategorias").html(categorias);
}

function agregarCategoria() {
    var nombre = $("#nombreCategoria").val();

    var categorias = $("#divCategorias").html();

    $("#divCategorias").html(categorias + "&nbsp;&nbsp;<button id=\"" + nombre + "\" class=\"btn btn-primary\" onclick=\"borrarCategoria('" + nombre + "')\">" + nombre + "</button>");
}

function DirigirCrearTienda() {
    $.ajax({
        url: '/Tienda/CrearTienda',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#container').html(data);
        }
    });
}


function datosGenerales() {
    $.ajax({
        url: '/Tienda/DatosGenerales',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#contenidoCrearTienda').html(data);
        }
    });
}

function crearCategorias() {
    $.ajax({
        url: '/Tienda/CrearCategorias',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#contenidoCrearTienda').html(data);
        }
    });
}

function crearTiposAtributo() {
    $.ajax({
        url: '/Tienda/CrearTiposAtributo',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#contenidoCrearTienda').html(data);
        }
    });
}

function crearPersonalizacion() {
    $.ajax({
        url: '/Tienda/CrearPersonalizacion',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#contenidoCrearTienda').html(data);
        }
    });
}