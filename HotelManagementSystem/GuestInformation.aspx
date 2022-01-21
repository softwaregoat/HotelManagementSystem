<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="GuestInformation.aspx.vb" Inherits="HotelManagementSystem.GuestInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
   <%-- <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>--%>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }

    </script>
     <style type="text/css">
        .chkChoice input 
{ 
    margin-left: 5px; 
}
        .chkChoice label 
{ 
    padding-left: 5px; 
}
.chkChoice td 
{ 
    padding-left: 10px; 
}
        .errorG{
            color:red;
        }
    </style>
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
   


    <!-- Main content -->
    <section class="content">
        <div class="row">
            <!-- left column -->
            <div class="col-md-6">

                <!-- Form Element sizes -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Búsqueda por Huésped</h3>
                          <%--  <span style="float:right;"  class="info-box-number">
                               
                            </span>  --%>
                    <div class="box-footer pull-right">
                         <asp:HiddenField ID="hdnRoomId" runat="server" />
                              <asp:HyperLink ID="hlCheckGuest" CssClass="btn btn-default" runat="server" NavigateUrl="~/RoomView.aspx"> <i class="fa fa-arrow-left"></i>  <span>Back</span> </asp:HyperLink>
                        <asp:HyperLink ID="HyperLink1" CssClass="btn btn-warning" runat="server" NavigateUrl="~/GuestInformation.aspx"><span>Limpiar</span></asp:HyperLink>
                              <asp:Button ID="btnSearch" CausesValidation="false" runat="server" Text="Buscar" CssClass="btn btn-info" /> 
                              <asp:Button ID="btnSave"   CssClass="btn btn-success" CausesValidation="false" runat="server" Text="Guardar" />
                        <%--<asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-info" NavigateUrl="~/CheckIn.aspx">Close</asp:HyperLink>--%>
                    </div>
                              </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="txtGuestID">ID del Huésped</label>
									<i class="required_form_item ">* Requerido</i>
                                      <asp:HiddenField ID="hdnGuestId" runat="server" />
                                    <asp:TextBox ID="txtGuestID" TabIndex="1"   placeholder="Guest ID" CssClass="form-control gid"  AutoPostBack="true" runat="server"></asp:TextBox>
                                  <asp:Label ID="errGuestId" runat="server" Text=""></asp:Label>
                              
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label for="txtGuestName">Nombre del Huésped</label>
                                    <i class="required_form_item ">* Requerido</i>
                                    <asp:TextBox ID="txtGuestName" TabIndex="2" CssClass="form-control" placeholder="Guest Name" AutoPostBack="true"  runat="server"></asp:TextBox>
                                 <asp:Label ID="errGuest" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                         <div class="row">
                            <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtAddress">Nacionalidad</label>
                            <i class="required_form_item ">* Requerido</i>
                            <asp:TextBox ID="txtAddress" TabIndex="3" CssClass="form-control" placeholder="Nationality" runat="server"></asp:TextBox>
                             <asp:Label ID="errAddress" runat="server" Text=""></asp:Label>
                        </div>
</div>
                             <div class="col-md-6">
                                 <asp:CheckBoxList ID="CheckBoxList1" TabIndex="4" CssClass="chkChoice" runat="server" RepeatDirection="Horizontal">
                                     <asp:ListItem Text="Lista VIP"  Value="1" onclick="MutExChkList(this);" /> 
                                     <asp:ListItem onclick="MutExChkList(this);" Text="Lista Negra" Value="2" />
                                 </asp:CheckBoxList>
                                <asp:DropDownList CssClass="form-control hide" ID="ddlNormalSpecial" runat="server">
                                    <asp:ListItem Value="Normal" Text="Normal">Normal</asp:ListItem>
                                    <asp:ListItem Value="Especial" Text="Especial">Especial</asp:ListItem> 
                                </asp:DropDownList>
                             </div>
                             </div>
                        <div class="form-group">
                             <label for="txtNote">Comentario del Huésped</label>  
                            <textarea id="txtNote" class="form-control" tabindex="5" maxlength="1000"  cols="20" rows="2" runat="server"></textarea>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <label for="ddlIDType">Tipo de ID</label>
                                <asp:DropDownList CssClass="form-control" TabIndex="6" ID="ddlIDType" runat="server">
                                    <asp:ListItem Text="Cédula" Value="Cédula">Cédula</asp:ListItem>
                                    <asp:ListItem Text="Licencia" Value="Licencia">Licencia</asp:ListItem>
                                    <asp:ListItem Text="Carnet del seguro" Value="Carnet del seguro">Carnet del seguro</asp:ListItem>
                                    <asp:ListItem Text="Carnet de residente" Value="Carnet de residente">Carnet de residente</asp:ListItem>
                                    <asp:ListItem Text="Pasaporte" Value="Pasaporte">Pasaporte</asp:ListItem> 
                                    <asp:ListItem Text="Otro" Value="Otro">Otro</asp:ListItem>  
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 hide">
                                <label for="txtIDNo">Número de Tarjeta</label>
                                <asp:TextBox ID="txtIDNo" CssClass="form-control" TabIndex="7"  placeholder="ID No" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <label for="ddlGender">Sexo</label>
                                <asp:DropDownList CssClass="form-control" TabIndex="8" ID="ddlGender" runat="server">
                                    <asp:ListItem>Hombre</asp:ListItem>
                                    <asp:ListItem>Mujer</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                              
                                <label for="txtPhoneNo">Número de Celular</label>
                            <asp:TextBox ID="txtPhoneNo" CssClass="form-control" TabIndex="9" placeholder="Phone No" runat="server"></asp:TextBox>
                            </div>
                        </div>

                         <div class="row">
                            <div class="col-md-6">
                                <label for="firstVisitDate">Fecha de la Primera Visita
</label>
                                 <asp:TextBox ID="firstVisitDate" CssClass="form-control" TabIndex="9" placeholder="Fecha de la Primera Visita" runat="server"></asp:TextBox>
                            
                            </div>
                            <div class="col-md-6">
                              
                                <label for="lastVisitDate">Fecha de la Última Visita
</label>
                            <asp:TextBox ID="lastVisitDate" CssClass="form-control" TabIndex="10" placeholder="Fecha de la Última Visita" runat="server"></asp:TextBox>
                            </div>
                        </div>



                        <div class="row hidden">
                        <div class="col-md-12">
                                <label for="txtArrivalDeparture">Comentario de Recamarera</label>
                                <asp:TextBox ID="txtArrivalDeparture" TabIndex="20" CssClass="form-control" placeholder="Maids Comments" runat="server"></asp:TextBox>
                            </div>
                            </div>
                        <div class="row">
                            
                            <div class="col-md-6 hidden">
                               
                                  <label for="txtPurpose">Purpose</label>
                                <asp:TextBox ID="txtPurpose" CssClass="form-control" TabIndex="11" placeholder="Purpose" runat="server"></asp:TextBox>
                            </div>
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
                        <h3 class="box-title">Detalle de las Visitas del Huésped</h3>
                    </div>
                    <div class="box-body">
                         
                          <div class="row"> 
                              
                        <div class="col-md-12" style="padding-left:5px;padding-right:5px; overflow:auto">
                              <asp:GridView ID="gvBill"  Width="100%"  CssClass="table table-responsive" ShowFooter="true"  runat="server" AutoGenerateColumns="false" OnRowDeleting="OnRowDeleting" OnSelectedIndexChanged="gvBill_SelectedIndexChanged" OnRowDataBound = "OnRowDataBound">
                                       <AlternatingRowStyle BackColor="White" />
                                  <EditRowStyle BackColor="#2461BF" />
                                  <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                  <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                  <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                  <RowStyle BackColor="#EFF3FB" />
                                  <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                  <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                  <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                  <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                  <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    <Columns>
                                          <asp:CommandField ShowSelectButton="True" ButtonType="Button"  ControlStyle-CssClass="btn-warning" SelectText=".edit" />
                                          <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText=".del" ControlStyle-CssClass="btn-danger" />
                                  
                                          <asp:TemplateField HeaderText="Tarjeta">
                                               <ItemTemplate>
                                                    <asp:Image ImageUrl="Assets/Img/exclamation-mark.png" Height="32" Width="32" ID="Image1" ToolTip='<%#  Eval("Comments").ToString() %>' CssClass='<%# If(Eval("Comments").ToString() = "", "hidden", "show") %>' runat="server" />
                                   
                                                    <asp:Label ID="CardNumber" Text='<%#  Eval("Tarjeta").ToString() %>' runat="server" />
                                               </ItemTemplate>
                                           </asp:TemplateField>

                                             <asp:BoundField DataField="Date/Hour" HeaderText="Date/Hour" />
                           <asp:BoundField DataField="Room" HeaderText="Room" />  
                           <asp:BoundField DataField="ROOM_TYPE" HeaderText="ROOM TYPE" /> 
                           <asp:BoundField DataField="Rate" HeaderText="Rate" />
                           <asp:BoundField DataField="PayType" HeaderText="Pay Type" />
                           <asp:BoundField DataField="Tax" HeaderText="Tax" />
                            <asp:BoundField DataField="Discount" HeaderText="Discount" />
                           <asp:BoundField DataField="Promotion" HeaderText="Promotion" />
                           <asp:BoundField DataField="Payment" HeaderText="Payment" />
                                        <asp:TemplateField HeaderText="">
                               <ItemTemplate>
                                          <asp:HiddenField ID="ROOM_ID" runat="server" Value='<%# Bind("ROOM_ID") %>'></asp:HiddenField>
                               </ItemTemplate>
                           </asp:TemplateField>
                                  </Columns>
                             </asp:GridView>
                              
                        </div>
                              </div> 
                    </div>
                </div>
            </div>
        </div>
        <!-- /.row -->

      <%--  <div class="row">
            <div class="col-md-12">
                <div class="box box-info">
                   
                </div>
            </div>
        </div>--%>
    </section>
    <!-- /.content --> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">

   <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

<%--    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js" type="text/javascript"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>
<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css"
    rel="Stylesheet" type="text/css" />--%>

    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css" />
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script src="Assets/HotelManagSys.js"></script>
     <script src="Assets/loader-custom.js"></script>
    <script type="text/javascript"> 

        $(function () {

            $('.gid').focus();
            $("#asp_form").validationEngine();
            $('#<% =txtGuestName.ClientID %>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'GuestInformation.aspx/GetGuests',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[0],
                                    val: item.split('-')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("[id$=hfCustomerId]").val(i.item.val);
                },
                minLength: 3
            });
        });
        $('#<% =txtGuestID.ClientID %>').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'GuestInformation.aspx/GetGuestsIds',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item,
                                val: item
                            }
                        }))
                    },
                    error: function (response) {
                        //alert(response.responseText);
                    },
                    failure: function (response) {
                        //alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("[id$=hfCustomerId]").val(i.item.val);
            },
            minLength: 3
        });

    </script>
</asp:Content>
