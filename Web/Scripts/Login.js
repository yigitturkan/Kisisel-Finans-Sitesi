$(document).ready(function () {

    // Hatırlanan mail
    const rememberedEmail = localStorage.getItem('userEmail');
    if (rememberedEmail) {
        $('#Email').val(rememberedEmail);
        $('#RememberMe').prop('checked', true);
    }

    // Submit
    $('form').on('submit', function () {
        const rememberMe = $('#RememberMe').is(':checked');
        const email = $('#Email').val();

        if (rememberMe) {
            localStorage.setItem('userEmail', email);
        } else {
            localStorage.removeItem('userEmail');
        }
    });

});