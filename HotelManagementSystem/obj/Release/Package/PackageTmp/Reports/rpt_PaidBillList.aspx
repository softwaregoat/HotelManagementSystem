<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_PaidBillList.aspx.vb" Inherits="HotelManagementSystem.rpt_PaidBillList" %>

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
    <div class="wrapper">
    <section class="invoice">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hfDateFrom" runat="server" />
        <asp:HiddenField ID="hfDateTo" runat="server" />

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

        <asp:SqlDataSource ID="sqlBillList" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand=" SELECT        GUEST_ID, GuestName, Address, PhoneNo, Check_In_Date, Check_Out_Date, Total, TotalTAX, GrandTotal, DiscountAmount, NetAmount, PaidAmount, BalanceAmount, PaymentType, Status FROM            GuestInformation WHERE        (Check_Out_Date &gt;= @Check_Out_Date AND Check_Out_Date &lt;= @Check_Out_Date2) AND (BalanceAmount <= 0) ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfDateFrom" DbType="Date" Name="Check_Out_Date" PropertyName="Value" />
                <asp:ControlParameter ControlID="hfDateTo" DbType="Date" Name="Check_Out_Date2" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:GridView ID="GridView1"  Font-Size="13px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="False" DataKeyNames="GUEST_ID" DataSourceID="sqlBillList">
            <Columns>
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" InsertVisible="False" ReadOnly="True" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="GuestName" HeaderText="Guest Name" SortExpression="GuestName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" SortExpression="PhoneNo" />
                <asp:BoundField DataField="Check_In_Date" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Check In Date" SortExpression="Check_In_Date" />
                <asp:BoundField DataField="Check_Out_Date" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="Check Out Date" SortExpression="Check_Out_Date" />
                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                <asp:BoundField DataField="TotalTAX" HeaderText="Total TAX" SortExpression="TotalTAX" />
                <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total" SortExpression="GrandTotal" />
                <asp:BoundField DataField="DiscountAmount" HeaderText="Discount" SortExpression="DiscountAmount" />
                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" SortExpression="NetAmount" />
                <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amount" SortExpression="PaidAmount" />
                <asp:BoundField DataField="BalanceAmount" HeaderText="Balance" SortExpression="BalanceAmount" />
                <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" SortExpression="PaymentType" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            </Columns>
        </asp:GridView>

    </div>
    </form>
        </section>
        </div>
</body>
</html>
