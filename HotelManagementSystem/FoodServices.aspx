<%@ Page Title="Food Services" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FoodServices.aspx.vb" Inherits="HotelManagementSystem.FoodServices" %>
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
        Food Services
           <small>Food Sales</small>
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Food Services</li>
      </ol>
    </section>


   <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-12">

          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-body">

             <div class="box-header">
                   <div class="row">
                       <div class="col-md-3">
                           <asp:Button ID="btnCreateNew" CssClass="btn btn-info" runat="server" Text="CREATE NEW BILL" />
                       </div>
                   </div>
             </div>

               <div class="row">
               <div class="col-md-12">
                   <div class="table-responsive">
                   <asp:GridView ID="GridView1" CssClass="gvv table table-bordered table-striped" runat="server" AutoGenerateColumns="False">
                       <Columns>
                           <asp:BoundField DataField="BILL_NO" HeaderText="BILL NO" />
                           <asp:BoundField DataField="BILLING_DATE" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="DATE" />
                           <asp:BoundField DataField="GuestName" HeaderText="GUEST NAME" />
                           <asp:BoundField DataField="G_TOTAL" HeaderText="TOTAL" />
                           <asp:BoundField DataField="PAYMENT" HeaderText="PAYMENT" />
                           <asp:BoundField DataField="BALANCE" HeaderText="BALANCE" />
                           <asp:HyperLinkField Text="&nbsp;" DataNavigateUrlFields="BILL_NO" DataNavigateUrlFormatString="~/ItemCart.aspx?bill={0}">
                               <ControlStyle CssClass="label label-success fa fa-pencil" />
                           </asp:HyperLinkField>
                             <asp:TemplateField HeaderText="PRINT">
                               <ItemTemplate>
                                   <asp:Button Text="..." CssClass="btn btn-xs btn-default" runat="server" OnClientClick='<%# String.Format("return Print_Sales_Receipt({0});", Eval("BILL_NO")) %>' />
                               </ItemTemplate>
                           </asp:TemplateField>
                       </Columns>
                   </asp:GridView>
                    </div>
               </div>
               </div>

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
