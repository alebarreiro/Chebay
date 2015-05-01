var hayAtributos, hayCategorias, hayDatosGenerales, hayPersonalizacion, idCategorias, idTiposAtributo, tiendaCreada;
var categoriasCreadas = false;
hayAtributos = hayCategorias = hayDatosGenerales = hayPersonalizacion = tiendaCreada = false;
idCategorias = idTiposAtributo = 1;

function finalizarCreacionCategorias() {
    var categorias = new Array();

    var contenidoDiv = $("#divCategorias").html();


}

function finalizarDatosGenerales(){
    //enviar al TiendaController los datos generales, y setear hayDatosGenerales en true
    var titulo = $("#tituloTienda").val();
    var descripcion = $("#descripcionTienda").val();

    var datosGenerales = {
        titulo: titulo,
        descripcion : descripcion
    }

    $.ajax({
        url: '/Tienda/GuardarDatosGenerales',
        type: 'POST',
        dataType: "json",
        data : JSON.stringify(datosGenerales),
        success: function (data, textStatus, jqxhr) {
            $.notify({
                // options
                message: 'Se han guardado los datos generales correctamente.'
            }, {
                // settings
                type: 'success'
            });
        },
        error: function (data, textStatus, jqxhr) {
            $.notify({
                // options
                message: 'Error al guardar los datos generales.'
            }, {
                // settings
                type: 'danger'
            });
        }
    });


    hayDatosGenerales = true;
}

function finalizarCrearTienda() {
    var mensaje = "";
    if (!hayAtributos) {
        mensaje += "Falta crear los Tipos de Atributo de cada Categoria\n";
    }

    if (!hayCategorias) {
        mensaje += "Falta crear las categorias\n";
    }

    if (!hayDatosGenerales) {
        mensaje += "Falta agregar los Datos Generales\n";
    }

    if (!hayPersonalizacion) {
        mensaje += "Falta crear la Personalización\n";
    }

    if (mensaje.length > 0) {
        $.notify({
            // options
            message: mensaje
        }, {
            // settings
            type: 'danger'
        });
    }
    else {
        $.ajax({
            url: '/Tienda/FinalizarCreacionTienda',
            type: 'POST',
            dataType: "json",
            data: JSON.stringify(datosGenerales),
            success: function (data, textStatus, jqxhr) {
                $('#container').html(data);
            },
            error: function (data, textStatus, jqxhr) {
                $('#container').html(data);
            }
        });
    }
    
}


function borrarTipoAtributo(idTipoAtributo , atributo) {
    var atributos = $("#divTiposAtributo").html();
    atributos = atributos.replace("&nbsp;&nbsp;<button id=\"" + idTipoAtributo + "\" class=\"btn btn-primary\" onclick=\"borrarTipoAtributo('" + idTiposAtributo + "','" + nombre + "')\">" + atributo + "</button>", " ");
    $("#divTiposAtributo").html(atributos);
}

function agregarTipoAtributo() {
    var nombre = $("#nombreTipoAtributo").val();

    var categorias = $("#divTiposAtributo").html();

    $("#divTiposAtributo").html(categorias + "&nbsp;&nbsp;<button id=\"" + idTiposAtributo + "\" class=\"btn btn-primary\" onclick=\"borrarTipoAtributo('" + idTiposAtributo + "','" + nombre + "')\">" + nombre + "</button>");

    idTiposAtributo++;
}

function borrarCategoria(idCategoria, nombre) {
    var categorias = $("#divCategorias").html();
    categorias = categorias.replace("&nbsp;&nbsp;<button id=\"" + idCategoria + "\" class=\"btn btn-primary\" onclick=\"borrarCategoria('" + idCategoria + "')\">" + nombre + "</button>", " ");
    $("#divCategorias").html(categorias);
}

function agregarCategoria() {
    var nombre = $("#nombreCategoria").val();

    var categorias = $("#divCategorias").html();

    $("#divCategorias").html(categorias + "&nbsp;&nbsp;<button id=\"" + idCategorias + "\" class=\"btn btn-primary\" onclick=\"borrarCategoria('" + idCategorias + "', '" + nombre + "')\">" + nombre + "</button>");

    idCategorias++;
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
    if (!tiendaCreada) {
        $.notify({
            // options
            message: 'Debes ingresar los Datos Generales para pasar a crear categorías.'
        }, {
            // settings
            type: 'danger'
        });
    }
    else {
        $.ajax({
            url: '/Tienda/CrearCategorias',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                $('#contenidoCrearTienda').html(data);
            }
        });
    }
    
}


function crearTiposAtributo() {
    if (!tiendaCreada || !categoriasCreadas) {
        $.notify({
            // options
            message: 'Debes ingresar las categorías de la tienda para pasar a crear tipos de atributo.'
        }, {
            // settings
            type: 'danger'
        });
    }
    else {
        $.ajax({
            url: '/Tienda/CrearTiposAtributo',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                $('#contenidoCrearTienda').html(data);
            }
        });
    }
    
}

function crearPersonalizacion() {
    if (!tiendaCreada) {
        $.notify({
            // options
            message: 'Debes ingresar los datos generales de la tienda para poder personalizarla.'
        }, {
            // settings
            type: 'danger'
        });
    }
    else {
        $.ajax({
            url: '/Tienda/CrearPersonalizacion',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                $('#contenidoCrearTienda').html(data);
            }
        });
    }
    
}