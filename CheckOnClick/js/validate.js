
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


var uname2 = localStorage.getItem("Lusername");
if (uname2 != undefined && uname2 != null) {
    $('#UserName').val(uname2);
}

function validateLogin() {
    var value = true;
    if ($('#UserName').val() == '' || $('#Password').val() == '') {
        value = false;
    }
    if (value == false) {
        $('#err_login').text('* Please fill all the details !');
    }
    else {
        var uname = $('#UserName').val();
        localStorage.setItem("Lusername", uname);
    }
    return value;
}

function validateForgetPassword() {
    var value = true;
    if ($('#Email').val() == '' || $('#Password1'.val() == '') || $('#Password2'.val() == '')) {
        value = false;
    }
    if ($('#Email').val() != '' && !isEmail($('#Email').val())) {
        value = false;
    }

}

//       $('#btnSignup').click(function () {
$("#btnSignup").click(function () {

    if (validate_createAccount()) {
        var data = {
            "FullName": $('#FullName').val(),
            "Email": $('#Email').val(),
            "Gender": $('#Gender:checked').val(),
            "Contact": $('#Contact').val(),
            "PaPassword": $('#PaPassword').val()
        };

        $.ajax({
            type: "POST",
            url: "/Home/CreateAccount",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (result) {
                if (result == "Saved") {
                    //alert('Account Successfully Created! Please Login to start !');
                    swal('Success', 'Account Successfully Created! Please Login to start !', 'success')
                        .then((ok) => {
                            location.reload(true);
                        });
                }
                else if (result == "Email") {
                    //alert('Email has already registered, Please use another email');
                    swal('Error', 'Email has already registered, Please use another email !', 'error');
                }
                else if (result == "Contact") {
                    //alert('Contact Number has already registered, Please use another contact number');
                    swal('Error', 'Contact Number has already registered, Please use another contact number !', 'error');
                }
                else if (result == "UserName") {
                    //alert('Username has already taken by someone, Please use a different username');
                    swal('Error', 'Username has already taken by someone, Please use a different username !', 'error');
                }

                //location.reload(true);
            },
            failure: function (result) {
                //alert('Something went wrong: failure');
                swal('Error', 'Something went wrong: failure !', 'error')
                    .then((ok) => {
                        location.reload(true);
                    });
                //location.reload(true);
            },
            error: function (result) {
                //alert('Something went wrong : error');
                swal('Error', 'Something went wrong : error !', 'error')
                    .then((ok) => {
                        location.reload(true);
                    });
                //location.reload(true);
            }


        });
    }
    else {
        return false;
    }


});

$('#Changepassword').click(function () {
    var valid = true;
    var pass = 0;
    if ($('#Email1').val() == "") {
        valid = false;
        //swal("warning", "Please fill Fields", "warning");
        $('#err_forgetPass').text('* Please provide your registered email !');
    }
    else if (!isEmail($('#Email1').val())) {
        valid = false;
        //swal("warning", "Email is invalid", "warning");
        $('#err_forgetPass').text('* Email is invalid !');
    }
    else {
        $('#err_changePass').html('<i class="fa fa-spinner fa-spin" style="font-size:20px"></i> Sending details in the email ...');
        var updatepassword = {
            "Email": $('#Email1').val(),


        };

        $.post('/Home/ChangePassword', updatepassword, function (result) {
            if (result == 'saved') {

                //    swal("Success", "Details Sended to your Registerd Email Id !", "success");
                swal({
                    title: "Success",
                    text: "Details Sent to your Registerd Email Id !",
                    type: "success",
                    icon: "success",
                    button: "OK"
                }).then((value) => {
                    location.reload(true);
                });


            }
            else if (result == 'misMatched') {

                //    swal("Error", "Email is not Found, Please try again !", "error");
                swal({
                    title: "Error",
                    text: "Email is not Found, Please try again !",
                    type: "error",
                    icon: "error",
                    button: "OK"
                }).then((value) => {
                    $('#err_changePass').html('');
                });
            }
            else {

                //   swal("Error", "Something went wrong, Please try again", "error");
                swal({
                    title: "Error",
                    text: "Something went wrong, Please try again !",
                    type: "error",
                    icon: "error",
                    button: "OK"
                }).then((value) => {
                    $('#err_changePass').html('');
                });
            }
        });
    }
});


//sending data for query done by guest to the query table in database

$('#sendData').click(function () {
    var valid = true;
    if ($('#name').val() == "") {
        valid = false;
    }
    if ($('#email').val() == "") {
        valid = false;
    }
    if ($('#contactNumber').val() == "") {
        valid = false;
    }
    if ($('#message').val() == "") {
        valid = false;
    }
    if (valid == false) {
        $('#err_span').text('* Please fill all the fields !');
    }
    else if (!isEmail($('#email').val()) || !checkNumber($('#contactNumber').val())) {
        $('#err_span').text('* Email or Contact number is invalid !');
    }
    else {
        $('#err_span').html('<i class="fa fa-spinner fa-spin" style="font-size:20px"></i> Sending query ...');
        var guestQuery = {
            "name": $('#name').val(),
            "email": $('#email').val(),
            "contactNumber": $('#contactNumber').val(),
            "message": $('#message').val()
        };

        //alert($('#name').val());

        //$.ajax({
        //    type: "post",
        //    dataType: "json",
        //    url: '/Home/sendGuestQuery/?name=' + $('#name').val() + '?email=' + $('#email').val() + '?contactNumber=' + $('#contactNumber').val() + '?message=' + $('#message').val(),
        //})
        $.post('/Home/sendGuestQuery', guestQuery, function (result) {
            if (result == 'saved') {
                swal("Success", "Query successfully sent !", "success")
                    .then((value) => {
                        location.reload(true);
                    });
            }
            else if (result == 'notSaved') {
                swal("Error", "Problem sending the query, Please try again !", "error");
            }
            else {
                swal("Error", "Something went wrong, Please try again !", "error");
            }
        });
    }
});

function openModal() {
    $('#loginModal').modal('hide');
    $('#forgetpassword').modal('show');
}

$('#btnBackLogin').click(function () {
    $('#forgetpassword').modal('hide');
    $('#loginModal').modal('show');
});

// Book Appointments

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function checkNumber(phNum) {
    var pattern = new RegExp(/^[6789]\d{9}$/);
    return pattern.test(phNum);
}


$('#contd-option').hide();
$('#btnProceedGuest').hide();
$('#backAppoint').hide();
$('#form-guest').hide();
$('#backMenu').hide();
$('#form-signin').hide();
$('#btnProceedUser').hide();
$('#btnVerifyOTP').hide();
$('#form-otp').hide();

var tomorrow = new Date();
tomorrow.setDate(tomorrow.getDate() + 1);
var dd = tomorrow.getDate();
var mm = tomorrow.getMonth() + 1;
var yyyy = tomorrow.getFullYear();

if (mm < 10) {
    mm = '0' + mm;
}
if (dd < 10) {
    dd = '0' + dd;
}

var minDate = yyyy + "-" + mm + "-" + dd;

$('#selectDate').attr('min', minDate);

$('#selectDate').on('change', function () {
    if ($('#selectDate').val() != "") {
        $('#Doc_Spec_Id').removeAttr('disabled');
    }
});

$('#Doc_Spec_Id').on('change', function () {
    if ($('#Doc_Spec_Id option:selected').val() != null) {
        $('#selectDoc').removeAttr('disabled');
    }

    var spec_id = $('#Doc_Spec_Id option:selected').val();

    $.get("/Patient/LoadDoctor", { Spec_Id: spec_id }, function (data) {
        var opt = '<option>-- Select Doctor --</option>';
        $.each(data, function (i, v1) {
            opt += '<option value="' + v1.Value + '">' + v1.Text + '</option>';
        });

        $('#selectDoc').html(opt);
    });
});

$('#selectDoc').on('change', function () {
    var doc_id = $('#selectDoc option:selected').val();
    var slot_id;
    var time_from;
    var time_to;
    var disableTimeRanges;

    var apointDate = $('#selectDate').val();
    var bookData = {
        "AppointDate": apointDate,
        "Doc_Id": doc_id
    };

    $.get('/Patient/BookedSlots', bookData, function (result) {
        if (result != null) {
            var bookedResponse = "";
            $.each(result, function (i, v1) {
                bookedResponse += v1.AppointTime + " ";
            });

            if (bookedResponse == "") {
                $('#bookSlot').text('All slots are free.');
            }
            else {
                $('#bookSlot').text('Some slots are already booked - ' + bookedResponse);
            }
        }
    });



    $.get('/Patient/LoadSlots', { Doc_id: doc_id }, function (data) {
        var opt = '<option>-- Select Slot --</option>';
        $.each(data, function (i, v1) {
            slot_id = v1.Slot_Id;
            time_from = v1.Slot_From;
            time_to = v1.Slot_To;
            //        disableTimeRanges = "[['9:00am', '9:21am'], ['10:20am', '10:41am']]";
        });

        if (time_from != null && time_to != null) {
            $('#selectSlot').timepicker({
                timeFormat: 'hh:mm p',
                minTime: time_from,
                maxTime: time_to,
                interval: 20,
                //            disableTimeRanges: [['09:00 AM', '09:21 AM'], ['10:20 AM', '10:41 AM']],
                //            dropdown: true,
                scrollbar: true,

                change: function (time) {
                    var slot = $(this).val();

                    // Check whether the time is already book or not
                    var bookData = {
                        "AppointDate": $('#selectDate').val(),
                        "Doc_Id": $('#selectDoc option:selected').val()
                    };

                    $.get('/Patient/BookedSlots', bookData, function (result) {
                        if (result != null) {
                            var valid = true;
                            $.each(result, function (i, v1) {
                                if (v1.AppointTime == slot) {
                                    //       alert('The selected slot has already booked, Please select another slot.');
                                    swal("Warning", "The selected slot has already booked, Please select another slot", "warning");
                                    $('#btnBook').attr('disabled', true);
                                    valid = false;
                                }

                            });

                            if (valid == true) {
                                $('#btnBook').removeAttr('disabled');
                            }
                        }
                    });

                }
            });


        }
    });


});

$('#btnBook').click(function () {
    var valid = true;
    if ($('#selectDate').val() == "") {
        valid = false;
    }
    if ($('#Doc_Spec_Id option:selected').val() == null) {
        valid = false;
    }
    if ($('#selectDoc option:selected').val == null) {
        valid = false;
    }
    if ($('#selectSlot').val() == "") {
        valid = false;
    }
    if ($('#patientDesc').val() == "") {
        valid = false;
    }

    if (valid == false) {
        $('#err').text('* Please fill all the fields !');
    }
    else {
        $('#err').text('');

        var data = {
            "AppointDate": $('#selectDate').val(),
            "SpecId": $('#Doc_Spec_Id option:selected').val(),
            "DocId": $('#selectDoc option:selected').val(),
            "AppointSlot": $('#selectSlot').val(),
            "PatientDesc": $('#patientDesc').val()
        };


        $('#bk-appoint').hide('slow');
        $('#contd-option').show('slow');

        $('#btnBook').hide('slow');
        //    $('#btnProceed').show('slow');

        $('#backHome').hide('slow');
        $('#backAppoint').show('slow');
        $('#bookClose').hide('slow');
    }


});

$('#backAppoint').click(function () {
    $('#contd-option').hide('slow');
    $('#bk-appoint').show('slow');

    $('#btnProceedGuest').hide('slow');
    $('#btnProceedUser').hide('slow');
    $('#btnBook').show('slow');

    $('#backAppoint').hide('slow');
    $('#bookClose').show('slow');
});

$('input[name="user"]').on('change', function () {
    if ($('input[name="user"]:checked').val() == "guest") {
        $('#contd-option').hide('slow');
        $('#form-signin').hide('slow');
        $('#form-guest').show('slow');
        $('#backAppoint').hide('slow');
        $('#backMenu').show('slow');
        $('#btnProceedUser').hide('slow');
        $('#btnProceedGuest').show('slow');
    }

    if ($('input[name="user"]:checked').val() == "sign") {
        $('#contd-option').hide('slow');
        $('#form-guest').hide('slow');
        $('#form-signin').show('slow');
        $('#backAppoint').hide('slow');
        $('#backMenu').show('slow');
        $('#form-guest').hide('slow');
        $('#btnProceedUser').show('slow');
    }
});

$('#backMenu').click(function () {
    $('#form-guest').hide('slow');
    $('#form-signin').hide('slow');
    $('#contd-option').show('slow');

    $('#backMenu').hide('slow');
    $('#backAppoint').show('slow');

    $('#btnProceedGuest').hide('slow');
    $('#btnProceedUser').hide('slow');

    $('#form-otp').hide('slow');


});

var userType = 0;

$('#btnProceedGuest').click(function () {
    var valid = true;
    if ($('#Gname').val() == "") {
        valid = false;
    }
    if ($('#Gemail').val() == "") {
        valid = false;
    }
    if ($('#Gcontact').val() == "") {
        valid = false;
    }
    if ($('input[name="Ggender"]:checked').val() == null) {
        valid = false;
    }
    if (($('#Gemail').val() != '' && !isEmail($('#Gemail').val())) || ($('#Gcontact').val() != '' && !checkNumber($('#Gcontact').val()))) {
        valid = false;
        $('#err_guest').text('* Email or Contact number is Invalid !');
    }
    else {
        $('#err_guest').text('* Please fill all the details !');
    }

    if (valid == true) {
        $('#err_guest').html('<i class="fa fa-spinner fa-spin" style="font-size:20px"></i> Sending OTP to the given email and the contact number ...');
        var verifyData = {
            "Email": $('#Gemail').val(),
            "Contact": $('#Gcontact').val()
        };

        userType = 1;

        $.post('/Home/SendOneTimePassword', verifyData, function (result) {
            if (result == 'wrong') {
                swal('Warning', 'Email does not exist !', 'warning');
            }
            else if (result == 'sent') {
                $('#form-guest').hide('slow');
                $('#backMenu').hide('slow');
                $('#form-otp').show('slow');
                $('#btnProceedGuest').hide('slow');
                $('#btnVerifyOTP').show('slow');
            }
            else if (result == "email") {
                swal('Warning', 'Email already exists, Please continue to your account !', 'warning')
                    .then((value) => {
                        location.reload(true);
                    });
            }
            else if (result == "contact") {
                swal('Warning', 'Contact Number already exists, Please continue to your account !', 'warning')
                    .then((value) => {
                        location.reload(true);
                    });
            }
            else {
                swal('Error', 'Something went wrong, Please try again !', 'error');
            }
        });
    }
});


$('#btnProceedUser').click(function () {
    var valid = true;
    if ($('#Uname').val() == "") {
        valid = false;
    }
    if ($('#Uemail').val() == "") {
        valid = false;
    }
    if ($('#Ucontact').val() == null) {
        valid = false;
    }
    if ($('input[name="Ugender"]:checked').val() == null) {
        valid = false;
    }
    if ($('#UPassword').val() == null) {
        valid = false;
    }
    if ($('#UCPass').val() == null) {
        valid = false;
    }
    if (($('#Uemail').val() != '' && !isEmail($('#Uemail').val())) || ($('#Ucontact').val() != '' && !checkNumber($('#Ucontact').val()))) {
        valid = false;
        $('#err_user').text('* Email or Contact number is Invalid !');
    }

    else if ($('#UPassword').val() != null && $('#UCPass').val() != null && $('#UPassword').val() != $('#UCPass').val()) {
        //    valid = false;
        $('#err_user').text('* password and confirm password should be same !');

    }
    else {
        $('#err_user').text('* Please fill all the details !');
    }

    if (valid == true) {
        $('#err_user').html('<i class="fa fa-spinner fa-spin" style="font-size:20px"></i> Sending OTP to the given email and the contact number ...');
        var verifyData = {
            "Email": $('#Uemail').val(),
            "Contact": $('#Ucontact').val()
        };

        userType = 2;

        $.post('/Home/SendOneTimePassword', verifyData, function (result) {
            if (result == 'sent') {
                $('#form-signin').hide('slow');
                $('#backMenu').hide('slow');
                $('#form-otp').show('slow');
                $('#btnProceedUser').hide('slow');
                $('#btnVerifyOTP').show('slow');
            }
            else if (result == "email") {
                swal('Warning', 'Email already exists, Please continue to your account !', 'warning')
                    .then((value) => {
                        location.reload(true);
                    });
            }
            else if (result == "contact") {
                swal('Warning', 'Contact Number already exists, Please continue to your account !', 'warning')
                    .then((value) => {
                        location.reload(true);
                    });
            }
            else {
                swal('Error', 'Something went wrong, Please try again !', 'error');
            }
        });
    }
});

$('#btnVerifyOTP').click(function () {
    if ($('#OTP_field').val() == "") {
        $('#err_otp').text('* Please enter One Time Password to proceed !');
    }
    else {
        $('#err_otp').html('<i class="fa fa-spinner fa-spin" style="font-size:20px"></i> Validating the data ...');
        if (userType == 1) {
            var data1 = {
                "AppointDate": $('#selectDate').val(),
                "SpecId": $('#Doc_Spec_Id option:selected').val(),
                "DocId": $('#selectDoc option:selected').val(),
                "AppointSlot": $('#selectSlot').val(),
                "PatientDesc": $('#patientDesc').val(),

                "NewName": $('#Gname').val(),
                "NewEmail": $('#Gemail').val(),
                "NewContact": $('#Gcontact').val(),
                "NewGender": $('input[name=Ggender]:checked').val(),
                "NewPass": "123456",

                "UserOTP": $('#OTP_field').val()
            };

            $.post('/Home/SaveNewAppointment', data1, function (result) {
                if (result == "NotMatched") {
                    swal('Error', 'OTP is not matched, Please try again !', 'error');
                    $('#err_otp').html('');
                }
                else if (result == "booked") {
                    swal('Success', 'Appointment successfully booked ! You can use your contact number as UserName and Password 123456', 'success')
                        .then((value) => {
                            //Session["PatientUserName"] = $('#Ucontact').val();
                            //location.href = '/Patient/AppointHistory/';
                            location.reload(true);
                        });
                    //Session["PatientUserName"] = $('#Gcontact').val();
                    
                }
                else if (result == "NotBooked") {
                    swal('Error', 'Problem booking the appointment, Please try again !', 'error');
                    $('#err_otp').html('');
                }
                else {
                    swal('Error', 'Something went wrong, Please try again !', 'error');
                    $('#err_otp').html('');
                }
            });
        }
        else if (userType == 2) {
            var data2 = {
                "AppointDate": $('#selectDate').val(),
                "SpecId": $('#Doc_Spec_Id option:selected').val(),
                "DocId": $('#selectDoc option:selected').val(),
                "AppointSlot": $('#selectSlot').val(),
                "PatientDesc": $('#patientDesc').val(),

                "NewName": $('#Uname').val(),
                "NewEmail": $('#Uemail').val(),
                "NewContact": $('#Ucontact').val(),
                "NewGender": $('input[name=Ugender]:checked').val(),
                "NewPass": $('#Upassword').val(),

                "UserOTP": $('#OTP_field').val()
            };
            $.post('/Home/SaveNewAppointment', data2, function (result) {
                if (result == "NotMatched") {
                    swal('Error', 'OTP is not matched, Please try again !', 'error');
                    $('#err_otp').html('');
                }
                else if (result == "booked") {
                    swal('Success', 'Appointment successfully booked ! You can use your contact number as UserName with your password', 'success')
                        .then((value) => {
                        //    Session["PatientUserName"] = $('#Ucontact').val();
                        //    location.href = '/Patient/AppointHistory/';
                            location.reload(true);
                        });
                }
                else if (result == "NotBooked") {
                    swal('Error', 'Problem booking the appointment, Please try again !', 'error');
                    $('#err_otp').html('');
                }
                else {
                    swal('Error', 'Something went wrong, Please try again !', 'error');
                    $('#err_otp').html('');
                }
            });
        }
    }
});

// controlling Book Appointment section

$('#bookSection').hide();
$('#btnMenuBook').click(function () {
    $(window).scrollTop(0);
    $('#bookSection').slideToggle('slow');
});
$('#bookClose').click(function () {
    $('#bookSection').slideUp('slow');
});
