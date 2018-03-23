angular.module("umbraco").controller("SendMail",
    function MyImportController($scope, $routeParams, $http, fileUploadService, notificationsService) {

     

    $scope.fileSelected = function (files) {
        // In this case, files is just a single path/filename
        $scope.file = files;
    };
  
    $scope.myProperty = {
        label: 'bodyText',
        description: 'Load some stuff here',
        view: 'rte',
        config: {
            editor: {
                toolbar: ["code", "undo", "redo", "cut", "styleselect", "bold", "italic", "alignleft", "aligncenter",
                    "alignright", "bullist", "numlist", "link", "table"],
                stylesheets: [],
                dimensions: { height: 250, width: 400 }
            }
        }
    };

    $scope.submit = function (model) {
        alert(model.name);
    };
    $scope.uploadFile = function () {

     
        if ($scope.Email.subject =="") {
            notificationsService.error("Error", "subject cant be Empty!");
            return;
        }

        if ($scope.myProperty.value == null || $scope.myProperty.value.match(/^ *$/) != null || $scope.myProperty.value==undefined) {
            notificationsService.error("Error", "Email Body cant be Empty!");
            return;
        }
        $scope.Email.body = $scope.myProperty.value;

        if ($scope.Email.CC != '')
            if (!$scope.Email.CC.match('^([\\w+-.%]+@[\\w-.]+\\.[A-Za-z]{2,4},?)+$'))
        {
            notificationsService.error("Error", "Wrong CC Format");
            return;
        }

        if ($scope.Email.BCC != '')
            if (!$scope.Email.BCC.match('^([\\w+-.%]+@[\\w-.]+\\.[A-Za-z]{2,4},?)+$'))
            {
                notificationsService.error("Error", "Wrong BCC Format");
                return;
            }
               
        
        //^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4},?)+$


        if (!$scope.isUploading) {
            {
                notificationsService.success("Success", "Sending...")
                $scope.isUploading = true;
                fileUploadService.uploadFileToServer($scope.Email)
                    .then(function (response) {
                        if (response) {
                            notificationsService.success("Success", "Email Has been Sent!");
                        }
                        $scope.isUploading = false;
                    }, function (reason) {
                        notificationsService.error("Error", "Error Sending Email.");
                        $scope.isUploading = false;
                    });
            } 
        }
    };
    $scope.Email =
        {
            subject: '',
            CC: '',
            BCC: '',
            file: false,
            body: ''
        }
    $scope.isUploading = false;
});