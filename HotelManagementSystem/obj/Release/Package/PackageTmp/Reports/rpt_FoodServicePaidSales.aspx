<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_FoodServicePaidSales.aspx.vb" Inherits="HotelManagementSystem.rpt_FoodServicePaidSales" %>

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
    <div>
         <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [HotelInformation]"></asp:SqlDataSource>
         <asp:DataList ID="DataList1" Font-Size="15px" CssClass="center-block" runat="server" DataSourceID="SqlDataSource2">
            <ItemTemplate>
                <b>
                    <asp:Label ID="HotelNameLabel" runat="server" Text='<%# Eval("HotelName") %>' />
                </b>
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
                
        
        <asp:GridView ID="GridView1" Size="15px" CssClass="table table-striped table-bordered table-condensed" ShowFooter="true" runat="server" AutoGenerateColumns="False" DataKeyNames="BILL_NO" >
            <Columns>
                <asp:BoundField DataField="BILL_NO" HeaderText="BILL NO" InsertVisible="False" ReadOnly="True" SortExpression="BILL_NO" />
                <asp:BoundField DataField="BILLING_DATE" Dataformatstring="{0:d-MMM-yyyy}" HeaderText="BILLING DATE" SortExpression="BILLING_DATE" />
                <asp:BoundField DataField="GUEST_ID" HeaderText="GUEST ID" SortExpression="GUEST_ID" />
                <asp:BoundField DataField="ITEM_COST" HeaderText="ITEM COST" SortExpression="ITEM_COST"   DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="TAX_1" HeaderText="TAX_1" SortExpression="TAX_1"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="TAX_2" HeaderText="TAX_2" SortExpression="TAX_2"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="TAX_3" HeaderText="TAX_3" SortExpression="TAX_3"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="TOTAL_TAX" HeaderText="TOTAL TAX" SortExpression="TOTAL_TAX"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="G_TOTAL" HeaderText="GRAND TOTAL" SortExpression="G_TOTAL"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="DISCOUNT" HeaderText="DISCOUNT" SortExpression="DISCOUNT"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="PAYMENT" HeaderText="PAYMENT" SortExpression="PAYMENT"  DataFormatString="{0:0.00}" />
                <asp:BoundField DataField="BALANCE" HeaderText="BALANCE" DataFormatString="{0:0.00}" SortExpression="BALANCE" />
            </Columns>
        </asp:GridView>

    </div>
    </form>
</body>
</html>
