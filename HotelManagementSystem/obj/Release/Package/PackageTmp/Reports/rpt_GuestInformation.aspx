<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_GuestInformation.aspx.vb" Inherits="HotelManagementSystem.rpt_GuestInformation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
    <link rel="stylesheet" href="~/Assets/bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="~/Assets/bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="~/Assets/bower_components/Ionicons/css/ionicons.min.css" />
    <link rel="stylesheet" href="~/Assets/dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <section class="invoice">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [HotelInformation]"></asp:SqlDataSource>
    
                <div class="row">
      <div class="col-md-12">
        <h2 class="page-header">

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

        </h2>
      </div>
      <!-- /.col -->
    </div>    

        <h4 class="text-left">GUEST INFORMATION</h4>    
        <asp:SqlDataSource ID="sqlGuestData" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand= " SELECT        GuestInformation.GUEST_ID, GuestInformation.GuestName, GuestInformation.Address, GuestInformation.PhoneNo, GuestInformation.ID_Type, GuestInformation.ID_No, GuestInformation.Gender, GuestInformation.Purpose, 
                         GuestInformation.Arrival_n_Departure_Date_Time, GuestInformation.Note, GuestInformation.Check_In_Date, GuestInformation.Check_In_Time, GuestInformation.Check_Out_Date, GuestInformation.Check_Out_Time, 
                         GuestInformation.No_Of_Day, RoomType.ROOM_TYPE, RoomDetails.Room_No, GuestInformation.Rent_Day, GuestInformation.Total_Charges, GuestInformation.No_Of_Adult, GuestInformation.No_Of_Children, 
                         GuestInformation.Extra_Beds, GuestInformation.Per_Bed_Cost, GuestInformation.Total_Extra_Bed_Cost, GuestInformation.Boarding, GuestInformation.Food, GuestInformation.Laundry, GuestInformation.Telephone, 
                         GuestInformation.OtherCharges, GuestInformation.PaymentType, GuestInformation.TAX_1, GuestInformation.TAX_2, GuestInformation.TAX_3, GuestInformation.GrandTotal, GuestInformation.DiscountAmount, 
                         GuestInformation.NetAmount, GuestInformation.PaidAmount, GuestInformation.BalanceAmount, GuestInformation.Status, GuestInformation.BAR, GuestInformation.DINNER, GuestInformation.Breakfat, GuestInformation.SPA, 
                         GuestInformation.BanquetDinner, GuestInformation.Cleaning, GuestInformation.ServiceCharges, GuestInformation.DirtyRoom, GuestInformation.CheckOutNote, GuestInformation.TotalTAX, GuestInformation.Total FROM            RoomDetails INNER JOIN
                         RoomType ON RoomDetails.ROOM_TYPE_ID = RoomType.ROOM_TYPE_ID RIGHT OUTER JOIN GuestInformation ON RoomDetails.ROOM_ID = GuestInformation.ROOM_ID  WHERE (GuestInformation.GUEST_ID = @GUEST_ID) ">
            <SelectParameters>
                <asp:ControlParameter ControlID="HiddenField1" DefaultValue="0" Name="GUEST_ID" PropertyName="Value" Type="Int64" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:DetailsView ID="DetailsView1" runat="server" Font-Size="15px" CssClass="table table-striped table-bordered table-condensed" AutoGenerateRows="False" DataKeyNames="GUEST_ID" DataSourceID="sqlGuestData">
            <Fields>
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" InsertVisible="False" ReadOnly="True" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" SortExpression="GuestName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" SortExpression="PhoneNo" />
                <asp:BoundField DataField="ID_Type" HeaderText="ID Type" SortExpression="ID_Type" />
                <asp:BoundField DataField="ID_No" HeaderText="ID No" SortExpression="ID_No" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />
                <asp:BoundField DataField="Arrival_n_Departure_Date_Time" HeaderText="Arrival/Departure Date Time" SortExpression="Arrival_n_Departure_Date_Time" />
                <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
                <asp:BoundField DataField="Check_In_Date" HeaderText="Check In Date" Dataformatstring="{0:d-MMM-yyyy}" SortExpression="Check_In_Date" />
                <asp:BoundField DataField="Check_In_Time" HeaderText="Check In Time" SortExpression="Check_In_Time" />
                <asp:BoundField DataField="Check_Out_Date" HeaderText="Check Out Date" Dataformatstring="{0:d-MMM-yyyy}" SortExpression="Check_Out_Date" />
                <asp:BoundField DataField="Check_Out_Time" HeaderText="Check Out Time" SortExpression="Check_Out_Time" />
                <asp:BoundField DataField="No_Of_Day" HeaderText="No of Day" SortExpression="No_Of_Day" />
                <asp:BoundField DataField="ROOM_TYPE" HeaderText="Room Type" SortExpression="ROOM_TYPE" />
                <asp:BoundField DataField="Room_No" HeaderText="Room No" SortExpression="Room_No" />
                <asp:BoundField DataField="Rent_Day" HeaderText="Rent Day" SortExpression="Rent_Day" />
                <asp:BoundField DataField="No_Of_Adult" HeaderText="No Of Adult" SortExpression="No_Of_Adult" />
                <asp:BoundField DataField="No_Of_Children" HeaderText="No Of Children" SortExpression="No_Of_Children" />
                <asp:BoundField DataField="Extra_Beds" HeaderText="Extra Beds" SortExpression="Extra_Beds" />
                <asp:BoundField DataField="Per_Bed_Cost" HeaderText="Per Bed Cost" SortExpression="Per_Bed_Cost" />
                <asp:BoundField DataField="Total_Charges" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" HeaderText="Total Bed Charge" SortExpression="Total_Charges" />
                <asp:BoundField DataField="Total_Extra_Bed_Cost" HeaderText="Total Extra Bed Cost" SortExpression="Total_Extra_Bed_Cost" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="Boarding" HeaderText="Boarding" SortExpression="Boarding"  />
                <asp:BoundField DataField="Food" HeaderText="Food" SortExpression="Food"  />
                <asp:BoundField DataField="Laundry" HeaderText="Laundry" SortExpression="Laundry"  />
                <asp:BoundField DataField="Telephone" HeaderText="Telephone" SortExpression="Telephone"  />
                <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges"  />       
                <asp:BoundField DataField="BAR" HeaderText="BAR" SortExpression="BAR" />
                <asp:BoundField DataField="Dinner" HeaderText="Dinner" SortExpression="DINNER"  />
                <asp:BoundField DataField="Breakfat" HeaderText="Breakfat" SortExpression="Breakfat"  />
                <asp:BoundField DataField="SPA" HeaderText="SPA" SortExpression="SPA"  />
                <asp:BoundField DataField="BanquetDinner" HeaderText="Banquet Dinner" SortExpression="BanquetDinner" />
                <asp:BoundField DataField="Cleaning" HeaderText="Cleaning" SortExpression="Cleaning" />
                <asp:BoundField DataField="ServiceCharges" HeaderText="Service Charges" SortExpression="ServiceCharges"  />
                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true"/>
                <asp:BoundField DataField="TAX_1" HeaderText="TAX_1" SortExpression="TAX_1" />
                <asp:BoundField DataField="TAX_2" HeaderText="TAX_2" SortExpression="TAX_2" />
                <asp:BoundField DataField="TAX_3" HeaderText="TAX_3" SortExpression="TAX_3" />
                <asp:BoundField DataField="TotalTAX" HeaderText="Total TAX" SortExpression="TotalTAX" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true"/>
                <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total" SortExpression="GrandTotal" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true"/>
                <asp:BoundField DataField="DiscountAmount" HeaderText="Discount" SortExpression="DiscountAmount" />
                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" SortExpression="NetAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true"/>
                <asp:BoundField DataField="PaidAmount" HeaderText="Paid" SortExpression="PaidAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true"/>
                <asp:BoundField DataField="BalanceAmount" HeaderText="Balanc" SortExpression="BalanceAmount" ItemStyle-Font-Bold="true" HeaderStyle-Font-Bold="true"/>
                <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" SortExpression="PaymentType" />
                <asp:BoundField DataField="CheckOutNote" HeaderText="Check Out Note" SortExpression="CheckOutNote" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            </Fields>
        </asp:DetailsView>

       <asp:PlaceHolder ID="phQrCode" runat="server" /> 

        </section>
    </div>
    </form>
</body>
</html>
