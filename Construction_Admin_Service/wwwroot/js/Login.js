
$(function () {
    $("form[name='Loginform']").validate({
        rules: {

            Username: {
                required: true,
                email: true
            },
            password: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            Username: "Please enter a valid email address",
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

            Usernamereg: {
                required: true,
                email: true
            },
            Passwordreg: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                minlength: 6,
                equalTo: "#Passwordreg"
            }
        },
        messages: {
            Usernamereg: "Please enter a valid email address",
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



function UserLogin() {

    const uri = 'Auth/Login';
    const user = {
        Username: $('#Username').val(),
        Password: $('#password').val()
    };
    $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async:false,
        success: function (data) {
            sessionStorage.setItem("access_Token", data.data)
            if (data.success == true) {
                sessionStorage.setItem("access_Token", data.data)
                return window.location.href = "https://localhost:44354/home/Contractors";
            }
            else {
                alert(data.message);
            }
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}


 function RegisterUser() {
    const uri = 'Auth/Register';

    const user = {
        Username: $('#Usernamereg').val(),
        Password: $('#Passwordreg').val()
    };
    $.ajax({
        type: "POST",
        url: uri,
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
                  alert(data.message)
        },
        error: function () {
            alert('Error occured');
        }
    });


}