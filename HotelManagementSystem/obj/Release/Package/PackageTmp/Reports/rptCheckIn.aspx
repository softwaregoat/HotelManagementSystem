<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rptCheckIn.aspx.vb" Inherits="HotelManagementSystem.rptCheckIn" %>

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
        <div class="row">
             <asp:HiddenField ID="hfDateFrom" runat="server" />
        <asp:HiddenField ID="hfDateTo" runat="server" />
        <asp:HiddenField ID="hfUser" runat="server" />
        <asp:HiddenField ID="hrptType" runat="server" />
             <asp:HiddenField ID="hgType" runat="server" />
              <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [HotelInformation]"></asp:SqlDataSource>
        <asp:DataList ID="DataList1" Font-Size="15px" CssClass="center-block" runat="server" DataSourceID="SqlDataSource2">
            <ItemTemplate>
                <b><asp:Label ID="HotelNameLabel" runat="server" Text='<%# Eval("HotelName") %>' /> </b>
                <br />
                Dirección:
                <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                <br />
                Contacto:
                <asp:Label ID="ContactLabel" runat="server" Text='<%# Eval("Contact") %>' />
                <br />
                RUC:
                <asp:Label ID="TIN_NOLabel" runat="server" Text='<%# Eval("TIN_NO") %>' />
                <br />
                <br />
            </ItemTemplate>
        </asp:DataList>
           <h4 class="text-left"> <asp:Label ID="lblReportType" runat="server" Text=""></asp:Label> </h4>
               <h4 class="text-left"> <asp:Label ID="lblUser" runat="server" Text=""></asp:Label> </h4>
            <h2 class="text-center"><asp:Label ID="lblReportName" runat="server" Text=""></asp:Label></h2>
              <asp:GridView ID="gvReport" Font-Size="12px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="True">
             
        </asp:GridView>

            <br />
            <br />
            <br />
            Signature
        </div>
    </form>
     <script src="Assets/loader-custom.js"></script>
</body>
</html>
