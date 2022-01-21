<%@ Page Title="Food Service Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FoodServiceReport.aspx.vb" Inherits="HotelManagementSystem.FoodServiceReport" %>

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
        <h1>Food Service Report
        </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Food Service Report</li>
        </ol>
    </section>



    <!-- Main content -->
    <section class="content">
        <div class="row">
            <!--/.col (left) -->
            <div class="col-md-4">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Select Date</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label for="txtDateFrom">Date From</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDateFrom" CssClass="form-control datepicker validate[required]" ReadOnly="true" runat="server"></asp:TextBox>
                                    <span class="input-group-addon"><span class="add-on"><i class="fa  fa-calendar"></i></span></span>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label for="txtDateTo">Date To</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDateTo" CssClass="form-control datepicker validate[required]" ReadOnly="true" runat="server"></asp:TextBox>
                                    <span class="input-group-addon"><span class="add-on"><i class="fa  fa-calendar"></i></span></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:HyperLink ID="HyperLink2" runat="server" CssClass="btn btn-info" NavigateUrl="~/FoodServiceReport.aspx">Reset All</asp:HyperLink>
                    </div>
                </div>
            </div>

            <!--/.col (left) -->
            <div class="col-md-4">
                <div class="box box-info">
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnFoodServiceItemList" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Food Item List"></asp:Button>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnFoodServicePaidSales" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Paid Sales"></asp:Button>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnFoodServiceUnPaidSales" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="UnPaid Sales"></asp:Button>
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>



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
