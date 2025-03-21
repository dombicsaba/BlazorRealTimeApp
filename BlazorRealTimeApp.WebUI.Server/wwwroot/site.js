window.getClientId = function () {
    return localStorage.getItem("clientId");
};

window.setClientId = function (clientId) {
    localStorage.setItem("clientId", clientId);
};
