calendarBackEndApp.controller('addCampaignController', function ($scope, calendarBackEndModel, getJson, $route, $routeParams, $location, $log, $q, $timeout) {

	$scope.loadingShow = loadingShow;
	$scope.loadingHide = loadingHide;
	

	//Definicion de fechas
	$('#dateStartD').datetimepicker({
		format: 'MM/DD/YYYY hh:mm',
		
	});
	$('#dateEndD').datetimepicker({
		format: 'MM/DD/YYYY hh:mm'
	});

	
	
	$("#dateStartD").on("dp.change", function (e) {
		$("#dateEndD").data("DateTimePicker").minDate(e.date);
	});

	$("#dateStart").focus(function () {
		$("#dateStartD").data("DateTimePicker").show();
	});
	$("#dateEnd").focus(function () {
		$("#dateEndD").data("DateTimePicker").show();
	});

	
	//Cuando viene desde el Calendario - Se carga la fecha de inicio y final
	$scope.fechaEvento = $routeParams.fecha;
	if ($scope.fechaEvento) {
		var f = $scope.fechaEvento.substring(0, 10);
		var ff = f.split("-");
		var format = ff[1] + "/" + ff[2] + "/" + ff[0];
		$scope.dateStart = format;
		$scope.dateEnd = format;
		
	}

	function compareStrings(a, b) {
		// Assuming you want case-insensitive comparison
		a = a.toLowerCase();
		b = b.toLowerCase();
		return (a < b) ? -1 : (a > b) ? 1 : 0;
	}

	//Carga de datos en los selects y checkbox
	$scope.getAndLoadData = function (nameList) {
		var dataOrder = 0;
		getJson.get(nameList).success(function (data) {
			data.sort(function(a, b) {
				return compareStrings(a.name, b.name);
			});
			$scope[nameList] = data;
		});
	}

	$scope.getAndLoadData('businessSegmentList');
	$scope.getAndLoadData('businessUnitList');
	$scope.getAndLoadData('languagesList');
	$scope.getAndLoadData('targetAudienceList');
	$scope.getAndLoadData('theaterList');
	$scope.getAndLoadData('verticalsList');
	$scope.getAndLoadData('tacticList');
	$scope.getAndLoadData('ctaOfferList');
	// fin de carga

	//Funciones para Evaluar los valores de los Checkbox
	$scope.lenguajesSeleccionados = [];
	$scope.pss = {};

	$scope.evaluarListaLenguajes = function (valor) {
		var a = $scope.lenguajesSeleccionados.indexOf(valor);
		if (a === -1) {
			$scope.lenguajesSeleccionados.push(valor);
			
		} else {
			$scope.lenguajesSeleccionados.splice(a, 1);
			$scope.pss[valor] = false;
		}
	}

	$scope.changeLanguages = function (valor) {
		$scope.evaluarListaLenguajes(valor);
	};

	$scope.changeTheater = function() {
		$scope.getAndLoadData('subAreasList');
	};

	$scope.verticalesSeleccionados = [];
	$scope.vss = {};

	$scope.evaluarListaVerticales = function (valor) {
		var a = $scope.verticalesSeleccionados.indexOf(valor);
		if (a === -1) {
			$scope.verticalesSeleccionados.push(valor);
			//$scope.ps[valor] = true;
		} else {
			$scope.verticalesSeleccionados.splice(a, 1);
			$scope.vss[valor] = false;
		}
	}

	$scope.changeVerticals = function (valor) {
		$scope.evaluarListaVerticales(valor);

	};
	//FIN -- Funciones para Evaluar los valores de los Checkbox

	//FUNCIONES PARA CARGA DE SELECTS DESDE BASE DE DATOS -- SERVICIO WEB

	$scope.loadSelect = function (service, idToLoad, param) {
		$scope.loadingShow();
		var vistaActual = (param !== undefined) ? param : "fake";
		var datae = { 'vista': vistaActual };
		calendarBackEndModel.loadSelect(service, datae).then(function (result) {
			var data = result.d;
			try {
				
				var dataJson = $.parseJSON(data);
				if (dataJson.status === "ok") {
					try {
						$scope[idToLoad] = "";
						$scope[idToLoad] = dataJson.data;
						$scope.loadingHide();
					} catch (exception) {
						$scope.loadingHide();
						message("Error", "Attention", "error");
					}
				} else {
					$scope.loadingHide();
					message("Error", "Attention", "error");
				}
				
			} catch (e) {
				$scope.loadingHide();
				message("Error", "Attention", "error");
				
			}
			return false;
		});
	}

	//FIN -- FUNCIONES PARA CARGA DE SELECTS DESDE BASE DE DATOS -- SERVICIO WEB

	//Adicionar un nuevo evento calendario - Campaña

	$scope.addNewEvent = function () {
		 $('#createEvent').prop('disabled', true);
		$scope.loadingShow();
		var formulario = new Object();
		formulario.campaignName = $scope.campaignName;
		formulario.programName = $scope.programName;
		formulario.businessSegment = $scope.businessSegment;
		formulario.vertical = $scope.verticalesSeleccionados.join();
		formulario.theater = $scope.theater;
		

		if (validarFormulario(formulario)) {
			formulario.tacticName = $scope.tacticName;
			formulario.tacticStage = $scope.tacticStage;
			formulario.ctaOffer = $scope.ctaOffer;
			formulario.dateStart = $("#dateStart").val();
			formulario.dateEnd = $("#dateEnd").val();
			formulario.sfdcCampaignName = $scope.sfdcCampaignName;
			formulario.sfdcDashboardLink = $scope.sfdcDashboardLink;
			
			formulario.subArea = $scope.subArea;
			formulario.languages = $scope.lenguajesSeleccionados.join();
			formulario.businessUnit = $scope.businessUnit;
			formulario.comments = $scope.comments;
			formulario.promocode = $scope.promocode;
			formulario.targetAudience = $scope.targetAudience;
			formulario.agencyInternal = $scope.agencyInternal;
		    formulario.linkLibrary = $scope.linkLibrary;
			

			formulario.status = $scope.status;
			formulario.programInitials = $("#tacticName option:selected").data('icon');
			
			calendarBackEndModel.addNewEvent(formulario).then(function (result) {
				var data = result.d;
				try {
					
					var dataJson = $.parseJSON(data);
					if (dataJson.status === "ok") {
						limpiarFormulario(formulario);
						
						$('#languagesCh :checkbox').each(function () {
							$(this).prop('checked', false);
						});
						$('#statusCh :checkbox').each(function () {
							$(this).prop('checked', false);
						});
						$('#verticalCh :checkbox').each(function () {
							$(this).prop('checked', false);
						});
						$scope.loadingHide();

						$scope.message = {
							content: "Your campaign tactic has now been created and added to the calendar.",
							title: "Thank you!"
						}
						
						$("#messageModal").modal("show");

					} else {
						$('#createEvent').prop('disabled', false);
						$scope.loadingHide();
						if (dataJson.data === "NoSession") {
							document.location.href = "https://www4.avaya.com/mcs/site";
						} else {
							message("Error", "Attention", "danger");
						}
					}
					$('#createEvent').prop('disabled', false);
					
				} catch (e) {
					$('#createEvent').prop('disabled', false);
					$scope.loadingHide();
					message("Error", "Attention", "danger");
				}
			});
		} else {
			 $('#createEvent').prop('disabled', false);
			$scope.loadingHide();
			message("Please, check the required fields", "Alert", "danger");
		}
	}
	//FIN -- Adicionar un nuevo evento calendario - Campaña

	//Adicionar nueva campaña
	$scope.addCampaign = function() {
		$("#addCampaignModal").modal("show");
	};

	$scope.addCampaignSubmit = function () {
		$scope.loadingShow();
		calendarBackEndModel.addNewCampaign($scope.campaignName).then(function (result) {
			var data = result.d;
			try {
				var dataJson = $.parseJSON(data);
				if (dataJson.status === "ok") {
					//message("Content data saved", "Attention", "success");
					$scope.loadingHide();
					$scope.message = {
						title: "Attention",
						content: "Content data saved"
					};
					$("#messageModal").modal("show");
					$("#addCampaignModal").modal("hide");
					$scope.campaignName = "";
					$scope.loadSelect("Serviciosweb/CalendarService.asmx/GetCampaigns", "campaignsList");
				} else {
					$scope.loadingHide();
					if (dataJson.data === "NoSession") {
						document.location.href = "https://www4.avaya.com/mcs/site";
					} else {
						message("Error", "Attention", "danger");
					}
				}
				
			} catch (e) {
				$scope.loadingHide();
				message("Error", "Attention", "danger");
			}
		});
	};
	//FIN - Adicionar nueva campaña

	//Adicionar nueva tactica/program
	$scope.addProgram = function() {
		$("#addProgramModal").modal("show");
		
	};

	$scope.addProgramSubmit = function () {
		$scope.loadingShow();
		calendarBackEndModel.addNewProgram($scope.programName).then(function (result) {
			var data = result.d;
			try {
				var dataJson = $.parseJSON(data);
				if (dataJson.status === "ok") {
					$("#addProgramModal").modal("hide");
					$scope.programName = "";
					$scope.loadSelect("Serviciosweb/CalendarService.asmx/GetPrograms", "programsList");
				} else {
					if (dataJson.data === "NoSession") {
						document.location.href = "https://www4.avaya.com/mcs/site";
					} else {
						message("Error", "Attention", "danger");
					}
				}
				$scope.loadingHide();
			} catch (e) {
				message("Error", "Attention", "danger");
				$scope.loadingHide();
			}
		});

	};
	//FIN - Adicionar nueva tactica/program

});