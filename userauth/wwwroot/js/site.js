document.addEventListener("DOMContentLoaded", () => {
    const signInButton = document.getElementById("signin-button");
    if (signInButton) signInButton.addEventListener('click', signInButtonClick);
});

function signInButtonClick() {
    const userLoginInput = document.getElementById("signin-login");
    const userPasswordInput = document.getElementById("signin-password");
    if (!userLoginInput) throw "Елемент не знайдено: signin-login";
    if (!userPasswordInput) throw "Елемент не знайдено: signin-password";



    const userLogin = userLoginInput.value;
    const userPassword = userPasswordInput.value;
    if (userLogin.length === 0) {
        alert("Введіть логін");
        return;
    }
    if (userPassword.length === 0) {
        alert("Введіть пароль");
        return;
    }

    const data = new FormData();
    data.append("login", userLogin);
    data.append("password", userPassword);
    fetch("User/LogIn", { method: "POST", body: data })
        .then(r => r.json())
        .then(j => {
            console.log(j)

            if (typeof j.status != 'undefined') {
                if (j.status == 'OK') {
                    window.location.reload() // Оновлюємо сторінку як для автентифікованого користувача
                }
                else if (j.status == 'NO') {
                    const errorModal = new bootstrap.Modal(document.getElementById("loginAuthFailedModal"));
                    errorModal.show();
                }
            }
        });
}