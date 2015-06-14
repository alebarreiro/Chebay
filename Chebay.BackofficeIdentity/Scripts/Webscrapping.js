var categoriaSeleccionada;

function cargandoDatos(div) {
    paginaAntesLoadingCargarDatos = $(div).html();

    var loading = "<img src=\"Images/cargando.gif\" class=\"cargando\">";

    $(div).html(loading);
}

function finCargandoDatos(div) {
    $(div).html(paginaAntesLoadingCargarDatos);
}

function modalSeleccionarCategoriaWebScrapping(categoria) {
    categoriaSeleccionada = categoria;
    $("#modalSeleccionarCategoria").modal();
}

function seleccionarCategoria() {

    var datos = {
        idCategoria: categoriaSeleccionada
    }

    $("#errores").html("");
    //hago pedido al servidor
    cargandoDatos("#bodySeleccionarCategoria");
    $.ajax({
        url: '/Webscrapping/SeleccionarCategoriaSimple',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(datos),
        success: function (data, textStatus, jqxhr) {
            finCargandoDatos("#bodySeleccionarCategoria");
            $("#bodySeleccionarCategoria").html(data["Message"]);
            $("#botonSeleccionarCategoria").hide();
            $("#traerCategorias").show();
            $("#errores").html("");
        },
        error: function (data, textStatus, jqxhr) {
            finCargandoDatos("#bodySeleccionarCategoria");
            $("#errores").html("<p style=\"color : red;\">Error al seleccionar la categoría.</p>");
        }
    });
}

function traerCategorias() {
    var maxProductos = $("#cantProductosMaxima").val();
    if (isNaN(maxProductos)) {
        $("#errores").html("<p style=\"color : red;\">El valor ingresado debe ser un número.</p>");
    }
    else {

        var datos = {
            maxProductos: parseInt(maxProductos)
        }

        //hago pedido al servidor
        cargandoDatos("#bodySeleccionarCategoria");
        $.ajax({
            url: '/Webscrapping/TraerCategorias',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(datos),
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#bodySeleccionarCategoria");
                $("#bodySeleccionarCategoria").html(data["Message"]);
                $("#botonSeleccionarCategoria").hide();
                $("#traerCategorias").hide();
                $("#errores").html("");
                $("#tituloModalWebscrapping").html("Haga click en una categoría para listar las categorías hijas");
                //hay que mostrar un boton
            },
            error: function (data, textStatus, jqxhr) {
                finCargandoDatos("#bodySeleccionarCategoria");
                $("#errores").html("<p style=\"color : red;\">Error al seleccionar la cantidad máxima de productos a importar.</p>");
            }
        });
    }
}

function obtenerCategoriasHijas(categoria) {

        var datos = {
            categoria : categoria
        }
        
        //hago pedido al servidor
        cargandoDatos("#bodySeleccionarCategoria");
        $.ajax({
            url: '/Webscrapping/ObtenerCategoriasHijas',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(datos),
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#bodySeleccionarCategoria");
                $("#bodySeleccionarCategoria").html(data["Message"]);
                $("#botonSeleccionarCategoria").hide();
                $("#traerCategorias").hide();
                $("#errores").html("");
                $("#tituloModalWebscrapping").html("Seleccione una categoría para importar sus productos");
                //hay que mostrar un boton
            },
            error: function (data, textStatus, jqxhr) {
                finCargandoDatos("#bodySeleccionarCategoria");
                $("#errores").html("Error al obtener las categorías hijas.");
            }
        });
    }

    function traerProductosDeCategoria(categoria) {

        var datos = {
            idCategoria : categoria
        }

        //hago pedido al servidor
        cargandoDatos("#bodySeleccionarCategoria");
        $.ajax({
            url: '/Webscrapping/ConfirmarProductosDeCategoria',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(datos),
            success: function (data, textStatus, jqxhr) {
                finCargandoDatos("#bodySeleccionarCategoria");
                $("#errores").html("");
                $("#modalSeleccionarCategoria").modal('hide');
                $.notify({
                    // options
                    message: '<strong>Se han agregado los productos a la categoría correctamente.</strong>'
                }, {
                    // settings
                    type: 'success'
                });
                
                
            },
            error: function (data, textStatus, jqxhr) {
                finCargandoDatos("#bodySeleccionarCategoria");
                $("#errores").html("Error al agregar los productos a la categoría.");
            }
        });
    }

