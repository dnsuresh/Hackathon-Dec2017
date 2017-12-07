app.controller("homeCtrl", ['$scope', '$http', function ($scope, $http, $stateParams, $state) {
    $scope.showModal = false;
    $scope.buttonClicked = "";
    $scope.assets = [
        'Asset1', 'Asset2', 'Asset3', 'Asset4', 'Asset5', 'Asset6', 'Asset7', 'Asset8', 'Asset9', 'Asset10', 'Asset11', 'Asset12', 'Asset13', 'Asset14', 'Asset15', 'Asset16', 'Asset17', 'Asset18', 'Asset19', 'Asset20', 'Asset21', 'Asset22'
    ];
    $scope.toggleModal = function (btnClicked) {
        $scope.buttonClicked = btnClicked;
        $scope.showModal = !$scope.showModal;
    };
    $scope.JobDetails = [
        {
            JobID:1,
            JobDescrption: "JobDescrption1",
            Distancetravelled: "25Kms",
            Duration:"25"
        },
        {
            JobID: 2,
            JobDescrption: "JobDescrption2",
            Distancetravelled: "15Kms",
            Duration: "15"
        },
        {
            JobID: 3,
            JobDescrption: "JobDescrption3",
            Distancetravelled: "10Kms",
            Duration: "10"
        },
        {
            JobID: 4,
            JobDescrption: "JobDescrption4",
            Distancetravelled: "9Kms",
            Duration: "9"
        }
    ];
}]);    