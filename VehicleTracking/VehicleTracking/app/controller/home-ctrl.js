app.controller("homeCtrl", ['$scope', '$http', function ($scope, $http, $stateParams, $state) {
    $scope.showModal = false;
    $scope.buttonClicked = "";
    $scope.assets = ['Asset1', 'Asset2'];
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