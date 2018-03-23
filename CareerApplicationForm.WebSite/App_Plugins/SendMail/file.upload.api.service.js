angular.module("umbraco.resources")
.factory("fileUploadService", function ($http) {
    return {
        uploadFileToServer: function (Email) {
            var request = {
                subject: Email.subject,
                CC: Email.CC,
                BCC: Email.BCC,
                file: Email.file,
                body: Email.body
            };
            return $http({
                method: 'POST',
                url: "/umbraco/Surface/SendMail/SendMail",
                headers: { 'Content-Type': undefined },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("subject", data.subject);
                    formData.append("CC", data.CC);
                    formData.append("BCC", data.BCC);
                    formData.append("file", data.file);
                    formData.append("body", data.body);
                    return formData;
                },
                data: request
            }).then(function (response) {
                if (response) {
                    var fileName = response.data;
                    return fileName;
                } else {
                    return false;
                }
            });
        }
    };
});