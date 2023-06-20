document.addEventListener("DOMContentLoaded", () => {
    const signInButton = document.getElementById("signin-button");
    if (signInButton) signInButton.addEventListener('click', signInButtonClick);

    for (let element of document.querySelectorAll("[data-rate-value]")) {
        element.addEventListener('click', rateClick);
    }
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

function rateClick(e) {
    const div = e.target.closest('div');
    const userId = div.getAttribute('data-user-id');
    if (typeof userId == 'undefined' || userId.length == 0) {
        alert("Для оцінювання слід автентифікуватись");
        return;
    }
    const productId = div.getAttribute('data-product-id');
    const rate = e.target.getAttribute('data-rate-value');

    console.log(userId, productId, rate);

    window.fetch("/api/rate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            productId,
            userId,
            'rating': rate
        })
    })
        .then(r => r.json())
        .then(j => {
            console.log(j);
            if (j.status == true) {
                const parentDiv = e.target.closest("[data-product-id]");
                /*
                const element = rate > 0
                    ? parentDiv.querySelector(".rate-positive")
                    : parentDiv.querySelector(".rate-negative");
                element.innerText = Number(element.innerText) + 1;
                */
                parentDiv.querySelector(".rate-positive").innerText = j.positive;
                parentDiv.querySelector(".rate-negative").innerText = j.negative;
                parentDiv.querySelector(".rate-total").innerText = +j.negative + j.positive;
            }
        });
}