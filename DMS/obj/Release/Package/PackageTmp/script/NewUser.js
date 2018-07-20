/* function insert() {
        var checkedValue = $('.messageCheckbox:checked').val();
        document.getElementById("checkboxval").value = checkedValue;
    }
    function confirmpassword() {
        var userpassword = document.getElementById("Password").value();
        var confirmpswd = document.getElementById("confirmpassword").value();
        if (userpassword != confirmpswd) {
            alert("Password does not match");
        }
    }
  */

    $(document).ready(function () {
        $('#datatable').DataTable();
    });
    function Emailid() {
        var email = document.getElementById('Emailid');
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(email.value)) {
            //var errName = $("#Errordiv"); //Element selector
            //errName.html("Provid valid email"); // Put the message content inside div
            //errName.addClass('error-msg');
            return false;
        }
        else 
            return true;
            //alert('Provide a valid email');
            //$("#Emailid").focus();
            //document.getElementById('Emailid') = "";
            //return true
        }
 
  /*  function Inputvalidation() {
        var Username = document.getElementById("UserName").value;
        var Email = document.getElementById("Emailid").value;
        var Password = document.getElementById("Password").value;
        var Confirmpassword = document.getElementById("confirmpassword").value;
        var Rolename = document.getElementById("Rolename").value;

        if (Username == "") {
            alert("Enter  Username");
            document.getElementById("UserName").focus();
            return false;

        }
        if (Email == "") {
            alert("Enter Email id");
            document.getElementById("TittleOfAssingnid").focus();
            return false;

        } else if (Email != "") {
            var email = document.getElementById('Emailid');
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(email.value)) {
                //var errName = $("#Errordiv"); //Element selector
                //errName.html("Provid valid email"); // Put the message content inside div
                //errName.addClass('error-msg');
                alert('Provide a valid email');
                $("#Emailid").focus();
                document.getElementById('Emailid') = "";
            }
            }
            if (Password == "") {
                alert("Enter Password");
                $('#Password').focus();
                return false;
            }
            if (Confirmpassword == "") {
                alert("Enter ConfirmPassword");
                $('#confirmpassword').focus();
                return false;
            }
            if (Password != Confirmpassword) {
                alert("Password cannot match")
                //confirmpassword();
            }
            if (Rolename !="") {
                alert("Select any Role");
                $('#Rolename').focus();
                return false;
            }
            insert();
        }*/