async function logout() {
    try {
        await connection.invoke("LogoutReq");
        localStorage.removeItem('user');
        localStorage.setItem('login', false);
    } catch (err) {
        console.error(err);
    }
};