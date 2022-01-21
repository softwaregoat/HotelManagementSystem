<%@ Page Title="SMTP Settings" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SMTP.aspx.vb" Inherits="HotelManagementSystem.SMTP" %>
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
        SMTP Information
          <small>Configure GMAIL SMTP Settings</small>
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">SMTP Information</li>
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
                  <label for="txtHost">Host Name</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtHost" CssClass="form-control validate[required]"  placeholder="Host" runat="server"></asp:TextBox>
                </div>

                 <div class="form-group">
                  <label for="txtPort">Port</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtPort" CssClass="form-control validate[required]"  placeholder="Port" runat="server"></asp:TextBox>
               
                 </div>

                <div class="form-group">
                  <label for="txtUserName">UserName</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtUserName" CssClass="form-control validate[required]"  placeholder="UserName" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                  <label for="txtPassword">Password</label> <i class="required_form_item "> * Required</i>
                  <asp:TextBox ID="txtPassword" CssClass="form-control validate[required]"  placeholder="Password" runat="server" ></asp:TextBox>
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
        <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
