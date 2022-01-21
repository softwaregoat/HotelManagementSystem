<%@ Page Title="Dirty Room" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DirtyRoom.aspx.vb" Inherits="HotelManagementSystem.DirtyRoom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
       <script type="text/javascript">
           var auto_refresh = setInterval(
               function () {
                   window.location.reload();
               }, 30000); // refresh every 10000 milliseconds


       </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $(".gvv").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                'paging': true,
                'lengthChange': true,
                'searching': true,
                'ordering': true,
                "order": [[3, "desc"]],
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
        <h1>Habitaciones Sucias
          
            <small></small>
        </h1>
      <%--  <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Dirty Room</li>
        </ol>--%>
    </section>


    <!-- Main content -->
    <section class="content">
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">

                <!-- Form Element sizes -->
                <div class="box box-info">
                    <div class="box-body">
                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" CssClass="gvv table table-bordered table-striped" runat="server" AutoGenerateColumns="False" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="Room_No" HeaderText="Habitación" />
                                    <asp:BoundField DataField="CheckOut" HeaderText="Salida" />
                                    <asp:BoundField DataField="TimeDirty" HeaderText="Tiempo Sucia"/> 
                                     <asp:TemplateField HeaderText="Nombre de la Recamarera" ControlStyle-Width="100%">
                                    <ItemTemplate> 
                <%--<asp:TextBox ID="txtMaidName" Width="150px" CssClass="form-control" Text='<%# Bind("Email") %>' ReadOnly="True" placeholder="Maid Name" runat="server" ></asp:TextBox>--%>
          <div class="form-group">

                                        <asp:DropDownList  CssClass="form-control" ID="ddlMaidNames" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlMaidNames_SelectedIndexChanged" /> 
              <asp:Label ID="lblMaidErr" runat="server" Text=""></asp:Label>
          </div>
                             </ItemTemplate>
                                         </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Comentarios" ControlStyle-Width="100%">
                                     <ItemTemplate> 
                <asp:TextBox ID="txtMaidComments" Width="100%" AutoPostBack="true" CssClass="form-control" Text='<%# Bind("MaidComment") %>' placeholder="comments" runat="server" OnTextChanged="txtMaidComments_TextChanged"></asp:TextBox>
        
                             </ItemTemplate>
                                         </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Estado" />
                                    <asp:TemplateField HeaderText="¿Limpia?">
                                        <ItemTemplate>
                                            <asp:Button Text="Lista" CssClass="btn btn-success btn-xs" runat="server" OnClick="CleanDone" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="" ItemStyle-CssClass="hidden"  ControlStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                     <ItemTemplate>
                  <div class="col-md-3 col-xs-3">
                         <div class="form-group">
        <asp:HiddenField ID="ROOM_ID" runat="server" Value='<%# Bind("ROOM_ID") %>'></asp:HiddenField>
                             </div>
                      </div>
                             </ItemTemplate>
                                         </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="" ItemStyle-CssClass="hidden"  ControlStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                     <ItemTemplate>
                  <div class="col-md-3 col-xs-3">
                         <div class="form-group">
        <asp:HiddenField ID="GUEST_ID" runat="server" Value='<%# Bind("GUEST_ID") %>'></asp:HiddenField>
                             </div>
                      </div>
                             </ItemTemplate>
                                         </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>
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
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css" />
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <script src="Assets/HotelManagSys.js"></script> 
    <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();

           <%-- $('#<% =txtMaidComments.ClientID %>').focusout(function () {
                debugger;

                var sDat = $('#<% =txtMaidComments.ClientID %>').val();
            if (sDat != "" && sDat.length >= 10) {
                    $.ajax({
                        url: 'GuestInformation.aspx/CountGuestByDat',
                        data: "{ 'sDat': '" + sDat + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) { 
                            $('#<% =lblCount2.ClientID %>').text("+" + data.d + " Clients");
                        },
                        error: function (response) {
                            //alert(response.responseText);
                        },
                        failure: function (response) {
                            //alert(response.responseText);
                        }
                    });
                 }


             });--%>
        });
    </script>
</asp:Content>
