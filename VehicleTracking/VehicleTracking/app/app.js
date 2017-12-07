var app = angular.module("trackingApp", ["ui.router"]);
app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/');
    $stateProvider
    .state('home', {
        url: "/",
        templateUrl: "app/view/home.html"
    })
});