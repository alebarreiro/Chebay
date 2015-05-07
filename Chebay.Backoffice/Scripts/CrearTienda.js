var hayAtributos, hayCategorias, hayDatosGenerales, hayPersonalizacion, idCategorias, idTiposAtributo, tiendaCreada;
var categoriasCreadas = false;
hayAtributos = hayCategorias = hayDatosGenerales = hayPersonalizacion = tiendaCreada = false;
idCategorias = idTiposAtributo = 1;

var paginaAntesLoading, padreAgregarCategoria, paginaAntesLoadingAgregarCategoria;

function modalAgregarCategoria(padre) {
    padreAgregarCategoria = padre;
    $("#modalAgregarCategoria").modal();
}

function cargando() {
    paginaAntesLoading = $("#container").html();

    var loading = "<img src=\"Images/cargando.gif\" class=\"cargando\">";

    $("#container").html(loading);
}

function finCargando() {
    $("#container").html(paginaAntesLoading);
}

function cargandoAgregarCategoria() {
    paginaAntesLoadingAgregarCategoria = $("#divCategorias").html();

    var loading = "<img src=\"Images/cargando.gif\" class=\"cargando\">";

    $("#divCategorias").html(loading);
}

function finCargandoAgregarCategoria() {
    $("#divCategorias").html(paginaAntesLoadingAgregarCategoria);
}

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

    cargando();

    $.ajax({
        url: '/Tienda/GuardarDatosGenerales',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data : JSON.stringify(datosGenerales),
        success: function (data, textStatus, jqxhr) {
            finCargando();
            $.notify({
                // options
                message: '<strong>Se han guardado los datos generales correctamente.</strong>'
            }, {
                // settings
                type: 'success'
            });
        },
        error: function (data, textStatus, jqxhr) {
            finCargando();
            $.notify({
                // options
                message: '<strong>Error al guardar los datos generales.</strong>'
            }, {
                // settings
                type: 'danger'
            });
        }
    });


    hayDatosGenerales = true;
    tiendaCreada = true;
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



function agregarCategoria(tipoCategoria) {
    var nombre = $("#nombreCategoria").val();

    var categoriaNueva = {
        nombre: nombre,
        padre: padreAgregarCategoria,
        tipoCategoria : tipoCategoria
    }

    $("#modalAgregarCategoria").modal('hide');

    cargandoAgregarCategoria();

    $.ajax({
        url: '/Tienda/AgregarCategoria',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(categoriaNueva),
        success: function (data, textStatus, jqxhr) {
            $.notify({
                // options
                message: '<strong>Se ha agregado la categoría correctamente.</strong>'
            }, {
                // settings
                type: 'success'
            });
        },
        error: function (data, textStatus, jqxhr) {
            $.notify({
                // options
                message: '<strong>Error al agregar la categoría.</strong>'
            }, {
                // settings
                type: 'danger'
            });
        }
    });

    //refresca las categorias
    setTimeout(function(){
        $.ajax({
        url: '/Tienda/ObtenerCategorias',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            finCargandoAgregarCategoria();
            $('#divCategorias').html(data);
        }
    });
    }, 8000);
    

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

function notificarCategoriaSimple() {
    $.notify({
        // options
        message: '<strong>No se puede agregar categorías a una categoría simple.</strong>'
    }, {
        // settings
        type: 'danger'
    });
}

function crearCategorias() {
    if (!tiendaCreada) {
        $.notify({
            // options
            message: '<strong>Debes ingresar los Datos Generales para pasar a crear categorías.</strong>'
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
                cargandoAgregarCategoria();
            }
        });

        $.ajax({
            url: '/Tienda/ObtenerCategorias',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                finCargandoAgregarCategoria();
                $('#divCategorias').html(data);
            }
        });
    }
    
}


function crearTiposAtributo() {
    if (!tiendaCreada || !categoriasCreadas) {
        $.notify({
            // options
            message: '<strong>Debes ingresar las categorías de la tienda para pasar a crear tipos de atributo.</strong>'
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
            message: '<strong>Debes ingresar los datos generales de la tienda para poder personalizarla.</strong>'
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