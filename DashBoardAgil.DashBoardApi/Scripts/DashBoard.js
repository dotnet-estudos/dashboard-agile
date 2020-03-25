swal.setDefaults({ confirmButtonColor: '#337ab7', allowOutsideClick: false });


var dashboard = {
    alertInputDataCallback: function (mensagem, title, callback) {
        //swal({
        //    text: mensagem,
        //    title: title,
        //    type: "input",
        //    closeOnConfirm: false
        //}, function(valor) {
        //    callback(valor);
        //});

        swal({
            title: title,
            text: mensagem,
            type: "input",
            closeOnConfirm: false
        }, function (valor) {
	        if (valor === false)
	            return false;
	        if (valor === "") {
	            swal.showInputError("Nem um palpite?");
	            return false
	        }
	    swal("Parabéns!", "Você disse: " + valor, "success");
	});

    },
    alertSucesso: function (mensagem) {
        swal({
            text: mensagem,
            title: "",
            type: "success"
        });

    }
    , alertSucessoComCallback: function (mensagem, callback) {
        swal({
            title: "",
            text: mensagem,
            type: "success"
        }).then(callback);
    }
	, alertErro: function (mensagem) {
	    swal("", mensagem, "error");
	}
	, alertErroComCallback: function (mensagem, callback) {
	    swal("", mensagem, "error").then(callback);
	}
    , alertAtencao: function (mensagem) {
        swal("", mensagem, "warning");
    }
    , alertAtencaoComCallback: function (mensagem, callback) {
        swal("", mensagem, "warning").then(callback);;
    }
	, confirm: function (mensagem, callbackConfirm, callbackCancel) {
	    swal({
	        title: "",
	        text: mensagem,
	        type: "warning",
	        showCancelButton: true,
	        confirmButtonText: "Confirmar",
	        cancelButtonText: "Cancelar"
	    }).then(function () {
	        callbackConfirm();
	    }, function (cancel) {
	        if (callbackCancel)
	            callbackCancel();
	    });
	}
	, alert: function (titulo, mensagem) {
	    if (!titulo)
	        titulo = "Atenção";
	    swal(titulo, mensagem);
	}, showModal: function (modalTitle, responsive, content, width, height) {
	    $('#dashboardModal .modal-title').text(modalTitle);

	    if (responsive) {
	        $('#dashboardModal .modal-dialog').css('width', '');
	        $('#ModalContent').css('height', '');
	        $('#ModalContent').css('overflow-x', 'hidden');
	        $('#ModalContent').css('overflow-y', 'auto');
	    } else {
	        $('#dashboardModal .modal-dialog').css('width', '96%');
	        $('#ModalContent').css('height', '585px');
	        $('#ModalContent').css('overflow-x', 'hidden');
	        $('#ModalContent').css('overflow-y', 'auto');
	    }

	    if (width) $('#dashboardModal .modal-dialog').css('width', width);
	    if (height) $('#ModalContent').css('height', height);

	    if (content) {
	        $('#ModalContent').html(content);
	    }

	    $('#dashboardModal').modal('show');
	}, CloseModal: function () {
	    $('#ModalContent').html('');
	    $('#dashboardModal').modal('toggle');
	}, Upsert: function (url, idForm, callBackFunction) {
	    $(document).ready(function () {
	        $("#" + idForm).submit(function (event) {
	            event.preventDefault();
	            var formData = new FormData($(this)[0]);

	            $.ajax({
	                url: url,
	                type: "POST",
	                cache: false,
	                contentType: false,
	                processData: false,
	                async: true,
	                data: formData,
	                beforeSend: function () {
	                    $("#divCarregando").show();
	                    $(".field-validation-valid").html("");
	                },
	                success: function (result) {
	                    $("#divCarregando").hide();

	                    if (jQuery.isArray(result)) {
	                        $.each(result, function () {
	                            $(".field-validation-valid[data-valmsg-for='" + this.key + "']").html(this.msg).addClass("show text-danger");
	                            $("#" + this.key).focus();
	                        });
	                        return false;
	                    } else if (!result.key) {
	                        $("#dashboardModal #ModalContent").html(result);
	                    } else {
	                        if (!callBackFunction) callBackFunction = "CallBackFunction";
	                        AlertOnReturn(result, callBackFunction);
	                    }
	                }
	            });
	        });
	    });
	}
    , ConfigurarCombosUnidadeSecretaria: function (idComboSecretaria, idComboNumUnidade, defaultText) {
        var comboSecretaria = document.getElementById(idComboSecretaria);
        var comboUnidade = document.getElementById(idComboNumUnidade);

        if (comboSecretaria !== null) {
            comboSecretaria.onchange = function () {
                var codSecretaria = this.options[this.selectedIndex].value;
                if (codSecretaria)
                    PopulaComboUnidades(comboUnidade, codSecretaria, defaultText);
                else
                    LimpaCombo(comboUnidade, defaultText);
            };
        }
    }
}



function DateBR(date) {
    var d = date;//new Date();

    var curr_day = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();

    var curr_hour = d.getHours();
    var curr_min = d.getMinutes();
    var curr_sec = d.getSeconds();

    curr_month++; // In js, first month is 0, not 1
    year_2d = curr_year.toString().substring(0, 4);
    curr_day = "0" + curr_day;
    return curr_day.substring((curr_day.length==3?1:0),3) + "/" + curr_month + "/" + year_2d;
}


function PopulaComboUnidades(comboUnidade, codSecretaria, defaultText) {
    $.ajax({
        url: "/Base/ObtemUnidadesPorSecretaria",
        type: "GET",
        dataType: "JSON",
        data: { codSecretaria: codSecretaria },
        success: function (unidades) {
            LimpaCombo(comboUnidade, defaultText);
            $.each(unidades, function (i, unidade) {
                var option = $("<option></option>").val(unidade.NumUnidade).html(unidade.DescUnidade);
                $(comboUnidade).append(option);
            });
        }
    });
}

function LimpaCombo(combo, defaultText) {
    if (!defaultText)
        defaultText = "Todos";

    $(combo).html("");
    var option = $("<option></option>").val("").html(defaultText);
    $(combo).append(option);
}

function GetFormData(pindexedArray, sendFormData) {
    var indexedArray = {};

    if (pindexedArray) indexedArray = pindexedArray;

    var unindexedArray = $("form").serializeArray();

    if (sendFormData) {
        $.map(unindexedArray, function (n, i) {
            indexedArray[n["name"]] = n["value"];
        });
    }

    return indexedArray;
}

function ConfigurarMascara(idCampo, mascara) {
    $(document).ready(function () {
        var $campo = $("#" + idCampo);
        if (!Modernizr.inputtypes.date) {
            $campo.inputmask(mascara);
        } else {
            if ($campo.attr('type') !== 'date' && $campo.attr('type') !== 'time') {
                $campo.inputmask(mascara);
            } else if ($campo.attr('type') === 'date') {
                var from = $campo.attr('value').split('/'), d = new Date(from[2], from[1] - 1, from[0]),
                    day = (('' + d.getDate()).length < 2 ? '0' : '') + d.getDate(),
                    month = (('' + (d.getMonth() + 1)).length < 2 ? '0' : '') + (d.getMonth() + 1),
                    year = d.getFullYear();
                $campo.attr('value', [year, month, day].join('-'));
            }
        }
    });
}

function AlertOnReturn(result, callBackFunction) {
    if (result.key === "__Sucesso") {
        dashboard.alertSucessoComCallback(result.msg, eval(callBackFunction));
        $("#dashboardModal").modal("hide");
    } else if (result.key === "__Erro") {
        dashboard.alertErro(result.msg);
    } else if (result.key === "__Alerta") {
        dashboard.alertAtencao(result.msg);
    } else {
        dashboard.alertErro("Ocorreu algum erro ao executar a operação.");
        $("#dashboardModal").modal("hide");
    }
}

function ConfirmaRemoveArquivo(id) {
    var $element = $('#' + id);

    if ($element.data("excluir")) {
        return true;
    }

    dashboard.confirm("Tem certeza que deseja remover este registro?", function () {
        $element.data("excluir", true);
        $element.click();
    });
    return false;
}

Date.prototype.getWeek = function () {
    var target = new Date(this.valueOf());
    var dayNr = (this.getDay() + 6) % 7;
    target.setDate(target.getDate() - dayNr + 3);
    var firstThursday = target.valueOf();
    target.setMonth(0, 1);
    if (target.getDay() != 4) {
        target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
    }
    return 1 + Math.ceil((firstThursday - target) / 604800000);
}

function getDateRangeOfWeek(weekNo, year) {
    var d1 = new Date(year,0,1);
    numOfdaysPastSinceLastMonday = eval(d1.getDay() - 1);
    d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
    var weekNoToday = d1.getWeek();
    var weeksInTheFuture = eval(weekNo - weekNoToday);
    d1.setDate(d1.getDate() + eval(7 * weeksInTheFuture));
    var rangeIsFrom = d1.getDate() + "/" + eval(d1.getMonth() + 1) + "/" + d1.getFullYear();
    d1.setDate(d1.getDate() + 6);
    var rangeIsTo = d1.getDate() + "/" + eval(d1.getMonth() + 1) + "/" + d1.getFullYear();
    return rangeIsFrom + " to " + rangeIsTo;
};
