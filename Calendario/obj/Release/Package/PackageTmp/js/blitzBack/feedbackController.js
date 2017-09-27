//*   CONTROLADOR LINKS   *//
var oTableTl;

calendarBackEndApp.controller('feedbackController', function ($scope, calendarBackEndModel, $route, $routeParams, $location) {
    
    $scope.mostrarListaFeedbacks = function (result) {
        try {
            oTableTl = $('#feedbackDt').DataTable({
                "aLengthMenu": [[15, 30, 60, 120, -1], [15, 30, 60, 120, "All"]],
                "bProcessing": true,
                responsive: true,
                "autoWidth": false,
                "aaData": result,
                "aoColumns": [{ "mDataProp": "feedbackId" }, { "mDataProp": "feebackNumber" },{ "mDataProp": "feedbackObservations" }, { "mDataProp": "feedbackDate" }],
                "aaSorting": [[0, "asc"]]
            });

            $('#feedbackDt tbody').on('click', 'tr', function () {
                oTableTl.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');

            });

            $('#feedbackDt tbody').on('click', 'button', function () {
                oTableTl.$('tr.selected').removeClass('selected');
                $(this).parents("tr").addClass("selected");
            });
        } catch (exception) {
            message("Error", "Attention", "error");
        }
        return false;
    };


    $scope.getFeedbacks = function () {
        calendarBackEndModel.getFeedbacks().then(function (result) {
            var data = result.d;
            try {
                var dataJson = $.parseJSON(data);
                if (dataJson.status === "ok") {
                    oTableTl = $('#feedbackDt').DataTable();
                    oTableTl.destroy();
                    try {
                        $scope.mostrarListaFeedbacks(dataJson.data);
                        $scope.datosUsers = dataJson.data;
                    } catch (exception) {

                        message("Error", "Attention", "error");
                    }
                } else {
                    if (dataJson.data === "NoSession") {
                        document.location.href = "index.html?BackPageSession=NoSession";
                    } else {
                        message("Error", "Attention", "error");
                    }
                }
            } catch (e) {
                message("Error", "Attention", "error");
            }
        });
    };

    function init() {
        $scope.getFeedbacks();
    }

    init();

    $scope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase === '$apply' || phase === '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
    
    $scope.doExport = function (params) {
        var options = {
            tableName: 'Campaigns',
            worksheetName: 'Campaigns'
        };
        $.extend(true, options, params);
        $('#feedbackDt').tableExport(options);

    }

    $scope.toExcel = function () {
        $scope.doExport({ type: 'excel' });
    }


});

