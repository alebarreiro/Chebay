

function login() {
    var mail = $('#mail').val();
    var pass = $('#pass').val();

    var objetoLogin = {
        mail: mail,
        pass: pass
    };

    $.ajax({
        url: '/Usuarios/Login',
        type: 'POST',
        dataType: "json",
        data: JSON.stringify(objetoLogin),
        success: function (data, textStatus, jqxhr) {
            cargarInicio();
        },
        error: function (data, textStatus, jqxhr) {
            $("#container").html("<div class=\"alert alert-danger\"><strong>Error! :</strong>Datos incorrectos.</div>");
        }
    });

}

function cargarInicio() {
    $.ajax({
        url: '/Home/Inicio',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#container').html(data);
        }
    });
}

function Registrarse() {

    $.ajax({
        url: '/Usuarios/Registro',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#container').html(data);
        }
    });
}

function ConfirmarRegistro(){
    var mail = $("#mailRegistro").val();
    var pass = $("#passRegistro").val();

    var objetoRegistro = {
        mail: mail,
        pass: pass
    };

    $.ajax({
        url: '/Usuarios/RegistrarUsuario',
        type: 'POST',
        dataType: "json",
        data: JSON.stringify(objetoRegistro),
        success: function (data, textStatus, jqxhr) {
            $('#container').html(data);
        }
    });

}