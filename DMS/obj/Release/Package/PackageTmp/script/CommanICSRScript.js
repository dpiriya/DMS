
//function durationofmonth(dateofvalue, finishdate) {
//    var stratyear = dateofvalue.split('-');
//    var closeyear = finishdate.split('-');
//    var startmonth = stratyear[1];
//    var closemonth = closeyear[1];
//    var startdate = stratyear[0];
//    var closedate = closeyear[0];
//    var datein;
//    stratyear = stratyear[2];
//    closeyear = closeyear[2];
//    var months = (closeyear - stratyear) * 12;
//    if (startmonth > closemonth) {
//        months += startmonth - closemonth;
//    }
//    else {
//        months += closemonth - startmonth;
//    }
//    if (startdate > closedate) {
//        datein = (startdate - closedate) / 30;
//        if (datein != 0) {
//            months += 1;
//        }
//    }
//    else {
//        datein = (closedate - startdate) / 30;
//        if(datein!=0)
//        {
//            months += 1;
//        }
//    }
//    return months;
//}
function durationofmonth(dateofvalue, finishdate) {
    var stratyear = dateofvalue.split('-');
    var closeyear = finishdate.split('-');
    var startmonth = stratyear[1];
    var closemonth = closeyear[1];
    var startdate = stratyear[0];
    var closedate = closeyear[0];
    var datein;
    stratyear = stratyear[2];
    closeyear = closeyear[2];
    var months = 0;
    startmonth = stratyear * 12 + startmonth*1;
    closemonth = closeyear * 12 + closemonth*1;
    months = closemonth - startmonth;
    if (startdate > closedate) {
        datein = (startdate - closedate) / 30;
        if (datein != 0) {
            months += 1;
        }
    }
    else {
        datein = (closedate - startdate) / 30;
        if (datein != 0) {
            months += 1;
        }
    }
    return months;
}
function Convertedintoday(GetDate) {
    var getdates = GetDate.split('-');
    var Getday = getdates[0];
    var Getmonth = getdates[1];
    var Getyear = getdates[2];
    var Getyears = Getyear * 365;
    var Getmonths = Getmonth * 30;
    var Getdays = Getday * 1;
    Getdays = Getmonths + Getdays + Getyears;
    return Getdays;

}

$(function () {

    $('.Dateformat').each(function () {

        var Getdatevalue = $(this).text();
        Getdatevalue = Getdatevalue.trim();
        if (Getdatevalue != "") {
            var Onlydate = Getdatevalue.match(/.{1,10}/g);
            Onlydate = Onlydate[0];
            if (Onlydate == "01-01-0001") {
                $(this).text('N/A');
            }
            else
                $(this).text(Onlydate);
        }
    });

    $('.Onlynumeric').on('keypress', function (e) {
        var deleteCode = 8; var backspaceCode = 46;
        var key = e.which;
        if ((key >= 48 && key <= 57) || key === deleteCode || key === backspaceCode || (key >= 37 && key <= 40) || key === 0) {
            character = String.fromCharCode(key);
            if (character != '.' && character != '%' && character != '&' && character != '(' && character != '\'') {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    });
    $('.Onlyalphabet').on('keypress', function (e) {
        var deleteCode = 8; var backspaceCode = 46;
        var key = e.which;
        if ((key >= 65 && key <= 90) || key === deleteCode || key === backspaceCode || (key >= 97 && key <= 122) || key === 0 || key == 32) {
            character = String.fromCharCode(key);
            if (character != '.' && character != '%' && character != '&' && character != '(' && character != '\'') {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    });
    //$('.Alphanumeric').keydown(function (e) {
    //        var key = e.keyCode;
    //        if ((key == 32) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)) {
    //            e.preventDefault();
    //}
    //});
    $('.Alphanumeric').keyup(function () {
        var yourInput = $(this).val();
        re = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
        var isSplChar = re.test(yourInput);
        if (isSplChar) {
            var no_spl_char = yourInput.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
            $(this).val(no_spl_char);
        }
    });

});
$(function () {
    windowHeight = $(window).height() - 155;
    $('.box-header').css('min-height', windowHeight); // Handler for .ready() called.
});
function CalculateDuration(dStartDate, dEndDate)
{   
    var aFromDate = dStartDate.split("-");
    var fdd1 = aFromDate[0]; //get the day part- From date
    var fmm1 = aFromDate[1]; //get the month part- From date
    var fyyyy1 = aFromDate[2]; //get the year part- From date
    var fromDate = new Date(parseFloat(fyyyy1), parseFloat(fmm1 - 1), parseFloat(fdd1, 10));
    var aToDate = dEndDate.split("-");
    var tdd1 = aToDate[0]; //get the day part-To date
    var tmm1 = aToDate[1]; //get the month part-To date
    var tyyyy1 = aToDate[2]; //get the year part-To date
    var toDate = new Date(parseFloat(tyyyy1), parseFloat(tmm1 - 1), parseFloat(tdd1, 10));

    if (dStartDate != "" && dEndDate != "") {        

        //Set 1 day in milliseconds

        var one_day = 1000 * 60 * 60 * 24

        //calculate duration
       
        var differenceTravel = toDate.getTime() - fromDate.getTime();

        var SDuration = Math.abs((differenceTravel) / (1000 * 60 * 60 * 24));

        if (SDuration == "NaN")

            SDuration = "";

        return SDuration;
    }
    else {

        SDuration = "";
        return SDuration;

    }        

}