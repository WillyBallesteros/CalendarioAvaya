//tipo de mensajes ->  info, success, warning, error
function message(texto, titulo, tipo) {

    BootstrapDialog.show({
        title: titulo,
        message: texto,
        cssClass: 'type-' + tipo
    });
    return false;

}

var modalLoading;
modalLoading = modalLoading || (function () {
    return {
        show: function (mensaje) {
            $("#loading").fadeIn("slow");
        },
        hide: function () {
            $("#loading").fadeOut("slow");
        },
    };
})();

var searchIntoJson = function (obj, column, value) {
    var results = [];
    var searchField = column;
    var searchVal = value;
    for (var i = 0 ; i < obj.length ; i++) {
        if (obj[i][searchField] == searchVal) {
            results.push(obj[i]);

        }
    }
    return results;
}

var searchFirstIntoJson = function (obj, column, value) {
    var searchField = column;
    var foo = [];
    var searchVal = value;
    for (var i = 0 ; i < obj.length ; i++) {
        if (obj[i][searchField] == searchVal) {
            foo = obj[i];
        }
    }
    return foo;
}

var CambiarConstrasena = function (actual, nueva, conf) {
    if (actual !== "" && nueva !== "" && conf !== "") {
        if (nueva === conf) {
            var datae = { 'actual': actual, 'nueva': nueva };
            $.ajax({
                type: "POST",
                url: "register.asmx/CambiarContrasena",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(datae),
                dataType: "json",
                success: function (resultado) {
                    var tales = resultado.d.indexOf("NoSession");
                    if (tales !== -1) {
                        document.location.href = "login.html?BackPageSession=NoSession";
                    }
                    tales = resultado.d.indexOf("ok");
                    if (tales != -1) {
                        titulo = (lang === "ESP") ? "Atención" : "Attention";
                        mensaje = (lang === "ESP") ? "Contraseña cambiada correctamente" : "Password changed successfully";
                        message(mensaje, titulo, "success");
                        $('#recoveryModal').modal('hide');
                    } else {
                        titulo = (lang === "ESP") ? "Atención" : "Attention";
                        mensaje = (lang === "ESP") ? "Se ha presentado un error, por favor trate más tarde" : "An error occurred, please try later";
                        message(mensaje, titulo, "success");

                    }
                },
                error: function (error) {
                    titulo = (lang === "ESP") ? "Atención" : "Attention";
                    mensaje = (lang === "ESP") ? "Se ha presentado un error, por favor trate más tarde" : "An error occurred, please try later";
                    message(mensaje, titulo, "success");

                }
            });
        } else {
            titulo = (lang === "ESP") ? "Atención" : "Attention";
            mensaje = (lang === "ESP") ? "La actual y la nueva contraseña no coinciden" : "The passwords do not match";
            message(mensaje, titulo, "danger");
        }
    } else {

        titulo = (lang === "ESP") ? "Atención" : "Attention";
        mensaje = (lang === "ESP") ? "Todos los datos son requeridos" : "Please fill in all requested data";
        message(mensaje, titulo, "warning");
    }


    return false;
}
var nombreUsuario = "";
var validarSession = function () {
    var datae = { 'data': 'data' };
    $.ajax({
        type: "POST",
        url: "register.asmx/ValidarSession",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(datae),
        dataType: "json",
        success: function (result) {
            var tales = result.d.indexOf("NoSession");
            if (tales !== -1) {
                document.location.href = "login.html?BackPageSession=NoSession";
            }
            nombreUsuario = result.d;
            $("#usuarioNick").html(result.d);
        },
        error: function (error) {
            document.location.href = "login.html?BackPageSession=NoSession";
        }
    });
    return false;
}



var searchCharIntoJson = function (obj, column, value) {
    var results = [];
    var valueField;
    var searchField = column;
    var searchVal = value;
    searchVal = searchVal.toLowerCase();
    for (var i = 0 ; i < obj.length ; i++) {
        valueField = obj[i][searchField];
        valueField = valueField.toLowerCase();
        if (valueField.indexOf(searchVal) !== -1) {
            results.push(obj[i]);

        }
    }
    return results;
}



//Obtener los parametros de la URL
var QueryString = function () {
    var query_string = {};
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (typeof query_string[pair[0]] === "undefined") {
            query_string[pair[0]] = pair[1];
        } else if (typeof query_string[pair[0]] === "string") {
            var arr = [query_string[pair[0]], pair[1]];
            query_string[pair[0]] = arr;
        } else {
            query_string[pair[0]].push(pair[1]);
        }
    }
    return query_string;
}();

function textCounter(field, cnt, maxlimit) {
    var cntfield = document.getElementById(cnt);
    if (field.value.length > maxlimit) { // if too long...trim it!
        //field.value = field.value.substring(0, maxlimit);
        cntfield.value = maxlimit - field.value.length;
        $('#' + cnt).css('color', '#cc0000');
    }
        // otherwise, update 'characters left' counter
    else {
        cntfield.value = maxlimit - field.value.length;
        $('#' + cnt).css('color', '#000000');
    }
}


function eliminarUltimoCaracter(texto, caracter) {
    if ((texto.substring(texto.length - 1, texto.length)) === caracter) {
        texto = texto.substring(0, texto.length - 1);
    }
    return texto;
}

function eliminarPrimerCaracter(texto, caracter) {
    if ((texto.substring(0, 1)) === caracter) {
        texto = texto.substring(1, texto.length);
    }
    return texto;
}

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (obj, start) {
        for (var i = (start || 0), j = this.length; i < j; i++) {
            if (this[i] === obj) { return i; }
        }
        return -1;
    };
}

function getInternetExplorerVersion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');
    var trident = ua.indexOf('Trident/');
    if (msie > 0) {
        // Si es IE 10 o más viejo => retorna el número de versión
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    if (trident > 0) {
        // Si es IE 11 o más nuevo => retorna el número de versión
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    // otro browser
    return false;
}

//Validaciones de campos
function validate(valor) {
    if (valor == null || valor.length < 1 || /^\s+$/.test(valor)) {
        return false;
    }
    else {
        return true;
    }
}

function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

//Elimina los caracteres especiales en un texto para poderlos mostrar sin errores
function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}

//Validar campo email
function validateEmail(valor) {
    var correo = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!correo.exec(valor)) {
        return false;
    } else {
        return true;
    }
}

function validateNumber(valor) {
    if (validate(valor)) {
        if (isNaN(valor)) {
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
}

function setSelectByValue(eID, val) {
    var ele = document.getElementById(eID);
    for (var ii = 0; ii < ele.length; ii++)
        if (ele.options[ii].value === val) {
            ele.options[ii].selected = true;
            return true;
        }
    return false;
}

function setSelectByValueDiffType(eID, val) {
    val = String(val);
    var ele = document.getElementById(eID);
    for (var ii = 0; ii < ele.length; ii++)
        if (ele.options[ii].value === val) {
            ele.options[ii].selected = true;
            return true;
        }
    return false;
}

function setSelectByText(eID, text) {
    var ele = document.getElementById(eID);
    for (var ii = 0; ii < ele.length; ii++)
        if (ele.options[ii].text == text) {
            ele.options[ii].selected = true;
            return true;
        }
    return false;
}

var validarExtension = function (nombre) {
    var ext = (nombre.substring(nombre.lastIndexOf(".") + 1)).toUpperCase();
    var respuesta = 0;
    var extension = new Object();
    extension.EXE = "EXE";
    extension.COM = "COM";
    extension.VB = "VB";
    extension.VBS = "VBS";
    extension.VBEE = "VBE";
    extension.CMDD = "CMD";
    extension.BATT = "BAT";
    extension.WSS = "WS";
    extension.WSFF = "WSF";
    extension.SCRR = "SCR";
    extension.SHS = "SHS";
    extension.PIF = "PIF";
    extension.HTA = "HTA";
    extension.JAR = "JAR";
    extension.JS = "JS";
    extension.JSE = "JSE";
    extension.LNK = "LNK";

    for (var i in extension) {
        if (extension[i] === ext) {
            respuesta = parseInt(respuesta) + 1;
            break;
        }
        else {
            respuesta = parseInt(respuesta) + 0;
        }
    }
    if (respuesta === 0) {
        return true;
    } else {
        return false;
    }
}







