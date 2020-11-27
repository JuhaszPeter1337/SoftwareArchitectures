var app = angular.module("App", []);

app.controller("Controller", ["$scope", function($scope) {
    $scope.init = function() {
        connection = new signalR.HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Debug)
            .withUrl("http://localhost:5000/default", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
            accessTokenFactory: () => localStorage.getItem("token")
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
              this.user = user;
              localStorage.setItem("user", JSON.stringify(user));
              localStorage.setItem("token", user.token)
              window.location.href = "/main.html";
            }
        });

        connection.on("RedirectLogin", (user) => {
          if(user != null){
            this.user = user;
            localStorage.setItem("user", JSON.stringify(user));
            window.location.href = "/main.html";
          }
      });

        connection.on("LoginFailed", () => {
            document.getElementById("error_message_login").innerHTML="Wrong username or password";
        });

        connection.on("Logout", (success) => {
            if(success){
                localStorage.removeItem("user");
                localStorage.removeItem("token");
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
          e = $scope.events.find(ev => ev.event_id == id);
          e.isfavorite = true;
          $scope.hasFavorite = true;
          $scope.$apply();
        });

        connection.on("RemoveFav", (id) => {
          e = $scope.events.find(ev => ev.event_id == id);
          e.isfavorite = false;
          $scope.hasFavorite = false;
          $scope.events.forEach(e => {
              if(e.isfavorite)
                  $scope.hasFavorite = true;
          });
          $scope.$apply();
        });

        connection.on("Message", (id, message) => {
          e = $scope.events.find(ev => ev.event_id == id);
          e.messages.push(message);
          $scope.$apply();
        });

        connection.on("Edit", (success) => {
          if(success)
              window.location.href = "/main.html";
        });

        connection.on("GetUser", (user) => {
          this.user = user;
          localStorage.setItem("user", JSON.stringify(user));
        });

        connection.on("AddEvent", (success) => {
          if(success)
              window.location.href = "/main.html";
        });
    };

    $scope.sendmessage = async function(id) {
        var event_id = id;
        var content = document.getElementById('real-comment-' + id).value;
        try {
          await connection.invoke("SendMessage", event_id, content);
          document.getElementById('real-comment-' + id).value="";
        } catch (err) {
          console.error(err);
        }
      };

      $scope.addfavorite = async function(id){
        var event_id = id;
        try {
          await connection.invoke("AddFavorite", event_id);
        } catch (err) {
          console.error(err);
        }
      };

      $scope.removefavorite = async function(id){
        var event_id = id;
        try {
          await connection.invoke("RemoveFavorite", event_id);
        } catch (err) {
          console.error(err);
        }
      };

    $scope.events = [];
}]);

