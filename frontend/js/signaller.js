var app = angular.module("App", []);

app.controller("Controller", ["$scope", function($scope) {
    $scope.init = function() {
        connection = new signalR.HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Debug)
            .withUrl("http://localhost:5000/default", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
            })
            .build();

        async function start() {
            try {
                await connection.start();
                console.log("SignalR Connected.");
            } catch (err) {
                console.log(err);
                setTimeout(start, 5000);
            }
        };

        connection.onclose(start);

        // Start the connection.
        start();

        connection.on("Registered", (success) => {
            if(success)
                window.location.href = "/index.html";
            else
                document.getElementById("error_message").innerHTML="Username already taken";
        });

        connection.on("Login", (user) => {
            if(user != null){
                sessionStorage.setItem("login", true);
                sessionStorage.setItem("user", JSON.stringify(user));
                window.location.href = "/main.html";
            }
        });

        connection.on("LoginFailed", () => {
            
        });

        connection.on("Logout", (success) => {
            if(success){
                sessionStorage.setItem("login", false);
                sessionStorage.removeItem("user");
                window.location.href = "/index.html";
            }
        });

        connection.on("Events", (events) => {
            $scope.events = events;
            $scope.$apply();
        });

        connection.on("Message", (event_id, user, message) => {
            
        });
    }
    $scope.events = [];
}]);