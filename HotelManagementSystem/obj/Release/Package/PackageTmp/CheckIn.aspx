<%@ Page Title="Check In" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CheckIn.aspx.vb" Inherits="HotelManagementSystem.CheckIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/css/bootstrap-datetimepicker-standalone.min.css" rel="stylesheet" />
    
   <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script> 
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <script type="text/javascript">

        function showModal() {
            $("#myModal").modal('show');
        }
        function showModalInvoice() {
            //$("#myModalInvoiceInfo").modal('show');
            $("#myModalInvoiceInfo").removeClass("fade hide");
            $("#myModalInvoiceInfo").dialog({
                width: 600,
                modal: true
            });
        }
        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }
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
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
        <div class="fade hide"   id="myModalInvoiceInfo" role="dialog" title="Estado de Cuenta">
      
            <div style="border-radius:15px;">
                 
                    <h2 class="modal-title text-center">Estado de Cuenta</h2>
                
                <div id="dvInvoice">
                     
             
                    
                             <div class="form-group">
                                 <label for="">Nombre de la factura:</label> 
                                 <input  type="text" class="form-control"  id="invName" placeholder="Ingrese nombre de la factura"/>
                        </div>
                     
                       
                            <div class="form-group">
                                 <label for="">RUC o ID de la factura:</label> 
                                 <input  type="text" class="form-control"  id="invId" placeholder="Ingrese RUC o ID de la factura"/>
                        </div>
                      <div class="form-group">
                           <input  type="checkbox"  id="AsGuest" />
                                 <label for="">No mostrar el nombre del huésped</label> 
                                
                        </div>
                    </div> 
                </div>
                <div class="modal-footer"> 
                    <input type="button"   onclick="GenerateInvoice()" class="btn btn-info" value="Confirmar" /> 
                       <%-- <asp:Button ID="btnInvoiceCheckedIn" CssClass="btn btn-info" runat="server" Text="Confirm"></asp:Button>--%>
                   
                     <button type="button" class="btn btn-default pull-right" onclick="closemodal()">Cancelar</button>
                </div>
            </div>
        
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Registro de Habitación
          
            <small></small>
        </h1>
        
        <%--<ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">Check In</li>
        </ol>--%>
    </section>
     
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <!-- left column -->
            <div class="col-md-6">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                <!-- Form Element sizes -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Detalle del Huésped</h3>
                         
                        <span style="float:right;"  class="info-box-number">
                            <asp:HyperLink ID="lnkChangeRoom" style="color:white!important;margin-right:5px" CssClass="btn btn-info" runat="server" Visible="false" Enabled="false">Cambiar Habitación</asp:HyperLink>
                            <%--<a style="color:white!important;margin-right:5px" class="btn btn-info" href="RoomView.aspx?croom_id=<% =strRoomId %>"></a>--%>
                            <asp:Label ID="lblroomNo"  runat="server" Text=""></asp:Label><asp:Label ID="roomstatus" runat="server" Text=""></asp:Label></span>  
                   
                              </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="txtGuestID">ID del Huésped</label>
                                    <asp:HiddenField ID="hdnRowUpdate" runat="server" Value="0" />
                                     <asp:HiddenField ID="hdnNewPayment" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnGuestId" runat="server" />
                                    <asp:HiddenField ID="hdnRoomId" runat="server" />
                                     <asp:HiddenField ID="hdnTaxPercent" runat="server" />
                                     <asp:HiddenField ID="hdnRoomLastInfoId" runat="server" />
                                    <asp:TextBox ID="txtGuestID"  TabIndex="0" placeholder="Guest ID" CssClass="form-control gid"  AutoPostBack="true" runat="server"></asp:TextBox>
                                    <span id="spnGuestErr"></span>
                                     
                                     <asp:Label ID="errGuestId" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label for="txtGuestName">Nombre del Huésped</label>
                                    <i class="required_form_item ">* Requerido</i>
                                    <asp:TextBox ID="txtGuestName" CssClass="form-control" placeholder="Guest Name" runat="server"></asp:TextBox>
                                     <asp:Label ID="errGuest" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                         <div class="row">
                            <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtAddress">Nacionalidad</label>
                            <i class="required_form_item ">* Requerido</i>
                            <asp:TextBox ID="txtAddress" CssClass="form-control" placeholder="Nationality" runat="server"></asp:TextBox>
                         <asp:Label ID="errAddress" runat="server" Text=""></asp:Label>
                        </div>
</div>
                             <div class="col-md-6">
                                 <asp:CheckBoxList ID="CheckBoxList1"  CssClass="chkChoice" runat="server" TextAlign="Right" RepeatDirection="Horizontal" >
                                     <asp:ListItem Text="Lista VIP" Value="1" onclick="MutExChkList(this);" /> 
                                     <asp:ListItem onclick="MutExChkList(this);" Text="Lista Negra" Value="2" />
                                 </asp:CheckBoxList>
                                <asp:DropDownList CssClass="form-control" ID="ddlNormalSpecial" runat="server">
                                    <asp:ListItem Value="Normal" Text="Normal">Normal</asp:ListItem>
                                    <asp:ListItem Value="Especial" Text="Especial">Especial</asp:ListItem> 
                                </asp:DropDownList>
                             </div>
                             </div>
                        <div class="form-group">
                             <label for="txtNote">Comentario del Huésped</label> 
                            <%--<textarea id="txtNote" class="form-control" placeholder="Note" cols="20" rows="2" runat="server"></textarea>--%>
                            <textarea id="txtNote" class="form-control" maxlength="1000"  cols="20" rows="2" runat="server"></textarea>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <label for="ddlIDType">Tipo de ID</label>
                                <asp:DropDownList CssClass="form-control" ID="ddlIDType" runat="server">
                                    <asp:ListItem>Cédula</asp:ListItem>
                                    <asp:ListItem>Licencia</asp:ListItem>
                                    <asp:ListItem>Carnet del seguro</asp:ListItem>
                                    <asp:ListItem>Carnet de residente</asp:ListItem>
                                    <asp:ListItem>Pasaporte</asp:ListItem> 
                                     <asp:ListItem>Otro</asp:ListItem> 
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label for="txtIDNo">Número de Tarjeta</label>
                                <asp:TextBox ID="txtIDNo" CssClass="form-control"  placeholder="ID No"  runat="server"></asp:TextBox>
                                <span id="spnIdErr"></span>
                                <asp:Label ID="errIDNo" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <label for="ddlGender">Sexo</label>
                                <asp:DropDownList CssClass="form-control" ID="ddlGender" runat="server">
                                    <asp:ListItem>Hombre</asp:ListItem>
                                    <asp:ListItem>Mujer</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                              
                                <label for="txtPhoneNo">Número de Celular</label>
                            <asp:TextBox ID="txtPhoneNo" CssClass="form-control" placeholder="Phone No" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                        <div class="col-md-12">

                                <label for="txtArrivalDeparture">Comentario de Recamarera</label>
                               </div>
                             <div class="col-md-11">

                            <asp:TextBox ID="txtArrivalDeparture" CssClass="form-control" placeholder="Maids Comments" runat="server"></asp:TextBox>
                                 
                            </div>
                            <div class="col-md-1">
                                <asp:Image ImageUrl="Assets/Img/exclamation-mark.png" Height="32" Width="32" ID="Image1"  Visible="false" runat="server" />
                            </div>
                            </div>
                        <div class="row">
                            
                            <div class="col-md-6 hidden">
                               
                                  <label for="txtPurpose">Purpose</label>
                                <asp:TextBox ID="txtPurpose" CssClass="form-control" placeholder="Purpose" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
                            </ContentTemplate>
                           <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCheckInSubmit" EventName="Click" /> 
                                 <asp:AsyncPostBackTrigger ControlID="txtGuestID" EventName="TextChanged" /> 
                    </Triggers>
                            </asp:UpdatePanel>
                <!-- /.box -->
            </div>
            <!--/.col (left) -->
            <div class="col-md-6">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h4 class="box-title">Detalle de la Habitación</h4>
                          <span style="float:right; padding-bottom:3px;"  class="info-box-number"> 
              
              <asp:Button ID="lnkBlockRoom" UseSubmitBehavior="false"  style="color:white!important;background-color:black" CssClass="btn btn-info" CausesValidation="false" runat="server"  Text="Bloquear Hab." />
              <asp:HiddenField ID="hdnStatusId" runat="server" />
       </span>
                         <span> <b id="remaintext" runat="server" visible="false">             Quedan:</b>
                            <asp:Label ID="lblremaintime"  runat="server" Text=""></asp:Label></span>  
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            
                    <div class="box-body">

                        <div class="row">
                            <div class="col-md-6">
                                  <label for="ddlHours">¿Cuánto tiempo?</label>  <i class="required_form_item "> * Requerido</i>
                             <asp:DropDownList Width="100%"  CssClass="form-control select2" ID="ddlHours" runat="server" ClientIDMode="Static" /> 
                                 <%--validate[required,custom[integer],minSize[1]]--%>
    <asp:Label ID="errDdlHours" runat="server" Text=""></asp:Label>
                            </div>
                             <div class="col-md-6">
                                  <label for="ddlPromotion">Promoción</label> 
                             <asp:DropDownList Width="100%" CssClass="form-control" ID="ddlPromotion" runat="server" ClientIDMode="Static" /> 
                                <asp:HiddenField ID="totalhours" runat="server" Value="0" />
                                <asp:HiddenField ID="currentTime" runat="server" Value="0" />
                            </div>
                            
                           
                            
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label for="txtCheckInDate">Hora de Entrada</label>
                               <%-- <div class="input-group">
                                    <asp:TextBox ID="txtCheckInDate" CssClass="form-control datepicker validate[required]"  runat="server"></asp:TextBox>
                                    <span class="input-group-addon"><span class="add-on"><i class="fa  fa-calendar"></i></span></span>
                                </div>--%>
                                  <div class="form-group">
                                  <div class='input-group date' id='datetimepicker1' onblur="SetCheckOut()">
               <asp:TextBox ID="txtCheckInDate" CssClass="form-control validate[required]"   runat="server" AutoPostBack="True"></asp:TextBox>
               <span class="input-group-addon">
               <span class="glyphicon glyphicon-calendar"></span>
               </span>
            </div></div>
                            </div>
                              <div class="col-md-6">
                                <label for="txtCheckOutDate">Hora de Salida</label>
                                    <div class="form-group">
                                

  <div class='input-group date' id='datetimepicker2' onblur="ValidateCheckOut()">
               <asp:TextBox ID="txtCheckOutDate" CssClass="form-control"  onkeyup="ValidateCheckOut()" runat="server"></asp:TextBox>
               <span class="input-group-addon">
               <span class="glyphicon glyphicon-calendar"></span>
               </span>
 <asp:Label ID="errCheckout" runat="server" Text=""></asp:Label>
                            
            </div>



                                        </div>
                            </div>
                        </div>
                        <div class="row">
                             <div class="col-md-4 hidden">
                                <label for="txtCheckInTime">Hora de Entrada</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtCheckInTime" CssClass="form-control timepicker" runat="server"></asp:TextBox>
                                    <span class="input-group-addon"><span class="add-on"><i class="fa  fa-clock-o"></i></span></span>
                                </div>
                            </div>
                          
                            <div class="col-md-4 hidden">
                                <label for="txtCheckOutTime">Hora de Salida</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtCheckOutTime" CssClass="form-control timepicker" runat="server"></asp:TextBox>
                                    <span class="input-group-addon"><span class="add-on"><i class="fa  fa-clock-o"></i></span></span>
                                </div>
                            </div>
                           
                        </div>
                        <br />
                        <div class="box-header with-border">
                            <h3 class="box-title">Detalle de Pago</h3>
                        </div>
                          <div class="row">
                            <div class="col-md-2">
                                 <label for="txtRoomRent">Tarifa</label>
                                <asp:TextBox ID="txtRoomRent" CssClass="form-control" placeholder="Rent" runat="server" AutoPostBack="True"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                      <label for="txtRoomTax">ITBMS</label>
                                <asp:TextBox ID="txtRoomTax" CssClass="form-control" placeholder="Tax" runat="server" AutoPostBack="True"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label for="txtRoomDiscount">Descuento</label>
                                <asp:TextBox ID="txtRoomDiscount" CssClass="form-control" placeholder="Descuento" runat="server" AutoPostBack="True"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                     <label for="txtRentTotal">Total</label>
                                <asp:TextBox ID="txtRentTotal" CssClass="form-control" placeholder="Total" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                     <label for="ddlPayType">Forma de Pago</label>
                                <i class="required_form_item ">* Requerido</i>
                                <asp:DropDownList ID="ddlPayType" Width="100%" CssClass="form-control select2" ClientIDMode="Static" runat="server">
                                    <%--validate[required,custom[integer],minSize[1]]--%>
                                   <asp:ListItem Text="Seleccione forma" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Efectivo" Value="1"></asp:ListItem>
                                     <asp:ListItem Text="Visa" Value="2"></asp:ListItem>
                                     <asp:ListItem Text="MasterCard" Value="3"></asp:ListItem>
                                </asp:DropDownList>
 <asp:Label ID="errPayType" runat="server" Text=""></asp:Label>                          
                                </div>
                              
                        <div class="col-md-12" style="padding-top:5px;">
                             <div class="row"> 
                                 <div class="col-md-11" style="overflow:auto;margin-left:15px;">
                                     
                            <asp:GridView ID="gvBill"   Width="100%" HeaderStyle-Font-Size="Smaller" RowStyle-Font-Size="Small"   CssClass="table table-responsive"  ShowFooter="true"    OnRowDataBound = "OnRowDataBound"    OnSelectedIndexChanged="gvBill_SelectedIndexChanged" runat="server" AutoGenerateColumns="true"   OnRowDeleting="OnRowDeleting">
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
                                      <asp:TemplateField HeaderText="" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button ID="btngvselect" runat="server" CausesValidation="false"
                                                CommandName="Select" Text=".editar"  ControlStyle-CssClass="btn-warning" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                              <%--     <asp:TemplateField InsertVisible="false">  
                    <ItemTemplate>  
                        <asp:Button ID="btn_Edit" runat="server" Text=".editar" CommandName="Edit" ControlStyle-CssClass="btn-warning"   />  
                    </ItemTemplate>  
                                       </asp:TemplateField>--%>
                                                                            <%-- <asp:CommandField ShowSelectButton="True" ButtonType="Button"   ControlStyle-CssClass="btn-warning" SelectText=".editar" />--%>
                                        <%--<asp:CommandField  ButtonType="Button" ShowCancelButton="true"  CancelText="cancel"  ControlStyle-CssClass="btn-default" />
                                                <asp:CommandField  ButtonType="Button" UpdateText="update"     ControlStyle-CssClass="btn-info" />--%>
                                      <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText=".borrar" ControlStyle-CssClass="btn-danger" />
                                     
                                  </Columns>
                             </asp:GridView>
                                 </div>
                                 <div class="col-md-1"></div>
                             </div>
                              
                        </div>
                              </div>
                        <div class="row hidden">
                            <div class="col-md-4">
                                <label for="ddlRoomNo"># Hab.</label>
                                <i class="required_form_item ">* Requerido</i>
                                <asp:DropDownList ID="ddlRoomNo" Width="100%" CssClass="form-control select2 validate[required,custom[integer],minSize[1]]" OnChange="GetSelectedRoomInfo(this);" ClientIDMode="Static" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label for="txtRoomType">Tipo Hab.</label>
                                <asp:TextBox ID="txtRoomType" CssClass="form-control" placeholder="Room Type" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtRentnDay">Tarifa/Fecha</label>
                                <asp:TextBox ID="txtRentnDay" CssClass="form-control validate[required]" placeholder="Rent/Day" runat="server" Text="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row hidden">
                            <div class="col-md-4">
                                <label for="txtTotalCharges">Total Charges</label>
                                <asp:TextBox ID="txtTotalCharges" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtNoOfAdult">No. Of Adult</label>
                                <asp:TextBox ID="txtNoOfAdult" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtNoOfChildren">No. Of Children</label>
                                <asp:TextBox ID="txtNoOfChildren" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                            </div>
                        </div>

                        <div class="box-header with-border hidden">
                            <h3 class="box-title">Extra Bed Information</h3>
                        </div>
                        <div class="row hidden">
                            <div class="col-md-4">
                                <label for="txtExtraBeds">Extra Beds</label>
                                <asp:TextBox ID="txtExtraBeds" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtPerBedCost">Per Bed Cost</label>
                                <asp:TextBox ID="txtPerBedCost" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="txtTotalExtraBedCost">Total Extra Bed Cost</label>
                                <asp:TextBox ID="txtTotalExtraBedCost" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                        </ContentTemplate>
                          <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlHours" EventName="SelectedIndexChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="ddlPromotion" EventName="SelectedIndexChanged" />
                    </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!-- /.row -->

        <div class="row">
            <div class="col-md-12">
                <div class="box box-info">
                    <div class="box-footer pull-right">
                        <input type="button"  onclick="showModalInvoice()" class="btn btn-info" value="Factura" />
                       
                         <asp:Button ID="btnCheckOut" CssClass="btn btn-info" runat="server" Enabled="false" Text="Salida"></asp:Button>
                         <asp:Button ID="btnAddNew" CssClass="btn btn-info" runat="server" Text="Añadir" Visible="false"></asp:Button>
                        <asp:Button ID="btnCheckInSubmit" CssClass="btn btn-info" runat="server"
                             
                            Text="Guardar"></asp:Button>
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-info" NavigateUrl="~/RoomView.aspx">Cerrar</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
    <div class="col-md-4 hidden">
                                <label for="txtNoOfDay">No Of Day 
     </label>
                              <asp:TextBox ID="txtNoOfDay" CssClass="form-control validate[required]" placeholder="No Of Day" runat="server"></asp:TextBox>
                            </div>
         
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">

    
   <%-- <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js" type="text/javascript"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>
<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css"
    rel="Stylesheet" type="text/css" />--%>

     <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
      


    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css" />
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/js/bootstrap-datetimepicker.min.js"></script>
    
    <%--<script src="Assets/date.format.js"></script>--%>
    <script src="Assets/HotelManagSys.js"></script>
    <%--<script src="Assets/loader-custom.js"></script>--%> 
    <script type="text/javascript">



        $(function () {
            
            $('.gid').focus();
            //debugger;
            //_tday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"),
            //    _tmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"),
            //    //d = new Date, d.toLocaleString('en-US', { hour: 'numeric', hour12: false }), nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear(), nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), nyear < 1e3 && (nyear += 1900), 0 == nhour ? (ap = " AM", nhour = 12) : nhour <= 11 ? ap = " AM" : 12 == nhour ? ap = " PM" : nhour >= 13 && (ap = " PM", nhour -= 12), nmin <= 9 && (nmin = "0" + nmin), nsec <= 9 && (nsec = "0" + nsec), ndate <= 9 && (ndate = "0" + ndate);
            //    d = new Date, d.toLocaleString('en-US', { hour: 'numeric', hour12: false }), _nday = d.getDay(), _nmonth = d.getMonth(), _ndate = d.getDate(), _nyear = d.getYear(), _nhour = d.getHours(), _nmin = d.getMinutes(), _nmin <= 9 && (_nmin = "0" + _nmin), _nsec = d.getSeconds(), _nsec <= 9 && (_nsec = "0" + _nsec), _nyear < 1e3 && (_nyear += 1900), _ndate <= 9 && (_ndate = "0" + _ndate);
            //_monthNum = (parseInt(_nmonth) + 1), _monthNum <= 9 && (_monthNum = "0" + _monthNum);
            // _nhour = (parseInt(_nhour)), _nhour <= 9 && (_nhour = "0" + _nhour);
            //_nmin = (parseInt(_nmin)), _nmin <= 9 && (_nmin = "0" + _nmin);
            //_nsec = (parseInt(_nsec)), _nsec <= 9 && (_nsec = "0" + _nsec);
            //_ndate = (parseInt(_ndate)), _ndate <= 9 && (_ndate = "0" + _ndate);

            let now = new Date();
            var dateStringWithTime = moment(now).format('DD/MM/YYYY HH:mm:ss');
            var checkIn = $('#<% =txtCheckInDate.ClientID %>').val();
            var checkOut = $('#<% =txtCheckOutDate.ClientID %>').val();

            if (checkIn == "" && checkOut == "") {
               <%-- $('#<% =txtCheckInDate.ClientID %>').val(_ndate + "/" + _monthNum + "/" + _nyear + " " + _nhour + ":" + _nmin + ":" + _nsec);
                $('#<% =txtCheckOutDate.ClientID %>').val(_ndate + "/" + _monthNum + "/" + _nyear + " " + _nhour + ":" + _nmin + ":" + _nsec);--%>
                $('#<% =txtCheckInDate.ClientID %>').val(dateStringWithTime);
                $('#<% =txtCheckOutDate.ClientID %>').val(dateStringWithTime);

                checkIn = $('#<% =txtCheckInDate.ClientID %>').val();
                checkOut = $('#<% =txtCheckOutDate.ClientID %>').val();
            }

            $("#ddlHours").change(function () {
                $('#datetimepicker1').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });

                var datetimesplitted = checkIn.split('/');
                var years = datetimesplitted[2].split(' ');
                var days = datetimesplitted[1] + "/" + datetimesplitted[0] + "/" + years[0] + " " + years[1];

                //debugger;

                var mindat = moment(days).format('MM/DD/YYYY HH:mm:ss');
                $('#datetimepicker2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss', minDate: mindat });

                var codatetimesplitted = checkIn.split('/');
                var coyears = codatetimesplitted[2].split(' ');
                var totalhour = $('#<% =totalhours.ClientID %>').val();
            //var cohrs = parseInt(coyears[0]) + parseInt(totalhour);
            var codays = codatetimesplitted[1] + "/" + codatetimesplitted[0] + "/" + coyears[0] + " " + coyears[1];
            var hourselect = $("#ddlHours option:selected").text();
            if (hourselect !== 'Please select hour(s)') {
                totalhour = parseInt(totalhour) + parseInt(hourselect);
            }

            var outdt = moment(codays).add(parseInt(totalhour),'hours').format('DD/MM/YYYY HH:mm:ss');
            //if (hourselect > 0) { alert(hourselect);}
            //else { 
                $('#<% =txtCheckOutDate.ClientID %>').val(outdt);
            }
            );

            $('#<% =btnCheckInSubmit.ClientID %>').click(function () {
                let now = new Date();
                var dateStringWithTime = moment(now).format('DD/MM/YYYY HH:mm:ss');
                $('#<% =currentTime.ClientID %>').val(dateStringWithTime);
            });

            
           /* }*/
            
           // var datetimesplitted = checkIn.split('/');
           // var years = datetimesplitted[2].split(' ');
            //var days = datetimesplitted[0] + "/" + datetimesplitted[1] + "/" + years[0] + " " + years[1];
            // var days = datetimesplitted[0] + "/" + datetimesplitted[1] + "/" + years[0] + " " + years[1];

            $("#asp_form").validationEngine();
            $('[id*=lnkBlockRoom]').bind("click", function () {
                $("#asp_form").validationEngine('detach');
            });
            $('[id*=HyperLink5]').bind("click", function () {
                $("#asp_form").validationEngine('detach');
            });

       <%--     $('#<% =txtGuestID.ClientID %>').keyup(function () {
              
                var guestid = $('#<% =txtGuestID.ClientID %>').val();
                if (guestid != "") {
                    $.ajax({
                        url: 'GuestInformation.aspx/CheckGuests',
                        data: "{ 'GuestId': '" + guestid + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == '1') {
                              
                            }
                            else { 
                            }
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
             
               
            });
        
            $('#<% =txtGuestID.ClientID %>').change(function () {
                debugger;
                var guestid = $('#<% =txtGuestID.ClientID %>').val();
                if (guestid != "") {
                    $.ajax({
                        url: 'GuestInformation.aspx/CheckGuests',
                        data: "{ 'GuestId': '" + guestid + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == '1') { 
                            }
                            else { 
                            }
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                  }


            });--%>

            $('#<% =txtIDNo.ClientID %>').focusout(function () {
                
                var idno = $('#<% =txtIDNo.ClientID %>').val();
                if (idno != "" && idno.length >= 3) {
                    $.ajax({
                        url: 'GuestInformation.aspx/CheckGuestIDNo',
                        data: "{ 'IdNO': '" + idno + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == '1') {
                               // $('#<% =txtIDNo.ClientID %>').val('');
                                $('#spnIdErr').html("" + idno + " already exists!");
                                $('#spnIdErr').addClass("errorG");
                            }
                            else {
                                $('#spnIdErr').html("");
                                $('#spnIdErr').removeClass("errorG");
                            }
                        },
                        error: function (response) {
                            //alert(response.responseText);
                        },
                        failure: function (response) {
                            //alert(response.responseText);
                        }
                    });
                }

            });

            //Dim CheckIn_Date As Date = DateTime.ParseExact(txtCheckInDate.Text, "d/M/yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None)
            //txtCheckOutDate.Text = Format(CheckIn_Date.AddHours(ConvertingNumbers(totalhours.Value)), "d/M/yyyy h:mm tt")
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

<%--
            var checkin = $('#<% =txtCheckInDate.ClientID %>').val();
            $('#datetimepicker2').data("DateTimePicker").minDate(checkin);--%>
        });
       <%-- $('#datetimepicker1').datetimepicker().on('changeDate', function (ev) {
          
            $('#<% =txtCheckInDate.ClientID %>').change();
            SetCheckOut();
        });--%>
      function SetCheckOut() {
            //debugger;
            var chein = $('#<% =txtCheckInDate.ClientID %>').val();
            var totalhour = $('#<% =totalhours.ClientID %>').val();
              var datetimesplitted = chein.split('/');
            var years = datetimesplitted[2].split(' ');
            var days = datetimesplitted[1] + "/" + datetimesplitted[0] + "/" + years[0] + " " + years[1];

            //if (parseInt(datetimesplitted[0]) > 12) {
            //     days = datetimesplitted[1] + "/" + datetimesplitted[0] + "/" + years[0] + " " + years[1];

            //}

            let dayobj = new Date(days);

            if (parseInt(totalhour) > 0) {
               // dayobj.setHours(dayobj.getHours() + parseInt(totalhour));
                var datewithTime = moment(dayobj).add(parseInt(totalhour),'hours').format('DD/MM/YYYY HH:mm:ss');
                var datetimesplitted = datewithTime.split('/');
                var years = datetimesplitted[2].split(' ');
                var days = datetimesplitted[1] + "/" + datetimesplitted[0] + "/" + years[0] + " " + years[1];
                var mindat = moment(days).format('MM/DD/YYYY HH:mm:ss');
                $('#datetimepicker2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss', minDate: mindat });
                var outdat = moment(days).format('DD/MM/YYYY HH:mm:ss');
                $('#<% =txtCheckOutDate.ClientID %>').val(outdat);
                chein = outdat;
                console.log(chein);
            }
            else {
                $('#<% =txtCheckOutDate.ClientID %>').val(chein);
               }

            var datetimesplitted = chein.split('/');
            var years = datetimesplitted[2].split(' ');
            var days = datetimesplitted[1] + "/" + datetimesplitted[0] + "/" + years[0] + " " + years[1];
           //// $('#datetimepicker2').datetimepicker('destroy'); 
              //$('#datetimepicker2').datetimepicker({ format: 'DD/MM/YYYY hh:mm a', minDate: new Date(days) });
            console.log(days);
            var mindat = moment(days).format('MM/DD/YYYY HH:mm:ss')
            $('#datetimepicker2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss', minDate: mindat });
        }
        function ValidateCheckOut() { 
            var chein = $('#<% =txtCheckInDate.ClientID %>').val();
            var checkout = $('#<% =txtCheckOutDate.ClientID %>').val();
            var datetimesplitted = chein.split('/');
            var years = datetimesplitted[2].split(' ');
            var days = datetimesplitted[1] + "/" + datetimesplitted[0] + "/" + years[0] + " " + years[1];
            let now = new Date(days);
            var dayobj = moment(now).format('DD/MM/YYYY HH:mm:ss');
            //var datetimesplitted = chein.split('/');
            //var years = datetimesplitted[2].split(' ');
            //var days = datetimesplitted[0] + "/" + datetimesplitted[1] + "/" + years[0] + " " + years[1];
            ////$('#datetimepicker2').datetimepicker('destroy');
            $('#datetimepicker2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss', minDate: dayobj });
            //var formatted = dateFormat(chein, "dd/MM/yyyy");
            //var formattedtim = dateFormat(chein, "h:MM TT");
            //var formatted2 = dateFormat(checkout, "dd/MM/yyyy");
            //var formattedtim2 = dateFormat(checkout, "h:MM TT");
            //var dayobj = new Date(formatted);
            //var dayobjout = new Date(formatted2);
            //monthNum = (parseInt(dayobj.getMonth()));
            //monthNum2 = (parseInt(dayobjout.getMonth()));
            <%--if ((chein != "" && checkout != "") && Date.parse(chein) > Date.parse(checkout)) {
               // alert("To date should be greater than From date.");
               
                $('#<% =txtCheckOutDate.ClientID %>').val(chein);
               return false;
            }--%>
            <%--if ((chein != "" && checkout != "") && monthNum2 < monthNum) {
                // alert("To date should be greater than From date.");

                $('#<% =txtCheckOutDate.ClientID %>').val(chein);
                  return false;
              }--%>
            //var dayobj = new Date(chein);
            //var dayobjout = new Date(checkout);

            //    var formatted = dateFormat(dayobj, "dd/MM/yyyy");
            //    var formattedtim = dateFormat(dayobj, "h:MM TT");
            //    tday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"),
            //        tmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"),
            //        nday = dayobj.getDay(), nmonth = dayobj.getMonth(), ndate = dayobj.getDate(), nyear = dayobj.getYear(), nhour = dayobj.getHours(), nmin = dayobj.getMinutes(), nsec = dayobj.getSeconds(), nyear < 1e3 && (nyear += 1900), 0 == nhour ? (ap = " AM", nhour = 12) : nhour <= 11 ? ap = " AM" : 12 == nhour ? ap = " PM" : nhour >= 13 && (ap = " PM", nhour -= 12), nmin <= 9 && (nmin = "0" + nmin), nsec <= 9 && (nsec = "0" + nsec), ndate <= 9 && (ndate = "0" + ndate);
            //monthNum = (parseInt(nmonth)), monthNum <= 9 && (monthNum = "0" + monthNum);

            //var date1 = new Date(chein);
            //var date2 = new Date(checkout);

            //var formatOut = dateFormat(dayobjout, "dd/MM/yyyy");
            //var formatOuttim = dateFormat(dayobjout, "h:MM TT");
            //Otday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"),
            //    Otmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"),
            //    Onday = dayobjout.getDay(), Onmonth = dayobjout.getMonth(), Ondate = dayobjout.getDate(), Onyear = dayobjout.getYear(), Onhour = dayobjout.getHours(), Onmin = dayobjout.getMinutes(), Onsec = dayobjout.getSeconds(), Onyear < 1e3 && (Onyear += 1900), 0 == Onhour ? (ap = " AM", Onhour = 12) : Onhour <= 11 ? ap = " AM" : 12 == Onhour ? ap = " PM" : Onhour >= 13 && (ap = " PM", Onhour -= 12), Onmin <= 9 && (Onmin = "0" + Onmin), Onsec <= 9 && (Onsec = "0" + Onsec), Ondate <= 9 && (Ondate = "0" + Ondate);
            //OmonthNum = (parseInt(Onmonth)), OmonthNum <= 9 && (OmonthNum = "0" + OmonthNum);

           <%-- if (date1.getTime() > date2.getTime()) {

                $('#<% =txtCheckOutDate.ClientID %>').val(chein);
            }

            $('#datetimepicker2').data("DateTimePicker").minDate(date1);--%>
        }
        function closemodal() {
            $("#myModalInvoiceInfo").dialog('close');
        }
    </script>
</asp:Content>
