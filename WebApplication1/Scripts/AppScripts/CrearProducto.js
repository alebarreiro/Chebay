

var atributosIngresados,
    categoriaSeleccionada,
    datosIngresados,
    fadeOut = 'fadeOutLeft',
    fadeIn = 'fadeInRight',
    divActual;


var marker;
var map;

function cargarMapa() {

    if (document.getElementById('map') != null) {

        map = new GMaps({
            el: '#map',
            mapTypeId: google.maps.MapTypeId.HYBRID,
            center: new google.maps.LatLng(-34.905510300, -56.192056200),
            width: '100%',
            height: '400px',
            zoom: 16,
            click: function (event) {
                map.removeMarkers();
                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                marker = map.addMarker({
                    lat: lat,
                    lng: lng,
                    draggable: true,
                });
            }
        });
    }
}


datosProd = function () {
    if (!divActual) {
        divActual = $('#datosProducto');
    }
    res = validarDatosProd();
    if (res.error) {
        swal("Datos incompletos", res.msg)
    } else {
        divActual.hide();
        $("#datosCategoria").show();
        divActual = $("#datosCategoria");
    }
}

volverProd = function (anterior) {
    divAnterior = $('#' + anterior);
    divActual.hide();
    divAnterior.show();
    divActual = divAnterior;
}

datosCat = function () {
    if (!categoriaSeleccionada) {
        swal("Datos incompletos", "Selecciona una categoría para continuar.");
    } else {
        divActual.hide();
        $("#datosAtributos").show();
        divActual = $("#datosAtributos");
    }
}

datosAtributos = function () {
    divActual.hide();
    $("#geolocalizacion").show();
    divActual = $("#geolocalizacion");
    cargarMapa();
}

datosImagenes = function () {
    divActual.hide();
    $("#paso4-agregarimagenes").show();
}

isInt = function(n) {
    return n % 1 === 0;
}

validarDatosProd = function () {
    var titulo = $('#titulo').val(),
        descripcion = $('#descripcion').val(),
        precioBase = parseInt($("#precioInicial").val(), 10),
        precioComprarYa = parseInt($("#precioComprarYa").val(), 10),
        fechaCierre = $('#fechaCierre').val(),
        horaCierre = $('#horaCierre').val(),
        fecha,
        msgError = "",
        hayError = false;

    if (titulo == "") {
        msgError += "- Ingrese un título para el producto.\n"
        hayError = true;
    }
    if (descripcion == "") {
        msgError += "- Ingrese una descripción al producto. \n"
        hayError = true;
    }
    if (!precioBase) {
        msgError += "- Ingrese un precio base válido. \n"
        hayError = true;
    }
    if (!precioComprarYa) {
        msgError += "- Ingrese un precio comprar ya válido. \n"
        hayError = true;
    }
    if (precioBase && precioComprarYa && (precioComprarYa <= precioBase)) {
        msgError += "- El precio comprar ya debe superar el precio base. \n"
        hayError = true;
    }
    if (!fechaCierre || !horaCierre) {
        msgError += "- Ingrese la fecha y hora de cierre. \n"
        hayError = true;
    }
    if (fechaCierre && horaCierre) {
        fecha = new Date(fechaCierre + " " + horaCierre);
        if (Date.now() >= fecha) {
            msgError += "- La fecha de cierre no debe ser anterior a la fecha actual. \n"
            hayError = true;
        }
    }
    return { error: hayError, msg: msgError };
}

confirmarProducto = function () {
    $('#btnConfirmarSubasta').html("Confirmar subasta <i class=\"fa fa-spinner fa-pulse\"></i>");

    /* VALIDAR LOS DATOS DEL PRODUCTO */
    var titulo = $('#titulo').val(),
        descripcion = $('#descripcion').val(),
        precioBase = parseInt($("#precioInicial").val(), 10),
        precioComprarYa = parseInt($("#precioComprarYa").val(), 10),
        fechaCierre = $('#fechaCierre').val(),
        horaCierre = $('#horaCierre').val(),
        fecha,
        datosProducto,
        attrValues = [],
        attrValue,
        msgError = "", 
        hayError = false;

    if (titulo == "") {
        msgError += "- Ingrese un título para el producto.\n"
        hayError = true;
    }
    if (descripcion == "") {
        msgError += "- Ingrese una descripción al producto. \n"
        hayError = true;
    }
    if (!precioBase) {
        msgError += "- Ingrese un precio base válido. \n"
        hayError = true;
    }
    if (!precioComprarYa) {
        msgError += "- Ingrese un precio comprar ya válido. \n"
        hayError = true;
    }
    if (precioBase && precioComprarYa && (precioComprarYa <= precioBase)){
        msgError += "- El precio comprar ya debe superar el precio base. \n"
        hayError = true;
    }
    if (!fechaCierre || !horaCierre){
        msgError += "- Ingrese la fecha y hora de cierre. \n"
        hayError = true;
    }
    if (fechaCierre && horaCierre) {
        fecha = new Date(fechaCierre + " " + horaCierre);
        if (Date.now() >= fecha){
            msgError += "- La fecha de cierre no debe ser anterior a la fecha actual. \n"
            hayError = true;
        }
    }

    //error de coordenadas no seleccionadas
    if (!marker) {
        msgError += "- Ingrese el punto de localización del producto. \n"
        hayError = true;
    }
    if (!categoriaSeleccionada){
        msgError += "- Debe seleccionar una categoria. \n"
        hayError = true;
    } else {
        if (atributos) {
            $.each(atributos, function () {
                $input = $("#input-" + this.clave);
                if ($input.val()) {
                    attrValue = {};
                    attrValue.TipoAtributoID = this.etiqueta;
                    attrValue.etiqueta = this.etiqueta;
                    attrValue.valor = $input.val();
                    attrValues.push(attrValue);
                }
            });
        }
    }

    if ( hayError ) {
        swal("Datos incompletos", msgError)
    } else {
        datosProducto = {
            'titulo': titulo,
            'descripcion': descripcion,
            'precioBase': precioBase,
            'precioComprarYa': precioComprarYa,
            'fechaCierre': moment(moment(fechaCierre + " " + horaCierre)).utc().format("YYYY-MM-DD HH:mm").toString(),
            'catID': categoriaSeleccionada,
            'latitud': '' + marker.getPosition().lat(),
            'longitud': '' + marker.getPosition().lng()
        }
        
        $.ajax({
            type: "POST",
            url: $("#crearProducto").data('request-url'),
            data: {
                producto: datosProducto,
                atributos: attrValues
            },
            accept: 'application/json',
            success: function (data) {
                $('#btnConfirmarSubasta').html("Confirmar subasta");
                $('#btnConfirmarSubasta').hide();
                swal("éxito!", "Iniciaste una nueva subasta! \nAhora puedes agregar imágenes al producto.", "success");
                datosImagenes();
                $('#seccion-imagenes').attr('data-prodid', data.Message);
                $('#btnFinalizarSubasta').show();
                $('#btnConfirmarSubasta').hide();
            },
            error: function (error) {
                swal("Oops...", "No se pudo crear el nuevo producto: " + error, "error");
            }
        });
    }
}


seleccionarCategoriaSimple = function (catID) {
    categoriaSeleccionada = catID;
    $("#categoriaSeleccionada").html("Categoria seleccionada: #"+ catID);
    var datosProducto = JSON.parse(localStorage.getItem("DatosProducto"));
    if (datosProducto) {
        datosProducto.CatID = catID;
        localStorage.setItem("DatosProducto", JSON.stringify(datosProducto));
    } else {
        datosProducto = {};
        datosProducto.CatID = catID;
        localStorage.setItem("DatosProducto", JSON.stringify(datosProducto));
    }

    $.ajax({
        type: "GET",
        url: $("#content-atributos").data("request-url"),
        data: {
            catId: catID
        },
        accept: 'application/json',
        success: function (data) {
            atributos = data.Atributos;
            if (data.Atributos.length > 0) {
                $.each(data.Atributos, function () {
                    this.esBool = function () {
                        return this.tipoDato == "BOOL" || this.tipoDato == "BINARY";
                    }
                });
            }
            var template = $('#atributos-template').html();
            var templateItem = $('#atributoItem').html();
            Mustache.parse(template);
            var viewObject = {
                Atributos: data.Atributos
            };
            var rendered = Mustache.render(template, viewObject, { atributoItem: templateItem });
            $("#content-atributos").html(rendered);
        },
        error: function (error) {
            console.warn(error);
        }
    });
}

showUserName = function (name) {
    return name && name.indexOf("@") != -1 ? name.substring(0, name.indexOf("@")) : name;
}

parseUtfName = function (name) {
    if (name) {
        return name.replace("@","%40")
    } else {
        return "";
    }
}
