var button = document.querySelector(".buttonPurchase");
button.addEventListener("click", function (e) {
    var dataToSend = localStorage.getItem('shoppingCart');

    $.ajax({
        type: 'POST',
        url: '/Purchase/Purchase',
        data: { key: dataToSend },
        success: function(response) {
            // Handle success
            console.log("all good");
        },
        error: function(xhr, status, error) {
            // Handle error
        }
    });
});
