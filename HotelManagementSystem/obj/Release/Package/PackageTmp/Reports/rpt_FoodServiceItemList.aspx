<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_FoodServiceItemList.aspx.vb" Inherits="HotelManagementSystem.rpt_FoodServiceItemList" %>

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

        <h4 class="text-left">FOOD ITEM LIST</h4> 

        <asp:GridView ID="GridView1" Size="15px" CssClass="table table-striped table-bordered table-condensed" runat="server" AutoGenerateColumns="False" DataKeyNames="FOOD_ITEM_ID" DataSourceID="sqlItemList">
            <Columns>
                <asp:BoundField DataField="FOOD_ITEM_ID" HeaderText="ITEM ID" InsertVisible="False" ReadOnly="True" SortExpression="FOOD_ITEM_ID" />
                <asp:BoundField DataField="FOOD_ITEM_NAME" HeaderText="ITEM NAME" SortExpression="FOOD_ITEM_NAME" />
                <asp:BoundField DataField="FOOD_ITEM_PRICE" HeaderText="PRICE" SortExpression="FOOD_ITEM_PRICE" />
            </Columns>
        </asp:GridView>

        <asp:SqlDataSource ID="sqlItemList" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [FoodItem]"></asp:SqlDataSource>

    </div>
    </form>
</body>
</html>
