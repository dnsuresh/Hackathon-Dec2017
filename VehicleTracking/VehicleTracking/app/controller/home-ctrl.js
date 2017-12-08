app.controller("homeCtrl", ['$scope', '$http', function ($scope, $http, $stateParams, $state) {
    $scope.showModal = false;
    $scope.buttonClicked = "";
    getAssets();
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