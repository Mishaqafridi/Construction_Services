
$("#tblEmployees").on('click', '#btnedit', function () {
    var currentRow = $(this).closest("tr");
    $("#ContractorId").val(currentRow.find("td:eq(0)").text());
    $("#ContractorName").val(currentRow.find("td:eq(1)").text());
    $("#Email").val(currentRow.find("td:eq(2)").text());
    $("#Phone").val(currentRow.find("td:eq(3)").text());
    $("#ContractorAddress").val(currentRow.find("td:eq(6)").text());

});


async function GetEmployeeById(id) {
    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44354/home/Login";
    }
    const uri = 'https://localhost:44354/Contractor/GetContactorById';
    try {
        await $.ajax({
            type: "GET",
            url: uri,
            data: { 'id': id },
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            success: function (result) {
                $("#Id").val(result.id);
                $("#FullName").val(result.fullName);

            },
        });
    } catch (error) {
        console.error(error);
    }
}






$(document).ready(function () {
   
    const uri = 'https://localhost:44354/Contractor/GetAll';


    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44354/home/Login";
    }
    $.ajax({
        type: 'GET',
        url: uri,
        contentType: "application/json;charset=utf-8",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
        },
        success: function (data) {
            console.log(data);
            $.each(data, function (i, item) {
                GetAllContractors();
                var body = "<tr>";
                body += "<td>" + item.contractorId + "</td>";
                body += "<td>" + item.contractorName + "</td>";
                body += "<td>" + item.email + "</td>";
                body += "<td>" + item.phone + "</td>";
                body += "<td>" + item.contractorAddress + "</td>";
                //body += "<td>" + item.contractorQuotations.quotationAmount + "</td>";
                //body += "<td>" + item.contractorQuotations.ammountPaid + "</td>";

                    body += "<td><input type='button'  style='margin-left: 15px;' id='btnedit'  class='btn btn-primary' data-toggle='modal' data-target='#userModel'  value='Edit'/></td>";
                body += "</tr>";
                $("#tblEmployees tbody").append(body);
            });
            $('#tblEmployees').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    'excel', 'print'
                ]
            });
        },
        error: function () {
            //alert('No Access!');
        }
    });





});







$(function () {
    $("form[name='Employeereg']").validate({
        rules: {
            ContractorName: {
                required: true,
                minlength: 7,
                maxlength: 30
            },
            Email: {
                required: true,
                email: true
            },
            Phone: {
                required: true,
                minlength: 7,
                maxlength:15
            },

            ContractorAddress: {
                required: true,
                minlength: 7,
                maxlength: 50
            },
 
        },
        messages: {
            ContractorName: "Please enter valid Full Name",
            Email: "Please enter a valid email address",
            Phone: "Please enter valid Phone No",
            ContractorAddress: "Please enter address"
        },

        submitHandler: function (form) {
            form.submit();
            AddEmployee();
        }
    });
});

$(function () {
    $("form[name='Paymentform']").validate({
        rules: {
            QuotatinDetail: {
                required: true,
                minlength: 7,
                maxlength: 30
            },

            QuotationAmount: {
                required: true,
                minlength: 2,
                maxlength: 10
            },

            AmmountPaid: {
                required: true,
                minlength: 2,
                maxlength: 10
            },
            Contractors: "required",

        },
        messages: {
            ContractorName: "Please enter valid Full Name",
            Email: "Please enter a valid email address",
            Phone: "Please enter valid Phone No",
            ContractorAddress: "Please enter address"
        },

        submitHandler: function (form) {
            form.submit();
            AddPayment();
        }
    });
});



async function getdate() {


}

function AddEmployee() {

    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44354/home/Login";
    }
    if ($('#ContractorId').val() > 0) {

        const Employee = {
            ContractorName: $('#ContractorName').val(),
            Email: $('#Email').val(),
            ContractorId: $('#ContractorId').val(),
            Phone: $('#Phone').val(),
            ContractorAddress: $('#ContractorAddress').val()
        };
        const uri = '/Contractor/UpdateContactor';
        $.ajax({
            type: "POST",
            url: uri,
            data: JSON.stringify(Employee),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            async: false,
            success: function (data) {
                    alert(data.message)
            },
            error: function () {
                alert('Error occured');
            }
        });

    }

    else {
        const uri = '/Contractor/AddContactor';
        const Employee = {
            ContractorName: $('#ContractorName').val(),
            Email: $('#Email').val(),
            Phone: $('#Phone').val(),
            ContractorAddress: $('#ContractorAddress').val()
        };
        $.ajax({
            type: "POST",
            url: uri,
            data: JSON.stringify(Employee),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            async: false,
            success: function (data) {
                    alert(data.message)
            },
            error: function () {
                alert('Error occured');
            }
        });
    }


}


//async function GetAllNationality() {
//    if (sessionStorage.getItem("access_Token") == null) {
//        return window.location.href = "https://localhost:44354/home/Login";
//    }
//    $('#Nationality').empty();
//    const uri = 'https://localhost:44354/Employee/GetAllNationality'; 
//    try {
//   await $.ajax({
//        type: "GET",
//        url: uri, 
//        contentType: "application/json;charset=utf-8",
//       dataType: "json",
//       headers: {
//           'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
//       },
//       success: function (result) {
//           console.log(result);
//           $.each(result, function (key, items) {
//               console.log(items);
//               $.each(items, function (key, item) {
//                   console.log(item);
//                    var option = document.createElement("option");
//                   option.innerText = item.nationality_Name;
//                    option.value = item.id;
//                    document.getElementById('Nationality').appendChild(option);
//                });
//            });
//        },
//   });
//    } catch (error) {
//        console.error(error);
//    }
//}


function AddPayment() {

    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44354/home/Login";
    }

   // AddPaypalPayment($('#QuotationAmount').val())
    const uri = '/Contractor/AddContactorPayment';
        const Employee = {
            QuotatinDetail: $('#QuotatinDetail').val(),
            QuotationAmount: $('#QuotationAmount').val(),
            AmmountPaid: $('#AmmountPaid').val(),
            Quotationstatus: true,
            Quotationstatus: true,
            ContractorId: $('#Contractor').val()
        };
        $.ajax({
            type: "POST",
            url: uri,
            data: JSON.stringify(Employee),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            async: false,
            success: function (data) {
                console.log(data);
                return window.location.href = data.message;
                alert(data.message)
            },
            error: function () {
                alert('Error occured');
            }
        });
  //  }


}

function AddPaypalPayment(PaidAmount) {
    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44354/home/Login";
    }
     const uri = 'https://localhost:44354/PayPal/PaymentWithPaypal';

    alert(uri);
    try {
         $.ajax({
            type: "GET",
            url: uri,
            data: { iCancel: null, blogId: "", PayerID: "", guid: "", PaidAmount: PaidAmount},
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            success: function (result) {
   return true

            },
        });
    } catch (error) {
        console.error(error);
    }
}


async function GetAllContractors() {
    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44397/home/Login";
    }
    $('#Contractor').empty();
    const uri = 'https://localhost:44354/Contractor/GetContractorDrop';
    try {
        await $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            success: function (result) {
            
                $.each(result, function (key, items) {
       
                    $.each(items, function (key, item) {
       
                        var option = document.createElement("option");
                        option.innerText = item.contractorName;
                        option.value = item.contractorId;
                        document.getElementById('Contractor').appendChild(option);
                    });
                });
            },
        });
    } catch (error) {
        console.error(error);
    }
}

async function GetAllExperience() {
    if (sessionStorage.getItem("access_Token") == null) {
        return window.location.href = "https://localhost:44354/home/Login";
    }
    $('#Nationality').empty();
    const uri = 'https://localhost:44354/Employee/GetAllExperience'; 
    try {
        await $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem("access_Token")
            },
            success: function (result) {
                $.each(result, function (key, items) {
                    $.each(items, function (key, item) {
                        var option = document.createElement("option");
                        option.innerText = item.detail;
                        option.value = item.id;
                        document.getElementById('Experience').appendChild(option);
                    });
                });
            },
        });
    } catch (error) {
        console.error(error);
    }
}