<%@ Page Title="Hotel Information" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="HotelInformation.aspx.vb" Inherits="HotelManagementSystem.HotelInformation" %>
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
   <!-- Content Header (Page header) -->
    <section class="content-header">
      <h1>
        Hotel Information
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Hotel Information</li>
      </ol>
    </section>


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

   <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-6">

          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-body">

                <div class="form-group">
                  <label for="txtHotelName">Hotel Name</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtHotelName" CssClass="form-control validate[required]"  placeholder="Hotel Name" runat="server"></asp:TextBox>
                </div>

                 <div class="form-group">
                  <label for="txtAddress">Address</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtAddress" CssClass="form-control validate[required]"  placeholder="Address" runat="server"></asp:TextBox>
               
                 </div>

                <div class="form-group">
                  <label for="txtContact">Contact</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtContact" CssClass="form-control validate[required]"  placeholder="Contact" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                  <label for="txtTinNo">TIN No</label>
                  <asp:TextBox ID="txtTinNo" CssClass="form-control"  placeholder="TIN No" runat="server"></asp:TextBox>
                </div>

            </div>
              <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Submit" > </asp:button>
              </div>
            <!-- /.box-body -->
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
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css"/>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
     <script src="Assets/loader-custom.js"></script>
        <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
