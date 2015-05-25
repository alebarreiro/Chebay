var atributosIngresados,
    categoriaSeleccionada,
    datosIngresados;

datosProd = function () {
    $("#datosProducto").toggle(1000);
}

datosCat = function () {
    $("#datosCategoria").toggle(1000);
}

datosAtributos = function () {
    $("#datosAtributos").toggle(1000);
}

datosImagenes = function () {
    $("#datosImagenes").toggle(1000);
}

isInt = function(n) {
    return n % 1 === 0;
}

confirmarProducto = function () {
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
            console.log("Atributos seleccionados: ");
            console.log(attrValues);
        }
    }

    if ( hayError ) {
        swal("Datos incompletos", msgError)
    } else {
        debugger;
        datosProducto = {
            'titulo': titulo,
            'descripcion': descripcion,
            'precioBase': precioBase,
            'precioComprarYa': precioComprarYa,
            'fechaCierre': fechaCierre + " " + horaCierre,
            'catID' : categoriaSeleccionada
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
                swal("éxito!", "Iniciaste una nueva subasta! \nAhora puedes agregar imágenes al producto.", "success");
                debugger;
                $("#paso4-agregarimagenes").show();
                $('#seccion-imagenes').attr('data-prodid', data.Message);
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
