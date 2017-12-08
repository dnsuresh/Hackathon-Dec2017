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
            alert('failure');
        });
    }
    function getJobDetails(assetid) {
        var url = "http://localhost/VehicleTrackingWebAPI/JobDetails?AssetID=1";
        $http.get(url)
        .then(function (response) {
            $scope.JobDetails = response.data;
        })
        .catch(function (response) {
            alert('failure');
        });
    }
}]);    