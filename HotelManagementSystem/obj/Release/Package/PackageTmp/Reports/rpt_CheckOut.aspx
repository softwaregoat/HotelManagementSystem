<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_CheckOut.aspx.vb" Inherits="HotelManagementSystem.rpt_CheckOut" %>

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
            <b class="text-left">Check Out</b>
         <asp:SqlDataSource ID="sqlCheckout" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT        GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, GuestInformation.Arrival_n_Departure_Date_Time, GuestInformation.Check_In_Date, 
                         GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.No_Of_Day, GuestInformation.Rent_Day, 
                         GuestInformation.Total_Charges, GuestInformation.Extra_Beds, GuestInformation.Per_Bed_Cost, GuestInformation.Total_Extra_Bed_Cost, GuestInformation.Boarding, GuestInformation.Food, GuestInformation.Laundry, 
                         GuestInformation.Telephone, GuestInformation.OtherCharges, GuestInformation.PaymentType, GuestInformation.TAX_1, GuestInformation.TAX_2, GuestInformation.TAX_3, GuestInformation.GrandTotal, 
                         GuestInformation.DiscountAmount, GuestInformation.NetAmount, GuestInformation.PaidAmount, GuestInformation.BalanceAmount, GuestInformation.Status, GuestInformation.BAR, GuestInformation.DINNER, 
                         GuestInformation.Breakfat, GuestInformation.SPA, GuestInformation.BanquetDinner, GuestInformation.Cleaning, GuestInformation.ServiceCharges, GuestInformation.CheckOutNote, GuestInformation.TotalTAX, 
                         GuestInformation.Total FROM            RoomType INNER JOIN
                         RoomDetails ON RoomType.ROOM_TYPE_ID = RoomDetails.ROOM_TYPE_ID RIGHT OUTER JOIN
                         GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID WHERE (GuestInformation.GUEST_ID = @GUEST_ID)">
             <SelectParameters>
                 <asp:ControlParameter ControlID="HiddenField1" DefaultValue="0" Name="GUEST_ID" PropertyName="Value" Type="Int64" />
             </SelectParameters>
        </asp:SqlDataSource>

        <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-condensed small" AutoGenerateRows="False" DataKeyNames="GUEST_ID" DataSourceID="sqlCheckout">
            <Fields>
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" InsertVisible="False" ReadOnly="True" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" SortExpression="GuestName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" SortExpression="PhoneNo" />
                <asp:BoundField DataField="Check_In_Date" HeaderText="Check In Date" Dataformatstring="{0:d-MMM-yyyy}" SortExpression="Check_In_Date" />
                <asp:BoundField DataField="Check_In_Time" HeaderText="Check In Time" SortExpression="Check_In_Time" />
                <asp:BoundField DataField="Check_Out_Date" HeaderText="Check Out Date" Dataformatstring="{0:d-MMM-yyyy}" SortExpression="Check_Out_Date" />
                <asp:BoundField DataField="Check_Out_Time" HeaderText="Check Out Time" SortExpression="Check_Out_Time" />
                <asp:BoundField DataField="ROOM_TYPE" HeaderText="Room Type" SortExpression="ROOM_TYPE" />
                <asp:BoundField DataField="Room_No" HeaderText="Room No" SortExpression="Room_No" />
                <asp:BoundField DataField="Extra_Beds" HeaderText="Extra Beds" SortExpression="Extra_Beds" />
                <asp:BoundField DataField="Per_Bed_Cost" HeaderText="Per Bed Cost" SortExpression="Per_Bed_Cost" />
                <asp:BoundField DataField="No_Of_Day" HeaderText="No Of Day" SortExpression="No_Of_Day" />
                <asp:BoundField DataField="Rent_Day" HeaderText="Rent Day" SortExpression="Rent_Day" />
                <asp:BoundField DataField="Total_Charges" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Total Charges" SortExpression="Total_Charges" />
                <asp:BoundField DataField="Total_Extra_Bed_Cost" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Total Extra Bed Cost" SortExpression="Total_Extra_Bed_Cost" />
                <asp:BoundField DataField="BAR" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="BAR" SortExpression="BAR" />
                <asp:BoundField DataField="DINNER" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="DINNER" SortExpression="DINNER" />
                <asp:BoundField DataField="Breakfat" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Breakfat" SortExpression="Breakfat" />
                <asp:BoundField DataField="SPA" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="SPA" SortExpression="SPA" />
                <asp:BoundField DataField="BanquetDinner" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Banquet Dinner" SortExpression="BanquetDinner" />
                <asp:BoundField DataField="Cleaning" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Cleaning" SortExpression="Cleaning" />
                <asp:BoundField DataField="ServiceCharges" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Service Charges" SortExpression="ServiceCharges" />
                <asp:BoundField DataField="Boarding" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Boarding" SortExpression="Boarding" />
                <asp:BoundField DataField="Food" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Food" SortExpression="Food" />
                <asp:BoundField DataField="Laundry" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Laundry" SortExpression="Laundry" />
                <asp:BoundField DataField="Telephone" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Telephone" SortExpression="Telephone" />
                <asp:BoundField DataField="OtherCharges" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Other Charges" SortExpression="OtherCharges" />
                <asp:BoundField DataField="Total" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Total" SortExpression="Total" />
                <asp:BoundField DataField="TAX_1" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="TAX 1  " SortExpression="TAX_1" />
                <asp:BoundField DataField="TAX_2" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="TAX 2" SortExpression="TAX_2" />
                <asp:BoundField DataField="TAX_3" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="TAX 3" SortExpression="TAX_3" />
                <asp:BoundField DataField="TotalTAX" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Total TAX" SortExpression="TotalTAX" />
                <asp:BoundField DataField="GrandTotal" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Grand Total" SortExpression="GrandTotal" />
                <asp:BoundField DataField="DiscountAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Discount Amount" SortExpression="DiscountAmount" />
                <asp:BoundField DataField="NetAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Net Amount" SortExpression="NetAmount" />
                <asp:BoundField DataField="PaidAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Paid Amount" SortExpression="PaidAmount" />
                <asp:BoundField DataField="BalanceAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Balance" SortExpression="BalanceAmount" />
                <asp:BoundField DataField="PaymentType" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Payment Type" SortExpression="PaymentType" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                <asp:BoundField DataField="CheckOutNote" HeaderText="Check Out Note" SortExpression="CheckOutNote" />
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
