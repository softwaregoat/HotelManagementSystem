<%@ Page Title="Home" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Dashboard.aspx.vb" Inherits="HotelManagementSystem.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
     <script src="Assets/loader-custom.js"></script>
    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Dashboard 
         
            <small>Hotel Management System</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Dashboard</li>
        </ol>
    </section>


    <div class="modal modal-danger fade" id="myModal" role="dialog">
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

    <section class="content">

        <div class="callout callout-info">
            <h4>
                <asp:Label ID="lblGreeting" runat="server" Text=""></asp:Label></h4>
            <i class="fa fa-user-circle-o" aria-hidden="true"></i>
            <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
        </div>

        <!-- Info boxes -->
        <div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-aqua-gradient"><i class="ion ion-android-checkbox-outline"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Check In</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblCheckIn" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-blue-gradient"><i class="ion ion-android-exit"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Check Out</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblCheckOut" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->

            <!-- fix for small devices only -->
            <div class="clearfix visible-sm-block"></div>

            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-yellow-gradient"><i class="ion ion-android-list"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Reservation</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblReservation" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-maroon-gradient"><i class="ion ion-android-cancel"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Cancelled</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblCancelled" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->

        <!-- Info boxes -->
        <div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-maroon-gradient"><i class="ion ion-paintbucket"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Dirty Room</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblDirtyRoom" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-purple-gradient"><i class="ion ion-pizza"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Food Item</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblFoodItem" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->

            <!-- fix for small devices only -->
            <div class="clearfix visible-sm-block"></div>

            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-green-gradient"><i class="ion ion-android-cart"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Invoice</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblInvoice" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-teal-gradient"><i class="ion ion-ios-contact-outline"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Users</span>
                        <span class="info-box-number">
                            <asp:Label ID="lblUsers" runat="server" Text="0"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->



    </section>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">
</asp:Content>
