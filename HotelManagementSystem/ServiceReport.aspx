<%@ Page Title="Reportes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ServiceReport.aspx.vb" Inherits="HotelManagementSystem.ServiceReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>--%>
    <%--<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>--%>  
    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }
        //function GetClock() { d = new Date, nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear(), nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), nyear < 1e3 && (nyear += 1900), 0 == nhour ? (ap = " AM", nhour = 12) : nhour <= 11 ? ap = " AM" : 12 == nhour ? ap = " PM" : nhour >= 13 && (ap = " PM", nhour -= 12), nmin <= 9 && (nmin = "0" + nmin), nsec <= 9 && (nsec = "0" + nsec), document.getElementById("clockbox").innerHTML = tday[nday] + ", " + tmonth[nmonth] + " " + ndate + ", " + nyear + " " + nhour + ":" + nmin + ":" + nsec + ap, setTimeout("GetClock()", 1e3) } tday = new Array("Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"), tmonth = new Array("enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"), window.onload = GetClock;
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
       Reportes
      </h1>
     <%-- <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Reportes</li>
      </ol>--%>
    </section>



 <!-- Main content -->
    <section class="content">
      
        <!--/.col (left) -->
        <div class="row">
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Selecciona el Rango de Fechas</h3>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label for="txtDateFrom">Desde</label>
                       <%--   <label for="txtDateFrom">Date From</label>
                          <div class="input-group">
                            <asp:TextBox ID="txtDateFrom" CssClass="form-control datepicker validate[required]"  runat="server"></asp:TextBox>
                            <span class="input-group-addon"> <span class="add-on"><i class="fa  fa-calendar"></i></span> </span>
                          </div>--%>
                                          <div class='input-group date' id='datetimepicker1'>
               <asp:TextBox ID="txtDateFrom" CssClass="form-control validate[required]"   runat="server"></asp:TextBox>
               <span class="input-group-addon">
               <span class="glyphicon glyphicon-calendar"></span>
               </span>
            </div>
                        </div>

                        <div class="col-md-6">
                            <label for="txtDateTo">Hasta</label>
                         <%-- <label for="txtDateTo">Date To</label>
                          <div class="input-group">
                            <asp:TextBox ID="txtDateTo" CssClass="form-control datepicker validate[required]"  runat="server"></asp:TextBox>
                            <span class="input-group-addon"> <span class="add-on"><i class="fa  fa-calendar"></i></span> </span>
                          </div>--%>
                                 <div class='input-group date' id='datetimepicker2'>
               <asp:TextBox ID="txtDateTo" CssClass="form-control validate[required]"   runat="server"></asp:TextBox>
               <span class="input-group-addon">
               <span class="glyphicon glyphicon-calendar"></span>
               </span>
            </div>
                        </div>
                    </div>
                    <div class="row">
                         <div class="col-md-8">
                         <label for="ddlUsers">Recepcionista</label> 
                             <asp:DropDownList Width="100%" CssClass="form-control" ID="ddlUsers" runat="server" ClientIDMode="Static" /> 
                             </div>
                    </div>
                </div>
                <div class="box-footer">
                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="btn btn-info" NavigateUrl="~/ServiceReport.aspx">Reiniciar</asp:HyperLink>
                </div>
            </div>
        </div>
             <div class="col-md-6">
            <div class="box box-info">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnCheckInRpt" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Reporte de Habitaciones"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnDirtyRpt" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Reporte de Habitaciones Sucias"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnGuestRpt" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Reporte de Huéspedes"></asp:Button>
                        </div>
                    </div><br />
                </div>
            </div>
        </div>

   

      </div>
      <!-- /.row -->
        <div class="row">
               <div class="col-md-12" >
              <div class="box box-info" style="overflow:auto;">
                <div class="box-body">
                     
                          <div class="row">
                              <div class="col-md-8">
                                  <h3>
                                      <asp:Label ID="reportTitle" runat="server" Text="Reporte de Habitaciones"></asp:Label>
                                   </h3>
                              </div>
                              <div class="col-md-4"> 
                                   <asp:Panel ID="pnlFilter" Visible="false" runat="server">
                                   <div class="form-inline pull-right">
                                     <label for="ddlNormalSpecial">Filter :</label>              
                  <asp:DropDownList CssClass="form-control" ID="ddlNormalSpecial" runat="server" ClientIDMode="Static">
                       <asp:ListItem Value="ALL" Text="ALL">Todas</asp:ListItem>
                                    <asp:ListItem Value="Normal" Text="Normal">Normal</asp:ListItem>
                                    <asp:ListItem Value="Especial" Text="Especial">Especial</asp:ListItem> 
                      <asp:ListItem Value="Ventas" Text="Ventas">Ventas</asp:ListItem> 
                                </asp:DropDownList> </div>
                                       
                      </asp:Panel>
                              </div> 



                              </div>
          <asp:GridView ID="gvReport" Width="100%"  CssClass="table table-bordered table-condensed table-responsive" runat="server"  OnRowDeleting="OnRowDeleting" OnSelectedIndexChanged="gvReport_SelectedIndexChanged" OnRowDataBound = "OnRowDataBound" CellPadding="3" GridLines="Vertical" BackColor="White" ShowFooter="true" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">
              <AlternatingRowStyle BackColor="#DCDCDC" />
              <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
              <HeaderStyle BackColor="#48a5db" Font-Bold="True" ForeColor="White" />
              <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
              <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
              <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
              <SortedAscendingCellStyle BackColor="#F1F1F1" />
              <SortedAscendingHeaderStyle BackColor="#0000A9" />
              <SortedDescendingCellStyle BackColor="#CAC9C9" />
              <SortedDescendingHeaderStyle BackColor="#000065" />
               <Columns>
                                      <asp:CommandField ShowSelectButton="True" ButtonType="Button"  ControlStyle-CssClass="btn-warning" SelectText=".edit" />
                                      <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText=".del" ControlStyle-CssClass="btn-danger" />
                                  </Columns>
                    </asp:GridView>
                    </div>
                  </div>
         <div class="row">
            <div class="col-md-12">
                <asp:Panel ID="pnlExport" Visible="false" runat="server">
                     <div class="box box-info">
                    <div class="box-footer pull-right">
                        <asp:HiddenField ID="hdnreporttype" runat="server" />
                         <asp:Button ID="btnInvoice" CssClass="btn btn-info" runat="server" Text="Imprimir"></asp:Button>
                         <asp:Button ID="btnExport" CssClass="btn btn-info" runat="server"  Text="Exportar CSV"></asp:Button> 
                    </div>
                </div>
                </asp:Panel>
               
            </div>
        </div>
      </div> 
        </div>
    <div class="row hidden">
        <!--/.col (left) -->
        <div class="col-md-4">
            <div class="box box-info">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <label for="txtGuestID">ID</label><i class="required_form_item"> * Requerido</i>
                             <div class="input-group">
                                 <asp:HiddenField ID="hdnGuestId" runat="server" />
                                    <asp:TextBox ID="txtGuestID" TextMode="Number" CssClass="form-control validate[required,funcCall[NumberFormat[]]" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnFindGuestAllInfo" CssClass="btn btn-info btn-flat" runat="server" Text="Previo de Detalles" />
                                    </span>
                              </div>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportPaidBillList" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Paid Bill List"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportUnPaidBillList" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="UnPaid Bill List"></asp:Button>
                        </div>
                    </div><br />
                </div>
            </div>
        </div>
          <!--/.col (left) -->
        <div class="col-md-4">
            <div class="box box-info">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportCheckInList" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Check In List"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportCheckOutList" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Check Out List"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportCancelList" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Cancel List"></asp:Button>
                        </div>
                    </div><br />
                </div>
            </div>
        </div>


        <!--/.col (left) -->
        <div class="col-md-4">
            <div class="box box-info">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportReservationList" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Reservation List"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportChangeRoomHistory" CssClass="btn btn-info btn-block btn-flat" runat="server" Text="Change Room History"></asp:Button>
                        </div>
                    </div> <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnServiceReportExtraBed" CssClass="btn btn-info btn-flat btn-block" runat="server" Text="Extra Bed  Report"></asp:Button>
                        </div>
                    </div><br />
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
       <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/js/bootstrap-datetimepicker.min.js"></script>
    
    <script src="Assets/date.format.js"></script>
    <script src="Assets/HotelManagSys.js"></script>
     <script src="Assets/loader-custom.js"></script>
   
        <script type="text/javascript">
            $(function () {
                $("#asp_form").validationEngine(); 
                _tday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"),
                    _tmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"),
                    _d = new Date, _d.toLocaleString('en-US', { hour: 'numeric', hour12: false }), _nday = _d.getDay(), _nmonth = _d.getMonth(), _ndate = _d.getDate(), _nyear = _d.getYear(), _nhour = _d.getHours(), _nmin = _d.getMinutes(), _nsec = _d.getSeconds(), _nyear < 1e3 && (_nyear += 1900), _ndate <= 9 && (_ndate = "0" + _ndate);
                //d = new Date, d.toLocaleString('en-US', { hour: 'numeric', hour12: false }), nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear(), nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), nyear < 1e3 && (nyear += 1900), 0 == nhour ? (ap = " AM", nhour = 12) : nhour <= 11 ? ap = " AM" : 12 == nhour ? ap = " PM" : nhour >= 13 && (ap = " PM", nhour -= 12), nmin <= 9 && (nmin = "0" + nmin), nsec <= 9 && (nsec = "0" + nsec), ndate <= 9 && (ndate = "0" + ndate);
                _monthNum = (parseInt(_nmonth) + 1), _monthNum <= 9 && (_monthNum = "0" + _monthNum);
                var checkIn = $('#<% =txtDateFrom.ClientID %>').val();
            var checkOut = $('#<% =txtDateTo.ClientID %>').val();


            if (checkIn == "" && checkOut == "") {
               <%-- $('#<% =txtDateFrom.ClientID %>').val(ndate + "/" + monthNum+ "/" + nyear + " " + nhour + ":" + nmin + ap);
                $('#<% =txtDateTo.ClientID %>').val(ndate + "/" + monthNum + "/" + nyear + " " + nhour + ":" + nmin + ap);--%>
                $('#<% =txtDateFrom.ClientID %>').val(_ndate + "/" + _monthNum + "/" + _nyear + " " + _nhour + ":" + _nmin + ":" +_nsec);
                $('#<% =txtDateTo.ClientID %>').val(_ndate + "/" + _monthNum + "/" + _nyear + " " + _nhour + ":" + _nmin + ":" + _nsec); 
                 checkIn = $('#<% =txtDateFrom.ClientID %>').val();
                checkOut = $('#<% =txtDateTo.ClientID %>').val();


            } 
            var datetimesplitted = checkIn.split('/');
            var years = datetimesplitted[1].split(' ');
              //  var days = years[0] + "-" + datetimesplitted[1] + "-" + datetimesplitted[0] + " " + years[1];
                var days = datetimesplitted[0] + "/" + datetimesplitted[1] + "/" + years[0];
            $('#datetimepicker1').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });

                $('#datetimepicker2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });//, minDate: new Date(days) 
            });


           
        </script> 
</asp:Content>
