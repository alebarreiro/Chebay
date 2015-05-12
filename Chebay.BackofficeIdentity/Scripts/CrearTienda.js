var hayAtributos, hayCategorias, hayDatosGenerales, hayPersonalizacion, idCategorias, idTiposAtributo, tiendaCreada;
var categoriasCreadas = false;
hayAtributos = hayCategorias = hayDatosGenerales = hayPersonalizacion = tiendaCreada = false;

var paginaAntesLoading, padreAgregarCategoria, paginaAntesLoadingCargarDatos, categoriaAgregarTipoAtributo;

function modalAgregarCategoria(padre) {
    padreAgregarCategoria = padre;
    $("#modalAgregarCategoria").modal();
}

function personalizar() {
    var color = $("#colorTienda").val();
    var datos = {
        color : color
    }

    cargandoDatos("#divPersonalizacion");

    $.ajax({
        url: '/Tienda/Personalizar',
        type: 'GET',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(datos),
        success: function (data, textStatus, jqxhr) {
            finCargandoDatos("#divPersonalizacion");
            $.notify({
                // options
                message: '<strong>Se ha personalizado la Tienda correctamente.</strong>'
            }, {
                // settings
                type: 'success'
            });
        },
        error: function (data, textStatus, jqxhr) {
            finCargandoDatos("#divPersonalizacion");
            $.notify({
                // options
                message: '<strong>Error al personalizar la Tienda.</strong>'
            }, {
                // settings
                type: 'danger'
            });
        }
    });
}

function verTiposAtributo(categoria, nombre) {
    $("#" + categoria).popover('hide');

    var datos = {
        idCategoria: categoria
    };

    cargandoDatos("#divTiposAtributo");

    $.ajax({
        url: '/Tienda/ObtenerTiposAtributo',
        type: 'GET',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(datos),
        success: function (data, textStatus, jqxhr) {
            finCargandoDatos("#divTiposAtributo");
            $("#bodyVerTiposAtributo").html(data);
        }
    });
    $("#tituloVerTiposAtributo").val('Ver Tipos de Atributo de la Categoría : ' + nombre);
    $("#modalVerTiposAtributo").modal('show');
}

function modalAgregarTipoAtributo(categoria, nombre) {
    $("#" + categoria).popover('hide');
    $("#tituloAgregarTipoAtributo").val('Agregar Tipo de Atributo a la Categoría : ' + nombre);
    categoriaAgregarTipoAtributo = categoria;
    $("#modalAgregarTipoAtributo").modal();
}

function cargando() {
    paginaAntesLoading = $("#container").html();

    var loading = "<img src=\"Images/cargando.gif\" class=\"cargando\">";

    $("#container").html(loading);
}

function finCargando() {
    $("#container").html(paginaAntesLoading);
}

function cargandoDatos(div) {
    paginaAntesLoadingCargarDatos = $(div).html();

    var loading = "<img src=\"Images/cargando.gif\" class=\"cargando\">";

    $(div).html(loading);
}

function finCargandoDatos(div) {
    $(div).html(paginaAntesLoadingCargarDatos);
}

function finalizarCreacionCategorias() {
    var categorias = new Array();

    var contenidoDiv = $("#divCategorias").html();


}

function finalizarDatosGenerales() {
    //enviar al TiendaController los datos generales, y setear hayDatosGenerales en true
    var titulo = $("#tituloTienda").val();
    var descripcion = $("#descripcionTienda").val();

    var datosGenerales = {
        titulo: titulo,
        descripcion: descripcion
    }

    cargando();

    $.ajax({
        url: '/Tienda/GuardarDatosGenerales',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(datosGenerales),
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

function borrarTipoAtributo(idTipoAtributo, atributo) {
    var atributos = $("#divTiposAtributo").html();
    atributos = atributos.replace("&nbsp;&nbsp;<button id=\"" + idTipoAtributo + "\" class=\"btn btn-primary\" onclick=\"borrarTipoAtributo('" + idTiposAtributo + "','" + nombre + "')\">" + atributo + "</button>", " ");
    $("#divTiposAtributo").html(atributos);
}

function agregarTipoAtributo() {
    var nombre = $("#nombreTipoAtributo").val();
    var tipo = $("tipoDatosTipoAtributo").val();

    var datosTipoAtributo = {
        nombre: nombre,
        tipo: tipo,
        categoria: categoriaAgregarTipoAtributo
    };

    $("#modalAgregarTipoAtributo").modal('hide');

    cargandoDatos("#divTiposAtributo");

    $.ajax({
        url: '/Tienda/AgregarTipoAtributo',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(datosTipoAtributo),
        success: function (data, textStatus, jqxhr) {
            $.notify({
                // options
                message: '<strong>Se ha agregado el tipo de atributo correctamente.</strong>'
            }, {
                // settings
                type: 'success'
            });
        },
        error: function (data, textStatus, jqxhr) {
            $.notify({
                // options
                message: '<strong>Error al agregar el tipo de atributo.</strong>'
            }, {
                // settings
                type: 'danger'
            });
        }
    });

    //refresca las categorias
    setTimeout(function () {
        $.ajax({
            url: '/Tienda/ObtenerCategoriasTipoAtributo',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#divTiposAtributo");
                $("#divTiposAtributo").html();
            }
        });
    }, 8000);

    $("#nombreTipoAtributo").val('');

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
        tipoCategoria: tipoCategoria
    }

    $("#modalAgregarCategoria").modal('hide');

    cargandoDatos("#divCategorias");

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
    setTimeout(function () {
        $.ajax({
            url: '/Tienda/ObtenerCategorias',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#divCategorias");
                $('#divCategorias').html(data);
            }
        });
    }, 8000);

    $("#nombreCategoria").val('');
    categoriasCreadas = true;

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
                cargandoDatos("#divCategorias");
            }
        });

        $.ajax({
            url: '/Tienda/ObtenerCategorias',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#divCategorias");
                $('#divCategorias').html(data);
            }
        });
    }

}


function crearTiposAtributo() {
    if (!tiendaCreada) {
        $.notify({
            // options
            message: '<strong>Debes ingresar los datos generales de la tienda para pasar a crear tipos de atributo.</strong>'
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
                cargandoDatos("#divTiposAtributo");
            }
        });
        $.ajax({
            url: '/Tienda/ObtenerCategoriasTipoAtributo',
            type: 'GET',
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#divTiposAtributo");
                $('#divTiposAtributo').html(data);
            }
        });
    }

}

function mostrarPopover(categoria, nombre) {
    var html = '<div class="popover" role="tooltip">"'
        + '<div class="arrow"></div><h3 class="popover-title">Seleccione la Acción a Realizar</h3>'
        + '<div class="popover-content">'
        + '<button class=\"btn btn-primary\" onclick=\"verTiposAtributo(' + categoria + ',' + nombre +')\">Ver Tipos de Atributo</button>'
        + '<button class=\"btn btn-success\" onclick=\"modalAgregarTipoAtributo(' + categoria + ',\'' + nombre + '\')\">Agregar Tipo de Atributo</button>'
        + '</div></div>';

    var options = {
        animation: true,
        html: true,
        template: html,
        placement : 'right'
    }
    $("#" + categoria).popover(options);
    $("#" + categoria).popover('show');
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