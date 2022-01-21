<%@ Page Title="Habitaciones" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RoomView.aspx.vb" Inherits="HotelManagementSystem.RoomView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fade hide" id="myModal" title="Message">



        <p>
            <asp:Label ID="lblMessege" runat="server" Text=""></asp:Label>
        </p>

    </div>
    <asp:HiddenField ID="hdnNewRoomId" runat="server" />
    <asp:HiddenField ID="hdnGuestId" runat="server" />
    <div class="fade hide" id="myModalChangeRoom" title="Cambiar de Habitación">



        <p>
            <% If tblOldRoomInfo.Rows.Count > 0 Then
                    GetRentHours(changeRoomId)
                            %>
            <span class="progress-description"><% =tblOldRoomInfo.Rows(0)("GuestName") %></span>
            <span class="progress-description"><% =rentHours %></span>
            <span class="progress-description">Habitación actual: <% =tblOldRoomInfo.Rows(0)("Room_No") %></span>
            <span id="spNewroom" class="progress-description"></span>
            <span class="progress-description">Hora de entrada: <% =tblOldRoomInfo.Rows(0)("Check_In_Date") %>/<% =tblOldRoomInfo.Rows(0)("Check_In_Time") %></span>
            <span class="progress-description">Hora de salida: <% =tblOldRoomInfo.Rows(0)("Check_Out_Date") %>/<% =tblOldRoomInfo.Rows(0)("Check_Out_Time") %></span>
            <% End If %>
        </p>
        <div class="modal-footer">
            <asp:Button ID="btnRoomChange" CssClass="btn btn-info hide" runat="server" OnClick="btnRoomChange_Click" Text="Confirmar" />
            <button onclick="SubmitbtnRoomChange();" class="btn btn-info">Confirmar</button>
            <%--<button type="button" class="btn btn-default pull-right" data-dismiss="modal">Cancel</button>--%>
            <asp:HyperLink ID="hlnkRoomChangeCancel" runat="server" CssClass="btn btn-default pull-right">Cancel</asp:HyperLink>
        </div>

    </div>

    <div class="fade hide" id="myModalFoodSale" role="dialog" title="Ventas">
        <div>
            <h2 class="modal-title text-center">Ventas</h2>

            <div class="row" id="dvFoodItem">
                <% If tblFoodItems.Rows.Count > 0 Then
                        For i = 0 To tblFoodItems.Rows.Count - 1
                            Dim txtid As String
                            txtid = "txtfood_" + tblFoodItems.Rows(i)("FOOD_ITEM_ID").ToString
                            %>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="<% =txtid %>"><% =tblFoodItems.Rows(i)("FOOD_ITEM_NAME") %></label>
                        <input type="text" class="form-control" name="<% =txtid %>" id="<% =txtid %>" placeholder="<% =tblFoodItems.Rows(i)("FOOD_ITEM_NAME") %>" />
                    </div>
                </div>

                <% Next%> <% End If %>
            </div>

            <p class="modal-footer">
                <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
                <%-- <asp:Button ID="btnFoodDelete" CssClass="btn btn-info" runat="server" Text="Eliminar" />--%>
                <input type="button" onclick="EliminateFoodSale()" class="btn btn-info" value="Eliminar" />
                <input type="button" onclick="SaveFoodSale()" class="btn btn-info" value="Confirmar" />
                <button type="button" class="btn btn-default pull-right" onclick="closemodal()">Cancel</button>
            </p>

        </div>
    </div>
    <!-- Content Header (Page header) -->
    <section class="content-header">

        <div class="col-md-12" style="padding-bottom: 5px">
            <div class="row">

                <div class="col-md-5">
                    <span class="h1">Habitaciones
      </span>
                </div>
                <div class="col-md-7">
                </div>
            </div>

        </div>
        <div class="col-md-12" style="padding-bottom: 5px;">
            <div class="row">
                <div class="col-md-5">
                    <div class="form-inline">

                        <%
                            Dim s As Integer
                            Dim bgcolors As String
                          %>
                        <%If tblRoomStatusTotal.Rows.Count > 0 Then

                                For s = 0 To tblRoomStatusTotal.Rows.Count - 1
                                    If s = 0 Then
                                        bgcolors = "font-weight:bold;color:" + tblRoomStatusTotal.Rows(s)("Status_Color")
                                    Else
                                        bgcolors = "padding-left:5px;font-weight:bold;color:" + tblRoomStatusTotal.Rows(s)("Status_Color")
                                    End If 
                            %>

                        <span class="h4" style="<% =bgcolors%>"><% =tblRoomStatusTotal.Rows(s)("Status") %>  <% =tblRoomStatusTotal.Rows(s)("Total") %></span>
                        <% Next
                            End If
                         %>
                    </div>
                </div>
                <div class="col-md-7">

                    <div class="form-inline pull-right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-info hidden" Text="Check Guest" />
                        <button type="button" onclick="showModalSale()" class="btn btn-danger" value="Ventas"><i class="fa fa-leaf"></i><span>Ventas</span></button>
                        <asp:HyperLink ID="hlDirtyRoom" CssClass="btn btn-warning" runat="server" NavigateUrl="~/DirtyRoom.aspx"> <i Class="fa fa-bed"></i>  <span>Limpiar hab.</span> </asp:HyperLink>
                        <asp:HyperLink ID="hlCheckGuest" CssClass="btn btn-info" runat="server" NavigateUrl="~/GuestInformation.aspx"> <i Class="fa fa-user-o"></i>  <span>Buscar Huésped</span> </asp:HyperLink>
                        <label for="txtSearch">Buscar: :</label>
                        <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Ingrese ID o Nombre" runat="server" AutoPostBack="True"></asp:TextBox>
                        <a href="RoomView.aspx" title="refresh searching"><i class="fa fa-refresh"></i></a>
                    </div>
                </div>
            </div>
        </div>


        <%--<ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Inicio </span> </asp:HyperLink>  </li>
        <li class="active">Habitaciones</li>
      </ol>--%>
    </section>

    <!-- Main content -->
    <section class="content">

        <div class="row">
            <%

                If sqlDTx.Rows.Count > 0 Then
                    Dim i As Integer
                    Dim GId As String
                    If tblOldRoomInfo.Rows.Count > 0 Then
                        GId = tblOldRoomInfo.Rows(0)("GUEST_ID")
                    End If
                    For i = 0 To sqlDTx.Rows.Count - 1
                        Dim bgcolor As String
                        bgcolor = "padding:2px!important;height:95px!important;border-radius:5px;background-color:" + sqlDTx.Rows(i)("Status_Color") + "!important;"

                        If sqlDTx.Rows(i)("rhour") Is "" Then
                            Dim remainmin As String() = sqlDTx.Rows(i)("rmin").ToString.Split(" ")
                            If remainmin.Length > 1 Then
                                If Convert.ToInt32(remainmin(0)) <= 15 Then
                                    bgcolor = "padding:2px!important;height:95px!important;border-radius:5px;background-color:yellow!important;color:black!important;"
                                End If
                            ElseIf (sqlDTx.Rows(i)("Status_ID") > 1 And sqlDTx.Rows(i)("Status_ID") < 3) And remainmin(0) Is "" Then
                                bgcolor = "padding:2px!important;height:95px!important;border-radius:5px;background-image:url('/Assets/Img/splash-multi-color.PNG')!important; background-repeat: no-repeat!important;background-size: 140px 95px!important;"

                            End If

                        End If

                        If (sqlDTx.Rows(i)("Status_ID")) = 3 Then
                            GetMaidCmments(sqlDTx.Rows(i)("ROOM_ID"))
                        Else
                            MaidCommentsIcon = ""
                        End If
                        GetRentHours(sqlDTx.Rows(i)("ROOM_ID"))

                        %>


            <%--<span class="info-box-icon"><i class="fa fa-bed"></i></span>--%>
            <%--<span class="info-box-text"><% =sqlDTx.Rows(i)("ROOM_TYPE") %></span> --%>
            <% If changeRoomId IsNot Nothing And sqlDTx.Rows(i)("Status_ID") = 1 Then

                                            %>
            <a style="color: white!important" onclick="showModalRoomChange(<% =sqlDTx.Rows(i)("ROOM_ID") %>,'<% =sqlDTx.Rows(i)("Room_No") %>',<% =GId %>)">
                <div class="col-md-2 col-sm-4 col-xs-8" style="padding-left: 5px!important; padding-bottom: 5px!important; padding-right: 0px!important; width: 140px!important;">
                    <div style="<% =bgcolor %>">
                        <%-- <span style="float:right"><% =sqlDTx.Rows(i)("rh") %> <% =sqlDTx.Rows(i)("rm") %> </span>--%>
                        <span class="info-box-number" style="font-size: 16px!important"><% =sqlDTx.Rows(i)("Room_No") %></span>
                        <span class="progress-description" style="font-size: 12px!important"><% =sqlDTx.Rows(i)("GuestName") %></span>
                        <%--        <span class="progress-description"><% =rentHours %></span>--%>
                        <span class="info-box-text" style="font-size: 13px!important"><% =sqlDTx.Rows(i)("Room_Status") %></span>
                    </div>
                </div>
            </a>
            <% Else%>
            <a style="color: white!important" href="CheckIn.aspx?room_id=<% =sqlDTx.Rows(i)("ROOM_ID") %>">
                <div class="col-md-2 col-sm-4 col-xs-8" style="padding-left: 5px!important; padding-bottom: 5px!important; padding-right: 0px!important; width: 140px!important;">
                    <div style="<% =bgcolor %>">
                        <% If sqlDTx.Rows(i)("Status_ID") > 1 Then %>
                        <span style="float: right; font-size: 11px!important"><% =sqlDTx.Rows(i)("rhour") %> <% =sqlDTx.Rows(i)("rmin") %></span>
                        <% End If %>
                        <span class="info-box-number" style="font-size: 15px!important"><% =sqlDTx.Rows(i)("Room_No") %></span>
                        <% If sqlDTx.Rows(i)("Status_ID") > 1 Then %>
                        <% IF (sqlDTx.Rows(i)("Guest_ID")) = 0 And sqlDTx.Rows(i)("Status_ID") < 4 Then %>

                        <span class="progress-description" style="font-size: 12px!important"><%=sqlDTx.Rows(i)("BGuestName")  %></span>
                        <% Else%>
                        <span class="progress-description" style="font-size: 12px!important"><% =sqlDTx.Rows(i)("GuestName")%></span>
                        <% End If %>
                        <% End If %>
                        <% If sqlDTx.Rows(i)("Status_ID") > 1 And sqlDTx.Rows(i)("Status_ID") < 4 Then %>

                        <span class="progress-description" style="font-size: 12px!important"><% =rentHours %>

                            <% If sqlDTx.Rows(i)("promo") IsNot "" Then %>
                                   - <% =sqlDTx.Rows(i)("promo") %>
                            <% End If %>

                                     </span>
                        <% End If %>

                        <span class="info-box-text" style="font-size: 13px!important">
                            <% =sqlDTx.Rows(i)("Room_Status") %>
                            <% If MaidCommentsIcon IsNot "" Then %>
                            <% =MaidCommentsIcon %>
                            <% End If %>
                                     </span>



                    </div>
                </div>
            </a>
            <% End If %>


            <% Next
                End If
                %>
        </div>
        <%-- <div class="col-md-12" style="padding-bottom:5px; padding-top:20px">
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                     <div class="form-inline pull-right"> 
                         <input type="button"  onclick="showModalSale()" class="btn btn-info" value="Sales" />
                </div>
                </div>
            </div>
           
        </div>--%>
    </section>

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
    <script type="text/javascript">
        var auto_refresh = setInterval(
            function () {
                window.location.reload();
            }, 30000); // refresh every 10000 milliseconds

        function SubmitbtnRoomChange() {
              <%= ClientScript.GetPostBackEventReference(btnRoomChange, String.Empty) %>;

           <%--     $('#<% =btnRoomChange.ClientID %>').trigger('click');  --%>
        };

        function showModalRoomChange(nroomid, nroomno, guest) {
            $('#spNewroom').html("Habitación nueva:" + nroomno);
            $('#<% =hdnNewRoomId.ClientID %>').val(nroomid);
                $('#<% =hdnGuestId.ClientID %>').val(guest);
            $("#myModalChangeRoom").removeClass("fade hide");
            $("#myModalChangeRoom").dialog({
                width: 500,
                modal: true
            });
        }



        function showModal() {

            $("#myModal").removeClass("fade hide");
            $("#myModal").dialog({
                width: 400,
                modal: true
            });
        }
        function showModalSale() {
            $("#myModalFoodSale").removeClass("fade hide");
            $("#myModalFoodSale").dialog({
                open: function (event, ui) {
                    $(this).parent().focus();
                },
                width: 600,
                modal: true
            });
        }
        function hideModalSale() {
            //$("#myModalFoodSale").removeClass("fade hide");
            $("#myModalFoodSale").dialog("close");
        }
        var foodarray = [];
        function SaveFoodSale() {
            var uid = $('#<%=hdnUserId.ClientID%>').val()
                var e = GetElementInsideContainer("dvFoodItem");
                foodarray = JSON.stringify({ 'foodService': foodarray, "uid": uid });
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: 'POST',
                    url: 'RoomView.aspx/Save',
                    data: foodarray,
                    success: function () {
                        $('#<%=lblMessege.ClientID%>').text("Food items saved successfully.");
                    $("#myModalFoodSale").dialog("close");
                    showModal();
                    window.location.reload();
                },
                failure: function (response) {
                    $('#<%=lblMessege.ClientID%>').text(response);
                    $("#myModalFoodSale").dialog("close");
                    showModal();
                    window.location.reload();
                }
            });
            console.log(foodarray);
        }
        function EliminateFoodSale() {
            var uid = $('#<%=hdnUserId.ClientID%>').val()
            var e = GetElementInsideContainer("dvFoodItem");
            foodarray = JSON.stringify({ 'foodService': foodarray, "uid": uid });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                url: 'RoomView.aspx/Eliminate',
                data: foodarray,
                success: function () {
                    $('#<%=lblMessege.ClientID%>').text("Food items deleted successfully.");
                    $("#myModalFoodSale").dialog("close");
                    showModal();
                    window.location.reload();
                },
                failure: function (response) {
                    $('#<%=lblMessege.ClientID%>').text(response);
                    $("#myModalFoodSale").dialog("close");
                    showModal();
                    window.location.reload();
                }
            });
            console.log(foodarray);
        }
        function GetElementInsideContainer(containerID) {
            var elm = {};
            var elms = document.getElementById(containerID).getElementsByTagName("input");
            for (var i = 0; i < elms.length; i++) {

                elm = elms[i];
                id = (elm.id).split('_')[1];
                txtval = $("#" + elm.id).val();
                if (txtval == "") {
                    txtval = "0";
                }

                item = { "id": id, "quantity": txtval };
                foodarray.push(item);
            }
            return elm;
        }



















        $(function () {

            $("#asp_form").validationEngine();
        });
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


        $(window).keydown(function (event) {
            if (event.keyCode == 117) { //f6 keycode
                console.log("Hey!  F6 event captured!");
                event.preventDefault();
                //document.getElementById("hlCheckGuest").click();
                window.open("GuestInformation.aspx");
            }
            else if (event.keyCode == 121) {
                event.preventDefault();
                //document.getElementById("hlCheckGuest").click();
                window.open("DirtyRoom.aspx");
            }
            else if (event.keyCode == 175) {
                showModalSale();
            }
        });

        function closemodal() {

            $("#myModalFoodSale").dialog('close');
        }
        </script>
</asp:Content>
