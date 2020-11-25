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
            document.getElementById("error_message_login").innerHTML="Wrong username or password";
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
            $scope.hasFavorite = false;
            $scope.events.forEach(e => {
                if(e.isfavorite)
                    $scope.hasFavorite = true;
            });
            $scope.$apply();
        });

        connection.on("AddFav", (id) => {
            $scope.events[id].isfavorite = true;
            $scope.hasFavorite = true;
            $scope.$apply();
        });

        connection.on("RemoveFav", (id) => {
            $scope.events[id].isfavorite = false;
            $scope.hasFavorite = false;
            $scope.events.forEach(e => {
                if(e.isfavorite)
                    $scope.hasFavorite = true;
            });
            $scope.$apply();
        });

        connection.on("Message", (event_id, message) => {
            $scope.events[event_id].messages.push(message);
            $scope.$apply();
        });

        connection.on("Edit", (success) => {
          if(success)
              window.location.href = "/main.html";
        });

        connection.on("AddEvent", (success) => {
          if(success)
              window.location.href = "/main.html";
        });
    };

    $scope.sendmessage = async function(id) {
        var login = sessionStorage.getItem("login");
        if(login){
          var u = sessionStorage.getItem("user");
          u = u ? JSON.parse(u) : undefined;
          var uname = u.username;
          var event_id = id;
          var content = document.getElementById('real-comment-' + id).value;
          var message = {username: uname, content: content};
          try {
            await connection.invoke("SendMessage", event_id, message);
            document.getElementById('real-comment-' + id).value="";
          } catch (err) {
            console.error(err);
          }
        }
      };

      $scope.addfavorite = async function(id){
        var login = sessionStorage.getItem("login");
        if(login){
          var u = sessionStorage.getItem("user");
          u = u ? JSON.parse(u) : undefined;
          var uname = u.username;
          var event_id = id;
          try {
            await connection.invoke("AddFavorite", event_id, uname);
          } catch (err) {
            console.error(err);
          }
        }
      };

      $scope.removefavorite = async function(id){
        var login = sessionStorage.getItem("login");
        if(login){
          var u = sessionStorage.getItem("user");
          u = u ? JSON.parse(u) : undefined;
          var uname = u.username;
          var event_id = id;
          try {
            await connection.invoke("RemoveFavorite", event_id, uname);
          } catch (err) {
            console.error(err);
          }
        }
      };

    $scope.events = [];
    $scope.favorites = [];
}]);