<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_ExtraServices.aspx.vb" Inherits="HotelManagementSystem.rpt_ExtraServices" %>
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
    <div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
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
            <h4 class="text-left">Extra Services</h4>

        <asp:SqlDataSource ID="sqlExtraBill" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT [GUEST_ID], [GuestName], [Address], [PhoneNo], [Check_In_Date], [Check_In_Time], [Check_Out_Date], [Check_Out_Time], [Boarding], [Food], [Laundry], [Telephone], [BAR], [DINNER], [Breakfat], [SPA], [BanquetDinner], [Cleaning], [ServiceCharges], [OtherCharges], (Boarding+ Food+ Laundry+ Telephone+ BAR+ DINNER+ Breakfat+ SPA+ BanquetDinner+ Cleaning+ ServiceCharges+ 
                         OtherCharges) As ToalVal FROM [GuestInformation] WHERE ([GUEST_ID] = @GUEST_ID)">
            <SelectParameters>
                <asp:ControlParameter ControlID="HiddenField1" DefaultValue="0" Name="GUEST_ID" PropertyName="Value" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-condensed" AutoGenerateRows="False" DataKeyNames="GUEST_ID" DataSourceID="sqlExtraBill">
            <Fields>
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" InsertVisible="False" ReadOnly="True" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" SortExpression="GuestName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" SortExpression="PhoneNo" />
                <asp:BoundField DataField="Check_In_Date" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Check In Date" SortExpression="Check_In_Date" />
                <asp:BoundField DataField="Check_In_Time"  HeaderText="Check In Time" SortExpression="Check_In_Time" />
                <asp:BoundField DataField="Check_Out_Date" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Check Out Date" SortExpression="Check_Out_Date" />
                <asp:BoundField DataField="Check_Out_Time" HeaderText="Check Out Time" SortExpression="Check Out Time" />
                <asp:BoundField DataField="Boarding" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Boarding" SortExpression="Boarding" />
                <asp:BoundField DataField="Food" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Food" SortExpression="Food" />
                <asp:BoundField DataField="Laundry" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Laundry" SortExpression="Laundry" />
                <asp:BoundField DataField="Telephone" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Telephone" SortExpression="Telephone" />
                <asp:BoundField DataField="BAR" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="BAR" SortExpression="BAR" />
                <asp:BoundField DataField="DINNER" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Dinner" SortExpression="DINNER" />
                <asp:BoundField DataField="Breakfat" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Breakfat" SortExpression="Breakfat" />
                <asp:BoundField DataField="SPA" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="SPA" SortExpression="SPA" />
                <asp:BoundField DataField="BanquetDinner" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Banquet Dinner" SortExpression="BanquetDinner" />
                <asp:BoundField DataField="Cleaning" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Cleaning" SortExpression="Cleaning" />
                <asp:BoundField DataField="ServiceCharges" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Service Charges" SortExpression="ServiceCharges" />
                <asp:BoundField DataField="OtherCharges" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Other Charges" SortExpression="OtherCharges" />
                <asp:BoundField DataField="ToalVal" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="TOTAL" SortExpression="ToalVal" />
            </Fields>
        </asp:DetailsView>
            <br />
            <br />
            <br />
            Signature
    </div>
    </form>
</body>
</html>
