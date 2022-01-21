<%@ Page Title="Lista de la Huéspedes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="GuestList.aspx.vb" Inherits="HotelManagementSystem.GuestList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
         <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/css/bootstrap-datetimepicker-standalone.min.css" rel="stylesheet" />
    
   <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script> 
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <script type="text/javascript">



        $('#<% =txtSearch.ClientID %>').autocomplete({
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

                    },
                    failure: function (response) {

                    }
                });
            },
            select: function (e, i) {
                $("[id$=hfCustomerId]").val(i.item.val);
            },
            minLength: 3
        });
        $(document).ready(function () {



            $(".gvv").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                'paging': true,
                'lengthChange': true,
                'searching': false,
                'ordering': true,
                'info': true,
                'autoWidth': true,
                "language": {
                    "paginate": {
                        "previous": "Previos",
                        "next":"Siguiente"
                    },
                    "lengthMenu": "Mostrar _MENU_ huéspedes",
                    "info":"Mostraring _START_ to _END_ of _TOTAL_ huéspedes"
                }
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
       Lista de la Huéspedes
      </h1>
     <%-- <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Dashboard.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Lista de los Huéspedes</li>
      </ol>--%>

         <div class="col-md-12" style="padding-bottom:5px">
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-8">
                     <div class="form-inline pull-right">
                      <label for="txtSearch">Search :</label>
                 <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="type guest name or id" runat="server" AutoPostBack="True"></asp:TextBox>
                         <a href="GuestList.aspx" title="refresh searching"><i class="fa fa-refresh"></i></a>
            </div>
                </div>
            </div>
           
        </div>
    </section>


   <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-12">

          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-header with-border">
                <div class="row">
                    <div class="col-md-4">
                         <h3 class="box-title">Detalle de los Huéspedes</h3>
                    </div>
                    <div class="col-md-6">
                        <span style="text-align:right;"  class="info-box-number"> 
                            <asp:Label ID="lblCount"  runat="server" Text=""></asp:Label></span>
                 
                         <span style="text-align:right;"  class="info-box-number"> 
                            <asp:Label ID="lblCount2"  runat="server" ForeColor="#009933" Text=""></asp:Label></span>
 
                    </div>
                    <div class="col-md-2">
                        
               <div class="form-group">
                <div class='input-group date' id='datetimepicker1'>
               <asp:TextBox ID="txtDate" CssClass="form-control"    runat="server"></asp:TextBox>
               <span class="input-group-addon">
               <span class="glyphicon glyphicon-calendar"></span>
               </span>
            </div> 
     </div>
                    </div>
                </div>
             
                </div>
           <div class="box-body">
               <div class="table-responsive">
                   <asp:GridView ID="GridView1" CssClass="gvv table table-bordered table-striped table-responsive" runat="server" AutoGenerateColumns="false">
                       <Columns>
                           <asp:BoundField DataField="GEST_ID" HeaderText="ID" />
                           <asp:BoundField DataField="GuestName" HeaderText="Nombre del Huésped" />
                           <asp:BoundField DataField="Gender" HeaderText="Sexo" />  
                           <asp:BoundField DataField="PhoneNo" HeaderText="Célular" /> 
                           <asp:BoundField DataField="Nationality" HeaderText="Nacionalidad" />
                           <asp:BoundField DataField="Social_Type" HeaderText="Lista" />
                           <asp:BoundField DataField="Note" HeaderText="Comentarios del Huésped" />
                           
                         <%--  <asp:BoundField DataField="Check_In_Date" Dataformatstring="{0:d-MMM-yyyy}"  HeaderText="CHECK IN" />
                           <asp:BoundField DataField="ROOM_TYPE" HeaderText="ROOM TYPE" />
                           <asp:BoundField DataField="Room_No" HeaderText="ROOM NO" />
                           <asp:BoundField DataField="Status" HeaderText="STATUS NO" />--%>

                           <asp:HyperLinkField Text="Editar" DataNavigateUrlFields="GUEST_ID" DataNavigateUrlFormatString="~/GuestInformation.aspx?id={0}">
                               <ControlStyle CssClass="btn btn-xs btn-default" />
                           </asp:HyperLinkField>
                         <%--  <asp:HyperLinkField Text="CANCEL" DataNavigateUrlFields="GUEST_ID" DataNavigateUrlFormatString="~/GuestList.aspx?act=cnl&amp;id={0}">
                               <ControlStyle CssClass="btn btn-xs btn-default" />
                           </asp:HyperLinkField>

                           <asp:TemplateField HeaderText="PRINT">
                               <ItemTemplate>
                                   <asp:Button Text="..." CssClass="btn btn-xs btn-default" runat="server" OnClientClick='<%# String.Format("return print_check_in_form({0});", Eval("GUEST_ID")) %>' />
                               </ItemTemplate>
                           </asp:TemplateField>--%>
                       </Columns>
                   </asp:GridView>
               </div>
           </div>
           <!-- /.box-body -->
       </div>
          <!-- /.box -->
        </div>
      </div>
      <!-- /.row -->


    </section>
    <!-- /.content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">

    
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
        <script type="text/javascript">
            $(function () {

                _tday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"),
                    _tmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"),
                    //d = new Date, d.toLocaleString('en-US', { hour: 'numeric', hour12: false }), nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear(), nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), nyear < 1e3 && (nyear += 1900), 0 == nhour ? (ap = " AM", nhour = 12) : nhour <= 11 ? ap = " AM" : 12 == nhour ? ap = " PM" : nhour >= 13 && (ap = " PM", nhour -= 12), nmin <= 9 && (nmin = "0" + nmin), nsec <= 9 && (nsec = "0" + nsec), ndate <= 9 && (ndate = "0" + ndate);
                    d = new Date, d.toLocaleString('en-US', { hour: 'numeric', hour12: false }), _nday = d.getDay(), _nmonth = d.getMonth(), _ndate = d.getDate(), _nyear = d.getYear(), _nhour = d.getHours(), _nmin = d.getMinutes(), _nmin <= 9 && (_nmin = "0" + _nmin), _nsec = d.getSeconds(), _nsec <= 9 && (_nsec = "0" + _nsec), _nyear < 1e3 && (_nyear += 1900), _ndate <= 9 && (_ndate = "0" + _ndate);
                _monthNum = (parseInt(_nmonth) + 1), _monthNum <= 9 && (_monthNum = "0" + _monthNum);
                 
                _ndate = (parseInt(_ndate)), _ndate <= 9 && (_ndate = "0" + _ndate);
                var checkIn = $('#<% =txtDate.ClientID %>').val();
                if (checkIn == "") {
                    $('#<% =txtDate.ClientID %>').val(_ndate + "/" + _monthNum + "/" + _nyear);
                }



                $('#datetimepicker1').datetimepicker({ format: 'DD/MM/YYYY' });
                $("#asp_form").validationEngine();

                $('#<% =txtDate.ClientID %>').focusout(function () {
                   
                    
                    var sDat = $('#<% =txtDate.ClientID %>').val();
            if (sDat != "" && sDat.length >= 10) {
                    $.ajax({
                        url: 'GuestInformation.aspx/CountGuestByDat',
                        data: "{ 'sDat': '" + sDat + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger;
                            $('#<% =lblCount2.ClientID %>').text(''); 
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


                 });
            });
        </script>
</asp:Content>
