<%@ Page Title="TAX Information" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TAXInfo.aspx.vb" Inherits="HotelManagementSystem.TAXInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>  
    <script type="text/javascript">
        $(document).ready(function () {
            $(".gvv").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                'paging': true,
                'lengthChange': true,
                'searching': true,
                'ordering': true,
                'info': true,
                'autoWidth': true
            });
        });
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
        TAX Information
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">TAX Information</li>
      </ol>
    </section>

   <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-4">

          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-body">


       <div class="row">
           <div class="col-md-8">
               <div class="form-group">
                   <label for="txtTAX_Name_1">TAX Name 1</label>
                   <asp:TextBox ID="txtTAX_Name_1" CssClass="form-control validate[required]" placeholder="TAX Name 1" runat="server"></asp:TextBox>
               </div>
           </div>
           <div class="col-md-4">
               <div class="form-group">
                   <label for="txtRate_1">Rate (%)</label>
                   <asp:TextBox ID="txtRate_1" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Rate" runat="server"></asp:TextBox>
               </div>
           </div>
       </div>

       <div class="row">
           <div class="col-md-8">
               <div class="form-group">
                   <label for="txtTAX_Name_2">TAX Name 2</label>
                   <asp:TextBox ID="txtTAX_Name_2" CssClass="form-control validate[required]" placeholder="TAX Name 2" runat="server"></asp:TextBox>
               </div>
           </div>
           <div class="col-md-4">
               <div class="form-group">
                   <label for="txtRate_2">Rate (%)</label>
                   <asp:TextBox ID="txtRate_2" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Rate" runat="server"></asp:TextBox>
               </div>
           </div>
       </div>

       <div class="row">
           <div class="col-md-8">
               <div class="form-group">
                   <label for="txtTAX_Name_3">TAX Name 3</label>
                   <asp:TextBox ID="txtTAX_Name_3" CssClass="form-control validate[required]" placeholder="TAX Name 3" runat="server"></asp:TextBox>
               </div>
           </div>
           <div class="col-md-4">
               <div class="form-group">
                   <label for="txtRate_3">Rate (%)</label>
                   <asp:TextBox ID="txtRate_3" CssClass="form-control validate[required,funcCall[NumberFormat[]]" placeholder="Rate" runat="server"></asp:TextBox>
               </div>
           </div>
       </div>


            </div>





              <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Submit" > </asp:button>
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-info" NavigateUrl="~/TAXInfo.aspx">Reset</asp:HyperLink>
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
    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <script src="Assets/HotelManagSys.js"></script>
     <script src="Assets/loader-custom.js"></script>
        <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
