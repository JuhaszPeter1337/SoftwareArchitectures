$(function () {
    var login = sessionStorage.getItem("login");
    if(login == true){
        var user = sessionStorage.getItem("user");
        if(user == null){
            window.location.href = "/login.html";
        }
    }
});