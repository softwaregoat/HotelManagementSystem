<%@ Page Title="Promotion" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Promotion.aspx.vb" Inherits="HotelManagementSystem.Promotion" %>
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
        Promociones
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Promotion</li>
      </ol>
    </section>

    <asp:HiddenField ID="hfldID" runat="server"/>

   <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-6">

          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-body">
           <div class="form-group">
               <label for="txtPromotion">Promoción</label> <i class="required_form_item "> * Requerido</i>
               <asp:TextBox ID="txtPromotion" CssClass="form-control validate[required]" placeholder="Nombre de la promoción" runat="server"></asp:TextBox>
           </div>
                 <div class="form-group">
               <label for="txtPromotionType">Tipo</label> <i class="required_form_item "> * Requerido</i>
                      <asp:DropDownList CssClass="form-control select2 validate[required,custom[integer],minSize[1]]" ID="ddlPromotionType" runat="server">
                                     <asp:ListItem Value="" Text="Seleccione el tipo">Select Type</asp:ListItem>
                          <asp:ListItem Value="1" Text="Extra">Tiempo adicional</asp:ListItem>
                                    <asp:ListItem Value="2" Text="%">% de descuento</asp:ListItem> 
                                </asp:DropDownList>
              
           </div>
                 <div class="form-group">
               <label for="txtPromotionValue">Valor</label> <i class="required_form_item "> * Requerido</i>
               <asp:TextBox ID="txtPromotionValue" CssClass="form-control validate[required]" placeholder="Introduzca un número entero" runat="server"></asp:TextBox>
           </div>
       </div>
              <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Confirmar" > </asp:button>
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-info" NavigateUrl="~/Promotion.aspx">Reiniciar</asp:HyperLink>
              </div>
            <!-- /.box-body -->
          </div>
          <!-- /.box -->
        </div>
        <!--/.col (left) -->

      </div>
      <!-- /.row -->
         <div class="row">
         <!-- left column -->
        <div class="col-md-12">
          <div class="box box-info">
            <div class="box-body">
                <div class="table-responsive">
                <asp:GridView ID="GridView1" CssClass="gvv table table-bordered table-striped table-responsive" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Promotion_ID" HeaderText="ID" />
                         <asp:BoundField DataField="Promotion" HeaderText="Promoción" />
                        <asp:BoundField DataField="Promotion_Type" HeaderText="Tipo" />
                          <asp:BoundField DataField="Promotion_Value" HeaderText="Valor" />
                        <asp:HyperLinkField Text="Editar" DataNavigateUrlFields="Promotion_ID" DataNavigateUrlFormatString="~/Promotion.aspx?act=edit&amp;id={0}">
                            <ControlStyle CssClass="label label-info" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField Text="Borrar" DataNavigateUrlFields="Promotion_ID" DataNavigateUrlFormatString="~/Promotion.aspx?act=del&amp;id={0}">
                            <ControlStyle CssClass="label label-danger" />
                        </asp:HyperLinkField>
                    </Columns>
                </asp:GridView>
                </div>
            </div>
            <!-- /.box-body -->
             </div>
        </div>
          </div>

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

