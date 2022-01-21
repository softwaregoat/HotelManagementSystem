$(function () {
        LoadData();
    });
 
    function LoadData() {
        var tBillNo = $("[id*=txtBillNo]");
        $.ajax({
            type: "POST",
            url: "ItemCart.aspx/GetSoldCartItem",
            data: '{BILL_NO: "' + tBillNo.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess
        });
    }

    function OnSuccess(response) {
        var xmlDoc = $.parseXML(response.d);
        var xml = $(xmlDoc);
        var customers = xml.find("Table");
        var row = $("[id*=gvItemCart] tr:last-child").clone(true);
        $("[id*=gvItemCart] tr").not($("[id*=gvItemCart] tr:first-child")).remove();
        $.each(customers, function () {
            var customer = $(this);
            AppendRow(row, $(this).find("ROW_ID").text(), $(this).find("FOOD_ITEM_NAME").text(), $(this).find("Quantity").text(), $(this).find("UnitPrice").text(), $(this).find("TotalPrice").text())
            row = $("[id*=gvItemCart] tr:last-child").clone(true);
        });
    }

    function AppendRow(row, ROW_ID, FOOD_ITEM_NAME, Quantity, UnitPrice, TotalPrice) {
        $(".ROW_ID", row).find("span").html(ROW_ID);
        $(".FOOD_ITEM_NAME", row).find("span").html(FOOD_ITEM_NAME);
        $(".Quantity", row).find("span").html(Quantity);
        $(".UnitPrice", row).find("span").html(UnitPrice);
        $(".TotalPrice", row).find("span").html(TotalPrice);
        row.find(".Delete").show();
        $("[id*=gvItemCart]").append(row);
    }


    //Delete event handler.
    $("body").on("click", "[id*=gvItemCart] .Delete", function () {
        if (confirm("Do you want to delete this item?")) {
            var row = $(this).closest("tr");
            var ROW_ID = row.find("span").html();
            $.ajax({
                type: "POST",
                url: "ItemCart.aspx/DeleteItem",
                data: '{ROW_ID: ' + ROW_ID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if ($("[id*=gvItemCart] tr").length > 2) {
                        row.remove();
                    } else {
                        row.find(".Delete").hide();
                        row.find("span").html('&nbsp;');
                    }
                }
            });
        }
        return false;
    });

    //Add event handler.
    $("body").on("click", "[id*=btnSoldItem]", function () {
        var tBillNo = $("[id*=txtBillNo]");
        var tFoodItem = $("[id*=ddlFoodItem]");
        var tQuantity = $("[id*=txtQuantity]");
        $.ajax({
            type: "POST",
            url: "ItemCart.aspx/InsertItemAddCart",
            data: '{BILL_NO: "' + tBillNo.val() + '", FOOD_ITEM_ID: "' + tFoodItem.val() + '", Quantity: "' + tQuantity.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var row = $("[id*=gvItemCart] tr:last-child").clone(true);
                var row = $("[id*=gvItemCart] tr:last-child");
                if ($("[id*=gvItemCart] tr:last-child span").eq(0).html() != "&nbsp;") {
                    row = row.clone();
                }
                AppendRow(row, $(response.d).find("ROW_ID").text(), $(response.d).find("FOOD_ITEM_NAME").text(), $(response.d).find("Quantity").text(), $(response.d).find("UnitPrice").text(), $(response.d).find("TotalPrice").text());
                tQuantity.val("");
                LoadData()
            },
            error: function () {
                $.toaster('Please enter valid information', 'Empty', 'warning');
            }
        });
        return false;
    });


    setInterval("TransDets()", 500);
    function TransDets() {
        var tBillNo = $("[id*=txtBillNo]");
        var tTax1 = $("[id*=txtTax1]");
        var tTax2 = $("[id*=txtTax2]");
        var tTax3 = $("[id*=txtTax3]");
        var tItemCost = $("[id*=txtItemCost]");
        var tTotalTax = $("[id*=txtTotalTax]");
        var tGTotal = $("[id*=txtGTotal]");
        var tPayable = $("[id*=txtPayable]");
        var tDiscount = $("[id*=txtDiscount]");
        $.ajax({
            type: "POST",
            dataType: "json",
            url: 'ItemCart.aspx/TransactionDetails',
            data: '{BILL_NO: "' + tBillNo.val() + '"}',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                tTax1.val($(data.d).find("TAX_1").text());
                tTax2.val($(data.d).find("TAX_2").text());
                tTax3.val($(data.d).find("TAX_3").text());
                tItemCost.val($(data.d).find("ITEM_COST").text());
                tTotalTax.val($(data.d).find("TOTAL_TAX").text());
                tGTotal.val($(data.d).find("G_TOTAL").text());
                tPayable.val(tGTotal.val() - tDiscount.val());
            }
        });
    }

    $("body").on("click", "[id*=btnPrintItemCart]", function () {
        var BillNo = $("[id*=txtBillNo]");
        window.open('Reports/rpt_SalesReceipt.aspx?id=' + BillNo.val(), '_blank', 'width=350,height=580,left=100,top=80');
        return false;
    });


    $("body").on("click", "[id*=btnFindGuest]", function () {
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
                        $("#loader").fadeOut(1000);
                        $.toaster('Guest ID not found.', 'Empty', 'danger');
                    }
                    else {
                        $("[id*=txtGuestName]").val($(response.d).find("GuestName").text());
                        $("#loader").fadeOut(1000);
                    }
                }
            });
        }
        return false;
    });

    $("#MainContent_txtDiscount").change(
        function () {
            var tGTotal = $("[id*=txtGTotal]");
            var tDiscount = $("[id*=txtDiscount]");
            var tPayable = $("[id*=txtPayable]");
            tPayable.val(tGTotal.val() - tDiscount.val());
        }
    );


    $("body").on("click", "[id*=btnCartInSubmit]", function () {
        var tBillNo = $("[id*=txtBillNo]");
        var tGuestID = $("[id*=txtGuestID]");
        var tDiscount = $("[id*=txtDiscount]");
        var tPayment = $("[id*=txtPayment]");
        $("#loader").show();
        $.ajax({
            type: "POST",
            url: "ItemCart.aspx/SaveItemCart",
            data: '{BILL_NO: "' + tBillNo.val() + '", GUEST_ID: "' + tGuestID.val() + '", DISCOUNT: "' + tDiscount.val() + '", PAYMENT: "' + tPayment.val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#loader").fadeOut(500);
                $.toaster('Data has been save successfully.', 'Saved', 'success');
            },
            error: function () {
                $("#loader").fadeOut(500);
                $.toaster('An error has occurred to submission data.', 'Empty', 'danger');
            }
        });
        return false;
    });
