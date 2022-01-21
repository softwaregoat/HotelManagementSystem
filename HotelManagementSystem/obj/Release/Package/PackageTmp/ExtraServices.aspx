<%@ Page Title="Extra Services" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ExtraServices.aspx.vb" Inherits="HotelManagementSystem.ExtraServices" %>

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
        <h1>Extra Services
          
            <small>Hotel Services</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Extra Services</li>
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
                        <img id="loader" style="display: none;" src="Assets/img/ajax-loader.gif" width="43" height="11" />
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtGuestID">Guest ID</label><i class="required_form_item"> * Required</i>
                                <div class="input-group">
                                    <asp:TextBox ID="txtGuestID" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnFindGuestBillByID" CssClass="btn btn-info btn-flat" runat="server" Text="FIND" />
                                    </span>
                                </div>

                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label for="txtGuestName">Guest Name</label>
                                    <asp:TextBox ID="txtGuestName" CssClass="form-control" placeholder="Guest Name" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtAddress">Address</label>
                                    <asp:TextBox ID="txtAddress" CssClass="form-control" placeholder="Address" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPhoneNo">Phone No</label>
                                    <asp:TextBox ID="txtPhoneNo" CssClass="form-control" placeholder="Phone No" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtCheckInDate">Check In Date</label>
                                <asp:TextBox ID="txtCheckInDate" CssClass="form-control" ReadOnly="true" placeholder="Check In Date" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtCheckOutDate">Check Out Date</label>
                                <asp:TextBox ID="txtCheckOutDate" CssClass="form-control" placeholder="Check Out Date" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtNoOfDay">No Of Day</label>
                                <asp:TextBox ID="txtNoOfDay" CssClass="form-control" placeholder="No Of Day" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
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

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtBedCost">Bed Cost</label>
                                <asp:TextBox ID="txtBedCost" CssClass="form-control" placeholder="Bed Cost" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtExtraBedCost">Extra Bed Cost</label>
                                <asp:TextBox ID="txtExtraBedCost" CssClass="form-control" placeholder="Extra Bed Cost" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtTotalBedCost">Total</label>
                                <asp:TextBox ID="txtTotalBedCost" CssClass="form-control" placeholder="Total" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!--/.col (left) -->
            <div class="col-md-6">
                <div class="box box-info">
                    <div class="box-body">

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtBoarding">Boarding</label>
                                <asp:TextBox ID="txtBoarding" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Boarding" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtFood">Food</label>
                                <asp:TextBox ID="txtFood" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Food" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtLaundry">Laundry</label>
                                <asp:TextBox ID="txtLaundry" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Laundry" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtTelephone">Telephone</label>
                                <asp:TextBox ID="txtTelephone" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Telephone" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtBar">Bar</label>
                                <asp:TextBox ID="txtBar" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Bar" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtDinner">Dinner</label>
                                <asp:TextBox ID="txtDinner" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Dinner" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtBreakfat">Breakfat</label>
                                <asp:TextBox ID="txtBreakfat" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Breakfat" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtSPA">SPA</label>
                                <asp:TextBox ID="txtSPA" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="SPA" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtBanquetDinner">Banquet Dinner</label>
                                <asp:TextBox ID="txtBanquetDinner" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Banquet Dinner" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtCleaning">Cleaning</label>
                                <asp:TextBox ID="txtCleaning" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Cleaning" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtServiceCharges">Service Charges</label>
                                <asp:TextBox ID="txtServiceCharges" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Service Charges" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtOtherCharges">Other Charges</label>
                                <asp:TextBox ID="txtOtherCharges" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Other Charges" runat="server"></asp:TextBox>
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
                        <asp:Button ID="btnExtraServicesSubmit" CssClass="btn btn-info" runat="server" Text="Submit"></asp:Button>
                        <asp:Button ID="btnPrintViewExServices" CssClass="btn btn-info" runat="server" Text="Print View"></asp:Button>
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-info" NavigateUrl="~/ExtraServices.aspx">Reset</asp:HyperLink>
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
    <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
