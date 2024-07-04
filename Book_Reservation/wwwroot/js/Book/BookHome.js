
    // Get references to the input fields
    var minPriceInput = document.getElementById("minPrice");
    var maxPriceInput = document.getElementById("maxPrice");

    // Attach an event listener to the input fields
    minPriceInput.addEventListener("input", validatePriceInput);
    maxPriceInput.addEventListener("input", validatePriceInput);

    // Function to validate the input fields
    function validatePriceInput() {
        var minPrice = parseFloat(minPriceInput.value);
    var maxPrice = parseFloat(maxPriceInput.value);

        if (!isNaN(minPrice) && !isNaN(maxPrice) && minPrice > maxPrice) {
        maxPriceInput.setCustomValidity("Max price should be greater than min price.");
        } else {
        maxPriceInput.setCustomValidity("");
        }
    }