
var correoUsuario;
var loading;
var done = "";


loading = loading || (function () {
    var pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:250px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">Loading...</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
    return {
        show: function (texto) {
            if (texto !== undefined) {
                pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:500px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">' + texto + '</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
            } else {
                pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog" style="width:250px!important;top: 90px;"> <div class="modal-content" style="background-color:rgba(255, 255, 255,1);border-radius: 5px;"> <div class="modal-header" style="padding-top:5px!important;padding-bottom:5px!important;padding-left:15px;padding-right:15px;border-bottom: 0px!important;"> <h4 style="text-align:center;"><strong style="color:#cc0000;">Loading...</strong> &nbsp;&nbsp;&nbsp; <img src="images/gears32.gif" /></h4></div></div></div></div>');
            }
            pleaseWaitDiv.modal("show");
        },
        hide: function () {
            pleaseWaitDiv.modal("hide");
        }
    };
})();

var GetUserName = function () {
    var datae = { 'vista': "fake" };
    $.ajax({
        type: "POST",
        url: "WebServices/Utilities.asmx/GetUserName",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(datae),
        dataType: "json",
        success: function (result) {
            var data = result.d;
            var dataJson = $.parseJSON(data);
            if (dataJson.status === "ok") {
                $("#username").html("<strong>" + dataJson.username + "</strong>");
                $("#userName").html("<strong>" + dataJson.username + "</strong>");
                if (dataJson.adminTranslation === "YES") {
                    $("#translationAdmin").html("Translations");
                }
            } else {
                if (dataJson.data === "NoSession") {
                    document.location.href = "index.html?BackPageSession=NoSession";
                } else {
                    $("#username").val("User");
                }
            }


        },
        error: function (error) {
            $("#username").val("User");
        }
    });
    return false;
}


var validarPeso = function (idFile) {
    var archivo = $('#' + idFile);
    var picsize = (archivo[0].files[0].size);
    if (picsize > 10485760) {
        return false;
    } else {
        return true;
    }
}

var validarExtensionImg = function (nombre) {
    var ext = (nombre.substring(nombre.lastIndexOf(".") + 1)).toUpperCase();
    if (ext === "JPEG" || ext === "JPG" || ext === "" || ext === "PNG") {
        return true;
    } else {
        return false;
    }
}

function validateAvayaEmail(email) {
    email = email.toUpperCase();
    var splitted = email.match("^(.+)@AVAYA\.COM$");
    if (splitted == null) return false;
    if (splitted[1] != null) {
        var regexpUser = /^\"?[\w-_\.]*\"?$/;
        if (splitted[1].match(regexpUser) == null) return false;
        return true;
    }
    return false;
}

var options = [];

$('.dropdown-menu a').on('click', function (event) {

    var $target = $(event.currentTarget),
        val = $target.attr('data-value'),
        $inp = $target.find('input'),
        idx;

    if ((idx = options.indexOf(val)) > -1) {
        options.splice(idx, 1);
        setTimeout(function () { $inp.prop('checked', false) }, 0);
    } else {
        options.push(val);
        setTimeout(function () { $inp.prop('checked', true) }, 0);
    }

    $(event.target).blur();

    console.log(options);

    $("#tags").html(options);



    return false;
});




var limpiarFormulario = function (obj) {
    for (var i in obj) {
        $("#" + i).val("");
        $("#" + i + "Msg").removeClass("has-success");
        $("#" + i + "Ch").removeClass("has-success");
        
    }
}

var limpiarFormularioAdd = function (obj) {
    for (var i in obj) {
        $("#" + i + "Add").val("");
        $("#" + i + "AddMsg").removeClass("has-success");

    }
}

var loadSelects = function (item, id) {
    $("#" + id).empty();
    $("#" + id).append('<option value="" selected="selected"></option>');
    $.each(item, function (i, valor) {
        $("#" + id).append('<option value="' + valor.Id + '">' + valor.Nombre + '</option>');
    });
    $("#loading").fadeOut("slow");
}

function validarFormulario(obj) {
    var respuesta = 0;
    for (var i in obj) {
        if (obj[i] == null || obj[i].length < 1 || /^\s+$/.test(obj[i])) {
                respuesta = respuesta + 1;
                $("#" + i + "Msg").removeClass("has-success");
                $("#" + i + "Msg").addClass("has-error");
                $("#" + i + "Ch").removeClass("has-success");
                $("#" + i + "Ch").addClass("has-error");
            
        } else {
            if (i.indexOf("Email") !== -1) {
                var correo = /^[A-Za-z][A-Za-z0-9_\.-]*@[A-Za-z0-9_-]+\.[A-Za-z0-9_.]+[A-za-z]$/;
                if (!correo.exec(obj[i])) {
                    respuesta = respuesta + 1;
                    $("#" + i + "Msg").removeClass("has-success");
                    $("#" + i + "Msg").addClass("has-error");
                    $("#" + i + "Ch").removeClass("has-success");
                    $("#" + i + "Ch").addClass("has-error");
                } else {
                    respuesta = respuesta + 0;

                    $("#" + i + "Msg").removeClass("has-error");
                    $("#" + i + "Msg").addClass("has-success");
                    $("#" + i + "Ch").removeClass("has-error");
                    $("#" + i + "Ch").addClass("has-success");
                }
            } else {
                respuesta = respuesta + 0;
                $("#" + i + "Msg").removeClass("has-error");
                $("#" + i + "Msg").addClass("has-success");
                $("#" + i + "Ch").removeClass("has-error");
                $("#" + i + "Ch").addClass("has-success");
            }

        }
    }
    if (respuesta === 0) {
        return true;
    } else {
        return false;
    }
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

var limpiarFormularioV2 = function (obj) {
    for (var i in obj) {
        $("#" + i).val("");
        $("#" + i + "Msg").removeClass("has-success");
        $("#" + i + "Msg").removeClass("has-error");
    }
}

function validarFormularioV2(obj, required) {
    var respuesta = 0;
    $.each(required, function (index, value) {
        $.each(obj, function (key, value2) {
            if (value === key) {
                if (value2 == null || value2.length < 1 || /^\s+$/.test(value2) || value2 == "? undefined:undefined ?" || value2 == "?") {
                    respuesta = respuesta + 1;
                    $("#" + value + "Msg").removeClass("has-success");
                    $("#" + value + "Msg").addClass("has-error");
                } else {
                    if (value.indexOf("Email") !== -1) {
                        var correo = /^[A-Za-z][A-Za-z0-9_\.-]*@[A-Za-z0-9_-]+\.[A-Za-z0-9_.]+[A-za-z]$/;
                        if (!correo.exec(value2)) {
                            respuesta = respuesta + 1;
                            $("#" + value + "Msg").removeClass("has-success");
                            $("#" + value + "Msg").addClass("has-error");
                        } else {
                            respuesta = respuesta + 0;

                            $("#" + value + "Msg").removeClass("has-error");
                            $("#" + value + "Msg").addClass("has-success");
                        }
                    } else {
                        respuesta = respuesta + 0;
                        $("#" + value + "Msg").removeClass("has-error");
                        $("#" + value + "Msg").addClass("has-success");
                    }

                }
            }
        });

    });

    if (respuesta === 0) {
        return true;
    } else {
        return false;
    }
}

var formatMesDiaAnio = function(fecha) {
    var date = new Date(fecha);
    var fechaFormato = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    return fechaFormato;
}

function downloadFile(file, carpeta) {
    var iframe = document.getElementById("hiddenDownloader");
    if (iframe === null) {
        iframe = document.createElement('iframe');
        iframe.id = "hiddenDownloader";
        iframe.style.visibility = 'hidden';
        document.body.appendChild(iframe);
    }
    iframe.src = "GenericHandlers/DownloadFileTranslation.ashx?file=" + file + "&carpeta=" + carpeta;
}

$(document).ready(function () {
    // Optimalisation: Store the references outside the event handler:
    var $window = $(window);

    //function checkWidth() {
    //    var windowsize = $window.width();
    //    if (windowsize < 440) {
    //        $(".navbar-brand").html("<span style=\"width:220px!important;\" data-toggle=\"tooltip\" data-placement=\"bottom\" title=\"Avaya Marketing Creative Services Request System\">AVAYA MCS</span>");
    //        $("#Exit").html("<i class='fa fa-sign-out'></i>");
    //    } else {
    //        $(".navbar-brand").html("Avaya Marketing Creative Services Request System");
    //        $("#Exit").html("</i><span>&nbsp;&nbsp;&nbsp;Sign Out</span>");
    //    }
    //}

    //// Execute on load
    //checkWidth();
    // Bind event listener
    //$(window).resize(checkWidth);

    $( "#aside" ).load( "headerAside.html", function( response, status, xhr ) {
        if ( status == "error" ) {
            var msg = "Sorry but there was an error: ";
            $( "#error" ).html( msg + xhr.status + " " + xhr.statusText );
        }
    });
});

try {
    $("[name='publish']").bootstrapSwitch();
} catch (e) {
    var error = e;
}
