
function insert() {
    var val = [];
    $(':checkbox:checked').each(function (i) {
        val[i] = $(this).val();
    });
    document.getElementById("chckval").value = val;
    if (val == "") {
        blurt(
              {

                  title: 'Role Permissions',
                  text: "Select atleast one menu item.",
                  type: 'info',
                  okButtonText: 'OK',
                  idFocus: 'Rolename'
              }
          );
        event.preventDefault();
        document.getElementById("Rolename").focus();
        return false;
    }
}
function TextchangingoftheButton() {
    
    var val = [];
    $(':checkbox:checked').each(function (i) {
        val[i] = $(this).val();
    });
    document.getElementById("chckval").value = val;
    if (val != "") {
        $("#Save").html('<i class="fa fa-save"></i> &nbsp;Update');
    }
    else
    {
        $("#Save").html('<i class="fa fa-save"></i> &nbsp;Save');
    }
}
function TOPCheckboxCheckingandUnchecking() {
    var inputElems = document.getElementsByClassName("MenuCheckbox");
    var count = 0;
    var total =inputElems.length;
    for (var i = 0; i < inputElems.length; i++) {
        if (inputElems[i].checked == true) {
            count++;

        }
        var checked = count;
        if (total == count) {
            $("#chcklist").prop("checked", true);
            
        }
        else {
            $("#chcklist").prop("checked", false);
        }
    }
}
function Roledata() {
    var rolVal = document.getElementById("Rolename").value;
        $("#chcklist").prop("disabled", false);
        $('#divRolemap').css('display', 'block');
        $('#Messageid').hide();
        $('#Button').css('display', 'block');
        if (rolVal == 0) {
            $('#Button').css('display', 'none');
            $("#chcklist").prop("checked", false);
            $('table tbody#rolesId').html('<tr><td  colspan="2" style="text-align:center" >No mapped records. </td></tr>');
            return false;
        }
        else {
            if (rolVal == 1) {
                $('#Button').css('display', 'none');
                $("#chcklist").prop("checked", true);
            }
        var data = {
            Rolename: rolVal,
        }
        $.ajax({
            type: "POST",
            url: "../Home/roleSelect",
            data: JSON.stringify(data),
            datatype: JSON,
            contentType: "application/json",
            success: function (responsec) {
                var responcemsg = responsec.role;

                $('#rolesId').replaceWith('<tbody id="rolesId">' + responcemsg + '</tbody>');
                //  RoleMapping();
                // $('#grd').css({ "display": "block" });
                TOPCheckboxCheckingandUnchecking();
                TextchangingoftheButton();

            },
            error: function () {

            }
        });

    }
}

