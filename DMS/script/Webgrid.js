/*$(".add").live("click", function () {

    var existrow = $('.save').length;
    if (existrow == 0) {
        var index = $("#grid tbody tr").length + 1;
        var Department_Name = "Department_Name_" + index + "_new";
        var Co_ordinator_code = "Co_ordinator_code_" + index +"_new";
        var Name = "Name_" + index + "_new";
        var InstitueId = "InstitueId_" + index + "_new";
        var Save = "Save_" + index;
        var Cancel = "Cancel_" + index;

        var tr = '<tr class="alternate-row"><td><span> <input id="' + Department_Name +'" type="text" class="search" onkeypress= "auto();" /></span></td>' +
             '<td><span> <input id="' + Co_ordinator_code + '" type="text" onchange="GetDeptdata(this)"  /></span></td></td>' +
             '<td><span> <input id="' + Name + '" type="text"  readonly/></span></td>' +
             '<td><span> <input id="' + InstitueId + '" type="text"  readonly /></span></td>' +
             '<td> <a href="#" id="' + Save + '" class="save">Save</a><a href="#" id="' + Cancel + '"  class="icancel">Cancel</a></td>' +
         '</tr>';

        $("#grid tbody").append(tr);
        $("#"+Department_Name).focus();
    }
    else {
        alert('First Save your previous record !!');
    }

});*/


function auto() {
    var url = '@Url.RouteUrl("DefaultApi", new { httproute = "", controller = "WebGridApi" })';
    $('.search').autocomplete({

        source: function (request, response) {
            alert(url);
            $.ajax({
                url: url,
                data: { query: request.term },
                dataType: 'json',
                type: 'GET',
                success: function (data) {
                    alert(data);
                    response($.map(data, function (item) {
                        return {
                            label: item.DEPT_NAME,
                            value: item.DEPT_NAME
                        }
                    }));
                }
            })
        },
        select: function (event, ui) {
            $('.search').val(ui.item.label);
            $('#Id').val(ui.item.value);
            return false;
        },
        minLength: 1
    });
}
/*function GetDeptdata(data) {
    var txt1val = "";
    var txt2val = "";
    var Dataid = data.id;
    var flag = "";
    var splits = Dataid.split("_")
    if (splits[4] == "new")
    {
        var depval = $("#Department_Name_" + splits[3] + "_new").val();
        var codeval = $("#Co_ordinator_code_" + splits[3] + "_new").val();
        txt1val = depval;
        txt2val = codeval;
        flag = "0";
    }
    else
    {
        var depval1 = $("#Department_Name_" + splits[3]).val();
        var codeval1 = $("#Co_ordinator_code_" + splits[3]).val();
        txt1val = depval1;
        txt2val = codeval1;
      
        flag = "1";
    }
    if (txt1val != "" && txt2val != "") {

        $.ajax({
            type: "POST",
            url: "../Proposal/Getdetails",
            data: {
                "Dept_Name": txt1val,
                "Co_ordinate_code": txt2val},
            datatype: JSON,
            contentType: "application/json",
            success: function (response) {
                var PiName = response.deptnames[0];
                var InstuteId = response.deptnames[1];
                // var Instituteid = responsec.Co_ordinate;Department
                if (flag == "1") {
                    $("#Name_" + splits[3]).val(PiName);
                    $("#InstitueId_" + splits[3]).val(InstuteId);
                } else if (flag == "0") {
                    alert("Hai");
                    $("#Name_" + splits[3] + "_new").val(PiName);
                    $("#InstitueId_" + splits[3] + "_new").val(InstuteId);
                }
                //document.getElementById("Department").value() = Name;
            },
            error: function () {

            }
        });
    }
}*/