function showDetails(productId) {
    var productDetails = document.getElementById(productId);
    if (productDetails.style.display === "none" || productDetails.style.display === "") {
        productDetails.style.display = "block";
    } else {
        productDetails.style.display = "none";
    }
}
