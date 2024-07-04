$(document).ready(function () {
    $('#ChooseImg').change(function (e) {
        var url = $('#ChooseImg').val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (ChooseImg.files && ChooseImg.files[0] && (ext == "gif" || ext == "jpg" || ext == "jfif" || ext == "png" || ext == "bmp")) {
            var reader = new FileReader();
            reader.onload = function () {
                var output = document.getElementById('PrevImg');
                output.src = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
        }

    });
});

function validateForm() {
    // Reset highlighting on all fields before performing validation
    var allInputs = document.querySelectorAll('input, select');
    allInputs.forEach(function (input) {
        input.classList.remove('highlight-input');
        input.removeAttribute('placeholder'); // Remove any existing placeholder
    });

    var fieldsToHighlight = []; // Array to store fields that need attention

    // Check if BookName is not provided
    var bookNameInput = document.getElementById("BookName");
    if (!bookNameInput.value.trim()) {
        fieldsToHighlight.push("Book Name");
        bookNameInput.classList.add('highlight-input');
        bookNameInput.placeholder = "This field is required"; // Set placeholder
    }

    // Check if BookType is not selected
    var bookTypeSelect = document.getElementById("BookTypeId");
    if (!bookTypeSelect.value.trim()) {
        fieldsToHighlight.push("Book Type");
        bookTypeSelect.classList.add('highlight-input');
        bookTypeSelect.placeholder = "Please select a book type"; // Set placeholder
    }

    // Check if the number of stock is not provided
    var numStockInput = document.getElementById("numStock");
    if (!numStockInput.value.trim()) {
        fieldsToHighlight.push("Number of Stock");
        numStockInput.classList.add('highlight-input');
        numStockInput.placeholder = "Please enter the number of stock"; // Set placeholder
    }

    // Continue with other validations...

    if (fieldsToHighlight.length > 0) {
        // Show an alert with the list of fields that need attention
        Swal.fire({
            icon: 'warning',
            title: 'Missing Required Fields',
            html: 'The following fields need attention:<br>' + fieldsToHighlight.join(', '),
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });
        return false; // Prevent form submission
    }

    return true; // Allow form submission
}


