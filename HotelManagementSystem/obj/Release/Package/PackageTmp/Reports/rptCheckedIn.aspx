<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rptCheckedIn.aspx.vb" Inherits="HotelManagementSystem.rptCheckedIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
      <!-- Bootstrap 3.3.7 -->
  <link rel="stylesheet" href="~/Assets/bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans&display=swap" rel="stylesheet" />
    <style> body { font-family: 'Open Sans', sans-serif; } </style>
</head>
<body>
    <form class="container" id="form1" runat="server">
       
             <asp:HiddenField ID="hfDateFrom" runat="server" />
        <asp:HiddenField ID="hfDateTo" runat="server" />
        <asp:HiddenField ID="hfUser" runat="server" />
        <asp:HiddenField ID="hrptType" runat="server" />
             <asp:HiddenField ID="hgType" runat="server" />
             <asp:HiddenField ID="hdnGuestId" runat="server" />
             <asp:HiddenField ID="hdnRoomId" runat="server" />
              <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [HotelInformation]"></asp:SqlDataSource>
     
            <h2 class="text-center"> ESTADO DE CUENTA </h2>
        <div class="row">
            <div class="col-md-8" style="float:left">
                       <asp:DataList ID="DataList1" Font-Size="15px" CssClass="center-block" runat="server" DataSourceID="SqlDataSource2">
            <ItemTemplate>
                 <h5 class="text-left"><asp:Label ID="HotelNameLabel" runat="server" Text='<%# Eval("HotelName") %>' /> </h5>
                
                 <h5 class="text-left"><asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' /></h5>
                
                 <h5 class="text-left"><asp:Label ID="ContactLabel" runat="server" Text='<%# Eval("Contact") %>' /></h5>
                <h5 class="text-left">
                <asp:Label ID="TIN_NOLabel" runat="server" Text='<%# Eval("TIN_NO") %>' />
               </h5>
            </ItemTemplate>
        </asp:DataList>
            </div>
            <div class="col-md-4"></div>
        </div>
           <div class="row">
                  <div class="col-md-6" style="float:left">
                       <h5 class="text-left"><asp:Label ID="lblReportName" runat="server" Text=""></asp:Label></h5>
                       <h5 class="text-left"><asp:Label ID="lblInvId" runat="server" Text=""></asp:Label></h5>
                       <h5 class="text-left"><asp:Label ID="lblGuestName" runat="server" Text="" Visible="false"></asp:Label></h5>
                       <h5 class="text-left"><asp:Label ID="lblGestId" runat="server" Text="" Visible="false"></asp:Label></h5>
                  
                  </div>
                   <div class="col-md-6" style="float:right">
                       <h5 class="text-left"><asp:Label ID="lblTarjeta" runat="server" Text=""></asp:Label></h5>
                        <h5 class="text-left"><asp:Label ID="lblRoomNo" runat="server" Text=""></asp:Label></h5>
                         <h5 class="text-left"><asp:Label ID="lblCheckInDateTime" runat="server" Text=""></asp:Label></h5>
                         <h5 class="text-left"><asp:Label ID="lblCheckOutDateTime" runat="server" Text=""></asp:Label></h5>
                       <h5 class="text-left"> <asp:Label ID="lblUser" runat="server" Text=""></asp:Label> </h5> 
             <h5 class="text-left"> <asp:Label ID="lblReportType" runat="server" Text=""></asp:Label> </h5>
              
                   </div> 
              </div>
           
              <asp:GridView ID="gvReport" Font-Size="12px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="True">
             
        </asp:GridView>

            <br />
            <br />
            <br />
            Signature
       
    </form>
     <script src="Assets/loader-custom.js"></script>
</body>
</html>
