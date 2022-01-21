<%@ Page Title="Add Extra Bed" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddExtraBed.aspx.vb" Inherits="HotelManagementSystem.AddExtraBed" %>

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
        <h1>Add Extra Bed
          
            <small>Room Services</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Add Extra Bed</li>
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
                                    <asp:TextBox ID="txtGuestID" TextMode="Number" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnFindGuestRoomChageInfo" CssClass="btn btn-info btn-flat" runat="server" Text="FIND" />
                                    </span>
                                </div>

                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label for="txtGuestName">Guest Name</label>
                                    <i class="required_form_item">* Required</i>
                                    <asp:TextBox ID="txtGuestName" CssClass="form-control validate[required]" placeholder="Guest Name" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtPhoneNo">Phone No</label>
                                <asp:TextBox ID="txtPhoneNo" CssClass="form-control" placeholder="Phone No" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-8">
                                <label for="txtAddress">Address</label>
                                <asp:TextBox ID="txtAddress" CssClass="form-control" placeholder="Address" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="box-header with-border">
                            <h3 class="box-title">Room Information</h3>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtCurrentRoomType">Room Type</label>
                                <asp:TextBox ID="txtCurrentRoomType" CssClass="form-control" placeholder="Room Type" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtCurrentRoomNo">Room No</label>
                                <asp:TextBox ID="txtCurrentRoomNo" CssClass="form-control" placeholder="Room No" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtCurretnRentPerdayDay">Rent Per Day</label>
                                <asp:TextBox ID="txtCurretnRentPerdayDay" CssClass="form-control" placeholder="Rent/Day" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="box-header with-border">
                            <h3 class="box-title">Extra Bed</h3>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label for="txtBed">Bed</label>
                                <asp:TextBox ID="txtBed" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Bed" runat="server" TextMode="Number"></asp:TextBox>
                            </div>
                            <div class="col-md-8">
                                <label for="txtComment">Comment</label>
                                <asp:TextBox ID="txtComment" CssClass="form-control" placeholder="Reason" runat="server"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Submit"></asp:Button>
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-info" NavigateUrl="~/AddExtraBed.aspx">Reset</asp:HyperLink>
                    </div>
                </div>
                <!-- /.box -->
            </div>
            <!--/.col (left) -->
        </div>
        <!-- /.row -->

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
