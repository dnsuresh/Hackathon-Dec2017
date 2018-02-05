app.controller("homeCtrl", ['$scope', '$http', function ($scope, $http, $stateParams, $state) {
    $scope.showModal = false;
    $scope.buttonClicked = "";
    getAssets();
    $scope.assetClicked = function () {
        debugger;
        var a = angular.module('trackingApp', []);
        var frameEle = document.getElementById("myIframe").contentWindow;
        frameEle.postMessage('{"command":"assetsAdded","payload":[{"id":"89764","heading":"S","location":{"lat":33.346069,"lon":-111.95575},"text":"TabAFunny","toolTipText":"","colorHex":"#00FF00","hasOrder":false,"voiceEnabled":false,"hasTicket":false,"eventStatusCode":null,"eventColorHex":"#000000","voiceStatus":null}]}', '*');
        frameEle.postMessage('{"command":"assetsModified","payload":[{"id":"89764","heading":"N","location":{"lat":33.346069,"lon":-111.95575},"text":"TabAFunny","toolTipText":"<font size=&#34;+1&#34;><u><b>Asset:</b></u> TabAFunny</font><br><b>Status:</b> 30 Mph N -12/8/2017 10:50:00 AM","colorHex":"#00FF00","hasOrder":false,"voiceEnabled":false,"hasTicket":false,"eventStatusCode":null,"eventColorHex":"#000000","voiceStatus":null}]}', '*');
        frameEle.postMessage('{"command":"zoomToExtent","payload":[{"lat":33.346069,"lon":-111.955749},{"lat":33.346069,"lon":-111.955749}]}', '*');
    };
    $scope.toggleModal = function (asset) {
        $scope.buttonClicked = asset.AssetName;
        $scope.showModal = !$scope.showModal;
        getJobDetails(asset.AssetID);
    };
    function getAssets() {
        var url = "http://localhost/VehicleTrackingWebAPI/Assets";
        $http.get(url)
        .then(function (response) {
            $scope.assets = response.data;
        })
        .catch(function (response) {
            $scope.assets = [
                {
                    "AssetID": 1,
                    "AssetName": "Asset1"
                },
                {
                    "AssetID": 2,
                    "AssetName": "Asset2"
                }
            ];
        });
    }
    function getJobDetails(assetid) {
        var url = "http://localhost/VehicleTrackingWebAPI/JobDetails?AssetID=1";
        $http.get(url)
        .then(function (response) {
            $scope.JobDetails = response.data;
        })
        .catch(function (response) {
            $scope.JobDetails = [
                {
                    "JobID": 1,
                    "JobDescription": "Job Desc 1",
                    "DistaceTravelled": 25,
                    "Duration": 25
                },
                {
                    "JobID": 2,
                    "JobDescription": "Job Desc 2",
                    "DistaceTravelled": 15,
                    "Duration": 15
                },
                {
                    "JobID": 3,
                    "JobDescription": "Job Desc 1",
                    "DistaceTravelled": 18,
                    "Duration": 18
                }
            ];
        });
    }
}]);    