﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_GuestList.aspx.vb" Inherits="HotelManagementSystem.rpt_GuestList" %>

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


        <asp:SqlDataSource ID="sqlServiceDat" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT        GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, GuestInformation.ID_Type, GuestInformation.ID_No, GuestInformation.Gender, GuestInformation.Purpose, 
                         GuestInformation.Arrival_n_Departure_Date_Time, GuestInformation.Note, GuestInformation.Check_In_Date, GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, 
                         GuestInformation.No_Of_Day, RoomDetails.Room_No, RoomType.ROOM_TYPE, GuestInformation.No_Of_Adult, GuestInformation.No_Of_Children, GuestInformation.Extra_Beds
                         FROM            RoomType INNER JOIN RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                         GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.No_Of_Day WHERE ((GuestInformation.Check_In_Date &gt;= @Check_In_Date) AND (GuestInformation.Check_In_Date &lt;= @Check_In_Date2) AND (GuestInformation.Status LIKE '%' + @Status + '%')) ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfDateFrom" DbType="Date" Name="Check_In_Date" PropertyName="Value" />
                <asp:ControlParameter ControlID="hfDateTo" DbType="Date" Name="Check_In_Date2" PropertyName="Value" />
                <asp:ControlParameter ControlID="hfStatus" Name="Status" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

        
        <asp:GridView ID="GridView1" Font-Size="12px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="False" DataKeyNames="GUEST_ID" DataSourceID="sqlServiceDat">
            <Columns>
                <asp:BoundField DataField="GUEST_ID"  HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" SortExpression="GuestName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" SortExpression="PhoneNo" />
                <asp:BoundField DataField="ID_Type" HeaderText="ID Type" SortExpression="ID_Type" />
                <asp:BoundField DataField="ID_No" HeaderText="ID No" SortExpression="ID_No" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />
                <asp:BoundField DataField="Arrival_n_Departure_Date_Time" HeaderText="Arrival & DepartureDate Time" SortExpression="Arrival_n_Departure_Date_Time" />
                <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
                <asp:BoundField DataField="Check_In_Date" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Check I nDate" SortExpression="Check_In_Date" />
                <asp:BoundField DataField="Check_In_Time" HeaderText="Check In Time" SortExpression="Check In Time" />
                <asp:BoundField DataField="Check_Out_Date" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Check Out Date" SortExpression="Check_Out_Date" />
                <asp:BoundField DataField="Check_Out_Time" HeaderText="Check Out Time" SortExpression="Check Out Time" />
                <asp:BoundField DataField="No_Of_Day" HeaderText="No Of Day" SortExpression="No Of Day" />
                <asp:BoundField DataField="Room_No" HeaderText="Room No" SortExpression="Room_No" />
                <asp:BoundField DataField="ROOM_TYPE" HeaderText="Room Type" SortExpression="ROOM_TYPE" />
                <asp:BoundField DataField="No_Of_Adult" HeaderText="No Of Adult" SortExpression="No_ Of Adult" />
                <asp:BoundField DataField="No_Of_Children" HeaderText="No Of Children" SortExpression="No Of Children" />
                <asp:BoundField DataField="Extra_Beds" HeaderText="Extra Beds" SortExpression="Extra Beds" />
            </Columns>
        </asp:GridView>

 
    </form>

</body>
</html>
