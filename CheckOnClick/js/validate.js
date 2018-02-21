

function signup_validate(f1) {
	var pass = f1.pass1.value;
	var cpass = f1.cpass1.value;

	if(pass === cpass) {
		return true;
	}
	else {
		alert('Passwords are mismatched');
		return false;
	}
}
