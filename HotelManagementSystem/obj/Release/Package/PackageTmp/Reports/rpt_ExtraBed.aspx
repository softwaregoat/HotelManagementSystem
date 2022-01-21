<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_ExtraBed.aspx.vb" Inherits="HotelManagementSystem.rpt_ExtraBed" %>

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

    <form  id="form1" runat="server">
        <asp:HiddenField ID="hfDateFrom" runat="server" />
        <asp:HiddenField ID="hfDateTo" runat="server" />
        <asp:HiddenField ID="hfStatus" runat="server" />

        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [HotelInformation]"></asp:SqlDataSource>
        <asp:DataList ID="DataList1" Font-Size="15px" CssClass="center-block" runat="server" DataSourceID="SqlDataSource2">
            <ItemTemplate>
                <b><asp:Label ID="HotelNameLabel" runat="server" Text='<%# Eval("HotelName") %>' /> </b>
                <br />
                Address:
                <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                <br />
                Contact:
                <asp:Label ID="ContactLabel" runat="server" Text='<%# Eval("Contact") %>' />
                <br />
                TIN NO:
                <asp:Label ID="TIN_NOLabel" runat="server" Text='<%# Eval("TIN_NO") %>' />
                <br />
                <br />
            </ItemTemplate>
        </asp:DataList>

        <h4 class="text-left"> <asp:Label ID="lblReportType" runat="server" Text=""></asp:Label> </h4>

        <asp:SqlDataSource ID="sqlServiceDat" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="  SELECT        ExtraBed.SOLD_DATE, ExtraBed.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, RoomType.ROOM_TYPE, RoomDetails.Room_No, ExtraBed.NO_OF_BED, 
                                         ExtraBed.Comment FROM      ExtraBed LEFT OUTER JOIN  RoomType RIGHT OUTER JOIN  RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                                         GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID ON ExtraBed.GUEST_ID = GuestInformation.GUEST_ID  WHERE ((ExtraBed.SOLD_DATE &gt;= @SOLD_DATE) AND (ExtraBed.SOLD_DATE &lt;= @SOLD_DATE2)) ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfDateFrom" DbType="Date" Name="SOLD_DATE" PropertyName="Value" />
                <asp:ControlParameter ControlID="hfDateTo" DbType="Date" Name="SOLD_DATE2" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:GridView ID="GridView1" Font-Size="12px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="False" DataSourceID="sqlServiceDat">
            <Columns>
                <asp:BoundField DataField="SOLD_DATE" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Sold Date" SortExpression="SOLD_DATE" />
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="GuestName" SortExpression="Guest_Name" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" SortExpression="PhoneNo" />
                <asp:BoundField DataField="ROOM_TYPE" HeaderText="Room Type" SortExpression="ROOM_TYPE" />
                <asp:BoundField DataField="Room_No" HeaderText="Room No" SortExpression="Room_No" />
                <asp:BoundField DataField="NO_OF_BED" HeaderText="No of Bed" SortExpression="NO_OF_BED" />
                <asp:BoundField DataField="Comment" HeaderText="Comments" SortExpression="Comment" />
            </Columns>
        </asp:GridView>


 
    </form>

</body>
</html>
