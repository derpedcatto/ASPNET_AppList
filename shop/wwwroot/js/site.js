document.addEventListener("DOMContentLoaded", () => {
    for (let element of document.querySelectorAll("[data-rate-value]")) {
        element.addEventListener('click', rateClick);
    }
});

function rateClick(e) {
    const div = e.target.closest('div');
    const userId = div.getAttribute('data-user-id');
    /*
    if (typeof userId == 'undefined' || userId.length == 0) {
        alert("Для оцінювання слід автентифікуватись");
        return;
    }
    */
    const productId = div.getAttribute('data-product-id');
    const rate = e.target.getAttribute('data-rate-value');

    console.log(userId, productId, rate);

    window.fetch("/api/rate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            productId,
            // userId,
            'rating': rate
        })
    })
        .then(r => r.json())
        .then(j => {
            console.log(j);
        });
}