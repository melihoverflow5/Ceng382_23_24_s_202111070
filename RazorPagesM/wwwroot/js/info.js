document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('calculateButton').addEventListener('click', function() {
        var firstNum = parseFloat(document.getElementById('firstNumber').value) || 0;
        var secondNum = parseFloat(document.getElementById('secondNumber').value) || 0;
        var result = firstNum + secondNum;
        
        var resultBox = document.getElementById('resultBox');
        resultBox.textContent = 'The result is: ' + result;
        resultBox.style.display = 'block';
    });

    document.getElementById('toggleFormButton').addEventListener('click', function() {
        var toggleFormButton = document.getElementById('toggleFormButton');
        var formContainer = document.getElementById('formContainer');
        var resultBox = document.getElementById('resultBox');
        if (formContainer.style.display === 'none') {
            toggleFormButton.textContent = 'Hide Form';
            formContainer.style.display = 'block';
        } else {
            formContainer.style.display = 'none';
            toggleFormButton.textContent = 'Show Form';
            if (resultBox.style.display === 'block') {
                resultBox.style.display = 'none';
            }
        }
    });

    document.getElementById('toggleInfoButton').addEventListener('click', function() {
        var toggleInfoButton = document.getElementById('toggleInfoButton');
        var infoContainer = document.getElementById('infoContainer');
        if (infoContainer.style.display === 'none') {
            infoContainer.style.display = 'block';
            toggleInfoButton.textContent = 'Hide Information';
        } else {
            infoContainer.style.display = 'none';
            toggleInfoButton.textContent = 'Show Information';
        }
    });
});
