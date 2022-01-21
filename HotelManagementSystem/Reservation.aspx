<%@ Page Title="Reservation" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Reservation.aspx.vb" Inherits="HotelManagementSystem.Reservation" %>
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
      <h1>
        Reservation
           <small>Room Service</small>
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Reservation</li>
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
              <h3 class="box-title">Reservation Information</h3>
            </div>
           <div class="box-body">
               <div class="row">
                   <div class="col-md-4">
                              <div class="form-group">
                                  <label for="txtGuestID">Guest ID</label>
                                    <asp:TextBox ID="txtGuestID" TextMode="Number" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                              </div>
                   </div>
                   <div class="col-md-8">
                           <label for="txtGuestName">Guest Name</label> <i class="required_form_item "> * Required</i>
                           <asp:TextBox ID="txtGuestName" CssClass="form-control validate[required]" placeholder="Guest Name" runat="server"></asp:TextBox>
                   </div>
               </div>

               <div class="form-group">
                   <label for="txtAddress">Address</label> <i class="required_form_item "> * Required</i>
                   <asp:TextBox ID="txtAddress" CssClass="form-control validate[required]" placeholder="Address" runat="server"></asp:TextBox>
               </div>

                     <label for="txtPhoneNo">Phone No</label>
                     <asp:TextBox ID="txtPhoneNo" CssClass="form-control" placeholder="Phone No" runat="server"></asp:TextBox>


                       <div class="form-group">
                           <label for="txtNotes">Notes</label> 
                           <asp:TextBox ID="txtNotes" CssClass="form-control" placeholder="Notes" runat="server"></asp:TextBox>
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
              <h3 class="box-title">Reservation Information</h3>
            </div>
                <div class="box-body">

                    <div class="row">
                        <div class="col-md-4">
                          <label for="txtRevCheckInDate">Check In Date</label> <i class="required_form_item "> * Required</i>
                          <div class="input-group">
                            <asp:TextBox ID="txtRevCheckInDate" CssClass="form-control datepicker validate[required]" runat="server"></asp:TextBox>
                            <span class="input-group-addon"> <span class="add-on"><i class="fa  fa-calendar"></i></span> </span>
                          </div>
                        </div>
                        <div class="col-md-4">
                          <label for="txtRevCheckInTime">Check In Time</label> 
                          <div class="input-group">
                            <asp:TextBox ID="txtRevCheckInTime" CssClass="form-control timepicker" runat="server"></asp:TextBox>
                            <span class="input-group-addon"> <span class="add-on"><i class="fa  fa-clock-o"></i></span> </span>
                          </div>
                        </div>
                        <div class="col-md-4">
                            <label for="txtNoOfDay">No Of Day</label> <i class="required_form_item "> * Required</i>
                            <asp:TextBox ID="txtNoOfDay" CssClass="form-control validate[required]" placeholder="No Of Day" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <label for="ddlRoomNo">Room No</label> <i class="required_form_item "> * Required</i>
                            <asp:DropDownList ID="ddlRoomNo" Width="100%" CssClass="form-control select2 validate[required,custom[integer],minSize[1]]" OnChange="GetAllRoomInfo(this);" ClientIDMode="Static" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="txtChangeRoomType">Room Type</label>
                            <asp:TextBox ID="txtChangeRoomType" CssClass="form-control" placeholder="Room Type" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="txtChangeRentnDay">Rent/Day</label>
                            <asp:TextBox ID="txtChangeRentnDay" CssClass="form-control validate[required]" placeholder="Rent/Day"  runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtNoOfAdult">No. Of Adult</label>
                            <asp:TextBox ID="txtNoOfAdult" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="txtNoOfChildren">No. Of Children</label>
                            <asp:TextBox ID="txtNoOfChildren" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                        </div>
                        <div class="col-md-4"></div>
                    </div>

                    <div class="box-header with-border">
                      <h3 class="box-title">Payment Info</h3>
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
                            <label for="txPayment">Payment</label>
                            <asp:TextBox ID="txPayment" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                        </div>
                        <div class="col-md-4">

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
                        <asp:Button ID="btnReservationSubmit" CssClass="btn btn-info" runat="server" Text="Submit" > </asp:button>
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-info" NavigateUrl="~/Reservation.aspx">Reset</asp:HyperLink>
                      </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">
    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css"/>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script src="Assets/HotelManagSys.js"></script>
        <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
