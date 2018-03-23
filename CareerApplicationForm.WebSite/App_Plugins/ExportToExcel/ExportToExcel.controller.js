var duplicateNodeCntrl = angular.module("umbraco");
duplicateNodeCntrl.controller("Export", function ($scope, $filter, $http, $routeParams, editorState, contentResource, notificationsService, navigationService, appState, $timeout) {
    var selectedNode = appState.getMenuState("currentNode");
  


    $scope.Export = function () {

        $http({
            method: "POST",
            url: "/umbraco/surface/ExportToExcel/Export"
        }).then(function mySuccess(response) {
            notificationsService.success("Nodes Exported", "");
        }, function myError(response) {
            notificationsService.error("Error Exporting Content", "");
        });

    }
});





//notificationsService.success("Nodes duplicated", "Your new nodes are ready, you may have to refresh your browser.");
//                } else {
//    recursiveCopy(remain, parent, copynode)
//}
//            }, function (err) {
//    notificationsService.error("Error duplicating nodes", "There has been a problem duplicating your nodes.");
