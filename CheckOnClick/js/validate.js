
function validate_createAccount() {
    var valid = true;

    if ($('#FullName').val() == '') {
        $('#err_fullname').text('* Please fill the first name');
        valid = false;
    }
    else {
        $('#err_fullname').text('');
    }

    if ($('#PaUserName').val() == '') {
        $('#err_username').text('* Please fill the user name');
        valid = false;
    }
    else {
        $('#err_username').text('');
    }

    if ($('#Gender:checked').val() == '') {
        $('#err_gender').text('* Please Choose a gender');
        valid = false;
    }
    else {
        $('#err_gender').text('');
    }

    if ($('#Contact').val() == '') {
        $('#err_contact').text('* Please fill the contact number');
        valid = false;
    }
    else {
        $('#err_contact').text('');
    }

    if ($('#Email').val() == '') {
        $('#err_email').text('* Please fill the email');
        valid = false;
    }
    else {
        $('#err_email').text('');
    }

    if ($('#PaPassword').val() == '') {
        $('#err_pass').text('* Please fill the password');
        valid = false;
    }
    else {
        $('#err_pass').text('');
    }

    if ($('#ConfirmPassword').val() == '') {
        $('#err_conPass').text('* Please fill the confirm password');
        valid = false;
    }
    else {
        $('#err_conPass').text('');
    }

    if ($('#PaPassword').val() != '' && $('#ConfirmPassword').val() != '' && $('#PaPassword').val() != $('#ConfirmPassword').val()) {
        $('#err_conPass').text('* Password and Confirm Password should be same');
        valid = false;
    }
    else {
        $('#err_conPass').text('');
    }

    return valid;
}