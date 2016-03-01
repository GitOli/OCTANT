$(document).ready(function() {
    var url = "";
    $("#dialog-alert").dialog({
        autoOpen: false,
        resizable: false,
        height: 170,
        width: 350,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function(event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
        buttons: {
            "OK": function() {
                $(this).dialog("close");

            },
            "Cancel": function() {
                $(this).dialog("close");
            }
        }
    });

    if ('@TempData["msg"]' != "") {
        $("#dialog-alert").dialog('open');
    }

    $("#dialog-edit").dialog({
        title: 'Create User',
        autoOpen: false,
        resizable: false,
        width: 400,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function(event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $(this).load(url);
        }
    });

    $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 170,
        width: 350,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function(event, ui) {
            $(".ui-dialog-titlebar-close").hide();

        },
        buttons: {
            "OK": function() {
                $(this).dialog("close");
                window.location.href = url;
            },
            "Cancel": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#dialog-detail").dialog({
        title: 'View User',
        autoOpen: false,
        resizable: false,
        width: 400,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function(event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $(this).load(url);
        },
        buttons: {
            "Close": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#lnkCreate").live("click", function(e) {
        //e.preventDefault(); //use this or return false
        url = $(this).attr('href');
        $("#dialog-edit").dialog('open');

        return false;
    });

    $(".lnkEdit").live("click", function(e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        $(".ui-dialog-title").html("Update User");
        $("#dialog-edit").dialog('open');

        return false;
    });

    $(".lnkDelete").live("click", function(e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        $("#dialog-confirm").dialog('open');

        return false;
    });

    $(".lnkDetail").live("click", function(e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        $("#dialog-detail").dialog('open');

        return false;
    });

    $("#btncancel").live("click", function(e) {
        $("#dialog-edit").dialog("close");
        return false;
    });
});