async function logout() {
    var u = sessionStorage.getItem("user");
    u = u ? JSON.parse(u) : undefined;
    if(u != undefined){
        var uname = u.username;
        try {
            await connection.invoke("LogoutReq", uname);
        } catch (err) {
            console.error(err);
        }
    }
};