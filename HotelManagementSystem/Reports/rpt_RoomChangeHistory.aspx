<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_RoomChangeHistory.aspx.vb" Inherits="HotelManagementSystem.rpt_RoomChangeHistory" %>

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

        <asp:SqlDataSource ID="sqlServiceDat" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand=" SELECT        ChangeRoom.Room_Alter_Date, ChangeRoom.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, RoomType_1.ROOM_TYPE AS R_TYPE_FROM, 
                         RoomDetails_1.Room_No AS R_NO_FROM, RoomType.ROOM_TYPE AS NEW_ROOM_TYPE, RoomDetails.Room_No AS NEW_ROOM_NO, ChangeRoom.Reason FROM            GuestInformation RIGHT OUTER JOIN
                         RoomDetails AS RoomDetails_1 INNER JOIN  RoomType AS RoomType_1 ON RoomDetails_1.ROOM_TYPE_ID = RoomType_1.ROOM_TYPE_ID RIGHT OUTER JOIN
                         ChangeRoom LEFT OUTER JOIN  RoomType INNER JOIN
                         RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID ON ChangeRoom.NEW_ROOM_ID = RoomDetails.ROOM_ID ON RoomDetails_1.ROOM_ID = ChangeRoom.FROM_ROOM_ID ON 
                         GuestInformation.GUEST_ID = ChangeRoom.GUEST_ID  WHERE ((ChangeRoom.Room_Alter_Date &gt;= @Room_Alter_Date) AND (ChangeRoom.Room_Alter_Date &lt;= @Room_Alter_Date2)) ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfDateFrom" DbType="Date" Name="Room_Alter_Date" PropertyName="Value" />
                <asp:ControlParameter ControlID="hfDateTo" DbType="Date" Name="Room_Alter_Date2" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:GridView ID="GridView1" Font-Size="12px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="False" DataSourceID="sqlServiceDat">
            <Columns>
                <asp:BoundField DataField="Room_Alter_Date" HeaderText=" Alter Date" Dataformatstring="{0:d-MMM-yyyy}" SortExpression="Room_Alter_Date" />
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" SortExpression="GuestName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" SortExpression="PhoneNo" />
                <asp:BoundField DataField="R_TYPE_FROM" HeaderText=" " SortExpression="R_TYPE_FROM" />
                <asp:BoundField DataField="R_NO_FROM" HeaderText="PREVIOUS ROOM NO" SortExpression="R_NO_FROM" />
                <asp:BoundField DataField="NEW_ROOM_TYPE" HeaderText="" SortExpression="NEW_ROOM_TYPE" />
                <asp:BoundField DataField="NEW_ROOM_NO" HeaderText="NEW ROOM NO" SortExpression="NEW_ROOM_NO" />
                <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" />
            </Columns>
        </asp:GridView>

 
    </form>

</body>
</html>
