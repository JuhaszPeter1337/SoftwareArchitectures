$(async function () {
    var login = sessionStorage.getItem("login");
    if(login){
        var user = sessionStorage.getItem("user");
        try {
            await connection.invoke("GetEvents", user.username);
          } catch (err) {
            console.error(err);
          }
    }
    else{
        window.location.href = "/index.html";
    }
});