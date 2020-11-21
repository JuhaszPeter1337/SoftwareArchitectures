const connection = new signalR.HubConnectionBuilder()
    .withUrl("/")
    .configureLogging(signalR.LogLevel.Information)
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
        document.getElementById("error_message").text="Username already taken";
});

connection.on("Login", (user) => {
    if(user != null){
        sessionStorage.setItem("login", true);
        sessionStorage.setItem("user", user);
        window.location.href = "/main.html";
    }
});

connection.on("LoginFailed", () => {
    
});

connection.on("Logout", (success) => {
    
});

connection.on("Message", (event_id, user, message) => {
    
});