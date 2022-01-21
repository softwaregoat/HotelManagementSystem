<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="rpt_SalesReceipt.aspx.vb" Inherits="HotelManagementSystem.rpt_SalesReceipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
    <link href="https://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans&display=swap" rel="stylesheet" />
    <style>
        body {
            font-family: 'Open Sans', sans-serif;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT * FROM [HotelInformation]"></asp:SqlDataSource>
            <asp:DataList ID="DataList1" CssClass="text-center" runat="server" DataSourceID="SqlDataSource2">
                <ItemTemplate>
                    <b>
                        <asp:Label ID="HotelNameLabel" runat="server" Text='<%# Eval("HotelName") %>' /></b>
                    <br />
                    Address:
                <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                    <br />
                    Contact:
                <asp:Label ID="ContactLabel" runat="server" Text='<%# Eval("Contact") %>' />
                    <br />
                    TIN NO:
                <asp:Label ID="TIN_NOLabel" runat="server" Text='<%# Eval("TIN_NO") %>' />
                </ItemTemplate>
            </asp:DataList>
            <h4 class="text-center">---------------- BILL ----------------</h4>
            <asp:SqlDataSource ID="sqlInvInfo" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT FoodServices.BILL_NO, FoodServices.BILLING_DATE, GuestInformation.GuestName, UserInformation.FirstName, UserInformation.LastName FROM FoodServices LEFT OUTER JOIN UserInformation ON FoodServices.USER_ID = UserInformation.USER_ID LEFT OUTER JOIN GuestInformation ON FoodServices.GUEST_ID = GuestInformation.GUEST_ID WHERE (FoodServices.BILL_NO = @BILL_NO)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="HiddenField1" DefaultValue="0" Name="BILL_NO" PropertyName="Value" Type="Int64" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:DataList ID="DataList2" runat="server" DataKeyField="BILL_NO" DataSourceID="sqlInvInfo" RepeatLayout="Flow" ShowFooter="False" ShowHeader="False">
                <ItemTemplate>
                    Bill No:
                <asp:Label ID="BILL_NOLabel" runat="server" Text='<%# Eval("BILL_NO") %>' />
                    <br />
                    Billing Date:
                <asp:Label ID="BILLING_DATE" runat="server" Text='<%# Format(Eval("BILLING_DATE"), "dd-MMM-yyyy") %>' />
                    <br />
                    Operator name:
                    <asp:Label ID="FirstName" runat="server" Text='<%# Eval("FirstName") %>' />
                    &nbsp;<asp:Label ID="LastName" runat="server" Text='<%# Eval("LastName") %>' />
                    <br />
                    Guest Name:
                <asp:Label ID="GuestNameLabel" runat="server" Text='<%# Eval("GuestName") %>' />
                </ItemTemplate>
            </asp:DataList>


            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT        FoodItem.FOOD_ITEM_NAME, FoodServiceDetails.Quantity, FoodServiceDetails.TotalPrice
                                FROM            FoodItem INNER JOIN   FoodServiceDetails ON FoodItem.FOOD_ITEM_ID = FoodServiceDetails.FOOD_ITEM_ID WHERE        (FoodServiceDetails.BILL_NO = @BILL_NO)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="HiddenField1" DefaultValue="0" Name="BILL_NO" PropertyName="Value" Type="Int64" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="GridView1" CssClass="table table-bordered table-responsive table-condensed small" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="FOOD_ITEM_NAME" HeaderText="Item Name" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Total" />
                </Columns>
            </asp:GridView>

            <asp:DataList ID="DataList3" CssClass="text-left" runat="server" DataSourceID="sqlTransactionDetails">
                <ItemTemplate>
                    Total :
                <asp:Label ID="ITEM_COSTLabel" runat="server" Text='<%# Eval("ITEM_COST") %>' />
                    <br />
                    TAX:
                <asp:Label ID="TOTAL_TAXLabel" runat="server" Text='<%# Eval("TOTAL_TAX") %>' />
                    <br />
                    Grand Total :
                <asp:Label ID="G_TOTALLabel" runat="server" Text='<%# Eval("G_TOTAL") %>' />
                    <br />
                    Discount:
                <asp:Label ID="DISCOUNTLabel" runat="server" Text='<%# Eval("DISCOUNT") %>' />
                    <br />
                    <b>Net Payable:<asp:Label ID="Payable" runat="server" Text='<%# Eval("G_TOTAL") - Eval("DISCOUNT") %>' /></b>
                    <br />
                    Payment:
                <asp:Label ID="PAYMENTLabel" runat="server" Text='<%# Eval("PAYMENT") %>' />
                    <br />
                    Balance:
                <asp:Label ID="BALANCELabel" runat="server" Text='<%# Eval("BALANCE") %>' />
                </ItemTemplate>
            </asp:DataList>

            <i>
                <asp:Label ID="lblTaxInfo" CssClass="text-primary small" runat="server" Text=""></asp:Label></i>

            <asp:SqlDataSource ID="sqlTransactionDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT [G_TOTAL], [TOTAL_TAX], [ITEM_COST], [PAYABLE], [PAYMENT], [BALANCE], [DISCOUNT] FROM [FoodServices] WHERE ([BILL_NO] = @BILL_NO)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="HiddenField1" DefaultValue="0" Name="BILL_NO" PropertyName="Value" Type="Int64" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
