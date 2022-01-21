<%@ Page Title="Check Out" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CheckOut.aspx.vb" Inherits="HotelManagementSystem.CheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="modal modal-info fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMessege" runat="server" Text="Label"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default pull-right" data-dismiss="modal">OK</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Check Out
          
            <small>Hotel CheckOut Information</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Check Out</li>
        </ol>
    </section>


    <!-- Main content -->
    <section class="content">
        <div class="row">
            <!-- left column -->
            <div class="col-md-6">

                <!-- Form Element sizes -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Guest Information</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtGuestID">Guest ID</label><i class="required_form_item"> * Required</i>
                                <div class="input-group">
                                    <asp:TextBox ID="txtGuestID" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnFindGuestCheckOutID" CssClass="btn btn-info btn-flat" runat="server" Text="FIND" />
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label for="txtGuestName">Guest Name</label>
                                    <asp:TextBox ID="txtGuestName" CssClass="form-control" ReadOnly="true" placeholder="Guest Name" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtAddress">Address</label>
                            <asp:TextBox ID="txtAddress" CssClass="form-control" placeholder="Address" ReadOnly="True" runat="server"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtPhoneNo">Phone No</label>
                            <asp:TextBox ID="txtPhoneNo" CssClass="form-control" placeholder="Phone No" ReadOnly="True" runat="server"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtRoomType">Room Type</label>
                                <asp:TextBox ID="txtRoomType" CssClass="form-control" placeholder="Room Type" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtRoomNO">Room No</label>
                                <asp:TextBox ID="txtRoomNO" CssClass="form-control" placeholder="Room No" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtRentNDay">Rent/Day</label>
                                <asp:TextBox ID="txtRentNDay" CssClass="form-control" placeholder="Rent/Day" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtNote">Note</label>
                            <asp:TextBox ID="txtNote" CssClass="form-control" placeholder="Note" runat="server"></asp:TextBox>
                        </div>


                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!--/.col (left) -->
            <div class="col-md-6">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">CheckIn/CheckOut Information</h3>
                    </div>
                    <div class="box-body">

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtCheckInDate">Check In Date</label>
                                <asp:TextBox ID="txtCheckInDate" CssClass="form-control" Placeholder="Check In Date" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtCheckInTime">Check In Time</label>
                                <asp:TextBox ID="txtCheckInTime" CssClass="form-control" Placeholder="Check In Time" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtNoOfDay">No Of Day</label>
                                <asp:TextBox ID="txtNoOfDay" CssClass="form-control" ReadOnly="true" Placeholder="No Of Day" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtCheckOutDate">Check Out Date</label>
                                <asp:TextBox ID="txtCheckOutDate" CssClass="form-control" ReadOnly="true" Placeholder="Check Out Date" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtCheckOutTime">Check Out Time</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtCheckOutTime" CssClass="form-control timepicker" runat="server"></asp:TextBox>
                                    <span class="input-group-addon"><span class="add-on"><i class="fa  fa-clock-o"></i></span></span>
                                </div>
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <br />

                        <div class="box-header with-border">
                            <h3 class="box-title">Billing Detail's</h3>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtTax1">TAX 1 :
                                    <asp:Label ID="lblTAX1" runat="server" Text=""></asp:Label>
                                </label>
                                <asp:TextBox ID="txtTax1" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtTax2">TAX 2 :
                                    <asp:Label ID="lblTAX2" runat="server" Text=""></asp:Label></label>
                                <asp:TextBox ID="txtTax2" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtTax3">TAX 3 :
                                    <asp:Label ID="lblTAX3" runat="server" Text=""></asp:Label></label>
                                <asp:TextBox ID="txtTax3" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtGrandTotal">Grand Total </label>
                                <asp:TextBox ID="txtGrandTotal" CssClass="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtDiscount">Discount</label>
                                <asp:TextBox ID="txtDiscount" CssClass="form-control" placeholder="Discount" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtNetAmount">Net Amount</label>
                                <asp:TextBox ID="txtNetAmount" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="ddlPaymentType">Payment Type</label>
                                <asp:DropDownList ID="ddlPaymentType" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="UNPAID">UNPAID</asp:ListItem>
                                    <asp:ListItem Value="CASH">CASH</asp:ListItem>
                                    <asp:ListItem Value="CARD">CARD</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label for="txtPaidAmount">Paid Amount</label>
                                <asp:TextBox ID="txtPaidAmount" CssClass="form-control" placeholder="Paid Amount" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtBalance">Balance</label>
                                <asp:TextBox ID="txtBalance" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>



                    </div>
                </div>
            </div>
        </div>
        <!-- /.row -->

        <div class="row">
            <div class="col-md-12">
                <div class="box box-info">
                    <div class="box-footer">
                        <asp:Button ID="btnFinalCheckOut" CssClass="btn btn-warning" runat="server" Text="Check Out"></asp:Button>
                        <asp:Button ID="btnCheckOutSaveOnly" CssClass="btn btn-info" runat="server" Text="Save Only"></asp:Button>
                        <asp:Button ID="btnCheckOutPrintView" CssClass="btn btn-info" runat="server" Text="Final Bill View"></asp:Button>
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-info" NavigateUrl="~/CheckOut.aspx">Reset</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">
    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css" />
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script src="Assets/HotelManagSys.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();

            $("#MainContent_txtDiscount").change(
            function () {
                var tDiscount = $("[id*=txtDiscount]");
                var tGrandTotal = $("[id*=txtGrandTotal]");
                var tNetAmount = $("[id*=txtNetAmount]");
                var tBalance = $("[id*=txtBalance]");
                var tPaidAmount = $("[id*=txtPaidAmount]");
                tNetAmount.val(tGrandTotal.val() - tDiscount.val());
                tBalance.val(tNetAmount.val() - tPaidAmount.val());
            });

            $("#MainContent_txtPaidAmount").change(
            function () {
                var tDiscount = $("[id*=txtDiscount]");
                var tGrandTotal = $("[id*=txtGrandTotal]");
                var tNetAmount = $("[id*=txtNetAmount]");
                var tBalance = $("[id*=txtBalance]");
                var tPaidAmount = $("[id*=txtPaidAmount]");
                tNetAmount.val(tGrandTotal.val() - tDiscount.val());
                tBalance.val(tNetAmount.val() - tPaidAmount.val());
            });
        });
    </script>
</asp:Content>
