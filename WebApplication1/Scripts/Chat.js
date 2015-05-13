$(function () {
    var box = null;
    var groupChat = $.connection.chat;

    if (box) {
        box.chatbox("option", "boxManager").toggleBox();
    }
    else {
        box = $("#chat_div").chatbox({
            id: "chat_div",
            user: { key: "value" },
            title: "chat",
            messageSent: function (id, user, msg) {
                //myWS.send(msg);
                groupChat.server.sendGroup(tenanttienda, usuario, msg);
            }
        });
        //minimizo
        box.chatbox("toggleContent");
    };


    //Receive function
    groupChat.client.receiveGroup = function (message) {
        $("#chat_div").chatbox("option", "boxManager").addMsg(null, message);//addMsg(obj.id, obj.msg);
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        //register on group
        groupChat.server.joinGroup(tenanttienda);
    });

});

