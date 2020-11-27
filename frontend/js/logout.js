async function logout() {
    try {
        await connection.invoke("LogoutReq");
        localStorage.removeItem('user');
    } catch (err) {
        console.error(err);
    }
};