$('.select2').select2();

$('.datepicker').datepicker({
    format: 'dd/mm/yyyy',
    autoclose: true
});

$('.timepicker').timepicker({
    showInputs: false
}) 
function NumberFormat(field, rules, i, options) {
    var regex = /^\d+(\.\d{1,2})?$/;
    if (!regex.test(field.val())) {
        return "Please enter valid number. (Ex: 100, 12.56)"
    }
}

function DateFormat(field, rules, i, options) {
    var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-\.](0?[1-9]|1[012])[\/\-\.](\d{4})$/;
    if (!regex.test(field.val())) {
        return "Please enter valid date format. (Ex: dd/mm/yyyy)"
    }
}

function GetSelectedRoomInfo(e) {
    var SelectedValue = e.options[e.selectedIndex].value;
    var tRoomType = $("[id*=txtRoomType]");
    var tRentnDay = $("[id*=txtRentnDay]");
    $.ajax({
        type: "POST",
        url: "CheckIn.aspx/GetSelectedRoomInfo",
        data: '{ROOM_ID: "' + SelectedValue + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            tRoomType.val($(response.d).find("ROOM_TYPE").text());
            tRentnDay.val($(response.d).find("Room_Rent").text());
            CheckIn_Billing();
        }
    });
}


function GetAllRoomInfo(e) {
    var SelectedValue = e.options[e.selectedIndex].value;
    var tRoomType = $("[id*=txtChangeRoomType]");
    var tRentnDay = $("[id*=txtChangeRentnDay]");
    $.ajax({
        type: "POST",
        url: "CheckIn.aspx/GetSelectedRoomInfo",
        data: '{ROOM_ID: "' + SelectedValue + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            tRoomType.val($(response.d).find("ROOM_TYPE").text());
            tRentnDay.val($(response.d).find("Room_Rent").text());
        }
    });
}

$("#MainContent_txtCheckInDate").change(
    function () {
        const oneDay = 24 * 60 * 60 * 1000;
        var tCheckInDate = $("[id*=txtCheckInDate]"); 
        var tCheckOutDate = $("[id*=txtCheckOutDate]");
        var txtNoOfDay = $("[id*=txtNoOfDay]");
        var date = tCheckInDate.val();
        var d = new Date(date.split("/").reverse().join("-"));
        var dd = d.getDate();
        var mm = d.getMonth() + 1;
        var yy = d.getFullYear();
        const firstDate = new Date(yy + ", " + mm + ", " + dd);
        var date2 = tCheckOutDate.val();
        var d2 = new Date(date2.split("/").reverse().join("-"));
        var dd2 = d2.getDate();
        var mm2 = d2.getMonth() + 1;
        var yy2 = d2.getFullYear();
        const secondDate = new Date(yy2 + ", " + mm2 + ", " + dd2);
        const diffDays = Math.round((firstDate - secondDate) / oneDay);
        if (diffDays < 0) {
            txtNoOfDay.val(Math.round(Math.abs((firstDate - secondDate) / oneDay)));
        } else {
            $.toaster({ priority: 'warning', title: 'Invalid', message: 'Date.' });
        }
    }
);

$("#MainContent_txtCheckOutDate").change(
    function () {
        const oneDay = 24 * 60 * 60 * 1000;
        var tCheckInDate = $("[id*=txtCheckInDate]");
        var tCheckOutDate = $("[id*=txtCheckOutDate]");
        var txtNoOfDay = $("[id*=txtNoOfDay]");
        var date = tCheckInDate.val();
        var d = new Date(date.split("/").reverse().join("-"));
        var dd = d.getDate();
        var mm = d.getMonth() + 1;
        var yy = d.getFullYear();
        const firstDate = new Date(yy + ", " + mm + ", " + dd);
        var date2 = tCheckOutDate.val();
        var d2 = new Date(date2.split("/").reverse().join("-"));
        var dd2 = d2.getDate();
        var mm2 = d2.getMonth() + 1;
        var yy2 = d2.getFullYear();
        const secondDate = new Date(yy2 + ", " + mm2 + ", " + dd2);
        const diffDays = Math.round((firstDate - secondDate) / oneDay);
        if (diffDays < 0) {
            txtNoOfDay.val(Math.round(Math.abs((firstDate - secondDate) / oneDay)));
            }else{
            $.toaster({ priority: 'warning', title: 'Invalid', message: 'Date.' });
            }
    }
);

function CheckIn_Billing() {
    var tNoOfDay = $("[id*=txtNoOfDay]");
    var tRentnDay = $("[id*=txtRentnDay]");
    var tTotalCharges = $("[id*=txtTotalCharges]");
    var NoOfDay = $.isNumeric(tNoOfDay.val());
    var RentnDay = $.isNumeric(tRentnDay.val());
    if (!NoOfDay) {
        $.toaster({ priority: 'warning', title: 'Empty', message: 'CheckIn/CheckOut Information' });
        $("[id*=txtRoomType]").val("");
        $("[id*=txtRentnDay]").val("");
    }
    else if (!RentnDay) { $.toaster({ priority: 'warning', title: 'Empty', message: 'Room Information' }); }
    else {
        tTotalCharges.val(tRentnDay.val() * tNoOfDay.val());
    }
}


function print_check_in_form(GUEST_ID) {
    window.open('Reports/rpt_GuestInformation.aspx?id=' + GUEST_ID, '_blank', 'width=900,height=580,left=100,top=80');
    return false;
}

function Print_Sales_Receipt(BILL_NO) {
    window.open('Reports/rpt_SalesReceipt.aspx?id=' + BILL_NO, '_blank', 'width=350,height=580,left=100,top=80');
    return false;
}

$("body").on("click", "[id*=btnFindGuestBillByID]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: "ExtraServices.aspx/FindGuestBill",
            data: '{GUEST_ID: "' + tGuestID.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == "0") {
                    $.toaster('Guest ID not found.', 'Empty', 'danger');
                }
                else {
                    $("[id*=txtGuestName]").val($(response.d).find("GuestName").text());
                    $("[id*=txtAddress]").val($(response.d).find("Address").text());
                    $("[id*=txtPhoneNo]").val($(response.d).find("PhoneNo").text());
                    $("[id*=txtCheckInDate]").val(moment($(response.d).find("Check_In_Date").text() ).format('DD-MM-YYYY'));
                    $("[id*=txtCheckOutDate]").val(moment($(response.d).find("Check_Out_Date").text() ).format('DD-MM-YYYY'));
                    $("[id*=txtNoOfDay]").val($(response.d).find("No_Of_Day").text());
                    $("[id*=txtRoomNO]").val($(response.d).find("Room_No").text());
                    $("[id*=txtRoomType]").val($(response.d).find("ROOM_TYPE").text());
                    $("[id*=txtRentNDay]").val($(response.d).find("Rent_Day").text());
                    $("[id*=txtBedCost]").val($(response.d).find("Total_Charges").text());
                    $("[id*=txtExtraBedCost]").val($(response.d).find("Total_Extra_Bed_Cost").text());
                    $("[id*=txtTotalBedCost]").val($(response.d).find("TotalBedCost").text());
                    $("[id*=txtBoarding]").val($(response.d).find("Boarding").text());
                    $("[id*=txtFood]").val($(response.d).find("Food").text());
                    $("[id*=txtLaundry]").val($(response.d).find("Laundry").text());
                    $("[id*=txtTelephone]").val($(response.d).find("Telephone").text());
                    $("[id*=txtBar]").val($(response.d).find("BAR").text());
                    $("[id*=txtDinner]").val($(response.d).find("DINNER").text());
                    $("[id*=txtBreakfat]").val($(response.d).find("Breakfat").text());
                    $("[id*=txtSPA]").val($(response.d).find("SPA").text());
                    $("[id*=txtBanquetDinner]").val($(response.d).find("BanquetDinner").text());
                    $("[id*=txtCleaning]").val($(response.d).find("Cleaning").text());
                    $("[id*=txtServiceCharges]").val($(response.d).find("ServiceCharges").text());
                    $("[id*=txtOtherCharges]").val($(response.d).find("OtherCharges").text());
                    //$("[id*=txtTax1]").val($(response.d).find("TAX_1").text());
                    //$("[id*=txtTax2]").val($(response.d).find("TAX_2").text());
                    //$("[id*=txtTax3]").val($(response.d).find("TAX_3").text());
                    //$("[id*=txtGrandTotal]").val($(response.d).find("GrandTotal").text());
                    //$("[id*=txtDiscount]").val($(response.d).find("DiscountAmount").text());
                    //$("[id*=txtNetAmount]").val($(response.d).find("NetAmount").text());
                    //$("[id*=txtPaidAmount]").val($(response.d).find("PaidAmount").text());
                    //$("[id*=txtBalance]").val($(response.d).find("BalanceAmount").text());
                    //$("#MainContent_ddlPaymentType").val($(response.d).find("PaymentType").text());
                    //$("#MainContent_ddlPaymentType").selectmenu('refresh', true);
                    $("#loader").fadeOut(2000);
                }
            }
        });
    }
    return false;
});

$("body").on("click", "[id*=btnPrintViewExServices]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        window.open('Reports/rpt_ExtraServices.aspx?id=' + tGuestID.val(), '_blank', 'width=900,height=580,left=100,top=80');
    }
    return false;
});

$("body").on("click", "[id*=btnExtraServicesSubmit]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    var tBoarding = $("[id*=txtBoarding]");
    var tFood = $("[id*=txtFood]");
    var tLaundry = $("[id*=txtLaundry]");
    var tTelephone = $("[id*=txtTelephone]");
    var tBar = $("[id*=txtBar]");
    var tDinner = $("[id*=txtDinner]");
    var tBreakfat = $("[id*=txtBreakfat]");
    var tSPA = $("[id*=txtSPA]");
    var tBanquetDinner = $("[id*=txtBanquetDinner]");
    var tCleaning = $("[id*=txtCleaning]");
    var tServiceCharges = $("[id*=txtServiceCharges]");
    var tOtherCharges = $("[id*=txtOtherCharges]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'warning');
    }
    else {
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: "ExtraServices.aspx/SaveExtraServiceCharges",
            data: '{GUEST_ID: "' + tGuestID.val() + '", Boarding: "' + tBoarding.val() + '", Food: "' + tFood.val() + '", Laundry: "' + tLaundry.val() + '", Telephone: "' + tTelephone.val() + '", BAR: "' + tBar.val() + '", DINNER: "' + tDinner.val() + '", Breakfat: "' + tBreakfat.val() + '", SPA: "' + tSPA.val() + '", BanquetDinner: "' + tBanquetDinner.val() + '", Cleaning: "' + tCleaning.val() + '", ServiceCharges: "' + tServiceCharges.val() + '", OtherCharges: "' + tOtherCharges.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#loader").fadeOut(500);
                $.toaster('Data has been saved successfully.', 'Done', 'success');
            },
            error: function () {
                $("#loader").fadeOut(500);
                $.toaster('An error has occurred to submission data.', 'Empty', 'danger');
            }
        });
    }
    return false;
});


$("body").on("click", "[id*=btnFindGuestRoomChageInfo]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: "ExtraServices.aspx/FindGuestBill",
            data: '{GUEST_ID: "' + tGuestID.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == "0") {
                    $.toaster('Guest ID not found.', 'Empty', 'danger');
                }
                else {
                    $("[id*=txtGuestName]").val($(response.d).find("GuestName").text());
                    $("[id*=txtAddress]").val($(response.d).find("Address").text());
                    $("[id*=txtPhoneNo]").val($(response.d).find("PhoneNo").text());
                    $("[id*=txtCurrentRoomNo]").val($(response.d).find("Room_No").text());
                    $("[id*=txtCurrentRoomType]").val($(response.d).find("ROOM_TYPE").text());
                    $("[id*=txtCurretnRentPerdayDay]").val($(response.d).find("Rent_Day").text());
                    $("#loader").fadeOut(2000);
                }
            }
        });
    }
    return false;
});


$("#MainContent_txtExtraBeds").change(
    function () {
        var tExtraBeds = $("[id*=txtExtraBeds]");
        var tPerBedCost = $("[id*=txtPerBedCost]");
        var tTotalExtraBedCost = $("[id*=txtTotalExtraBedCost]");
        tTotalExtraBedCost.val(tExtraBeds.val() * tPerBedCost.val());
    }
);

$("#MainContent_txtPerBedCost").change(
    function () {
        var tExtraBeds = $("[id*=txtExtraBeds]");
        var tPerBedCost = $("[id*=txtPerBedCost]");
        var tTotalExtraBedCost = $("[id*=txtTotalExtraBedCost]");
        tTotalExtraBedCost.val(tExtraBeds.val() * tPerBedCost.val());
    }
);

$("body").on("click", "[id*=btnCheckOutSaveOnly]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    var tNote = $("[id*=txtNote]");
    var tCheckOutTime = $("[id*=txtCheckOutTime]");
    var tDiscount = $("[id*=txtDiscount]");
    var tPaymentType = $("[id*=ddlPaymentType]");
    var tPaidAmount = $("[id*=txtPaidAmount]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        /////////////////////////////
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: "CheckOut.aspx/SaveCheckOut",
            data: '{GUEST_ID: "' + tGuestID.val() + '", CheckOutNote: "' + tNote.val() + '", Check_Out_Time: "' + tCheckOutTime.val() + '", DiscountAmount: "' + tDiscount.val() + '", PaymentType: "' + tPaymentType.val() + '", PaidAmount: "' + tPaidAmount.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#loader").fadeOut(500);
                $.toaster('Data has been saved successfully.', 'Done', 'success');
            },
            error: function () {
                $("#loader").fadeOut(500);
                $.toaster('An error has occurred to submission data.', 'Empty', 'danger');
            }
        });
        /////////////////////////////
    }
    return false;
});


$("body").on("click", "[id*=btnCheckOutPrintView]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        window.open('Reports/rpt_CheckOut.aspx?id=' + tGuestID.val(), '_blank', 'width=900,height=580,left=100,top=80');
    }
    return false;
});

$("body").on("click", "[id*=btnFindGuestCheckOutID]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: "ExtraServices.aspx/FindGuestBill",
            data: '{GUEST_ID: "' + tGuestID.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == "0") {
                    $.toaster('Guest ID not found.', 'Empty', 'warning');
                }
                else {
                    $("[id*=txtGuestName]").val($(response.d).find("GuestName").text());
                    $("[id*=txtAddress]").val($(response.d).find("Address").text());
                    $("[id*=txtPhoneNo]").val($(response.d).find("PhoneNo").text());
                    $("[id*=txtCheckInDate]").val(moment($(response.d).find("Check_In_Date").text()).format('DD-MM-YYYY'));
                    $("[id*=txtCheckOutDate]").val(moment($(response.d).find("Check_Out_Date").text()).format('DD-MM-YYYY'));
                    $("[id*=txtCheckInTime]").val($(response.d).find("Check_In_Time").text());
                    $("[id*=txtCheckOutTime]").val($(response.d).find("Check_Out_Time").text());
                    $("[id*=txtNoOfDay]").val($(response.d).find("No_Of_Day").text());
                    $("[id*=txtNote]").val($(response.d).find("CheckOutNote").text());
                    $("[id*=txtRoomType]").val($(response.d).find("ROOM_TYPE").text());
                    $("[id*=txtRoomNO]").val($(response.d).find("Room_No").text());
                    $("[id*=txtRentNDay]").val($(response.d).find("Rent_Day").text());
                    $("[id*=txtTax1]").val($(response.d).find("TAX_1").text());
                    $("[id*=txtTax2]").val($(response.d).find("TAX_2").text());
                    $("[id*=txtTax3]").val($(response.d).find("TAX_3").text());
                    $("[id*=txtGrandTotal]").val($(response.d).find("GrandTotal").text());
                    $("[id*=txtDiscount]").val($(response.d).find("DiscountAmount").text());
                    $("[id*=txtNetAmount]").val($(response.d).find("NetAmount").text());
                    $("[id*=txtPaidAmount]").val($(response.d).find("PaidAmount").text());
                    $("[id*=txtBalance]").val($(response.d).find("BalanceAmount").text());
                    $("#MainContent_ddlPaymentType").val($(response.d).find("PaymentType").text());
                    //$("#MainContent_ddlPaymentType").selectmenu('refresh', true);
                    $("#loader").fadeOut(500);
                }
            },
            error: function () {
                $.toaster('An error has occurred.', 'Empty', 'danger');
            }
        });
    }
    return false;
});


$("body").on("click", "[id*=btnFinalCheckOut]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    var tNote = $("[id*=txtNote]");
    var tCheckOutTime = $("[id*=txtCheckOutTime]");
    var tDiscount = $("[id*=txtDiscount]");
    var tPaymentType = $("[id*=ddlPaymentType]");
    var tPaidAmount = $("[id*=txtPaidAmount]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        $.confirm({
            title: 'Check Out',
            content: 'Confirm to complete check out.',
            buttons: {
                confirm: function () {
                    /////////////////////////////
                    $("#loader").show();
                    $.ajax({
                        type: "POST",
                        url: "CheckOut.aspx/CompleteCheckOut",
                        data: '{GUEST_ID: "' + tGuestID.val() + '", CheckOutNote: "' + tNote.val() + '", Check_Out_Time: "' + tCheckOutTime.val() + '", DiscountAmount: "' + tDiscount.val() + '", PaymentType: "' + tPaymentType.val() + '", PaidAmount: "' + tPaidAmount.val() + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            $("#loader").fadeOut(500);
                            $.toaster('Data has been saved successfully.', 'Done', 'success');
                        },
                        error: function () {
                            $("#loader").fadeOut(500);
                            $.toaster('An error has occurred to submission data.', 'Empty', 'danger');
                        }
                    });
                    /////////////////////////////
                },
                cancel: function () {}
            }
        });
    }
    return false;
});


$("body").on("click", "[id*=btnServiceReportCheckInList]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_GuestList.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() + '&status=CheckIn', '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnServiceReportCheckOutList]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_GuestListCheckOut.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() + '&status=CheckOut', '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnServiceReportCancelList]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_GuestList.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() + '&status=Cancelled', '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnServiceReportReservationList]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_GuestList.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() + '&status=Reservation', '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnServiceReportChangeRoomHistory]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_RoomChangeHistory.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val(), '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnServiceReportExtraBed]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_ExtraBed.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val(), '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});


$("body").on("click", "[id*=btnFindGuestAllInfo]", function () {
    var tGuestID = $("[id*=txtGuestID]");
    if (tGuestID.val() == '') {
        $.toaster('Guest ID.', 'Empty', 'danger');
    }
    else {
        window.open('Reports/rpt_GuestInformation.aspx?id=' + tGuestID.val(), '_blank', 'width=900,height=580,left=100,top=80');
    }
    return false;
});

$("body").on("click", "[id*=btnServiceReportPaidBillList]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_PaidBillList.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() , '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnServiceReportUnPaidBillList]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_UnPaidBillList.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() , '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnFoodServiceItemList]", function () {
    window.open('Reports/rpt_FoodServiceItemList.aspx', '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnFoodServicePaidSales]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_FoodServicePaidSales.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val(), '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

$("body").on("click", "[id*=btnFoodServiceUnPaidSales]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    window.open('Reports/rpt_FoodServiceUnPaidSales.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val(), '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});


$("body").on("click", "[id*=btnInvoice]", function () {
    var tDateFrom = $("[id*=txtDateFrom]");
    var tDateTo = $("[id*=txtDateTo]");
    var users = $("#ddlUsers option:selected").text();
    var uid = $("#ddlUsers option:selected").val();
    var reporttype = $("[id*=hdnreporttype]"); 
    var gType = $("#ddlNormalSpecial option:selected").val();

    window.open('Reports/rptCheckIn.aspx?dFrom=' + tDateFrom.val() + '&dTo=' + tDateTo.val() + '&uid=' + uid + '&cUser=' + users + '&rptType=' + reporttype.val() + '&gType=' + gType +'', '_blank', 'width=900,height=580,left=100,top=80');
    return false;
});

function GenerateInvoice() {
//$("body").on("click", "[id*=btnInvoiceCheckedIn]", function () { 
    var guestId = $("[id*=hdnGuestId]").val();
    var roomId = $("[id*=hdnRoomId]").val();
    var invName = $("[id*=invName]").val();
    var invId = $("[id*=invId]").val();
    var IsAsGuest = $('#AsGuest').prop('checked');

    window.open('Reports/rptCheckedIn.aspx?guestId=' + guestId + '&roomId=' + roomId + '&invName=' + invName + '&invId=' + invId + '&IsAsGuest=' + IsAsGuest + '', '_blank', 'width=900,height=580,left=100,top=80');
   
}


