

function login() {
    var mail = $('#mail').val();
    var pass = $('#pass').val();

    $.ajax(

    )

}

function Registrarse() {

    $.ajax({
        url: '/Usuarios/Registro',
        type: 'GET',
        success: function (data, textStatus, jqxhr) {
            $('#container').html(data);
        }
    })
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
    })

}