$(function () {
    $("form[name='Loginform']").validate({
        rules: {
           
            Username: "required",
            password: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            Username: "Please enter Username",
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },
        },

        submitHandler: function (form) {
            form.submit();
            UserLogin();
        }
    });
});


$(function () {
    $("form[name='UserRegistration']").validate({
        rules: {

            Usernamereg: "required",
            Passwordreg: {
               // required: true,
                minlength: 6
            },
            ConfirmPassword: {
                minlength: 6,
                 equalTo: "#Passwordreg"
            }
        },
        messages: {
            Usernamereg: "Please enter Username",
            Passwordreg: {
                minlength: "Your password must be at least 6 characters long"
            },
            ConfirmPassword: {
                minlength: "Your password must be at least 6 characters long",
                equalTo: "Enter Confirm Password Same as Password"
            },
        },

        submitHandler: function (form) {
            form.submit();
            RegisterUser();
        }
    });
});